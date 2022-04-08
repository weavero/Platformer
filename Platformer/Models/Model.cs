using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Platformer.Models
{
    class Model
    {
        public Stopwatch Timer;

        public Player player;

        public List<Enemy> enemies;
        public List<Enemy> Enemies { get { return new List<Enemy>(enemies); } }

        public char[,] map;
        public char[,] Map{ get { return map; } }

        public Dictionary<int, string> Levels;

        public int currentLevel = 0;

        public int coin = 0;
        public int Coin { get { return coin; } }

        public int pickupableIndex;
        public int PickupableIndex { get { return pickupableIndex; } }

        public int Points { get; set; }

        public int Retries { get; set; }

        public MainWindow mainWindow { get; set; }

        public Model()
        {
            coin = 0;
            Timer = new Stopwatch();
        }

        public void SetPickupIndex(int Index)
        {
            pickupableIndex = Index;
        }
    }

    public class MapNotFoundException : Exception
    {
        public MapNotFoundException() : base()
        {
        }

        public MapNotFoundException(string message) : base(message)
        {
        }
    }

    public class GameCompleteArgs : EventArgs
    {
        public int Points { get; set; }
        public TimeSpan Time { get; set; }
    }
}
