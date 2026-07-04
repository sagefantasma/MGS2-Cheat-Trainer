using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Events;

namespace MGS2_CheatTrainer_V2.Views;

public partial class BooleanObjectDetailView : UserControl
{
    private Constants.Weapon? _weapon;
    private readonly MemoryManager _memoryManager;
    
    public IImage? EntityImage
    {
        get => ObjectImage?.Source;
        set => ObjectImage?.Source = value;
    }

    public BooleanObjectDetailView()
    {
        InitializeComponent();
        _memoryManager = App.Services.GetRequiredService<MemoryManager>();
    }

    private static Constants.Weapon DetermineWeapon(string input)
    {
        try
        {
            return Constants.WeaponsList.Find(x => input.ToLower().Contains($"{x.Shorthand}detailview", StringComparison.InvariantCultureIgnoreCase))!;
        }
        catch (Exception ex)
        {
            throw new NullReferenceException($"{input} is an unknown weapon");
        }
    }

    public void Enabled_OnClick(object sender, RoutedEventArgs e)
    {
        //TODO: implement
        _weapon ??= DetermineWeapon(Name!);
        _memoryManager.ToggleObject(_weapon);
    }

    public void LevelUp_OnClick(object? sender, RoutedEventArgs e)
    {
        //TODO: implement
        _weapon ??= DetermineWeapon(Name!);
        _memoryManager.LevelUpObject(_weapon);
    }

    public void MaxAmmo_OnClick(object? sender, RoutedEventArgs e)
    {
        //TODO: implement
        _weapon ??= DetermineWeapon(Name!);
        _memoryManager.MaxAmmo(_weapon);
    }
}