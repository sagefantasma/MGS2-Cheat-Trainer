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

public enum Tab
{
    Items,
    Weapons,
    Bosses,
    Stats,
    Strings,
    Cheats
}

public partial class MainWindow : Window
{
    public static event EventHandler<Tab>? TabActivated;
    
    
    public MainWindow()
    {
        InitializeComponent();
        Logging.StartLogger();
        Mgs2Monitor.EnableMonitor(new CancellationToken());
        Mgs2Monitor.GameHooked += OnGameHooked;
        ItemsTabView.UpdateStatusBar += OnUpdateStatusBar;
        WeaponsTabView.UpdateStatusBar += OnUpdateStatusBar;
        StatsTabView.UpdateStatusBar += OnUpdateStatusBar;
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
            foreach (var item in ItemsTabView.ItemGrid.Children)
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
            
            foreach (var weapon in WeaponsTabView.WeaponGrid.Children)
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
        if (Mgs2TabControl?.SelectedItem?.Equals(ItemsTab) == true)
        {
            TabActivated?.Invoke(this, Tab.Items);
        }
        else if (Mgs2TabControl?.SelectedItem?.Equals(WeaponsTab) == true)
        {
            TabActivated?.Invoke(this, Tab.Weapons);
        }
        else if (Mgs2TabControl?.SelectedItem?.Equals(StatsTab) == true)
        {
            TabActivated?.Invoke(this, Tab.Stats);
        }
        else if (Mgs2TabControl?.SelectedItem?.Equals(BossesTab) == true)
        {
            TabActivated?.Invoke(this, Tab.Bosses);
        }
        else if (Mgs2TabControl?.SelectedItem?.Equals(StringsTab) == true)
        {
            TabActivated?.Invoke(this, Tab.Strings);
        }
        else if (Mgs2TabControl?.SelectedItem?.Equals(CheatsTab) == true)
        {
            TabActivated?.Invoke(this, Tab.Cheats);
        }
    }
}