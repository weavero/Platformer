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
            health = 80;
            dx = 1;
        }
    }

    class BigEnemy : Enemy
    {
        public BigEnemy(double x, double y) : base(x, y, Config.bigEnemyWidth, Config.bigEnemyHeight)
        {
            health = 250;
            dx = 1;
        }
    }
}