using System;
using Avalonia.Controls;

namespace MGS2_CheatTrainer_V2.Views;

public partial class ItemTabView : UserControl
{
    public event EventHandler<string>? UpdateStatusBar;
    
    private void RequestStatusBarUpdate(object? obj, string message)
    {
        UpdateStatusBar?.Invoke(null, message);
    }
    
    public ItemTabView()
    {
        InitializeComponent();
        MainWindow.TabActivated += OnTabActivated;
    }

    private void OnTabActivated(object? sender, Tab e)
    {
        if (e == Tab.Items)
        {
            Logging.Logger?.Information("Items tab activated...");
        }
    }
}