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
        public SmallEnemy(double x, double y) : base(x, y, Config.SmallEnemyWidth, Config.SmallEnemyHeight)
        {
            lives = 1;
            Velocity = 3;
            Brush = Config.SmallEnemyBrush;
        }
    }

    class BigEnemy : Enemy
    {
        public BigEnemy(double x, double y) : base(x, y, Config.BigEnemyWidth, Config.BigEnemyHeight)
        {
            lives = 2;
            Velocity = 1;
            Brush = Config.BigEnemyBrush;
        }
    }

    class FlyingEnemy : Enemy
    {
        public FlyingEnemy(double x, double y) : base(x, y, Config.UnitWidth, Config.UnitHeight)
        {
            lives = 1;
            Velocity = 2;
            Brush = Config.FlyingEnemy1;
        }
    }
}