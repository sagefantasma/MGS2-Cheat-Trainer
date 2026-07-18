using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MGS2_CheatTrainer_V2.Views;

public partial class StringDetailView : UserControl
{
    public Mgs2Strings.Mgs2String GameString
    {
        get;
        set
        {
            field = value;
            Content = value.Tag;
            Name = value.Tag;
        }
    }
    
    public StringDetailView(Mgs2Strings.Mgs2String gameString)
    {
        InitializeComponent();
        GameString = gameString;
    }
}