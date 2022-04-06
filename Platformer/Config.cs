using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Platformer
{
    public static class Config
    {
        public static readonly Pen penBrush = new Pen(Brushes.Black, 1);

        public static readonly ImageBrush groundBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/img/grass.png")));
        public static readonly ImageBrush wallBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/img/box.png")));
        public static readonly ImageBrush finishBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/img/finish.png")));
        public static readonly ImageBrush backgroundBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/img/bg.png")));

        public static readonly ImageBrush CoinBrush = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/img/coinGold.png", UriKind.RelativeOrAbsolute)));
        public static readonly ImageBrush LifePickup = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/img/life_plusone.png", UriKind.RelativeOrAbsolute)));

        public static ImageBrush PlayerBrush = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/img/p1_stand.png")));
        public static ImageBrush SmallEnemyBrush = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/img/enemy1.jpg", UriKind.RelativeOrAbsolute)));
        public static ImageBrush BigEnemyBrush = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/img/enemy2.jpg", UriKind.RelativeOrAbsolute)));
        
        public static ImageBrush FlyingEnemy1 = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/img/flyFly1.png", UriKind.RelativeOrAbsolute)));
        public static ImageBrush FlyingEnemy2 = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/img/flyFly2.png", UriKind.RelativeOrAbsolute)));

        public static ImageBrush PlayerAnimation1 = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/img/player1.png", UriKind.RelativeOrAbsolute)));
        public static ImageBrush PlayerAnimation2 = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/img/player2.png", UriKind.RelativeOrAbsolute)));
        public static ImageBrush PlayerAnimationRev1 = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/img/rev_player1.png", UriKind.RelativeOrAbsolute)));
        public static ImageBrush PlayerAnimationRev2 = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/img/rev_player2.png", UriKind.RelativeOrAbsolute)));

        public static readonly List<ImageBrush> PlayerBrushes = new List<ImageBrush> { PlayerBrush, PlayerAnimation1, PlayerAnimation2, PlayerAnimationRev1, PlayerAnimationRev2 };

        public static readonly Uri JumpSound = new Uri("pack://application:,,,/Sounds/Jump.wav", UriKind.RelativeOrAbsolute);

        public static readonly int UnitWidth = 32;
        public static readonly int UnitHeight = 32;
        public static readonly int PlayerWidth = 32;
        public static readonly int PlayerHeight = 32;
        public static readonly int SmallEnemyWidth = 32;
        public static readonly int SmallEnemyHeight = 32;
        public static readonly int BigEnemyWidth = 64;
        public static readonly int BigEnemyHeight = 64;
    }
}
