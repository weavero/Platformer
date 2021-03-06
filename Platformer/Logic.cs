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

        public EventHandler<GameCompleteArgs> OnGameComplete;
        public EventHandler OnLevelChange;

        public Logic(Model model)
        {
            this.model = model;
            RegisterLevels();
            LoadLevel();
            model.Retries = 1000;
            model.Timer.Start();
        }

        public void Move()
        {
            if (model.player.GoLeft)
            {
                model.player.SetX(-6);
            }
            else if (model.player.GoRight)
            {
                model.player.SetX(6);
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
        double maxJump = 7;
        public void Jump()
        {
            if (i < 1)
            {
                jumpHeight = -maxJump;
                i++;
            }

            if (model.player.IsJumping)
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

            if (model.player.IsFalling)
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
        int dgRemovableIndex;
        int enemyRemovableIndex;
        public void CollisionCheck(Actor actor, DrawingGroup dg)
        {
            drawing = dg;
            dgRemovableIndex = -1;
            enemyRemovableIndex = -1;
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

                                        if (enemy is SmallEnemy)
                                        {
                                            model.Points += 20;
                                        }
                                        else
                                        {
                                            model.Points += 50;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (actor.Lives > 0 && !(actor as Player).WasDamaged)
                                {
                                    actor.MinusHealth();
                                }
                                else
                                {
                                    LevelRestart();
                                }
                            }
                        }
                        if (item.Brush == Config.finishBrush)
                        {
                            LevelComplete();
                            model.player.IsFalling = false;
                            model.player.IsJumping = false;
                        }
                        else if (item.Brush == Config.coinBrush)
                        {
                            model.coin++;
                            model.SetPickupIndex(dg.Children.IndexOf(item));
                            collision = false;
                        }
                        else if (item.Brush == Config.lifePickup)
                        {
                            model.Retries++;
                            model.SetPickupIndex(dg.Children.IndexOf(item));
                            collision = false;
                        }
                        else
                        {
                            //// go left
                            //if (actor.Area.Left > item.Bounds.Right && actor.Area.Bottom > item.Bounds.Top)
                            //{
                            //    GoLeft = false;
                            //}

                            ////go right
                            //if (actor.Area.Right > item.Bounds.Left && actor.Area.Bottom > item.Bounds.Top)
                            //{
                            //    GoRight = false;
                            //}

                            if (model.player.GoLeft)
                            {
                                if (actor.Area.Left < item.Bounds.Right && actor.Area.Bottom > item.Bounds.Top)
                                {
                                    model.player.GoLeft = false;
                                    actor.SetXY(item.Bounds.Right, actor.Area.Y);
                                }
                            }
                            else if (model.player.GoRight)
                            {
                                if (actor.Area.Right > item.Bounds.Left && actor.Area.Bottom > item.Bounds.Top)
                                {
                                    model.player.GoRight = false;
                                    actor.SetXY(item.Bounds.Left - actor.Area.Width, actor.Area.Y);
                                }
                            }
                            if (model.player.IsJumping)
                            {
                                if (oldPlayerPos.Bottom < item.Bounds.Top)
                                {
                                    actor.SetXY(actor.Area.Left, item.Bounds.Top - actor.Area.Height);
                                    model.player.IsJumping = false;
                                }
                                else if (actor.Area.Top > item.Bounds.Top)
                                {
                                    actor.SetXY(actor.Area.Left, item.Bounds.Bottom);
                                    model.player.IsFalling = true;
                                    model.player.IsJumping = false;
                                }

                            }
                            else if (model.player.IsFalling && actor.Area.Right > item.Bounds.Left && actor.Area.Left < item.Bounds.Right && oldPlayerPos.Bottom < item.Bounds.Top)
                            {
                                model.player.IsFalling = false;
                                actor.SetXY(actor.Area.Left, item.Bounds.Top - actor.Area.Height);
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
                                actor.SetX((actor as Enemy).Dx);
                            }
                            else if (actor.Area.Right > item.Bounds.Left && actor.Area.Bottom > item.Bounds.Top)
                            {
                                (actor as Enemy).TurnAround();
                                actor.SetX((actor as Enemy).Dx);
                            }
                        }
                    }
                }
            }
            if(actor is Player)
            {
                oldPlayerPos = actor.Area;
                if (actor.Area.Bottom > 1000)
                {
                    LevelRestart();
                    oldPlayerPos = new Rect(0, 0, 0, 0);
                }
                else if (!collision && !model.player.IsJumping)
                {
                    model.player.IsFalling = true;
                }
            }
            else if (actor is Enemy)
            {
                
            }
        }

        public bool IsGameOver()
        {
            return (model.Retries < 1) ? true : false;
        }

        public void LevelComplete()
        {
            model.currentLevel++;
            if (model.currentLevel < model.Levels.Count)
            {
                LoadLevel();
                OnLevelChange(this, null);
            }
            else
            {
                GameCompleteArgs args = new GameCompleteArgs();
                args.Points = model.Points;
                args.Time = model.Timer.Elapsed;
                OnGameComplete?.Invoke(this, args);
                model.Timer.Stop();
            }
        }

        public void LevelRestart()
        {
            ReloadLevel();
            oldPlayerPos = new Rect(0, 0, 0, 0);
            model.Retries--;
        }

        public void ReloadLevel()
        {
            LoadLevel();
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
            model.Levels = new Dictionary<int, string>();
            string[] files = Directory.GetFiles("Levels/", "*.level");

            int i = 0;
            foreach (string file in files)
            {
                model.Levels.Add(i, file);
                i++;
            }
        }

        public void LoadLevel()
        {
            try
            {
                StreamReader sr = new StreamReader(model.Levels[model.currentLevel]);
                List<string> sorok = new List<string>();
                while (!sr.EndOfStream)
                {
                    string sor = sr.ReadLine().Replace("\t", "    ");
                    sorok.Add(sor);
                }
                sr.Dispose();

                int max = 0;
                for (int i = 1; i < sorok.Count; i++)
                {
                    if (sorok[max].Length < sorok[i].Length)
                    {
                        max = i;
                    }
                }
                model.map = new char[sorok.Count, sorok[max].Length];

                for (int i = 0; i < model.map.GetLength(0); i++)
                {
                    for (int j = 0; j < sorok[i].Length; j++)
                    {
                        model.map[i, j] = Convert.ToChar(sorok[i].Substring(j, 1));
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                throw new MapNotFoundException(e.Message);
            }

            model.pickupableIndex = -1;
            model.enemies = new List<Enemy>();
            LoadActors();
        }

        private void LoadActors()
        {
            for (int i = 0; i < model.map.GetLength(0); i++)
            {
                for (int j = 0; j < model.map.GetLength(1); j++)
                {
                    switch (model.map[i, j])
                    {
                        case 'P':
                            model.player = new Player(j * Config.unitWidth, i * Config.unitHeight + (Config.unitHeight - Config.playerHeight));
                            break;

                        case 'e':
                            model.enemies.Add(new SmallEnemy(j * Config.unitWidth, i * Config.unitHeight + (Config.unitHeight - Config.smallEnemyHeight)));
                            break;

                        case 'E':
                            model.enemies.Add(new BigEnemy(j * Config.unitWidth, i * Config.unitHeight + (Config.unitHeight - Config.bigEnemyHeight)));
                            break;
                    }
                }
            }
        }
    }
}
