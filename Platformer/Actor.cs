using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Platformer
{
    abstract class Actor
    {
        protected int health;
        public int Health { get { return health; } }

        protected double vel;
        protected Rect area;

        public Rect Area { get { return area; } }

        protected Actor(double x, double y, int ActorWidth, int ActorHeight)
        {
            area = new Rect(x, y, ActorWidth, ActorHeight);
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

        public void TakenDamage()
        {
            health -= 10;
        }
    }
}
