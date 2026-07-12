using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace MGS2_CheatTrainer_V2.Views;

public partial class CheatsDetailView : UserControl
{
    private readonly Mgs2MemoryManager _memoryManager;
    public CheatsDetailView()
    {
        InitializeComponent();
        _memoryManager = App.Services.GetRequiredService<Mgs2MemoryManager>();
    }
}