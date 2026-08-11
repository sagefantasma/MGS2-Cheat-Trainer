using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Threading;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;

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
        Title = $"{Title} - v{Program.AppVersion}";
        try
        {
            Logging.StartLogger();
        }
        catch
        {
            IMsBox<ButtonResult> msgBox = MessageBoxManager.GetMessageBoxStandard(
                "Logging initialization failed!",
                $"We tried to start a debuglog, but something went wrong. Is {Logging.LogLocation} a valid directory on your PC?");
            msgBox.ShowAsync();
        }
        
        Mgs2Monitor.EnableMonitor(new CancellationToken());
        Mgs2Monitor.OnGameHooked += OnGameHooked;
        Mgs2Monitor.OnInvalidVersionDetected += OnInvalidVersionDetected;
        ItemsTabView.UpdateStatusBar += OnUpdateStatusBar;
        WeaponsTabView.UpdateStatusBar += OnUpdateStatusBar;
        StatsTabView.UpdateStatusBar += OnUpdateStatusBar;
        StringTabView.UpdateStatusBar += OnUpdateStatusBar;
        BossesTabView.UpdateStatusBar += OnUpdateStatusBar;
        CheatsTabView.UpdateStatusBar += OnUpdateStatusBar;
        Closed+=(_,_)=>Mgs2Monitor.OnGameHooked -= OnGameHooked; //TODO: Necessary?
        Task.Run(CheckForUpdates);
    }
    
    private static Window? GetMainWindow()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            return desktop.MainWindow;
        return null;
    }

    private void CheckForUpdates()
    {
        bool newerVersionAvailable = VersionSupport.CheckIfNewUpdateExists(Program.AppVersion);
        if (newerVersionAvailable)
        {
            Logging.Logger?.Debug("Newer version available, notifying user");
            Dispatcher.UIThread.Post(async void () =>
            {
                try
                {
                    IMsBox<ButtonResult> msgBox = MessageBoxManager.GetMessageBoxStandard(
                        "Update available",
                        "There is an updated version of this trainer available, would you like to view the Releases page?",
                        ButtonEnum.YesNo, windowStartupLocation: WindowStartupLocation);
                    if (await msgBox.ShowAsPopupAsync(GetMainWindow()) ==
                        ButtonResult.Yes) //Not working as intended for some reason :/
                    {
                        OpenUrl("https://github.com/sagefantasma/MGS2-Cheat-Trainer/releases");
                    }
                }
                catch (Exception e)
                {
                    //Squelch exception
                }
            });
        }
    }

    private void OnUpdateStatusBar(object? sender, string msg)
    {
        Dispatcher.UIThread.Post(() =>
        {
            StatusLabel.Text = msg;
            //await Task.Delay(2000);
            //StatusLabel.Text = "Ready";
        });
    }

    private void OnInvalidVersionDetected(object? sender, string msg)
    {
        Logging.Logger?.Error($"Incompatible game version detected: {msg}");
        Dispatcher.UIThread.Post(() =>
        {
            IMsBox<ButtonResult> msgBox = MessageBoxManager.GetMessageBoxStandard(
                "Incompatible game version detected!",
                msg, windowStartupLocation: WindowStartupLocation);
            msgBox.ShowAsPopupAsync(GetMainWindow());
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
        OpenUrl(Logging.LogLocation!);
    }

    private void OpenInstallLocationMenuItem_Click(object? sender, RoutedEventArgs e)
    {
        OpenUrl(AppDomain.CurrentDomain.BaseDirectory);
    }

    private void GithubMenuItem_Click(object? sender, RoutedEventArgs e)
    {
        OpenUrl("https://github.com/sagefantasma/MGS2-Cheat-Trainer/");
    }

    private void JoinDiscordMenuItem_Click(object? sender, RoutedEventArgs e)
    {
        OpenUrl("https://discord.gg/XUh58VfqDu");
    }

    private void OpenUrl(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch
        {
            OnUpdateStatusBar(null, $"Unable to open link, you can use this link instead: {url}");
        }
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