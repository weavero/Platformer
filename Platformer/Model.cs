using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Platformer
{
    class Model
    {
        public Player player;
        private List<Enemy> enemies;

        public List<Enemy> Enemies
        {
            get { return new List<Enemy>(enemies); }
        }

        private char[,] map;
        public char[,] Map
        {
            get { return map; }
        }

        private int coin = 0;
        public int Coin { get { return coin; } }

        public Model(string levelPath)
        {
            LoadLevel(levelPath);
        }
        public void LoadLevel(string levelPath)
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
            LoadActors();
        }

        public void LoadActors()
        {
            enemies = new List<Enemy>();
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
    }
}
