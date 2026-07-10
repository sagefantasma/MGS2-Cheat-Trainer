using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MGS2_CheatTrainer_V2.Views;

public partial class SingleStatBox : UserControl
{
    public string GroupBoxName
    {
        set
        {
            this.GroupBox.Header = value;
            this.SetButton.Content = $"Set {value}";
        }
    }

    public SingleStatBox()
    {
        InitializeComponent();
    }
}