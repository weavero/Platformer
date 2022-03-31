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
using Platformer.Models;
using System.Linq;

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
                db = new PlatformerContext();
                dataGrid.ItemsSource = db.LeaderboardEntries.ToList();
            }
        }

        private void ShowMainMenu(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            if (NameInput.Text.Trim().Length < 3 || !NameInput.Text.Any(x => Char.IsLetterOrDigit(x)))
            {
                Alert.Visibility = Visibility.Visible;
            }
            else
            {
                InsertIntoDatabase(Points, Time);
                window.ExitToMenu();
                Alert.Visibility = Visibility.Hidden;
            }
        }

        private void InsertIntoDatabase(int Points, TimeSpan Time)
        {
            LeaderboardEntry player = new LeaderboardEntry();
            player.Name = NameInput.Text.Trim();
            player.Points = Points;
            player.Time = Time.ToString(@"mm\:ss");
            db.LeaderboardEntries.Add(player);
            db.SaveChanges();
        }
    }
}
