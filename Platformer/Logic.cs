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
       
        public enum Direction { Left, Right }

        public bool GoLeft { get; set; }
        public bool GoRight { get; set; }
        public bool IsJumping { get; set; }
        public bool IsFalling { get; set; }


        Model model;

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
            }
            
        }

        int i = 0;
        double jumpHeight;
        double maxJump = 5;
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
                i--;
            }
        }

        int j = 0;
        double fallSpeed;
        public void Falling()
        {
            if (j < 1)
            {
                fallSpeed = 0.1;
                j++;
            }

            if (IsFalling)
            {
                if (fallSpeed < 10)
                {
                    fallSpeed += 0.1;
                }
                model.player.SetY(fallSpeed);

            }
            else
            {
                j--;
            }
        }

        public void GameTick()
        {
            Move();
            Jump();
            MoveAI();
            Falling();
        }

        public void CollisionCheck(Actor actor, DrawingGroup dg)
        {
            List<int> coinIndexes = new List<int>();
            bool collision = false;
            foreach (GeometryDrawing item in dg.Children)
            {
                if (actor.Area.IntersectsWith(item.Bounds) && item.Bounds.Height != actor.Area.Height)
                {
                    if (actor is Player)
                    {
                        collision = true;
                        if (item.Brush == Config.finishBrush)
                        {
                            isGameOver = true;
                        }
                        else if (item.Brush == Config.coinBrush)
                        {
                            model.CoinPickedup();
                            coinIndexes.Add(dg.Children.IndexOf(item));
                        }
                        else
                        {
                            if (GoLeft)
                            {
                                if (actor.Area.Left < item.Bounds.Right && actor.Area.Bottom > item.Bounds.Top)
                                {
                                    GoLeft = false;
                                }
                            }
                            else if (GoRight)
                            {
                                if (actor.Area.Right > item.Bounds.Left && actor.Area.Bottom > item.Bounds.Top)
                                {
                                    GoRight = false;
                                }
                            }
                            if (IsJumping)
                            {
                                IsJumping = false;
                            }
                            if (IsFalling && collision && item.Bounds.Bottom > actor.Area.Bottom)
                            {
                                IsFalling = false;
                            }
                        }
                    }
                    else if (actor is Enemy)
                    {
                        if (actor.Area.Left < item.Bounds.Right && actor.Area.Bottom > item.Bounds.Top)
                        {
                            (actor as Enemy).TurnAround();
                        }
                        else if (actor.Area.Right> item.Bounds.Left && actor.Area.Bottom > item.Bounds.Top)
                        {
                            (actor as Enemy).TurnAround();
                        }
                    }
                }
            }
            if (!collision && !IsJumping && actor is Player)
            {
                IsFalling = true;
            }
        }

        bool alive = true;
        bool isGameOver = false;
        public bool GameOver()
        {
            if (alive && isGameOver)
            {
                model.LoadLevel(@"../../../Levels/2.level");
                isGameOver = false;
                return true;
            }
            else if (!alive)
            {
                model.LoadLevel(@"../../../Levels/1.level");
                return false;
            }

            return false;
        }
    }
}
