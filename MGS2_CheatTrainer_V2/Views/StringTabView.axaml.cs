using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace MGS2_CheatTrainer_V2.Views;

public partial class StringTabView : UserControl
{
    private readonly Mgs2MemoryManager _memoryManager;
    private Mgs2Strings.Mgs2String? _activeString;
    public static event EventHandler<string>? UpdateStatusBar;
    
    public StringTabView()
    {
        InitializeComponent();
        _memoryManager = App.Services.GetRequiredService<Mgs2MemoryManager>();
        foreach (Mgs2Strings.Mgs2String gameString in Mgs2Strings.Mgs2StringList)
        {
            StringListBox.Items.Add(new StringDetailView(gameString));
        }
    }
    
    private static void RequestStatusBarUpdate(object? obj, string message)
    {
        UpdateStatusBar?.Invoke(obj, message);
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
        catch
        {
            RequestStatusBarUpdate(null, "Failed to update string. See debuglog for more info");
        }
    }
}