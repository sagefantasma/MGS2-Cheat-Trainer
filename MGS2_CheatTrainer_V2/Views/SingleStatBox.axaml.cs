using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace MGS2_CheatTrainer_V2.Views;

public partial class SingleStatBox : UserControl
{
    private readonly Mgs2MemoryManager _memoryManager;
    
    public string GroupBoxName
    {
        set
        {
            this.GroupBox.Header = value;
            this.SetButton.Content = $"Set {value}";
        }
    }

    public bool Editable
    {
        set
        {
            this.SetButton.IsEnabled = value;
        }
    }

    public SingleStatBox()
    {
        InitializeComponent();
        _memoryManager = App.Services.GetRequiredService<Mgs2MemoryManager>();
    }
}