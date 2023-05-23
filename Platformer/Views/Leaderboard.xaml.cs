using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Platformer.Data;

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
                PlatformerContext db = new PlatformerContext();
                dataGrid.ItemsSource = db.LeaderboardEntries.ToList().OrderByDescending(x => x.Points);
            }
        }

        private void Leaderboard_Loaded(object sender, RoutedEventArgs e)
        {
            window = (MainWindow)Window.GetWindow(this);
        }

        private void ShowMainMenu(object sender, RoutedEventArgs e)
        {
            window.ShowMenu();
        }
    }
}
