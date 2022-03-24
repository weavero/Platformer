using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Platformer.Views;

namespace Platformer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //.Content NE!
        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Uri iconUri = new Uri("../../../img/hitman.jpg", UriKind.RelativeOrAbsolute);
            Icon = BitmapFrame.Create(iconUri);
            InitializeComponent();
        }

        public void ShowPause()
        {
            PauseGrid.Visibility = Visibility.Visible;
        }

        public void NewGame()
        {
            MainMenuGrid.Visibility = Visibility.Hidden;
            GameGrid.Visibility = Visibility.Visible;
            Game.Visibility = Visibility.Visible;
            Game.TimerStart();
        }

        internal void ExitToMenu()
        {
            PauseGrid.Visibility = Visibility.Hidden;
            GameOverGrid.Visibility = Visibility.Hidden;
            MainMenuGrid.Visibility = Visibility.Visible;
        }

        public void ResumeGame()
        {
            PauseGrid.Visibility = Visibility.Hidden;
            Game.TimerStart();
        }

        public void ShowMenu()
        {
            LeaderboardGrid.Visibility = Visibility.Hidden;
            MainMenuGrid.Visibility = Visibility.Visible;
        }

        public void ShowGameOver()
        {
            GameOverGrid.Visibility = Visibility.Visible;
        }

        public void ShowLeaderboard()
        {
            MainMenuGrid.Visibility = Visibility.Hidden;
            LeaderboardGrid.Visibility = Visibility.Visible;
        }

        public void Exit()
        {
            this.Close();
        }
    }
}
