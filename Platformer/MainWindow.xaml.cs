using System.Windows;
using System.Windows.Media.Imaging;
using Platformer.Views;
using Platformer.Models;
using Platformer.Data;

namespace Platformer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Loaded += MainWindow_Loaded;
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Icon = BitmapFrame.Create(Config.iconUri);
            Background = Config.backgroundBrush;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public void ShowPause()
        {
            PauseGrid.Visibility = Visibility.Visible;
        }

        public void NewGame()
        {
            MainMenuGrid.Visibility = Visibility.Hidden;
            GameCompleteGrid.Visibility = Visibility.Hidden;
            GameOverGrid.Visibility = Visibility.Hidden;
            GameGrid.Visibility = Visibility.Visible;
            Game.NewGame();
            Game.TimerStart();
        }

        internal void ExitToMenu()
        {
            GameGrid.Visibility = Visibility.Hidden;
            PauseGrid.Visibility = Visibility.Hidden;
            GameOverGrid.Visibility = Visibility.Hidden;
            GameCompleteGrid.Visibility = Visibility.Hidden;
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

        public void ShowGameComplete(GameCompleteArgs e)
        {
            GameGrid.Visibility = Visibility.Hidden;
            GameCompleteGrid.Visibility = Visibility.Visible;
            GameFinish end = (GameFinish)GameCompleteGrid.Children[0];
            end.Points = e.Points;
            end.Time = e.Time;
        }

        public void Exit()
        {
            this.Close();
        }
    }
}
