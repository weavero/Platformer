using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace Platformer
{
    class Logic
    {
        public bool GoLeft { get; set; }
        public bool GoRight { get; set; }
        public bool IsJumping { get; set; }
        public bool IsFalling { get; set; }

        Model model;

        public EventHandler OnGameComplete;

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

        Rect oldPlayerPos;
        List<Enemy> oldEnemies;
        DrawingGroup drawing;
        public void CollisionCheck(Actor actor, DrawingGroup dg)
        {
            drawing = dg;
            int dgRemovableIndex = -1;
            int enemyRemovableIndex = -1;
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
                            if (oldPlayerPos.Bottom < item.Bounds.Top)
                            {
                                foreach (Enemy enemy in model.Enemies)
                                {
                                    if (enemy.Area.X - enemy.Dx == item.Bounds.X && enemy.Area.Y == item.Bounds.Y)
                                    {
                                        enemy.SetXY(-1000, -1000);
                                        dgRemovableIndex = dg.Children.IndexOf(item);
                                        enemyRemovableIndex = model.Enemies.IndexOf(enemy);
                                    }
                                }
                            }
                            else
                            {
                                if (actor.Lives > 0)
                                {
                                    actor.MinusHealth();
                                }
                                else
                                {
                                    LevelFail();
                                }
                            }
                        }
                        if (item.Brush == Config.finishBrush)
                        {
                            isLevelComplete = true;
                            LevelComplete();
                        }
                        else if (item.Brush == Config.coinBrush)
                        {
                            model.CoinPickedup();
                            model.SetPickupIndex(dg.Children.IndexOf(item));
                            collision = false;
                        }
                        else if (item.Brush == Config.lifePickup)
                        {
                            retries++;
                        }
                        else
                        {
                            if (GoLeft && !IsJumping && !IsFalling)
                            {
                                if (actor.Area.Left < item.Bounds.Right && actor.Area.Bottom > item.Bounds.Top)
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
                                else if (oldPlayerPos.Bottom > item.Bounds.Y)
                                {
                                    actor.SetXY(actor.Area.X, item.Bounds.Top - actor.Area.Height);
                                    IsJumping = false;
                                }
                            }
                            else if (IsFalling && actor.Area.Right > item.Bounds.Left && actor.Area.Left < item.Bounds.Right && oldPlayerPos.Bottom < item.Bounds.Top)
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
                oldPlayerPos = actor.Area;

                if (!collision && !IsJumping)
                {
                    IsFalling = true;
                }
            }
            else if (actor is Enemy)
            {
                
            }
        }

        bool isLevelComplete = false;
        int retries = 3;
        public int Retries { get { return retries; } }
        public bool GameOver()
        {
            if (model.player.Lives > 0 && isLevelComplete)
            {
                model.NextLevel();
                isLevelComplete = false;
                return true;
            }
            else if (model.player.Lives < 0 || model.player.Area.Y > 1000)
            {
                // Respawn után folyamatosan esne
                oldPlayerPos = new Rect();

                model.ReloadLevel();
                retries--;
                return true;
            }

            return false;
        }

        public bool IsGameOver()
        {
            if (retries < 1)
            {
                return true;
            }
            return false;
        }

        public void LevelComplete()
        {
            if (model.currentLevel < model.Levels.Count)
            {
                model.NextLevel();
            }
            else
            {
                OnGameComplete?.Invoke(this, null);
            }
        }

        public void LevelFail()
        {
            model.ReloadLevel();
            oldPlayerPos = new Rect();
            retries--;
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

        public void RegisterLevels()
        {
            string[] files = Directory.GetFiles("Levels/", "*.level");

            int i = 0;
            foreach (string file in files)
            {
                model.Levels.Add(i, file);
                i++;
            }
        }
    }
}
