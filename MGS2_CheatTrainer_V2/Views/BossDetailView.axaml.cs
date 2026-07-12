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

    public bool HasStamina
    {
        get;
        set
        {
            field = value;
            StaminaSlider.IsVisible = value;
            StaminaLabel.IsVisible = value;
            StaminaSlider.IsEnabled = value;
        }
    }

    public bool IsActive
    {
        get;
        set
        {
            field = value;
            if (value)
                ImageDarkener.Opacity = 1;
            else
                ImageDarkener.Opacity = .8;
        }
    }
    
    public BossDetailView()
    {
        InitializeComponent();
    }
}