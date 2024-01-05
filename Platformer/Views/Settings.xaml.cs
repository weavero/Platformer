using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Platformer.Views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        MainWindow window;
        public Settings()
        {
            Loaded += Settings_Loaded;
            InitializeComponent();
        }

        private void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            VolumeSlider.Value = Config.GameSoundVolume;
            window = (MainWindow)Window.GetWindow(this);
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double value = Math.Round(VolumeSlider.Value, 2);
            Config.GameSoundVolume = VolumeSlider.Value;
            VolumeLabel.Content = (value * 10).ToString("0");
        }

        private void HideSettings(object sender, RoutedEventArgs e)
        {
            window.HideSettings();
        }
    }
}
