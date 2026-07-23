using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MGS2_CheatTrainer_V2.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MGS2_CheatTrainer_V2.Views;

public partial class StringTabView : UserControl
{
    private readonly Mgs2MemoryManager _memoryManager;
    private Mgs2Strings.Mgs2String? _activeString;
    private bool stringsLoaded = false;
    public static event EventHandler<string>? UpdateStatusBar;
    
    public StringTabView()
    {
        InitializeComponent();
        _memoryManager = App.Services.GetRequiredService<Mgs2MemoryManager>();
        MainWindow.TabActivated += OnTabEntered;
        foreach (Mgs2Strings.Mgs2String gameString in Mgs2Strings.Mgs2StringList)
        {
            StringListBox.Items.Add(new StringDetailView(gameString));
        }
    }
    
    private static void RequestStatusBarUpdate(object? obj, string message)
    {
        UpdateStatusBar?.Invoke(obj, message);
    }
    
    private async void OnTabEntered(object? sender, Tab activated)
    {
        if (activated == Tab.Strings)
        {
            //This *works*, but it is terribly slow(on Linux) because we're doing AoB scans for each.
            //Little gain from doing this, so scrapping it instead. Neat idea though.
            /*if (!stringsLoaded)
            {
                RequestStatusBarUpdate(null, "Fetching current string values...");
                foreach (var str in StringListBox.Items)
                {
                    StringDetailView sdv = str as StringDetailView;
                    string currentValue = await Task.Run(() => _memoryManager.ReadGameString(sdv.GameString));
                    sdv.GameString.CurrentText = currentValue;
                }
                RequestStatusBarUpdate(null, "Current string values fetched!");
                stringsLoaded = true;
            }*/
        }
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (StringListBox.SelectedIndex == -1) return;
        _activeString = (StringListBox.SelectedItem as StringDetailView)?.GameString;
        InputTextBox.Text = _activeString!.CurrentText;
        InputTextBox.MaxLength = _activeString.MemoryOffset.Length;
        InputTextBox.IsEnabled = true;
        UpdateStringButton.IsEnabled = true;
    }

    private async void UpdateStringButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            string textToSet = InputTextBox.Text!;
            RequestStatusBarUpdate(null, $"Attempting to set {_activeString!.Tag} string to {textToSet}...");
            await Task.Run(() => _memoryManager.UpdateGameString(_activeString!, textToSet));
            RequestStatusBarUpdate(null, $"{_activeString!.Tag} updated! You will need to reload the area to see the change");
        }
        catch(Exception ex)
        {
            string errorBrief = $"Failed to update string for {_activeString?.Tag}";
            Logging.Logger?.Error($"{errorBrief}: {ex.Message}");
            RequestStatusBarUpdate(null, errorBrief);
        }
    }
}