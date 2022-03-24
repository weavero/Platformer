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
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        MainWindow window;
        public MainMenu()
        {
            InitializeComponent();
            Loaded += MainMenu_Loaded;
            
        }

        private void MainMenu_Loaded(object sender, RoutedEventArgs e)
        {
            window = (MainWindow)Window.GetWindow(this);
            
        }

        private void NewGame(object sender, RoutedEventArgs e)
        {
            window.NewGame();
        }

        private void Leaderboard(object sender, RoutedEventArgs e)
        {
            window.ShowLeaderboard();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            window.Exit();
        }
    }
}
