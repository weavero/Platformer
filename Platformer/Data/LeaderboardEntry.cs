using System;
using System.Collections.Generic;

#nullable disable

namespace Platformer.Data
{
    [Serializable]
    public partial class LeaderboardEntry
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public string Time { get; set; }
    }
}
