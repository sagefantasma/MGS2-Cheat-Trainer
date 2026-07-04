using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace MGS2_CheatTrainer_V2.Views
{
    public partial class MaxableObjectDetailView : UserControl
    {
        // These actions get set by MainWindow when it loads an item,
        // so the detail view doesn't need to know about MGS2 objects directly
        public Action<bool> OnToggle { get; set; }
        public Action<ushort> OnSetCurrent { get; set; }
        public Action<ushort> OnSetMax { get; set; }

        // Set to false for items that don't have a quantity (e.g. Body Armor)
        public bool HasCount
        {
            set
            {
                CurrentUpDown.IsVisible = value;
                MaxUpDown.IsVisible = value;
            }
        }

        public MaxableObjectDetailView()
        {
            InitializeComponent();
        }

        private void EnabledCheckBox_IsCheckedChanged(object sender, RoutedEventArgs e)
        {
            OnToggle?.Invoke(EnabledCheckBox.IsChecked == true);
        }

        private void CurrentBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSetCurrent?.Invoke((ushort)CurrentUpDown.Value);
        }

        private void MaxBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSetMax?.Invoke((ushort)MaxUpDown.Value);
        }
    }
}