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
        public static ImageBrush finishBrush = new ImageBrush(new BitmapImage(new Uri(@"../../../img/finish.png", UriKind.RelativeOrAbsolute)));
        public static Brush backgroundBrush = Brushes.Cyan;

        public static ImageBrush BackgroundImage = new ImageBrush(new BitmapImage(new Uri(@"../../../img/szily.jpg", UriKind.RelativeOrAbsolute)));
        public static ImageBrush playerBrush = new ImageBrush(new BitmapImage(new Uri(@"../../../img/corp2.png", UriKind.RelativeOrAbsolute)));
        public static ImageBrush smallEnemyBrush = new ImageBrush(new BitmapImage(new Uri(@"../../../img/enemy1.jpg", UriKind.RelativeOrAbsolute)));
        public static ImageBrush bigEnemyBrush = new ImageBrush(new BitmapImage(new Uri(@"../../../img/enemy2.jpg", UriKind.RelativeOrAbsolute)));
        public static ImageBrush coinBrush = new ImageBrush(new BitmapImage(new Uri(@"../../../img/coin.png", UriKind.RelativeOrAbsolute)));
        public static ImageBrush lifePickup = new ImageBrush(new BitmapImage(new Uri(@"../../../img/life_plusone.png", UriKind.RelativeOrAbsolute)));

        public static ImageBrush playerAnimation1 = new ImageBrush(new BitmapImage(new Uri(@"../../../img/player1.png", UriKind.RelativeOrAbsolute)));
        public static ImageBrush playerAnimation2 = new ImageBrush(new BitmapImage(new Uri(@"../../../img/player2.png", UriKind.RelativeOrAbsolute)));
        public static ImageBrush playerAnimationRev1 = new ImageBrush(new BitmapImage(new Uri(@"../../../img/rev_player1.png", UriKind.RelativeOrAbsolute)));
        public static ImageBrush playerAnimationRev2 = new ImageBrush(new BitmapImage(new Uri(@"../../../img/rev_player2.png", UriKind.RelativeOrAbsolute)));

        public static List<ImageBrush> playerBrushes = new List<ImageBrush> {playerBrush, playerAnimation1, playerAnimation2, playerAnimationRev1, playerAnimationRev2 };

        public static double windowWidth = 1280;
        public static double windowHeight = 720;
        public static int unitWidth = 32;
        public static int unitHeight = 32;
        public static int playerWidth = 30;
        public static int playerHeight = 50;
        public static int smallEnemyWidth = 30;
        public static int smallEnemyHeight = 40;
        public static int bigEnemyWidth = 60;
        public static int bigEnemyHeight = 60;
        public static int coinSize = 10;
    }
}
