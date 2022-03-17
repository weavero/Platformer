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

namespace Platformer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            Main.Content = new pControl();
            //Main.Content = new UserControl1();
        }

        pControl pausedGame;
        public void ShowGameOver()
        {
            if (!(Main.Content is UserControl1)) { pausedGame = (pControl)Main.Content; }
            Main.Content = new ContentControl();
            Main.Content = new UserControl1();
        }

        public void ShowGame()
        {
            Main.Content = new ContentControl();
            Main.Content = pausedGame;
        }
    }
}
