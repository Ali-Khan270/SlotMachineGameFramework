using UnityEngine;

namespace SlotMachine
{
    [CreateAssetMenu(fileName = "ReelModel", menuName = "ScriptableObjects/Reels", order = 2)]
    public class ReelModel : ScriptableObject
    {
        public float mTimeDelay = 0.1f;
        public float mStopTimeDelay = 1.5f;
        public float mBounceTimeDelay = 0.5f;
        public int mLoop = 10;
    }
}
