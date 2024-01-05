using System.Windows;
using System.Windows.Controls;

namespace Platformer.Views
{
    /// <summary>
    /// Interaction logic for PauseMenu.xaml
    /// </summary>
    public partial class PauseMenu : UserControl
    {
        MainWindow window;
        public PauseMenu()
        {
            InitializeComponent();
            Loaded += PauseMenu_Loaded;
        }

        private void PauseMenu_Loaded(object sender, RoutedEventArgs e)
        {
            window = (MainWindow)Window.GetWindow(this);
        }

        private void ResumeGame(object sender, RoutedEventArgs e)
        {
            window.ResumeGame();
        }

        private void ExitToMenu(object sender, RoutedEventArgs e)
        {
            window.ExitToMenu();

        private void ShowOptions(object sender, RoutedEventArgs e)
        {
            window.ShowSettings();
        }
    }
}
