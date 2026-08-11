using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace MGS2_CheatTrainer_V2.Views;

public partial class WeaponsTabView : UserControl
{
    public event EventHandler<string>? UpdateStatusBar;
    public static bool AllWeaponsModEnabled;
    
    private void RequestStatusBarUpdate(object? obj, string message)
    {
        UpdateStatusBar?.Invoke(null, message);
    }
    
    public WeaponsTabView()
    {
        InitializeComponent();
        MainWindow.TabActivated += OnTabActivated;
    }
    
    private void OnTabActivated(object? sender, Tab e)
    {
        if (e == Tab.Weapons)
        {
            Logging.Logger?.Information("Weapons tab activated...");
        }
    }

    private void AllWeaponsModCheckBox_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        AllWeaponsModEnabled = (bool)AllWeaponsModCheckBox.IsChecked!;
    }
}