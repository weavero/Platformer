using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Threading.Tasks;


namespace Platformer
{
    class Logic
    {
        Model model;
        public enum Direction { Left, Right }
        public event EventHandler RefreshScreen;

        static Random r = new Random();

        public Logic(Model model)
        {
            this.model = model;
        }

        public void Move(Direction d)
        {
            if (d == Direction.Left)
            {
                if (model.player.Area.Left - 10 < 0)
                {
                    model.player.SetX(200);
                }
                else
                {
                    model.player.SetX(-3);
                }
            }
            else
            {
                if (model.player.Area.Right > 500)
                {
                    model.player.SetX(-model.player.Area.X);
                }
                else
                {
                    model.player.SetX(3);
                }
            }
        }
        
        public void MoveAI()
        {
            foreach (Enemy enemy in model.Enemies)
            {
                enemy.SetX(enemy.Dx);
                if (enemy.getX() < 0 || enemy.Area.Right > 400)
                {
                    enemy.Dx = -enemy.Dx;
                }
            }
            //model.enemy.SetX(model.enemy.Dx);

            
        }

        public void Jump()
        {
            double jumpHeight = model.player.Area.Top - 50;
            while (model.player.Area.Top > jumpHeight)
            {
                model.player.SetY(-1);
                RefreshScreen?.Invoke(this, EventArgs.Empty);

            }
        }

        public void CollisionCheck()
        {
            
        }
    }
}
