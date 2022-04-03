using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Platformer
{
    class Enemy : Actor
    {
        protected Enemy(double x, double y, int EnemyWidth, int EnemyHeight) : base(x, y, EnemyWidth, EnemyHeight)
        {
            ;
        }

        public void TurnAround()
        {
            Velocity = -Velocity;
        }
    }

    class SmallEnemy : Enemy
    {
        public SmallEnemy(double x, double y) : base(x, y, Config.smallEnemyWidth, Config.smallEnemyHeight)
        {
            lives = 1;
            Velocity = 3;
            Brush = Config.smallEnemyBrush;
        }
    }

    class BigEnemy : Enemy
    {
        public BigEnemy(double x, double y) : base(x, y, Config.bigEnemyWidth, Config.bigEnemyHeight)
        {
            lives = 2;
            Velocity = 1;
            Brush = Config.bigEnemyBrush;
        }
    }
}