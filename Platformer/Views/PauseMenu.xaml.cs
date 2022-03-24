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
using Platformer;

namespace Platformer.Views
{
    /// <summary>
    /// Interaction logic for PauseMenu.xaml
    /// </summary>
    public partial class PauseMenu : UserControl
    {
        MainWindow window;
        public PauseMenu()
        {
            InitializeComponent();
            Loaded += PauseMenu_Loaded;
        }

        private void PauseMenu_Loaded(object sender, RoutedEventArgs e)
        {
            window = (MainWindow)Window.GetWindow(this);
        }

        private void ResumeGame(object sender, RoutedEventArgs e)
        {
            window.ResumeGame();
        }

        private void ExitToMenu(object sender, RoutedEventArgs e)
        {
            window.ExitToMenu();
        }
    }
}
