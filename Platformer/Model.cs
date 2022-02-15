using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Platformer
{
    class Model
    {
        public Player player;
        public Enemy enemy;
        List<Enemy> enemies = new List<Enemy>();

        public List<Enemy> Enemies
        {
            get { return enemies; }
        }

        char[,] map;

        public char[,] Map
        {
            get { return map; }
        }

        public Model(string levelPath)
        {
            //@"../../../Levels/1.level"
            PalyaBetolt(levelPath);
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    switch (map[i,j])
                    {
                        case 'P':
                            player = new Player(j * 20 + 20, i * 10 + 300);
                            break;

                        case 'E':
                            enemies.Add(new Enemy(j * 20 + 20, i * 10 + 300));
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        public void PalyaBetolt(string path)
        {
            StreamReader sr = new StreamReader(path);
            List<string> sorok = new List<string>();
            while (!sr.EndOfStream)
            {
                sorok.Add(sr.ReadLine());
            }

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
    }
}
