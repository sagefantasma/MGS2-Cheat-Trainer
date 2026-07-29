using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Logging;
using MGS2_CheatTrainer_V2.Models;
using Microsoft.Extensions.DependencyInjection;
using Avalonia.Threading;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;

namespace MGS2_CheatTrainer_V2.Views;

public partial class CheatsTabView : UserControl
{
    private readonly Mgs2MemoryManager _memoryManager;
    public static event EventHandler<string>? UpdateStatusBar;
    private CancellationTokenSource? _cts;
    
    public CheatsTabView()
    {
        InitializeComponent();
        _memoryManager = App.Services.GetRequiredService<Mgs2MemoryManager>();
        MainWindow.TabActivated += OnTabEntered;
        foreach(var control in MainGameCheats.Children)
            if (control is CheckboxCheatViewModel cheatViewModel)
                cheatViewModel.CheatToggled += RequestStatusBarUpdate;
        foreach(var control in VrCheats.Children)
            if (control is CheckboxCheatViewModel cheatViewModel)
                cheatViewModel.CheatToggled += RequestStatusBarUpdate;
        foreach(var control in UiCheats.Children)
            if (control is CheckboxCheatViewModel cheatViewModel)
                cheatViewModel.CheatToggled += RequestStatusBarUpdate;

        GuardAnimationsListBox.ItemsSource = Mgs2AoB.GuardAnimationList;
    }

    private void OnTabEntered(object? sender, Tab e)
    {
        if (e == Tab.Cheats)
        {
            Logging.Logger?.Information("Cheats tab activated...");
            _cts = new CancellationTokenSource();
            CancellationToken cancellationToken = _cts.Token;
            Task.Run(() => PeriodicTask.Run(GetPlayerVitals, TimeSpan.FromSeconds(.333), cancellationToken),
                cancellationToken);
        }
    }

    private void GetPlayerVitals()
    {
        ushort playerHp = _memoryManager.GetCurrentHp();
        ushort playerMaxHp = _memoryManager.GetCurrentMaxHp();
        ushort playerGrip = _memoryManager.GetCurrentGripGauge();
        
        Dispatcher.UIThread.Post(() =>
        {
            if (Math.Abs(HpBar.Maximum - playerMaxHp) > 1)
            {
                HpBar.Maximum = playerMaxHp;
            }
            //GripBar.Maximum = activeCharacter == Constants.PlayableCharacter.Snake ? 1800 : 3600;
            if (GripBar.Maximum < playerGrip)
            {
                GripBar.Maximum = playerGrip;
            }
            HpBar.Value = playerHp;
            GripBar.Value = playerGrip;
        });
    }

    private static void RequestStatusBarUpdate(object? obj, string message)
    {
        UpdateStatusBar?.Invoke(null, message);
    }

    private async void SetAnimationButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            Mgs2AoB.GuardAnimation? guardAnimation = GuardAnimationsListBox.SelectedItem as Mgs2AoB.GuardAnimation;
            Logging.Logger?.Information(
                $"User clicked on 'Start animation' button with {guardAnimation?.Name} animation selected");
            UpdateStatusBar?.Invoke(null, $"Attempting to set all guards animation to: {guardAnimation?.Name}");

            await Task.Run(async () =>
            {
                await GameCheat.CheatActions.ReplaceWithSpecificCode(Mgs2AoB.GuardAnimations,
                    guardAnimation?.Bytes ?? throw new InvalidOperationException(),
                    Mgs2Offset.GuardAnimations);
            });
            
