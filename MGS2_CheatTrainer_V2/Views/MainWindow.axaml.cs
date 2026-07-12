using System.Collections.Generic;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace MGS2_CheatTrainer_V2.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Logging.StartLogger();
        Mgs2Monitor.EnableMonitor(new CancellationToken());
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
    }
}