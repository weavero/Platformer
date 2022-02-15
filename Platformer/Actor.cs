using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Platformer
{
    abstract class Actor
    {
        protected double vel;
        protected Rect area;

        public Actor(double x, double y)
        {
            area = new Rect(x, y, 20, 20);
            vel = 0;
        }

        abstract public void Jump();

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