            UpdateStatusBar?.Invoke(null, $"All guards' animations have been set to {guardAnimation?.Name}~!");
        }
        catch (Exception ex)
        {
            Logging.Logger?.Error($"Failed to start guard animation: {ex}");
            UpdateStatusBar?.Invoke(null, $"Failed to force guard animation!");
            IMsBox<ButtonResult> msgBox = MessageBoxManager.GetMessageBoxStandard(
                "Failed to force guard animation",
                ex.Message);
            msgBox.ShowAsync();
        }
    }

    private async void ForceGuardSleepButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            Logging.Logger?.Information("User clicked on 'force guards to sleep' button");
            UpdateStatusBar?.Invoke(null, "Attempting to force all guards to sleep...");
            //force undo of wake(if done)
            await Task.Run(() =>
            {
                byte[] currentWake = GameCheat.CheatActions.ReadMemory(Mgs2AoB.ForceGuardsToWake, Mgs2Offset.ForceWake).Result;

                if (currentWake.SequenceEqual(Mgs2AoB.ForceGuardsToWakeBytes))
                {
                    Logging.Logger?.Information("Guards are currently forced awake, attempting to disable that");
                    GameCheat.CheatActions.ReplaceWithSpecificCode(Mgs2AoB.ForceGuardsToWake,
                        Mgs2AoB.StandardGuardWakeBytes, Mgs2Offset.ForceWake);
                    Logging.Logger?.Information("Guards are no longer forced awake");
                }

                GameCheat.CheatActions.ReplaceWithSpecificCode(Mgs2AoB.StandardGuardSleep,
                    Mgs2AoB.ForceGuardsToSleepBytes, Mgs2Offset.ForceSleep);
            });

            UpdateStatusBar?.Invoke(null, "All guards suddenly feel asleep!");
        }
        catch(Exception ex)
        {
            Logging.Logger?.Error($"Failed to force guard sleep: {ex}");
            UpdateStatusBar?.Invoke(null, $"Failed to force all guards to sleep!");
            IMsBox<ButtonResult> msgBox = MessageBoxManager.GetMessageBoxStandard(
                "Failed to force guard sleep",
                ex.Message);
            msgBox.ShowAsync();
        }
    }

    private async void ForceGuardWakeButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            Logging.Logger?.Information("User clicked on 'force guards to wake' button");
            UpdateStatusBar?.Invoke(null, "Attempting to force all guards to wake up...");
            //force undo of sleep(if done)
            await Task.Run(() =>
            {
                byte[] currentSleep =
                    GameCheat.CheatActions.ReadMemory(Mgs2AoB.ForceGuardsToSleep, Mgs2Offset.ForceSleep).Result;

                if (currentSleep.SequenceEqual(Mgs2AoB.ForceGuardsToSleepBytes))
                {
                    Logging.Logger?.Information("Guards are currently forced asleep, attempting to disable that");
                    GameCheat.CheatActions.ReplaceWithSpecificCode(Mgs2AoB.ForceGuardsToSleep,
                        Mgs2AoB.StandardGuardSleepBytes, Mgs2Offset.ForceSleep);
                    Logging.Logger?.Information("Guards are no longer forced asleep");
                }

                GameCheat.CheatActions.ReplaceWithSpecificCode(Mgs2AoB.StandardGuardWake,
                    Mgs2AoB.ForceGuardsToWakeBytes, Mgs2Offset.ForceWake);
            });

            UpdateStatusBar?.Invoke(null, "All guards have awoken!");
        }
        catch(Exception ex)
        {
            Logging.Logger?.Error($"Failed to force guard wake: {ex}");
            UpdateStatusBar?.Invoke(null, $"Failed to force all guards to wake up!");
            IMsBox<ButtonResult> msgBox = MessageBoxManager.GetMessageBoxStandard(
                "Failed to force guard wake",
                ex.Message);
            msgBox.ShowAsync();
        }
    }

    private void DecreasePullupsButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            ushort pullups = _memoryManager.ModifyGripLevel(false);
            UpdateStatusBar?.Invoke(null, $"Reduced pull-up count to {pullups}");
        }
        catch (Exception ex)
        {
            string errorBrief = "Failed to decrease player pull-up count";
            Logging.Logger?.Error($"{errorBrief}: {ex.Message}");
            UpdateStatusBar?.Invoke(null, "Failed to decrease pull-up count");
        }
    }

    private void IncreasePullupsButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            ushort pullups = _memoryManager.ModifyGripLevel(true);
            UpdateStatusBar?.Invoke(null, $"Increased pull-up count to {pullups}");
        }
        catch (Exception ex)
        {
            string errorBrief = "Failed to increase player pull-up count";
            Logging.Logger?.Error($"{errorBrief}: {ex.Message}");
            UpdateStatusBar?.Invoke(null, "Failed to increase pull-up count");
        }
    }

    private void HpBar_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        if (sender is Slider)
        {
            try
            {
                lock (HpBar)
                {
                    _memoryManager.ModifyCurrentHp((ushort)HpBar.Value);
                }
            }
            catch (Exception ex)
            {
                string errorBrief = "Failed to change player HP";
                Logging.Logger?.Error($"{errorBrief}: {ex.Message}");
                UpdateStatusBar?.Invoke(null, "Failed to change player HP");
            }
        }
    }

    private void GripBar_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        if (sender is Slider)
        {
            try
            {
                lock (GripBar)
                {
                    _memoryManager.ModifyCurrentGripGauge((ushort)GripBar.Value);
                }
            }
            catch (Exception ex)
            {
                string errorBrief = "Failed to change player grip";
                Logging.Logger?.Error($"{errorBrief}: {ex.Message}");
                UpdateStatusBar?.Invoke(null, "Failed to change player grip");
            }
        }
    }
}