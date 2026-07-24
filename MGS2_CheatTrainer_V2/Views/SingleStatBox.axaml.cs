using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using MGS2_CheatTrainer_V2.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MGS2_CheatTrainer_V2.Views;

public partial class SingleStatBox : UserControl
{
    private readonly Mgs2MemoryManager _memoryManager;

    internal GameStats.ModifiableStats Stat { get; set; }
    public event EventHandler<bool>? StatFrozen;
    public event EventHandler<string>? StatChanged;
    
    private void ChangeStat(string message)
    {
        StatChanged?.Invoke(null, message);
    }

    private void FreezeStat(bool freeze)
    {
        StatFrozen?.Invoke(this, freeze);
    }
    
    public string GroupBoxName
    {
        set
        {
            this.GroupBox.Header = value;
            this.SetButton.Content = $"Set {value}";
        }
    }

    public bool Editable
    {
        set
        {
            this.SetButton.IsEnabled = value;
        }
    }

    public SingleStatBox()
    {
        InitializeComponent();
        _memoryManager = App.Services.GetRequiredService<Mgs2MemoryManager>();
    }

    private void SetButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            Logging.Logger?.Information($"Attempting to set {Stat} to {ValueTextBox.Text}");
            _memoryManager.ChangeGameStat(Stat, short.Parse(ValueTextBox.Text!));
            ChangeStat($"{Stat} updated to: {ValueTextBox.Text}");
        }
        catch (Exception ex)
        {
            string errorBrief = $"Failed to modify {Stat}";
            Logging.Logger?.Error($"{errorBrief}: {ex.Message}");
            ChangeStat(errorBrief);
        }
    }

    private void ValueTextBox_OnGetFocus(object? sender, FocusChangedEventArgs e)
    {
        FreezeStat(true);
    }

    private void ValueTextBox_OnLostFocus(object? sender, FocusChangedEventArgs e)
    {
        FreezeStat(false);
    }
}