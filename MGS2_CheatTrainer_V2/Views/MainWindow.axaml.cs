using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace MGS2_CheatTrainer_V2.Views;

public partial class MainWindow : Window
{
    public static event EventHandler<bool>? StatsTabActivated;
    
    
    public MainWindow()
    {
        InitializeComponent();
        Logging.StartLogger();
        Mgs2Monitor.EnableMonitor(new CancellationToken());
        Mgs2Monitor.GameHooked += OnGameHooked;
        StatsDetailView.UpdateStatusBar += OnUpdateStatusBar;
        StringTabView.UpdateStatusBar += OnUpdateStatusBar;
        this.Closed+=(_,_)=>Mgs2Monitor.GameHooked -= OnGameHooked; //TODO: Necessary?
    }

    private void OnUpdateStatusBar(object? sender, string msg)
    {
        Dispatcher.UIThread.Post(async() =>
        {
            StatusLabel.Text = msg;
            //await Task.Delay(2000);
            //StatusLabel.Text = "Ready";
        });
    }

    private void OnGameHooked(object? sender, bool hooked)
    {
        Dispatcher.UIThread.Post(() =>
        {
            foreach (var item in ItemGrid.Children)
            {
                switch (item)
                {
                    case BooleanObjectDetailView booleanObjectDetailView:
                        booleanObjectDetailView.UpdateObjectEnabledState();
                        break;
                    case SpecialObjectDetailView specialObjectDetailView:
                        specialObjectDetailView.UpdateObjectEnabledState();
                        break;
                    case MaxableObjectDetailView maxableObjectDetailView:
                        maxableObjectDetailView.UpdateObjectEnabledState();
                        break;
                }
            }
            
            foreach (var weapon in WeaponGrid.Children)
            {
                switch (weapon)
                {
                    case BooleanObjectDetailView booleanObjectDetailView:
                        booleanObjectDetailView.UpdateObjectEnabledState();
                        break;
                    case SpecialObjectDetailView specialObjectDetailView:
                        specialObjectDetailView.UpdateObjectEnabledState();
                        break;
                    case MaxableObjectDetailView maxableObjectDetailView:
                        maxableObjectDetailView.UpdateObjectEnabledState();
                        break;
                }
            }
        });
    }

    private void ViewLogsMenuItem_Click(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void OpenInstallLocationMenuItem_Click(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void GithubMenuItem_Click(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void JoinDiscordMenuItem_Click(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void Mgs2TabControl_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        //throw new System.NotImplementedException();
        if (Mgs2TabControl?.SelectedItem?.Equals(StatsTab) != false)
        {
            StatsTabActivated?.Invoke(this, false);
        }
    }

    private void StatsTab_Tapped(object? sender, TappedEventArgs e)
    {
        StatsTabActivated?.Invoke(this, true);
    }
}