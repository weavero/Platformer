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
using Platformer.Models;

namespace Platformer.Controls
{
    class Logic
    {
        Model model;

        public event EventHandler<GameCompleteArgs> OnGameComplete;
        public event EventHandler OnLevelChange;

        public Logic(Model model)
        {
            this.model = model;
            RegisterLevels();
            LoadLevel();
            model.Retries = 3;
            model.Timer.Start();
        }

        public void GameTick()
        {
            model.player.Move();
            model.player.InvincibilityTick();
            MoveAI();
        }

        public void MoveAI()
        {
            foreach (Enemy enemy in model.Enemies)
            {
                enemy.SetX(enemy.Velocity);
            }
        }

        Rect oldPlayerPos;
        List<Enemy> oldEnemies;
        DrawingGroup drawing;
        int dgRemovableIndex;
        int enemyRemovableIndex;
        public void CollisionCheck(Actor actor, DrawingGroup dg)
        {
            //drawing = dg;
            //dgRemovableIndex = -1;
            //enemyRemovableIndex = -1;
            bool collision = false;
            foreach (GeometryDrawing item in dg.Children)
            {
                if (actor.Area.IntersectsWith(item.Bounds) && actor.Brush != (ImageBrush)item.Brush)
                {
                    if (actor is Player)
                    {
                        if (Config.EnemyBrushes.Contains(item.Brush))
                        {
                            if (oldPlayerPos.Bottom < item.Bounds.Top)
                            {
                                foreach (Enemy enemy in model.Enemies)
                                {
                                    if (enemy.Area.X - enemy.Velocity == item.Bounds.X && enemy.Area.Y == item.Bounds.Y)
                                    {
                                        enemy.MinusHealth();
                                        enemy.StopMoving();
                                        if (enemy.HitsToKill == 0)
                                        {
                                            enemy.SetXY(-2000, -2000);
                                            //dgRemovableIndex = dg.Children.IndexOf(item);
                                            //enemyRemovableIndex = model.Enemies.IndexOf(enemy);
                                            
                                            if (enemy is SmallEnemy)
                                            {
                                                model.Points += 20;
                                            }
                                            else if (enemy is BigEnemy)
                                            {
                                                model.Points += 50;
                                            }
                                            else if (enemy is FlyingEnemy)
                                            {
                                                model.Points += 20;
                                            }
                                        }
                                        (actor as Player).Bounce();
                                    }
                                }
                            }
                            else
                            {
                                if (actor.HitsToKill > 0)
                                {
                                    actor.Damaged();
                                }
                                else
                                {
                                    LevelRestart();
                                }
                            }
                        }
                        else if (item.Brush == Config.finishBrush)
                        {
                            LevelComplete();
                            actor.IsFalling = false;
                            actor.IsJumping = false;
                        }
                        else if (item.Brush == Config.CoinBrush)
                        {
                            model.coin++;
                            model.SetPickupIndex(dg.Children.IndexOf(item));
                            collision = false;
                        }
                        else if (item.Brush == Config.LifePickup)
                        {
                            model.Retries++;
                            model.SetPickupIndex(dg.Children.IndexOf(item));
                            collision = false;
                        }
                        else if (item.Brush == Config.spikeBrush)
                        {
                            if (actor.HitsToKill > 0)
                            {
                                (actor as Player).Bounce();
                                actor.Damaged();
                            }
                            else
                            {
                                LevelRestart();
                            }
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

                            // go left
                            if (actor.Velocity < 0)
                            {
                                if (actor.IsJumping || actor.IsFalling)
                                {
                                    
                                    if (actor.Area.Left < item.Bounds.Right && oldPlayerPos.Bottom < item.Bounds.Top)
                                    {

                                    }
                                    
                                }
                                else if (actor.Area.Left < item.Bounds.Right && actor.Area.Bottom > item.Bounds.Top)
                                {
                                    actor.Velocity = 0;
                                    actor.GoLeft = false;
                                    actor.SetXY(item.Bounds.Right, actor.Area.Y);
                                }
                            }
                            // go right
                            else if (actor.Velocity > 0)
                            {
                                if (actor.IsJumping || actor.IsFalling)
                                {
                                    if (oldPlayerPos.Bottom < item.Bounds.Top)
                                    {

                                    }
                                }
                                else if (actor.Area.Right > item.Bounds.Left && actor.Area.Bottom > item.Bounds.Top)
                                {
                                    actor.Velocity = 0;
                                    actor.GoRight = false;
                                    actor.SetXY(item.Bounds.Left - actor.Area.Width, actor.Area.Y);
                                }
                            }
                            if (actor.IsJumping)
                            {
                                if (oldPlayerPos.Bottom < item.Bounds.Top && oldPlayerPos.Top < item.Bounds.Top )//&& actor.Area.Bottom > item.Bounds.Top)
                                {
                                    actor.SetXY(actor.Area.Left, item.Bounds.Top - actor.Area.Height);
                                    actor.IsJumping = false;
                                }
                                else if (actor.Area.Top > item.Bounds.Top)
                                {
                                    actor.SetXY(actor.Area.Left, item.Bounds.Bottom);
                                    actor.IsFalling = true;
                                    actor.IsJumping = false;
                                }
                            }
                            // && actor.Area.Right > item.Bounds.Left && actor.Area.Left < item.Bounds.Right
                            else if (actor.IsFalling && oldPlayerPos.Bottom < item.Bounds.Top)
                            {
                                actor.IsFalling = false;
                                actor.SetXY(actor.Area.Left, item.Bounds.Top - actor.Area.Height);
                            }
                            collision = true;
                        }
                    }
                    else if (actor is Enemy)
                    {
                        if (item.Brush != model.player.Brush)
                        {
                            if (actor.Area.Left < item.Bounds.Right && actor.Area.Bottom > item.Bounds.Top)
                            {
                                (actor as Enemy).TurnAround();
                                actor.SetX((actor as Enemy).Velocity);
                            }
                            else if (actor.Area.Right > item.Bounds.Left && actor.Area.Bottom > item.Bounds.Top)
                            {
                                (actor as Enemy).TurnAround();
                                actor.SetX((actor as Enemy).Velocity);
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
                else if (!collision && !actor.IsJumping)
                {
                    actor.IsFalling = true;
                }
            }
            else if (actor is Enemy)
            {
                if (!collision)
                {
                    actor.IsFalling = true;
                }
            }
        }

        public bool IsGameOver()
        {
            return model.Retries < 1 ? true : false;
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
                args.Points = CalculatePoints();
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

        private int CalculatePoints()
        {
            //-   < 2:00 – 500 pont
            //- 2:00 – 2:29 – 400 pont
            //- 3:00 – 4:00 – 250 pont
            //-   > 4:00 – 100 pont

            int points = model.Points;
            if (model.Timer.Elapsed.Seconds < 120)
            {
                points += 500;
            }
            else if (model.Timer.Elapsed.Seconds >= 120 && model.Timer.Elapsed.Seconds < 149)
            {
                points += 400;
            }
            else if (model.Timer.Elapsed.Seconds >= 150 && model.Timer.Elapsed.Seconds < 239)
            {
                points += 250;
            }
            else if (model.Timer.Elapsed.Seconds >= 240)
            {
                points += 100;
            }

            points += model.coin * 10;
            points += model.Retries * 100;

            return points;
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
                            model.player = new Player(j * Config.UnitWidth, i * Config.UnitHeight);
                            break;

                        case 'e':
                            model.enemies.Add(new SmallEnemy(j * Config.UnitWidth, i * Config.UnitHeight));
                            break;

                        case 'E':
                            model.enemies.Add(new BigEnemy(j * Config.UnitWidth, i * Config.UnitHeight + (Config.UnitHeight - Config.BigEnemyHeight)));
                            break;

                        case 'f':
                            model.enemies.Add(new FlyingEnemy(j * Config.UnitWidth, i * Config.UnitHeight));
                            break;
                    }
                }
            }
        }
    }
}
