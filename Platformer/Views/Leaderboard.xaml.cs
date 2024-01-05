using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Platformer.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Platformer.Views
{
    /// <summary>
    /// Interaction logic for Leaderboard.xaml
    /// </summary>
    public partial class Leaderboard : UserControl
    {
        MainWindow window;
        public Leaderboard()
        {
            InitializeComponent();
            Loaded += Leaderboard_Loaded;
            IsVisibleChanged += Leaderboard_IsVisibleChanged;
        }

        private void Leaderboard_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
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

                //PlatformerContext db = new PlatformerContext();
                //dataGrid.ItemsSource = db.LeaderboardEntries.ToList().OrderByDescending(x => x.Points);
            }
        }

        private void Leaderboard_Loaded(object sender, RoutedEventArgs e)
        {
            window = (MainWindow)Window.GetWindow(this);
        }

        private void ShowMainMenu(object sender, RoutedEventArgs e)
        {
            window.BackToMenu();
        }
    }
}
