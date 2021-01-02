using System;
using System.Collections.Generic;

namespace SlotMachine
{
    [Serializable]
    public class PlayerProgressModel
    {
        public List<Level> levels = new List<Level>();
        private static int counter = 0;

        [Serializable]
        public class Level
        {
            public string name;
            public int level;
            public int fromXP;
            public int toXP;

            public Reward rewards;

            public Level()
            {
                name = "Level " + counter;
                counter++;
            }
        }

        [Serializable]
        public class Reward
        {
            public int diamonds = 0;
            public int coins = 0;
        }
    }
}
