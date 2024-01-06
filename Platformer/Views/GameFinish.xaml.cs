using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Platformer.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Platformer.Views
{
    /// <summary>
    /// Interaction logic for GameFinish.xaml
    /// </summary>
    public partial class GameFinish : UserControl
    {    
        //Scaffold-DbContext "Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = |DataDirectory|\PlatformerDatabase.mdf; Integrated Security = True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
        PlatformerContext db;
        public int Points;
        public TimeSpan Time;
        public GameFinish()
        {
            InitializeComponent();
            IsVisibleChanged += GameFinish_IsVisibleChanged;
        }

        private void GameFinish_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                LeaderboardEntry[] leaderboardEntries = new LeaderboardEntry[100];
                using (FileStream fs = File.OpenRead(@"Data/Leaderboard.dat"))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    leaderboardEntries = (LeaderboardEntry[])bf.Deserialize(fs);
                    try
                    {
                        dataGrid.ItemsSource = leaderboardEntries.ToList().OrderByDescending(x => x.Points);
                    }
                    catch
                    {

                    }
                }
                //db = new PlatformerContext();
                //dataGrid.ItemsSource = db.LeaderboardEntries.ToList().OrderByDescending(x => x.Points);
            }
        }

        private void ShowMainMenu(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            if (NameInput.Text.Trim().Length < 3 || NameInput.Text.Any(x => !Char.IsLetterOrDigit(x) && !Char.IsWhiteSpace(x)))
            {
                MessageBox.Show("A megadott név nem tartalmazhat speciális karaktert és minimum 3 karakter hosszúnak kell lennie!", "Hibás bemenet", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                InsertIntoDatabase(Points, Time);
                window.BackToMenu();
                NameInput.Text = "";
            }
        }

        private void InsertIntoDatabase(int Points, TimeSpan Time)
        {
            LeaderboardEntry[] leaderboardEntries = new LeaderboardEntry[200];
            using (FileStream fs = File.OpenRead(@"Data/Leaderboard.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                leaderboardEntries = (LeaderboardEntry[])bf.Deserialize(fs);
                int i = 0;
                while(leaderboardEntries[i] != null)
                {
                    i++;
                }
                leaderboardEntries[i] = new LeaderboardEntry
                {
                    Name = NameInput.Text.Trim(),
                    Points = Points,
                    Time = Time.ToString(@"mm\:ss")
                };

                bf.Serialize(fs, leaderboardEntries);
            }
            //LeaderboardEntry player = new LeaderboardEntry();
            //player.Name = NameInput.Text.Trim();
            //player.Points = Points;
            //player.Time = Time.ToString(@"mm\:ss");
            //db.LeaderboardEntries.Add(player);
            //db.SaveChanges();
        }
    }
}
