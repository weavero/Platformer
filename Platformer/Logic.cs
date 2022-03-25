using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Platformer
{
    class Logic
    {
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
            Animation();
        }

        Point oldPlayerBottomLeft, oldPlayerTopRight;
        Rect oldPlayerPos;
        List<Enemy> oldEnemies;
        public void CollisionCheck(Actor actor, DrawingGroup dg)
        {
            bool collision = false;
            foreach (GeometryDrawing item in dg.Children)
            {
                if (actor.Area.IntersectsWith(item.Bounds) && actor.Area.Height != item.Bounds.Height)
                {
                    if (actor is Player)
                    {
                        collision = true;
                        if (item.Brush == Config.bigEnemyBrush || item.Brush == Config.smallEnemyBrush)
                        {
                            if (oldPlayerBottomLeft.Y < item.Bounds.Top)
                            {
                                foreach (Enemy enemy in model.Enemies)
                                {
                                    if (enemy.Area.X - enemy.Dx == item.Bounds.X && enemy.Area.Y == item.Bounds.Y)
                                    {
                                        enemy.SetXY(-1000, -1000);
                                    }
                                }
                            }
                            else
                            {
                                actor.MinusHealth();
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
                                if (actor.Area.Left < item.Bounds.Right && oldPlayerBottomLeft.Y > item.Bounds.Top)
                                {
                                    GoLeft = false;
                                    actor.SetXY(item.Bounds.Right, actor.Area.Y);
                                }
                            }
                            else if (GoRight)
                            {
                                if (actor.Area.Right > item.Bounds.Left && actor.Area.Bottom > item.Bounds.Top)
                                {
                                    GoRight = false;
                                    actor.SetXY(item.Bounds.Left - actor.Area.Width, actor.Area.Y);
                                }
                            }
                            if (IsJumping)
                            {
                                if (actor.Area.Top > item.Bounds.Top)
                                {
                                    actor.SetXY(actor.Area.X, item.Bounds.Bottom);
                                    IsFalling = true;
                                    IsJumping = false;
                                }
                                else if (oldPlayerBottomLeft.Y > item.Bounds.Y)
                                {
                                    actor.SetXY(actor.Area.X, item.Bounds.Top - actor.Area.Height);
                                    IsJumping = false;
                                }
                            }
                            else if (IsFalling && actor.Area.Right > item.Bounds.Left && actor.Area.Left < item.Bounds.Right && oldPlayerBottomLeft.Y < item.Bounds.Top)
                            {
                                IsFalling = false;
                                actor.SetXY(actor.Area.X, item.Bounds.Top - actor.Area.Height);
                            }
                        }
                    }
                    else if (actor is Enemy)
                    {
                        if (item.Bounds.Height != Config.playerHeight)
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
            }
            if(actor is Player)
            {
                oldPlayerBottomLeft = new Point(actor.Area.Left, actor.Area.Bottom);
                oldPlayerTopRight = new Point(actor.Area.Right, actor.Area.Top);

                if (!collision && !IsJumping)
                {
                    IsFalling = true;
                }
            }
            else if (actor is Enemy)
            {
                
            }
        }

        bool isGameOver = false;
        int playerLives = 3;
        public int PlayerLives { get { return playerLives; } }
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
                // Respawn után folyamatosan esne
                oldPlayerBottomLeft = new Point();
                oldPlayerTopRight = new Point();

                model.ReloadLevel();
                playerLives--;
                return true;
            }

            return false;
        }

        private void Animation()
        {
            PlayerAnimation();
            EnemyAnimation();
        }

        int playerAnimationTick = 1;
        private void PlayerAnimation()
        {
            if (GoRight)
            {
                if (playerAnimationTick % 8 == 0)
                {
                    Config.playerBrush = new ImageBrush(new BitmapImage(new Uri(@"../../../img/player1.png", UriKind.RelativeOrAbsolute)));
                }
                else if(playerAnimationTick % 8 == 4)
                {
                    Config.playerBrush = new ImageBrush(new BitmapImage(new Uri(@"../../../img/player2.png", UriKind.RelativeOrAbsolute)));
                }
                playerAnimationTick++;
            }
            else if (GoLeft)
            {
                if (playerAnimationTick % 8 == 0)
                {
                    Config.playerBrush = new ImageBrush(new BitmapImage(new Uri(@"../../../img/rev_player1.png", UriKind.RelativeOrAbsolute)));
                }
                else if(playerAnimationTick % 8 == 4)
                {
                    Config.playerBrush = new ImageBrush(new BitmapImage(new Uri(@"../../../img/rev_player2.png", UriKind.RelativeOrAbsolute)));
                }
                playerAnimationTick++;
            }
            else
            {
                playerAnimationTick = 1;
                Config.playerBrush = new ImageBrush(new BitmapImage(new Uri(@"../../../img/corp2.png", UriKind.RelativeOrAbsolute)));
            }
        }

        int enemyAnimationTick = 1;
        private void EnemyAnimation()
        {

        }
    }
}
