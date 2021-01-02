using System.Collections.Generic;
using System;
using System.Linq;

namespace SlotMachine
{
    [Serializable]
    public class ReelRewardsModel
    {
        public List<ReelReward> reelRewards = new List<ReelReward>();

        [Serializable]
        public class Reward
        {
            public string type;
            public int amount;
        }

        [Serializable]
        public class MatchType
        {
            public int matchSymbols;
            public List<Reward> rewards = new List<Reward>();
        }

        [Serializable]
        public class ReelReward
        {
            public string reelFigure;
            public List<MatchType> matchType = new List<MatchType>();

            public bool HasMatchType(int match)
            {
                if (matchType.FirstOrDefault(s => s.matchSymbols.Equals(match)) != null)
                {
                    UnityEngine.Debug.Log(match + " matches : Found");
                    return true;
                }
                return false;
            }
        }
    }
}


