using System;
using System.Collections.Generic;
using System.Linq;

namespace SlotMachine
{
    [Serializable]
    public class PlayerItemsModel
    {
        private long xP;
        private int level = 1;
        private long coins;
        private long diamonds;
        private long spins;

        public long Xp
        {
            get { return xP; }
            set 
            { 
                xP += value;
            }
        }

        public long ResetXp
        {
            set
            {
                xP -= value;
            }
        }

        public int Level
        {
            get { return level; }
            set { level += value; }
        }

        public long Coins
        {
            get { return coins; }
            set { coins += value; }
        }

        public long Diamonds
        {
            get { return diamonds; }
            set { diamonds += value; }
        }

        public long Spins
        {
            get { return spins; }
            set { spins += value; }
        }

        public Village village;
        

        [Serializable]
        public class Village
        {
            public long Tower;
            public long Wall;
            public long Castle;
            public long FarmHouse;
        }
        
        
    }
}
