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
        public static readonly FontFamily Font = new FontFamily(new Uri("pack://application:,,,/"), "/Textures/Font/#Pericles Light");

        public static readonly ImageBrush groundBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Textures/Game/grass.png")));
        public static readonly ImageBrush wallBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Textures/Game/box.png")));
        public static readonly ImageBrush spikeBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Textures/Game/spikes.png")));
        public static readonly ImageBrush finishBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Textures/Game/flagRed2.png")));
        public static readonly ImageBrush backgroundBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Textures/Game/bg.png")));

        public static readonly ImageBrush CoinBrush = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Textures/Game/coinGold.png", UriKind.RelativeOrAbsolute)));
        public static readonly ImageBrush LifePickup = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Textures/Game/hud_heartFull.png", UriKind.RelativeOrAbsolute)));

        public static ImageBrush PlayerBrush = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Textures/Game/p1_stand.png")));
        public static ImageBrush SmallEnemyBrush = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Textures/Game/slimeGreen.png", UriKind.RelativeOrAbsolute)));
        public static ImageBrush BigEnemyBrush = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Textures/Game/blockerMad.png", UriKind.RelativeOrAbsolute)));
        
        public static ImageBrush FlyingEnemy1 = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Textures/Game/flyFly1.png", UriKind.RelativeOrAbsolute)));
        public static ImageBrush FlyingEnemy2 = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Textures/Game/flyFly2.png", UriKind.RelativeOrAbsolute)));

        public static readonly Uri iconUri = new Uri(@"pack://application:,,,/Textures/Game/hud_p1.png", UriKind.RelativeOrAbsolute);

        public static readonly List<Brush> PlayerBrushes = new List<Brush> { PlayerBrush };
        public static readonly List<Brush> EnemyBrushes = new List<Brush> { SmallEnemyBrush, BigEnemyBrush, FlyingEnemy1 };

        public static readonly Uri JumpSound = new Uri("pack://application:,,,/Sounds/Jump.wav", UriKind.RelativeOrAbsolute);

        public static readonly int UnitWidth = 32;
        public static readonly int UnitHeight = 32;
        public static readonly int PlayerWidth = 32;
        public static readonly int PlayerHeight = 32;
        public static readonly int SmallEnemyWidth = 32;
        public static readonly int SmallEnemyHeight = 32;
        public static readonly int BigEnemyWidth = UnitWidth * 2;
        public static readonly int BigEnemyHeight = UnitHeight * 2;
    }
}