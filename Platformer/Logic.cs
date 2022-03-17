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
            Falling();
            MoveAI();
        }

        public void CollisionCheck(Actor actor, DrawingGroup dg)
        {
            bool collision = false;
            foreach (GeometryDrawing item in dg.Children)
            {
                if (actor.Area.IntersectsWith(item.Bounds) && item.Bounds.Height != actor.Area.Height)
                {
                    if (actor is Player)
                    {
                        collision = true;
                        if (item.Brush == Config.bigEnemyBrush || item.Brush == Config.smallEnemyBrush)
                        {
                            if ((IsJumping || IsFalling) && actor.Area.Top < item.Bounds.Top)
                            {
                                
                            }
                            else
                            {
                                actor.TakenDamage();
                            }
                        }
                        if (item.Brush == Config.finishBrush)
                        {
                            isGameOver = true;
                        }
                        else if (item.Brush == Config.coinBrush)
                        {
                            model.CoinPickedup();
                            model.SetPickupIndex(dg.Children.IndexOf(item));
                            collision = false;
                        }
                        else
                        {
                            if (GoLeft)
                            {
                                if (actor.Area.Left < item.Bounds.Right && actor.Area.Bottom > item.Bounds.Top)
                                {
                                    if (IsJumping || IsFalling && actor.Area.Bottom > item.Bounds.Top)
                                    {
                                        
                                    }
                                    else
                                    {
                                        GoLeft = false;
                                        actor.SetXY(item.Bounds.Right, actor.Area.Y);
                                    }
                                }
                            }
                            else if (GoRight)
                            {
                                if (actor.Area.Right > item.Bounds.Left && actor.Area.Bottom > item.Bounds.Top)
                                {
                                    if (IsJumping || IsFalling)
                                    {
                                        
                                    }
                                    else
                                    {
                                        GoRight = false;
                                        actor.SetXY(item.Bounds.Left - actor.Area.Width, actor.Area.Y);
                                    }
                                }
                            }
                            if (IsJumping)
                            {
                                // ha az actor az item alatt van
                                // > item.bounds.bottom-mal bele tud menni az itembe valamiért?
                                if (actor.Area.Top > item.Bounds.Top)
                                {
                                    actor.SetXY(actor.Area.X, item.Bounds.Bottom);
                                    IsFalling = true;
                                    IsJumping = false;
                                }
                                else
                                {
                                    //actor.SetXY(actor.Area.X, item.Bounds.Top - actor.Area.Height);
                                    IsFalling = true;
                                    IsJumping = false;
                                }
                            }
                            else if (IsFalling && actor.Area.Right > item.Bounds.Left && actor.Area.Left < item.Bounds.Right)
                            {
                                if (actor.Area.Top < item.Bounds.Top)
                                {
                                    IsFalling = false;
                                    actor.SetXY(actor.Area.X, item.Bounds.Top - actor.Area.Height);
                                }
                                else { IsFalling = false; }
                            }
                        }
                    }
                    else if (actor is Enemy)
                    {
                        if (actor.Area.Left < item.Bounds.Right && actor.Area.Bottom > item.Bounds.Top)
                        {
                            (actor as Enemy).TurnAround();
                            actor.SetX(-1);
                        }
                        else if (actor.Area.Right > item.Bounds.Left && actor.Area.Bottom > item.Bounds.Top)
                        {
                            (actor as Enemy).TurnAround();
                            actor.SetX(1);
                        }
                    }
                }
            }
            if (!collision && !IsJumping && actor is Player)
            {
                IsFalling = true;
            }
        }

        bool isGameOver = false;
        public bool IsGameOver()
        {
            if (model.player.Health > 0 && isGameOver)
            {
                model.NextLevel();
                isGameOver = false;
                return true;
            }
            else if (model.player.Health < 0 || model.player.Area.Y > 1000)
            {
                model.ReloadLevel();
                return true;
            }

            return false;
        }
    }
}
