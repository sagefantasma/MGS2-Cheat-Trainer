using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using System.Threading;
using Avalonia.Markup.Xaml;
using MGS2_CheatTrainer_V2.ViewModels;
using MGS2_CheatTrainer_V2.Views;
using Microsoft.Extensions.DependencyInjection;

namespace MGS2_CheatTrainer_V2;

public partial class App : Application
{
    public static IServiceProvider Services =>
        _services ?? throw new InvalidOperationException("Services not initialized yet");
    private static IServiceProvider? _services;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        ServiceCollection collection = new();
        collection.AddSingleton<Mgs2MemoryManager>();

        _services = collection.BuildServiceProvider();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };

            desktop.Exit += (_, _) =>
            {
                Mgs2MemoryManager memoryManager = _services.GetService<Mgs2MemoryManager>();
                memoryManager?.Dispose();
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}