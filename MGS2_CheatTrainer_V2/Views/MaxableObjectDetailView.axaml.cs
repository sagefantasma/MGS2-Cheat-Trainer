using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Avalonia.Media;
using Microsoft.Extensions.DependencyInjection;

namespace MGS2_CheatTrainer_V2.Views
{
    public partial class MaxableObjectDetailView : UserControl
    {
        // These actions get set by MainWindow when it loads an item,
        // so the detail view doesn't need to know about MGS2 objects directly
        public required Action<bool> OnToggle { get; set; }
        public required Action<ushort> OnSetCurrent { get; set; }
        public required Action<ushort> OnSetMax { get; set; }
        private Constants.IMgs2Object? _object;
        private readonly Mgs2MemoryManager _memoryManager;

        // Set to false for items that don't have a quantity (e.g. Body Armor)
        public bool HasCount
        {
            set
            {
                CurrentUpDown.IsVisible = value;
                MaxUpDown.IsVisible = value;
            }
        }

        public IImage? EntityImage
        {
            get => ObjectImage?.Source;
            set => ObjectImage?.Source = value;
        }

        public string? Mgs2Object
        {
            get;
            set
            {
                field = value;
                GroupBox.Header = $"{Mgs2Object}";
            }
        }

        public MaxableObjectDetailView()
        {
            InitializeComponent();
            _memoryManager = App.Services.GetRequiredService<Mgs2MemoryManager>();
        }

        private void EnabledCheckBox_IsCheckedChanged(object sender, RoutedEventArgs e)
        {
            OnToggle?.Invoke(EnabledCheckBox.IsChecked == true);
        }

        private void CurrentBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSetCurrent?.Invoke((ushort)CurrentUpDown.Value);
            _object ??= Constants.DetermineObject(Name!);
            _memoryManager.UpdateObjectBaseValue(_object, (ushort)CurrentUpDown.Value);
        }

        private void MaxBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSetMax?.Invoke((ushort)MaxUpDown.Value);
            _object ??= Constants.DetermineObject(Name!);
            _memoryManager.UpdateObjectMaxValue(_object, (ushort)MaxUpDown.Value);
        }
    }
}