using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Platformer
{
    public static class Config
    {
        public static readonly Pen penBrush = new Pen(Brushes.Black, 1);
        public static readonly FontFamily Font = new FontFamily(new Uri("pack://application:,,,/"), ".GameResources/Font/#KenVector Future");
        public static Typeface typeface = new Typeface(Font, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal);

        public static readonly ImageBrush groundBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/GameResources/Textures/grass.png")));
        public static readonly ImageBrush wallBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/GameResources/Textures/box.png")));
        public static readonly ImageBrush spikeBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/GameResources/Textures/spikes.png")));
        public static readonly ImageBrush finishBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/GameResources/Textures/flagRed2.png")));
        public static readonly ImageBrush backgroundBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/GameResources/Textures/bg.png")));

        public static readonly ImageBrush CoinBrush = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/GameResources/Textures/coinGold.png", UriKind.RelativeOrAbsolute)));
        public static readonly ImageBrush LifePickup = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/GameResources/Textures/hud_heartFull.png", UriKind.RelativeOrAbsolute)));

        public static ImageBrush PlayerBrush = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/GameResources/Textures/p1_stand.png")));
        public static ImageBrush SmallEnemyBrush = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/GameResources/Textures/slimeGreen.png", UriKind.RelativeOrAbsolute)));
        public static ImageBrush BigEnemyBrush = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/GameResources/Textures/blockerMad.png", UriKind.RelativeOrAbsolute)));
        
        public static ImageBrush FlyingEnemy1 = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/GameResources/Textures/flyFly1.png", UriKind.RelativeOrAbsolute)));

        public static readonly Uri iconUri = new Uri(@"pack://application:,,,/GameResources/Textures/hud_p1.png", UriKind.RelativeOrAbsolute);

        public static readonly List<Brush> PlayerBrushes = new List<Brush> { PlayerBrush };
        public static readonly List<Brush> EnemyBrushes = new List<Brush> { SmallEnemyBrush, BigEnemyBrush, FlyingEnemy1 };

        // absolute path works only
        public static readonly Uri JumpSound = new Uri(@"GameResources/Sounds/Jump.wav", UriKind.RelativeOrAbsolute);
        public static readonly Uri GameMusic = new Uri(@"GameResources/Sounds/happy.mp3", UriKind.RelativeOrAbsolute);
        public static double GameSoundVolume = 0.1;

        public static readonly int UnitWidth = 32;
        public static readonly int UnitHeight = 32;
        public static readonly int PlayerWidth = 30;
        public static readonly int PlayerHeight = 30;
        public static readonly int SmallEnemyWidth = 32;
        public static readonly int SmallEnemyHeight = 32;
        public static readonly int BigEnemyWidth = UnitWidth * 2;
        public static readonly int BigEnemyHeight = UnitHeight * 2;
    }
}