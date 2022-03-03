using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Platformer
{
    abstract class Actor
    {
        protected int health;
        protected double vel;
        protected Rect area;

        public Rect Area { get { return area; } }

        public Actor(double x, double y)
        {
            area = new Rect(x, y, Config.playerWidth, Config.playerHeight);
            vel = 0;
        }

        public void SetX(double x)
        {
            area.X += x;
        }

        public void SetY(double y)
        {
            area.Y += y;
        }

        public void SetXY(double x, double y)
        {
            area.X = x;
            area.Y = y;
        }
    }
}
