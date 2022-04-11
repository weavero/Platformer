using System.Windows;
using System.Windows.Controls;

namespace Platformer.Views
{
    /// <summary>
    /// Interaction logic for GameOver.xaml
    /// </summary>
    public partial class GameOver : UserControl
    {
        MainWindow window;
        public GameOver()
        {
            InitializeComponent();
            Loaded += GameOver_Loaded;
        }

        private void GameOver_Loaded(object sender, RoutedEventArgs e)
        {
            window = (MainWindow)Window.GetWindow(this);
        }

        private void RestartGame(object sender, RoutedEventArgs e)
        {
            window.NewGame();
        }

        private void ToMainMenu(object sender, RoutedEventArgs e)
        {
            window.ExitToMenu();
        }
    }
}
