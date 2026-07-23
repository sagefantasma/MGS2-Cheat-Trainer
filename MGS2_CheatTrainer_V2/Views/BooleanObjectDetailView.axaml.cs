using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using MGS2_CheatTrainer_V2.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MGS2_CheatTrainer_V2.Views;

public partial class BooleanObjectDetailView : UserControl
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

    public void UpdateObjectEnabledState()
    {
        _object ??= Constants.DetermineObject(Name!);
        ushort value = _memoryManager.GetObjectValue(_object!);
        if (_object is not Constants.BooleanWeapon)
            EnabledCheckBox.IsChecked = value > 0;
        else
            EnabledCheckBox.IsChecked = value != 0xFFFF;
        _active = true;
    }

    public BooleanObjectDetailView()
    {
        InitializeComponent();
        _memoryManager = App.Services.GetRequiredService<Mgs2MemoryManager>();
    }

    public void Enabled_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!_active) return;
            _object ??= Constants.DetermineObject(Name!);
            _memoryManager.ToggleObject(_object!, (bool)EnabledCheckBox.IsChecked!);
            string enableState = EnabledCheckBox.IsChecked == true ? "enabled" : "disabled";
            ChangeStat($"{_object?.Name} is {enableState}");
        }
        catch (Exception ex)
        {
            string errorBrief = $"Failed to enable {Name!}";
            Logging.Logger?.Error($"{errorBrief}: {ex.Message}");
            ChangeStat(errorBrief);
        }
    }
    
    private void ChangeStat(string message)
    {
        ValueChanged?.Invoke(null, message);
    }
}