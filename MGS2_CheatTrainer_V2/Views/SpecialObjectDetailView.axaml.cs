using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Events;

namespace MGS2_CheatTrainer_V2.Views;

public partial class SpecialObjectDetailView : UserControl
{
    private Constants.IMgs2Object? _object;
    private readonly Mgs2MemoryManager _memoryManager;
    
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

    public void Enabled_OnClick(object sender, RoutedEventArgs e)
    {
        //TODO: implement
        _object ??= Constants.DetermineObject(Name!);
        _memoryManager.ToggleObject(_object!);
    }

    public void ModifyValue_OnClick(object? sender, RoutedEventArgs e)
    {
        //TODO: implement
        _object ??= Constants.DetermineObject(Name!);
        _memoryManager.UpdateObjectBaseValue(_object!, (ushort)CurrentUpDown.Value);
    }
}