using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using MGS2_CheatTrainer_V2.Models;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;

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
    
    private void SendStatusUpdate(string message)
    {
        ValueChanged?.Invoke(null, message);
    }
    
    public void UpdateObjectEnabledState()
    {
        try
        {
            _object ??= Constants.DetermineObject(Name!);
            ushort value = _memoryManager.GetObjectValue(_object!);
            EnabledCheckBox.IsChecked = value > 0;
            if (EnabledCheckBox.IsChecked == true)
            {
                CurrentUpDown.Value = value;
            }
            _active = true;
        }
        catch
        {
            //Squelch automated action errors
        }
    }

    public void Enabled_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!_active) return;
            string enableState = EnabledCheckBox.IsChecked == true ? "enabled" : "disabled";
            _object ??= Constants.DetermineObject(Name!);
            Logging.Logger?.Information($"Attempting to set {_object.Name} {enableState}...");
            _memoryManager.ToggleObject(_object!, (bool)EnabledCheckBox.IsChecked!);
            SendStatusUpdate($"{_object?.Name} is {enableState}");
        }
        catch (Exception ex)
        {
            _active = false;
            EnabledCheckBox.IsChecked = !EnabledCheckBox.IsChecked;
            string errorBrief = $"Failed to toggle {Name!}";
            Logging.Logger?.Error($"{errorBrief}: {ex.Message}");
            SendStatusUpdate(errorBrief);
            IMsBox<ButtonResult> msgBox = MessageBoxManager.GetMessageBoxStandard(
                errorBrief,
                ex.Message);
            msgBox.ShowAsync();
            _active = true;
        }
    }

    public void ModifyValue_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            _object ??= Constants.DetermineObject(Name!);
            Logging.Logger?.Information($"Attempting to set {_object.Name} {ValueName} to {CurrentUpDown.Value}...");
            _memoryManager.UpdateObjectBaseValue(_object!, (ushort)CurrentUpDown.Value!);
            SendStatusUpdate($"Updated {_object.Name} {ValueName} to: {CurrentUpDown.Value}");
            if (EnabledCheckBox.IsChecked == false)
            {
                _active = false;
                EnabledCheckBox.IsChecked = true;
                _active = true;
            }
        }
        catch (Exception ex)
        {
            string errorBrief = $"Failed to set {ValueName} for {Name!}";
            Logging.Logger?.Error($"{errorBrief}: {ex.Message}");
            SendStatusUpdate(errorBrief);
            IMsBox<ButtonResult> msgBox = MessageBoxManager.GetMessageBoxStandard(
                errorBrief,
                ex.Message);
            msgBox.ShowAsync();
        }
    }
}