using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace MGS2_CheatTrainer_V2.Views;

public partial class BossDetailView : UserControl
{
    public IImage? EntityImage
    {
        get => ObjectImage?.Source;
        set => ObjectImage?.Source = value;
    }
    
    public BossDetailView()
    {
        InitializeComponent();
    }
}