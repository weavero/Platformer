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
using Platformer.Models;

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
            Uri iconUri = new Uri("../../../img/hitman.jpg", UriKind.RelativeOrAbsolute);
            Icon = BitmapFrame.Create(iconUri);
            Background = Config.BackgroundImage;
            Width = SystemParameters.FullPrimaryScreenWidth;
            Height = SystemParameters.FullPrimaryScreenHeight;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            PlatformerContext db = new PlatformerContext();
            LeaderboardEntry player = new LeaderboardEntry
            {
                Name = "wwwWwWwwWwW",
                Points = 1000,
                Time = "12:52"
            };
            LeaderboardEntry a2 = new LeaderboardEntry
            {
                Name = "eeee",
                Points = 100,
                Time = "10:25"
            };
            LeaderboardEntry a1 = new LeaderboardEntry
            {
                Name = "gg4ew",
                Points = 1,
                Time = "55:25"
            };

            LeaderboardEntry a = new LeaderboardEntry
            {
                Name = "asda",
                Points = 340,
                Time = "03:25"
            };
            db.LeaderboardEntries.Add(player);
            db.LeaderboardEntries.Add(a);
            db.LeaderboardEntries.Add(a1);
            db.LeaderboardEntries.Add(a2);
            db.SaveChanges();

            
        }

        public void ShowPause()
        {
            PauseGrid.Visibility = Visibility.Visible;
        }

        public void NewGame()
        {
            MainMenuGrid.Visibility = Visibility.Hidden;
            GameGrid.Visibility = Visibility.Visible;
            GameOverGrid.Visibility = Visibility.Hidden;
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
