using System;
using System.Collections.Generic;

namespace SlotMachine
{
    [Serializable]
    public class PlayerVillage
    {
        public int id;
        public string PlayerName;
        public int PlayerLevel;
        public int PlayerCoins;
        public string PlayerImage;

        public int Coins
        {
            get { return PlayerCoins; }
            set { PlayerCoins -= value; }
        }

        public List<int> randomPrice = new List<int>();

        public void SetRandomPrice()
        {
            randomPrice.Clear();
            int tempCoins = Coins;

            for (int i = 0; i < 4; i++)
            {
                randomPrice.Add(UnityEngine.Random.Range(0, tempCoins));
                tempCoins -= randomPrice[i];
            }
        }
    }

    [Serializable]
    public class PlayersModel
    {
        public List<PlayerVillage> PlayersList;
        public int MinimumMatchFrequency;
        public int MaximumMatchFrequency;
    }
}
