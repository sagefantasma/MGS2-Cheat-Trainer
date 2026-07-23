using System;
using Avalonia.Controls;

namespace MGS2_CheatTrainer_V2.Views;

public partial class CheatsTabView : UserControl
{
    public static event EventHandler<string>? UpdateStatusBar;
    
    public CheatsTabView()
    {
        InitializeComponent();
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