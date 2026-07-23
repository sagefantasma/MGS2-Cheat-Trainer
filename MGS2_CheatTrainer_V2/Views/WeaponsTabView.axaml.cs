using System;
using Avalonia.Controls;

namespace MGS2_CheatTrainer_V2.Views;

public partial class WeaponsTabView : UserControl
{
    public event EventHandler<string>? UpdateStatusBar;
    //TODO: need to add support for the "All Weapons Mod" checkbox
    private void RequestStatusBarUpdate(object? obj, string message)
    {
        UpdateStatusBar?.Invoke(null, message);
    }
    
    public WeaponsTabView()
    {
        InitializeComponent();
    }
    
}