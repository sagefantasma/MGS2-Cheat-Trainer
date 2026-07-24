using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using MGS2_CheatTrainer_V2.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MGS2_CheatTrainer_V2.Views;

public partial class SpecialObjectDetailView : UserControl
{
    private Constants.IMgs2Object? _object;
    private readonly Mgs2MemoryManager _memoryManager;
    private bool _active;
    public event EventHandler<string>? ValueChanged;
    
    public IImage? EntityImage
    {
        get => ObjectImage?.Source;
        set => ObjectImage?.Source = value;
    }
    
    public string? Mgs2Object
    {
        get;
        set
        {
            field = value;
            GroupBox.Header = $"{Mgs2Object}";
        }
    }

    public string? ValueName
    {
        get;
        set
        {
            field = value;
            ValueDescriptor.Text = $"Current {ValueName}";
        }
    }

    public int? MaxValue
    {
        get;
        set
        {
            field = value;
            if(value != null)
                CurrentUpDown.Maximum = (decimal)value;
        }
    }

    public SpecialObjectDetailView()
    {
        InitializeComponent();
        _memoryManager = App.Services.GetRequiredService<Mgs2MemoryManager>();
    }
    
    private void ChangeStat(string message)
    {
        ValueChanged?.Invoke(null, message);
    }
    
    public void UpdateObjectEnabledState()
    {
        _object ??= Constants.DetermineObject(Name!);
        ushort value = _memoryManager.GetObjectValue(_object!);
        EnabledCheckBox.IsChecked = value > 0;
        _active = true;
    }

    public void Enabled_OnClick(object sender, RoutedEventArgs e)
    {
        if (!_active) return;
        string enableState = EnabledCheckBox.IsChecked == true ? "enabled" : "disabled";
        _object ??= Constants.DetermineObject(Name!);
        Logging.Logger?.Information($"Attempting to set {_object.Name} {enableState}...");
        _memoryManager.ToggleObject(_object!, (bool)EnabledCheckBox.IsChecked!);
        ChangeStat($"{_object?.Name} is {enableState}");
    }

    public void ModifyValue_OnClick(object? sender, RoutedEventArgs e)
    {
        if (EnabledCheckBox.IsChecked == false)
        {
            EnabledCheckBox.IsChecked = true;
        }
        _object ??= Constants.DetermineObject(Name!);
        Logging.Logger?.Information($"Attempting to set {_object.Name} value to {CurrentUpDown.Value}...");
        _memoryManager.UpdateObjectBaseValue(_object!, (ushort)CurrentUpDown.Value!);
        ChangeStat($"Updated {_object.Name} {ValueName} to: {CurrentUpDown.Value}");
    }
}