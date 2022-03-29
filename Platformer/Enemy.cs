using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Platformer
{
    class Enemy : Actor
    {
        protected double dx;
        public double Dx { get { return dx; } }

        protected Enemy(double x, double y, int EnemyWidth, int EnemyHeight) : base(x, y, EnemyWidth, EnemyHeight)
        {
            ;
        }

        public void TurnAround()
        {
            dx = -dx;
        }
    }

    class SmallEnemy : Enemy
    {
        public SmallEnemy(double x, double y) : base(x, y, Config.smallEnemyWidth, Config.smallEnemyHeight)
        {
            lives = 1;
            dx = 3;
        }
    }

    class BigEnemy : Enemy
    {
        public BigEnemy(double x, double y) : base(x, y, Config.bigEnemyWidth, Config.bigEnemyHeight)
        {
            lives = 2;
            dx = 1;
        }
    }
}