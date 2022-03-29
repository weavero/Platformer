using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Platformer
{
    class Player : Actor
    {
        
        public Player(double x, double y) : base(x, y, Config.playerWidth, Config.playerHeight)
        {
            lives = 2;
        }
    }
}
