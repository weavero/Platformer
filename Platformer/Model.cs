using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Platformer
{
    class Model
    {
        private Stopwatch timer;

        public Player player;

        private List<Enemy> enemies;
        public List<Enemy> Enemies { get { return new List<Enemy>(enemies); } }

        private char[,] map;
        public char[,] Map{ get { return map; } }

        public Dictionary<int, string> Levels;

        public int currentLevel = 1;

        public int coin = 0;
        public int Coin { get { return coin; } }

        private int pickupableIndex;
        public int PickupableIndex { get { return pickupableIndex; } }

        public MainWindow mainWindow { get; set; }

        public Model()
        {
            coin = 0;
            timer = new Stopwatch();
            RegisterLevels();
            LoadLevel(Levels[currentLevel]);
        }

        private void RegisterLevels()
        {
            Levels = new Dictionary<int, string>();
            Levels.Add(1, "Levels/1.level");
            Levels.Add(2, "Levels/2.level");
            Levels.Add(3, "Levels/3.level");
            Levels.Add(4, "Levels/4.level");
            Levels.Add(5, "Levels/5.level");
        }

        public void LoadLevel(string levelPath)
        {
            try
            {
                StreamReader sr = new StreamReader(levelPath);
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
                map = new char[sorok.Count, sorok[max].Length];

                for (int i = 0; i < map.GetLength(0); i++)
                {
                    for (int j = 0; j < sorok[i].Length; j++)
                    {
                        map[i, j] = Convert.ToChar(sorok[i].Substring(j, 1));
                    }
                }
            }
            catch(FileNotFoundException e)
            {
                throw new MapNotFoundException(e.Message);
            }
            
            pickupableIndex = -1;
            enemies = new List<Enemy>();
            LoadActors();
        }

        private void LoadActors()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    switch (map[i, j])
                    {
                        case 'P':
                            player = new Player(j * Config.unitWidth, i * Config.unitHeight + (Config.unitHeight - Config.playerHeight));
                            break;

                        case 'e':
                            enemies.Add(new SmallEnemy(j * Config.unitWidth, i * Config.unitHeight + (Config.unitHeight - Config.smallEnemyHeight)));
                            break;

                        case 'E':
                            enemies.Add(new BigEnemy(j * Config.unitWidth, i * Config.unitHeight + (Config.unitHeight - Config.bigEnemyHeight)));
                            break;
                    }
                }
            }
        }

        public void CoinPickedup()
        {
            coin++;
        }

        public void SetPickupIndex(int Index)
        {
            pickupableIndex = Index;
        }

        public void NextLevel()
        {
            if (currentLevel < Levels.Count)
            {
                currentLevel++;
                LoadLevel(Levels[currentLevel]);
            }
            else
            {
                GameFinished(); //todo
            }
        }

        public void ReloadLevel()
        {
            LoadLevel(Levels[currentLevel]);
        }

        public TimeSpan GetElapsedTime()
        {
            return timer.Elapsed;
        }

        public void StartTimer()
        {
            if (!timer.IsRunning)
            {
                timer.Start();
            }
        }

        public void StopTimer()
        {
            if (timer.IsRunning)
            {
                timer.Stop();
            }
        }

        private void GameFinished()
        {

        }
    }

    class MapNotFoundException : Exception
    {
        public MapNotFoundException() : base()
        {
        }

        public MapNotFoundException(string message) : base(message)
        {
        }
    }
}
