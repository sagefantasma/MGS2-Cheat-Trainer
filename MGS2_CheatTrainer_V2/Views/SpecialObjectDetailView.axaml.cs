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
        ushort value = 0; //TODO: make real value
        _memoryManager.UpdateObjectBaseValue(_object!, value);
    }
}