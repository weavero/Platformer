using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Platformer
{
    abstract class Actor
    {
        protected Rect area;

        public Rect Area { get { return area; } }

        //Life = Number of hits to kill
        protected int lives;
        public int Lives { get { return lives; } }

        protected double vel;

        public bool GoLeft { get; set; }
        public bool GoRight { get; set; }
        public bool IsJumping { get; set; }
        public bool IsFalling { get; set; }

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

        public void MinusHealth()
        {
            lives--;
        }

        public void PlusHealth()
        {
            lives++;
        }
    }
}
