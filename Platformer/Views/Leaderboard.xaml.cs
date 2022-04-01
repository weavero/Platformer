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
using System.Linq;
using Platformer;
using Platformer.Models;

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
