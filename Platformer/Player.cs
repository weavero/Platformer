using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Platformer
{
    class Player : Actor
    {
        public Rect Area
        {
            get { return area; }
        }

        public Player(double x, double y) : base(x, y)
        {
            ;
        }

        public override void Jump()
        {
            
        }
    }
}
