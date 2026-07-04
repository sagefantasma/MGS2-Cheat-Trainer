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
    private readonly MGS2MemoryManager _memoryManager;
    
    public IImage? EntityImage
    {
        get => ObjectImage?.Source;
        set => ObjectImage?.Source = value;
    }

    public SpecialObjectDetailView()
    {
        InitializeComponent();
        _memoryManager = App.Services.GetRequiredService<MGS2MemoryManager>();
    }

    private static Constants.IMgs2Object? DetermineObject(string input)
    {
        try
        {
            string viewName = input.ToLower();
            
            return Constants.ItemList.Find(x=> viewName.Contains($"{x.Shorthand}detailview", StringComparison.InvariantCultureIgnoreCase)) ??
                Constants.WeaponList.Find(x => viewName.Contains($"{x.Shorthand}detailview", StringComparison.InvariantCultureIgnoreCase)) ?? throw new Exception();
        }
        catch (Exception ex)
        {
            throw new NullReferenceException($"{input} is an unknown object");
        }
    }

    public void Enabled_OnClick(object sender, RoutedEventArgs e)
    {
        //TODO: implement
        _object ??= DetermineObject(Name!);
        _memoryManager.ToggleObject(_object!);
    }

    public void ModifyValue_OnClick(object? sender, RoutedEventArgs e)
    {
        //TODO: implement
        _object ??= DetermineObject(Name!);
        ushort value = 0; //TODO: make real value
        _memoryManager.UpdateObjectBaseValue(_object!, value);
    }
}