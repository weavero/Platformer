﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Platformer
{
    class Enemy : Actor
    {
        public double Dx, Dy;

        public Enemy(double x, double y) : base(x, y)
        {
            health = 100;
            Dx = 2;
            Dy = 2;
        }

        public double getX()
        {
            return area.X;
        }
    }
}