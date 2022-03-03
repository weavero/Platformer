using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Platformer
{
    static class Config
    {
        public static Pen penBrush = new Pen(Brushes.Black, 1);

        public static Brush groundBrush = Brushes.Green;
        public static Brush wallBrush = Brushes.Orange;
        public static Brush enemyBrush = Brushes.Red;
        public static Brush coinBrush = Brushes.Gold;
        public static Brush finishBrush = Brushes.Purple;
        public static Brush backgroundBrush = Brushes.Cyan;

        //public static ImageBrush CoinImage;
        //public static ImageBrush BackgroundImage;
        public static ImageBrush playerBrush = new ImageBrush(new BitmapImage(new Uri(@"../../../img/corp2.png", UriKind.RelativeOrAbsolute)));

        public static double windowWidth = 1280;
        public static double windowHeight = 720;
        public static int unitWidth = 30;
        public static int unitHeight = 70;
        public static int playerWidth = 50;
        public static int playerHeight = 100;

        
    }
}
