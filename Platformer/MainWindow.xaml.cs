using System.Windows;
using System.Windows.Media.Imaging;
using Platformer.Views;
using Platformer.Models;
using Platformer.Data;
using System.Windows.Controls;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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

            string path = @"Data/Leaderboard.dat";
            if (!File.Exists(path))
            {
                FileStream fs = File.Create(path);
                fs.Close();
                fs.Dispose();
                FileStream stream = File.OpenWrite(path);
                BinaryFormatter bf = new BinaryFormatter();
                LeaderboardEntry[] leaderboardEntries = new LeaderboardEntry[20];
                for (int i = 0; i < leaderboardEntries.Length; i++)
                {
                    LeaderboardEntry a = new LeaderboardEntry
                    {
                        Name = "0",
                        Points = 0,
                        Time = "55:55"
                    };
                    leaderboardEntries[i] = a;
                }
                bf.Serialize(stream, leaderboardEntries);
                stream.Dispose();
            }
            
            //PlatformerContext db = new PlatformerContext();
            //LeaderboardEntry player = new LeaderboardEntry
            //{
            //    Name = "Elek",
            //    Points = 700,
            //    Time = "11:03"
            //};
            //LeaderboardEntry a2 = new LeaderboardEntry
            //{
            //    Name = "egyvalaki",
            //    Points = 1305,
            //    Time = "02:25"
            //};
            //LeaderboardEntry a1 = new LeaderboardEntry
            //{
            //    Name = "Luca",
            //    Points = 400,
            //    Time = "02:55"
            //};
            //LeaderboardEntry a = new LeaderboardEntry
            //{
            //    Name = "Ádám",
            //    Points = 570,
            //    Time = "03:25"
            //};
            //db.LeaderboardEntries.Add(player);
            //db.LeaderboardEntries.Add(a);
            //db.LeaderboardEntries.Add(a1);
            //db.LeaderboardEntries.Add(a2);
            //db.SaveChanges();
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

        public void ShowSettings()
        {
            SettingsGrid.Visibility = Visibility.Visible;

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
