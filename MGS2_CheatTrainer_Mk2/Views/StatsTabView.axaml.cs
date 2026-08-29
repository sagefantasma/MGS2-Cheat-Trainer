using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Avalonia.Threading;
using MGS2_CheatTrainer_V2.Models;

namespace MGS2_CheatTrainer_V2.Views;

public partial class StatsTabView : UserControl
{
    private readonly Mgs2MemoryManager _memoryManager;
    private Stage? _lastKnownStage;
    private CancellationTokenSource? _cts;
    public static event EventHandler<string>? UpdateStatusBar;
    private static readonly Dictionary<GameStats.ModifiableStats, bool> FrozenStats = new();
    
    public StatsTabView()
    {
        InitializeComponent();
        foreach (var modifiableStat in Enum.GetValues<GameStats.ModifiableStats>())
        {
            FrozenStats[modifiableStat] = false;
        }
        _memoryManager = App.Services.GetRequiredService<Mgs2MemoryManager>();
        MainWindow.TabActivated += OnTabEntered;
        AlertsStatBox.StatChanged += RequestStatusBarUpdate;
        AlertsStatBox.StatFrozen += FreezeStatBox;
        KillsStatBox.StatChanged += RequestStatusBarUpdate;
        KillsStatBox.StatFrozen += FreezeStatBox;
        RationsStatBox.StatChanged += RequestStatusBarUpdate;
        RationsStatBox.StatFrozen += FreezeStatBox;
        ContinuesStatBox.StatChanged += RequestStatusBarUpdate;
        ContinuesStatBox.StatFrozen += FreezeStatBox;
        SavesStatBox.StatChanged += RequestStatusBarUpdate;
        SavesStatBox.StatFrozen += FreezeStatBox;
        ShotsFiredStatBox.StatChanged += RequestStatusBarUpdate;
        ShotsFiredStatBox.StatFrozen += FreezeStatBox;
        DamageTakenStatBox.StatChanged += RequestStatusBarUpdate;
        DamageTakenStatBox.StatFrozen += FreezeStatBox;
        MechsDestroyedStatBox.StatChanged += RequestStatusBarUpdate;
        MechsDestroyedStatBox.StatFrozen += FreezeStatBox;
        PlayTimeStatBox.StatChanged += RequestStatusBarUpdate;
        PlayTimeStatBox.StatFrozen += FreezeStatBox;
        SpecialItemsStatBox.StatChanged += RequestStatusBarUpdate;
        SpecialItemsStatBox.StatFrozen += FreezeStatBox;
    }
    
    private static void RequestStatusBarUpdate(object? obj, string message)
    {
        UpdateStatusBar?.Invoke(null, message);
    }

    private static void FreezeStatBox(object? obj, bool freeze)
    {
        SingleStatBox? statBox = obj as SingleStatBox;
        FrozenStats[statBox!.Stat] = freeze;
    }

    private void OnTabEntered(object? sender, Tab activated)
    {
        if (activated == Tab.Stats)
        {
            Logging.Logger?.Information("Stats tab opened, beginning stats monitoring...");
            _cts = new CancellationTokenSource();
            CancellationToken cancellationToken = _cts.Token;
            Task.Run(() => PeriodicTask.Run(UpdateScoringStats, TimeSpan.FromSeconds(1), cancellationToken), cancellationToken);
        }
        else
        {
            if (_cts == null) return;
            Logging.Logger?.Information("Leaving stats tab, ending stats monitoring...");
            _cts.Cancel();
        }
    }

    private void UpdateGameStats(GameStats gameStats, Difficulty difficulty, GameType gameType)
    {
        Dispatcher.UIThread.Post(() =>
        {
            if(!FrozenStats[GameStats.ModifiableStats.Alerts])
                AlertsStatBox.ValueTextBox.Text = gameStats.Alerts.ToString();
            if(!FrozenStats[GameStats.ModifiableStats.Kills])
                KillsStatBox.ValueTextBox.Text = gameStats.Kills.ToString();
            if(!FrozenStats[GameStats.ModifiableStats.Rations])
                RationsStatBox.ValueTextBox.Text = gameStats.Rations.ToString();
            if(!FrozenStats[GameStats.ModifiableStats.Continues])
                ContinuesStatBox.ValueTextBox.Text = gameStats.Continues.ToString();
            if(!FrozenStats[GameStats.ModifiableStats.Saves])
                SavesStatBox.ValueTextBox.Text = gameStats.Saves.ToString();
            if(!FrozenStats[GameStats.ModifiableStats.Shots])
                ShotsFiredStatBox.ValueTextBox.Text = gameStats.Shots.ToString();
            if(!FrozenStats[GameStats.ModifiableStats.DamageTaken])
                DamageTakenStatBox.ValueTextBox.Text = gameStats.DamageTaken.ToString();
            if(!FrozenStats[GameStats.ModifiableStats.MechsDestroyed])
                MechsDestroyedStatBox.ValueTextBox.Text = gameStats.MechsDestroyed.ToString();
            
            PlayTimeStatBox.ValueTextBox.Text = TimeSpan.FromSeconds(gameStats.PlayTime / 60).ToString(@"hh\:mm\:ss");
            SpecialItemsStatBox.ValueTextBox.Text = gameStats.SpecialItems == 0 ? "NONE" : "YES";
        });
        if (gameType == GameType.TankerPlant)
        {
            Rank? projectedRank = Rank.CurrentlyProjectedRank(gameStats, difficulty, gameType);
            UpdateStatusBar?.Invoke(null, $"Projected Rank: {projectedRank}");
        }
        else
        {
            UpdateStatusBar?.Invoke(null, $"Rank projection only supported for Tanker-Plant");
        }
    }

    private void UpdateScoringStats()
    {
        try
        {
            if (Mgs2Monitor.Mgs2Process == null)
                return;
            Stage currentStage = _memoryManager.GetStage(); //Always found, or error is thrown.
            if (currentStage.Name != _lastKnownStage?.Name)
            {
                //Logger.Debug($"User is now in stage: {currentStage}");
                _lastKnownStage = currentStage;
            }

            //if we're in a main menu, we shouldn't try to find stats right now.
            if (!StageNames.MenuStages.StageList.Contains(currentStage))
            {
                GameStats currentGameStats = _memoryManager.ReadGameStats();
                Difficulty currentDifficulty = _memoryManager.ReadCurrentDifficulty(); 
                
                GameType currentGameType = _memoryManager.ReadGameType(); 
                UpdateGameStats(currentGameStats, currentDifficulty, currentGameType);
            }
        }
        catch
        {
            UpdateStatusBar?.Invoke(null, "Unable to update game stats!");
        }
    }
}