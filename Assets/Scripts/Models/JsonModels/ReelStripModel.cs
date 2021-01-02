using System.Collections.Generic;
using System;
using UnityEngine;

namespace SlotMachine
{
    public enum ReelFigures
    {
        eCoin = 1,
        eAttack = 2,
        eRaid = 3,
        eWaterStamina = 4,
        ePlantStamina = 5,
        eFireStamina = 6,
        eEarthStamina = 7
    }

    [Serializable]
    public class ReelStripModel
    {
        public List<List<string>> reelstrip = new List<List<string>>();

        public string SaveToString()
        {
            return JsonUtility.ToJson(this);
        }

        public static ReelStripModel CreateFromJSON(string aJsonString)
        {
            return JsonUtility.FromJson<ReelStripModel>(aJsonString);
        }
    }

    [Serializable]
    public class SaveReelStrip
    {
        public List<float> currentStopReelScrollValues = new List<float>();
        public List<float> finalPosY = new List<float>();
    }
}
