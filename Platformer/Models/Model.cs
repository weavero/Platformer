using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Platformer.Models
{
    class Model
    {
        public Stopwatch Timer;

        public Player player;

        public List<Enemy> Enemies;

        public char[,] map;
        public char[,] Map{ get { return map; } }

        public Dictionary<int, string> Levels;

        public int currentLevel;

        public int coin;
        public int Coin { get { return coin; } }

        public int pickupableIndex;
        public int PickupableIndex { get { return pickupableIndex; } }

        public int Points { get; set; }

        public int Retries { get; set; }

        public MainWindow mainWindow { get; set; }

        public Model()
        {
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
