using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using static MGS2_CheatTrainer_V2.Mgs2Cheat;

namespace MGS2_CheatTrainer_V2.Views;

public partial class CheatsTabView : UserControl
{
    private readonly Mgs2MemoryManager _memoryManager;
    public static event EventHandler<string>? UpdateStatusBar;
    
    public CheatsTabView()
    {
        InitializeComponent();
        _memoryManager = App.Services.GetRequiredService<Mgs2MemoryManager>();
        foreach(var control in MainGameCheats.Children)
            if (control is CheckboxCheatViewModel cheatViewModel)
                cheatViewModel.CheatToggled += RequestStatusBarUpdate;
        foreach(var control in VrCheats.Children)
            if (control is CheckboxCheatViewModel cheatViewModel)
                cheatViewModel.CheatToggled += RequestStatusBarUpdate;
        foreach(var control in UiCheats.Children)
            if (control is CheckboxCheatViewModel cheatViewModel)
                cheatViewModel.CheatToggled += RequestStatusBarUpdate;
    }

    private static void RequestStatusBarUpdate(object? obj, string message)
    {
        UpdateStatusBar?.Invoke(null, message);
    }
}