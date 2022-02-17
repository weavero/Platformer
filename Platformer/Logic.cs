using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Platformer
{
    class Logic
    {
        Model model;
        public enum Direction { Left, Right }
        public event EventHandler RefreshScreen;

        public bool GoLeft { get; set; }
        public bool GoRight { get; set; }

        static Random r = new Random();

        public Logic(Model model)
        {
            this.model = model;
        }

        public void Move()
        {
            if (GoLeft)
            {
                if (model.player.Area.Left - model.player.Area.Width < 0)
                {
                    model.player.SetX(200);
                }
                else
                {
                    model.player.SetX(-3);
                }
            }
            else if(GoRight)
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
        }

        public void Jump()
        {
            double jumpHeight = model.player.Area.Top - 50;
            while (model.player.Area.Top > jumpHeight)
            {
                model.player.SetY(-1);
            }
        }

        public void GameTick()
        {

        }

        public void CollisionCheck()
        {
            
        }
    }
}
