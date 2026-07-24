using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Threading;
using MGS2_CheatTrainer_V2.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MGS2_CheatTrainer_V2.Views;

public partial class BossDetailView : UserControl
{
    private readonly Mgs2MemoryManager _memoryManager;
    private CancellationTokenSource? _cts;
    private bool _inFight;

    public required Constants.Boss Boss
    {
        get;
        set;
    }
    
    public IImage? EntityImage
    {
        get => ObjectImage?.Source;
        set => ObjectImage?.Source = value;
    }

    public bool HasStamina
    {
        get;
        set
        {
            field = value;
            StaminaSlider.IsVisible = value;
            StaminaLabel.IsVisible = value;
            StaminaSlider.IsEnabled = value;
        }
    }

    public bool IsActive
    {
        get;
        set
        {
            field = value;
            if (value)
            {
                ImageDarkener.Opacity = 0;
            }
            else
            {
                ImageDarkener.Opacity = .8;
            }

            IsEnabled = value;
        }
    }
    
    public BossDetailView()
    {
        InitializeComponent();
        _memoryManager = App.Services.GetRequiredService<Mgs2MemoryManager>();
        BossesTabView.ActiveBoss += OnActiveFight;
    }
    
    private void OnActiveFight(object? sender, Constants.Boss activeBoss)
    {
        if (activeBoss == Boss)
        {
            Logging.Logger?.Information($"{Boss} fight detected!");
            if (!_inFight)
            {
                _inFight = true;
                _cts = new CancellationTokenSource();
                CancellationToken cancellationToken = _cts.Token;
                Task.Run(() => PeriodicTask.Run(GetVitals, TimeSpan.FromSeconds(.333), cancellationToken),
                    cancellationToken);
            }
        }
        else
        {
            _inFight = false;
            if (_cts == null) return;
            Logging.Logger?.Information($"{Boss} fight no longer detected, ending their GetVitals thread.");
            _cts.Cancel();
            
        }
    }

    private void GetVitals()
    {
        try
        {
            BossVitals vitals = _memoryManager.GetBossVitals(Boss);

            Dispatcher.UIThread.Post(() =>
            {
                if (HpSlider.Maximum < vitals.Health)
                    HpSlider.Maximum = vitals.Health;
                HpSlider.Value = vitals.Health;

                if (vitals.HasStamina)
                {
                    if (StaminaSlider.Maximum < vitals.Stamina)
                        StaminaSlider.Maximum = vitals.Stamina;
                    StaminaSlider.Value = vitals.Stamina;
                }
            });
        }
        catch(Exception e)
        {
            string errorBrief = $"Failed to get vitals for {Boss}";
            Logging.Logger?.Error($"{errorBrief}: {e.Message}");
        }
    }

    private void HpSlider_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        if (sender is Slider)
        {
            try
            {
                lock (HpSlider)
                {
                    BossVitals bossVitals = BossVitals.ParseBossVitals(Boss);
                    bossVitals.Health = (int)HpSlider.Value;
                    _memoryManager.SetBossVitals(bossVitals);
                }
            }
            catch (Exception ex)
            {
                string errorBrief = $"Failed to change HP for {Boss}";
                Logging.Logger?.Error($"{errorBrief}: {ex.Message}");
            }
        }
    }

    private void StaminaSlider_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        if (sender is Slider)
        {
            try
            {
                lock (StaminaSlider)
                {
                    BossVitals bossVitals = BossVitals.ParseBossVitals(Boss);
                    bossVitals.Stamina = (int)StaminaSlider.Value;
                    _memoryManager.SetBossVitals(bossVitals);
                }
            }
            catch (Exception ex)
            {
                string errorBrief = $"Failed to change Stamina for {Boss}";
                Logging.Logger?.Error($"{errorBrief}: {ex.Message}");
            }
        }
    }
}