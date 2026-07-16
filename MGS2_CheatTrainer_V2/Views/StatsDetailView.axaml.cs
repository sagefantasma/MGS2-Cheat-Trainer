using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Avalonia.Threading;

namespace MGS2_CheatTrainer_V2.Views;

public partial class StatsDetailView : UserControl
{
    private readonly Mgs2MemoryManager _memoryManager;
    private Stage? _lastKnownStage;
    private CancellationTokenSource? _cts;
    public static event EventHandler<string> UpdateStatusBar;
    
    public StatsDetailView()
    {
        InitializeComponent();
        _memoryManager = App.Services.GetRequiredService<Mgs2MemoryManager>();
        MainWindow.StatsTabActivated += OnStatsTab;
        AlertsStatBox.StatChanged += RequestStatusBarUpdate;
        KillsStatBox.StatChanged += RequestStatusBarUpdate;
        RationsStatBox.StatChanged += RequestStatusBarUpdate;
        ContinuesStatBox.StatChanged += RequestStatusBarUpdate;
        SavesStatBox.StatChanged += RequestStatusBarUpdate;
        ShotsFiredStatBox.StatChanged += RequestStatusBarUpdate;
        DamageTakenStatBox.StatChanged += RequestStatusBarUpdate;
        MechsDestroyedStatBox.StatChanged += RequestStatusBarUpdate;
        PlayTimeStatBox.StatChanged += RequestStatusBarUpdate;
        SpecialItemsStatBox.StatChanged += RequestStatusBarUpdate;
    }
    
    private static void RequestStatusBarUpdate(object? obj, string message)
    {
        UpdateStatusBar?.Invoke(null, message);
    }

    private void OnStatsTab(object? sender, bool activated)
    {
        if (activated)
        {
            _cts = new CancellationTokenSource();
            CancellationToken cancellationToken = _cts.Token;
            Task.Run(() => PeriodicTask.Run(UpdateScoringStats, TimeSpan.FromSeconds(1), cancellationToken));
        }
        else
        {
            _cts?.Cancel();
        }
    }

    private void UpdateGameStats(Mgs2MemoryManager.GameStats gameStats, Difficulty difficulty)
    {
        Dispatcher.UIThread.Post(() =>
        {
            AlertsStatBox.ValueTextBox.Text = gameStats.Alerts.ToString();
            KillsStatBox.ValueTextBox.Text = gameStats.Kills.ToString();
            RationsStatBox.ValueTextBox.Text = gameStats.Rations.ToString();
            ContinuesStatBox.ValueTextBox.Text = gameStats.Continues.ToString();
            SavesStatBox.ValueTextBox.Text = gameStats.Saves.ToString();
            ShotsFiredStatBox.ValueTextBox.Text = gameStats.Shots.ToString();
            DamageTakenStatBox.ValueTextBox.Text = gameStats.DamageTaken.ToString();
            MechsDestroyedStatBox.ValueTextBox.Text = gameStats.MechsDestroyed.ToString();
            //PlayTimeStatBox.ValueTextBox.Text = gameStats.PlayTime.ToString(); //TODO: update to real time format
            PlayTimeStatBox.ValueTextBox.Text = TimeSpan.FromSeconds(gameStats.PlayTime / 60).ToString(@"hh\:mm\:ss");
            SpecialItemsStatBox.ValueTextBox.Text = gameStats.SpecialItems == 0 ? "NONE" : "YES";
        });
    }

    private void UpdateScoringStats()
    {
        try
        {
            Stage currentStage = Mgs2MemoryManager.GetStage(); //Always found, or error is thrown.
            if (currentStage?.Name != _lastKnownStage?.Name)
            {
                //Logger.Debug($"User is now in stage: {currentStage}");
                _lastKnownStage = currentStage!;
            }

            //if we're in a main menu, we shouldn't try to find stats right now.
            if (!StageNames.MenuStages.StageList.Contains(currentStage!))
            {
                Mgs2MemoryManager.GameStats currentGameStats = _memoryManager.ReadGameStats();
                Difficulty currentDifficulty = Mgs2MemoryManager.ReadCurrentDifficulty();
                //GameType currentGameType = MGS2MemoryManager.ReadGameType(); //TODO: finish determining how to determine what gametype we're in
                UpdateGameStats(currentGameStats, currentDifficulty); //TODO: reimplement
            }
        }
        catch (Exception e)
        {
            if (Mgs2Monitor.Mgs2Process != null)
            {
                //only write to log when we are actually in a game, and should have some stats to grab
                //Logger.Error($"Failed to update scoring stats! Error encountered: {e}");
            }
        }
    }
}