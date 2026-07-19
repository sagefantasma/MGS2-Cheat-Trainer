using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using MGS2_CheatTrainer_V2.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MGS2_CheatTrainer_V2.Views;

public partial class BossDetailView : UserControl
{
    private readonly Mgs2MemoryManager _memoryManager;
    private CancellationTokenSource? _cts;

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

    private bool _inFight = false;
    
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
            _cts?.Cancel();
            _inFight = false;
        }
    }

    private void GetVitals()
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
}