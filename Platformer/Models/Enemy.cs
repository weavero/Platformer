
namespace Platformer.Models
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

        public void StopMoving()
        {
            Velocity = 0;
        }

        public void Move()
        {
            SetX(Velocity);
        }
    }

    class SmallEnemy : Enemy
    {
        public SmallEnemy(double x, double y) : base(x, y, Config.SmallEnemyWidth, Config.SmallEnemyHeight)
        {
            hitsToKill = 1;
            Velocity = 3;
            Brush = Config.SmallEnemyBrush;
        }
    }

    class BigEnemy : Enemy
    {
        public BigEnemy(double x, double y) : base(x, y, Config.BigEnemyWidth, Config.BigEnemyHeight)
        {
            hitsToKill = 2;
            Velocity = 1;
            Brush = Config.BigEnemyBrush;
        }
    }

    class FlyingEnemy : Enemy
    {
        public FlyingEnemy(double x, double y) : base(x, y, Config.UnitWidth, Config.UnitHeight)
        {
            hitsToKill = 1;
            Velocity = 2;
            Brush = Config.FlyingEnemy1;
        }
    }
}