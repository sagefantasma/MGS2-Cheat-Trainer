using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace MGS2_CheatTrainer_V2.Views;

public partial class MainWindow : Window
{
    private readonly List<GuiObject> _itemGuiObjectList = new();
    public MainWindow()
    {
        InitializeComponent();
        InitializeItemTab();
    }

    private void ModifyConfigMenuItem_Click(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void LaunchMgs2MenuItem_Click(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
    }

    private void ItemListBox_Tapped(object? sender, TappedEventArgs e)
    {
        if (ItemListBox.SelectedItem is not GuiObject selected) return;

        ItemGroupHeader.Text = selected.Name;
        ItemDetailContent.Content = selected.AssociatedControl();
    }

    private void InitializeItemTab()
    {
        //TODO: make this not stupid
        _itemGuiObjectList.Add(new GuiObject());
        
        ItemListBox.ItemsSource = _itemGuiObjectList;
        ItemListBox.DisplayMemberBinding = new Avalonia.Data.Binding("Name");
    }

    private MaxableObjectDetailView MakeItemDetail(dynamic item, bool hasCount)
    {
        var view = new MaxableObjectDetailView();
        view.HasCount = hasCount;
        view.OnToggle = (on) => item.ToggleItem(on, _logger, StatusLabel);
        view.OnSetCurrent = (val) => item.UpdateCurrentCount(val, _logger, StatusLabel);
        view.OnSetMax = (val) => item.UpdateMaxCount(val, _logger, StatusLabel);
        return view;
    }
}