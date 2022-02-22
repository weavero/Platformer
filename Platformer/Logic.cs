using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Platformer
{
    class Logic
    {
        Model model;
        public EventHandler RefreshScreen;
        public enum Direction { Left, Right }

        public bool GoLeft { get; set; }
        public bool GoRight { get; set; }

        public bool IsJumping { get; set; }
        double jumpHeight;

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
            if (IsJumping)
            {
                jumpHeight += 0.1;
                model.player.SetY(jumpHeight);
            }
            else
            {
                //model.player.SetXY(model.player.Area.X, renderer.Ground.Bounds.Y - model.player.Area.Height - 1); -1 hogy megint tudjon ugrani
                IsJumping = false;
            }
        }

        public void GameTick()
        {

        }

        public void CollisionCheck(Actor actor, DrawingGroup dg)
        {
            foreach (GeometryDrawing item in dg.Children)
            {
                if (actor.Area.IntersectsWith(item.Bounds))
                {

                }
            }
        }
    }
}
