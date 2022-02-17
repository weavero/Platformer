using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Platformer
{
    static class Config
    {
        public static Pen penBrush = new Pen(Brushes.Black, 1);

        public static Brush groundBrush = Brushes.Green;
        public static Brush wallBrush = Brushes.Orange;
        public static Brush enemyBrush = Brushes.Red;
        public static Brush playerBrush = Brushes.Blue;
        public static Brush coinBrush = Brushes.Gold;

        public static double windowWidth = 1280;
        public static double windowHeight = 720;
    }
}
