using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MGS2_CheatTrainer_V2.Models;

namespace MGS2_CheatTrainer_V2.Views;

public partial class ButtonCheatViewModel : UserControl
{
    public event EventHandler<string>? CheatActivated;
    
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
            CheatButton.Content = value;
        }
    }
    
    private void ToggleCheat(string message)
    {
        CheatActivated?.Invoke(null, message);
    }
    
    public ButtonCheatViewModel()
    {
        InitializeComponent();
    }

    private async void CheatButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            GameCheat cheat = Mgs2Cheat.CheatList.Find(x => x.CheatType == Cheat);
            Logging.Logger?.Information($"Attempting to activate {CheatName}");
            ToggleCheat($"Attempting to activate {CheatName}...");
            IsEnabled = false;
            await Task.Run(() => cheat.CheatAction(true));
            IsEnabled = true;
            Logging.Logger?.Information($"{CheatName} 'successfully' activated.");
            ToggleCheat($"Finished attempting to activate {CheatName}. Results not guaranteed.");
        }
        catch (Exception ex)
        {
            string errorBrief = $"Failed to activate {CheatName}";
            Logging.Logger?.Error($"{errorBrief}: {ex.Message}");
            ToggleCheat(errorBrief);
        }
    }
}