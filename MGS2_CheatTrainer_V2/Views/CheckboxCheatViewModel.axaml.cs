using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MGS2_CheatTrainer_V2.Models;

namespace MGS2_CheatTrainer_V2.Views;

public partial class CheckboxCheatViewModel : UserControl
{
    public event EventHandler<string>? CheatToggled;
    
    public required Constants.Cheat Cheat
    {
        get;
        set;
    }

    public required string CheatName
    {
        get;
        set
        {
            field = value;
            CheatCheckBox.Content = value;
        }
    }

    public CheckboxCheatViewModel()
    {
        InitializeComponent();
    }
    
    private void ToggleCheat(string message)
    {
        CheatToggled?.Invoke(null, message);
    }
    
    private async void CheckboxCheat_OnIsCheckChanged(object? sender, RoutedEventArgs e)
    {
        try
        {
            GameCheat cheat = Mgs2Cheat.CheatList.Find(x => x.CheatType == Cheat);
            Logging.Logger?.Information($"Attempting to toggle {CheatName}");
            ToggleCheat($"Attempting to toggle {CheatName}...");
            IsEnabled = false;

            //These ifs handle "Radio-button"-like behavior for Force Sleep & Force Wake cheats.
            try
            {
                if (Cheat == Constants.Cheat.ForceGuardSleep && CheatCheckBox.IsChecked == true)
                    ((Parent as StackPanel)?.Children.First(x =>
                                (x as CheckboxCheatViewModel)?.Cheat == Constants.Cheat.ForceGuardWake) as
                            CheckboxCheatViewModel)?
                        .CheatCheckBox
                        .IsChecked = false;
                if (Cheat == Constants.Cheat.ForceGuardWake && CheatCheckBox.IsChecked == true)
                    ((Parent as StackPanel)?.Children.First(x =>
                                (x as CheckboxCheatViewModel)?.Cheat == Constants.Cheat.ForceGuardSleep) as
                            CheckboxCheatViewModel)?
                        .CheatCheckBox
                        .IsChecked = false;
            }
            catch
            {
                //Squelch exception here, it just means the opposite effect isn't toggled
            }

            bool toggleState = (bool)CheatCheckBox.IsChecked!;
            await Task.Run(() => cheat.CheatAction(toggleState));
            IsEnabled = true;
            Logging.Logger?.Information($"{CheatName} 'successfully' toggled.");
            ToggleCheat($"Finished attempting to toggle {CheatName}. Results not guaranteed.");
        }
        catch (Exception ex)
        {
            string errorBrief = $"Failed to toggle {CheatName}";
            Logging.Logger?.Error($"{errorBrief}: {ex.Message}");
            ToggleCheat(errorBrief);
        }
    }
}