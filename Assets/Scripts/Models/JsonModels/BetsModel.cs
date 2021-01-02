using System.Collections.Generic;

namespace SlotMachine
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    [System.Serializable]
    public class BetsMultiplier
    {
        public int bets;
        public int id;

    }
    [System.Serializable]
    public class BetsModel
    {
        public List<BetsMultiplier> mBets;
    }
}


