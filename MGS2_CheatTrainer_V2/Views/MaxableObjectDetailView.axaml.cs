using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Avalonia.Media;
using MGS2_CheatTrainer_V2.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MGS2_CheatTrainer_V2.Views
{
    public partial class MaxableObjectDetailView : UserControl
    {
        //TODO: is it better to have two separate textboxes for current/max, or just one textbox and two buttons like the MGS3 trainer?
        private Constants.IMgs2Object? _object;
        private readonly Mgs2MemoryManager _memoryManager;
        private bool _active = false;
        public event EventHandler<string>? ValueChanged;

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
        
        public void UpdateObjectEnabledState()
        {
            _object ??= Constants.DetermineObject(Name!);
            ushort value = _memoryManager.GetObjectValue(_object!);
            if (_object is not Constants.MaxableWeapon)
                EnabledCheckBox.IsChecked = value > 0;
            else
                EnabledCheckBox.IsChecked = value != 0xFFFF;
            _active = true;
        }

        private void EnabledCheckBox_IsCheckedChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!_active) return;
                _object ??= Constants.DetermineObject(Name!);
                _memoryManager.ToggleObject(_object!, (bool)EnabledCheckBox.IsChecked!);
                string enableState = EnabledCheckBox.IsChecked == true ? "enabled" : "disabled";
                ChangeStat($"{_object?.Name} is {enableState}");
            }
            catch (Exception ex)
            {
                string errorBrief = $"Failed to enable {Name!}";
                Logging.Logger?.Error($"{errorBrief}: {ex.Message}");
                ChangeStat(errorBrief);
            }
        }

        private void CurrentBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (EnabledCheckBox.IsChecked == false)
                {
                    EnabledCheckBox.IsChecked = true;
                }

                _object ??= Constants.DetermineObject(Name!);
                _memoryManager.UpdateObjectBaseValue(_object, (ushort)CurrentUpDown.Value);
                ChangeStat($"Updated {_object.Name} Current Count to: {CurrentUpDown.Value}");
            }
            catch (Exception ex)
            {
                string errorBrief = $"Failed to set current count for {Name!}";
                Logging.Logger?.Error($"{errorBrief}: {ex.Message}");
                ChangeStat(errorBrief);
            }
        }

        private void MaxBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _object ??= Constants.DetermineObject(Name!);
                _memoryManager.UpdateObjectMaxValue(_object, (ushort)MaxUpDown.Value);
                ChangeStat($"Updated {_object.Name} Max Count to: {CurrentUpDown.Value}");
            }
            catch (Exception ex)
            {
                string errorBrief = $"Failed to set max count for {Name!}";
                Logging.Logger?.Error($"{errorBrief}: {ex.Message}");
                ChangeStat(errorBrief);
            }
        }
        
        private void ChangeStat(string message)
        {
            ValueChanged?.Invoke(null, message);
        }
    }
}