using UnityEngine;

namespace SlotMachine
{
    [CreateAssetMenu(fileName = "SymbolModel", menuName = "ScriptableObjects/Symbols", order = 1)]
    public class SymbolModel : ScriptableObject
    {
        public Sprite mSprite;
        public Vector2 size;
        public ReelFigures mReelFigure;
    }
}