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


        double maxJump = 5;
        double jumpHeight;
        int i = 0;

        bool alive = true;

        public Logic(Model model)
        {
            this.model = model;
        }

        public void Move()
        {
            if (GoLeft)
            {
                model.player.SetX(-3);
            }
            else if (GoRight)
            {
                model.player.SetX(3);
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
            if (i < 1)
            {
                jumpHeight = -maxJump;
                i++;
            }

            if (IsJumping)
            {
                model.player.SetY(jumpHeight += 0.1);
            }
            else
            {
                //model.player.SetXY(model.player.Area.X, renderer.Ground.Bounds.Y - model.player.Area.Height - 1); //-1 hogy megint tudjon ugrani
                while (true)
                {
                    break;
                }
                i--;
            }
        }

        public void GameTick()
        {

        }

        public void CollisionCheck(Actor actor, DrawingGroup dg)
        {
            foreach (GeometryDrawing item in dg.Children)
            {
                if (actor.Area.IntersectsWith(item.Bounds) && item.Brush != Config.playerBrush) //&& item.Brush != Config.backgroundBrush)
                {
                    if (item.Brush == Config.finishBrush)
                    {
                        GameOver();
                    }
                    else
                    {
                        if (GoLeft)
                        {
                            if (model.player.Area.Left - 1 < item.Bounds.Right && model.player.Area.Bottom > item.Bounds.Top)
                            {
                                GoLeft = false;
                            }
                        }
                        else if (GoRight)
                        {
                            if (model.player.Area.Right + 1 > item.Bounds.Left && model.player.Area.Bottom > item.Bounds.Top)
                            {
                                GoRight = false;
                            }
                        }
                        if (IsJumping)
                        {
                            IsJumping = false;
                            model.player.SetXY(model.player.Area.X, item.Bounds.Y - model.player.Area.Height);
                        }
                    }
                }
            }
        }

        private void GameOver()
        {
            if (alive)
            {
                model.PalyaBetolt(@"../../../Levels/2.level");
            }
            else
            {
                model.PalyaBetolt(@"../../../Levels/1.level");
            }
        }
    }
}
