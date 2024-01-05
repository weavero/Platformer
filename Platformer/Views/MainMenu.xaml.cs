using System.Windows;
using System.Windows.Controls;

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

        private void Settings(object sender, RoutedEventArgs e)
        {
            window.ShowSettings();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {

        }
    }
}
