using SlotMachine;
using UnityEngine;
using UnityEngine.UI;

public class SymbolView : MonoBehaviour
{
    [Header("Model")]
    public SymbolModel mSymbol;

    [Header("UI")]
    public Image mSymbolImage;

    public void BindData(SymbolModel model)
    {
        mSymbol = model;
        mSymbolImage.sprite = mSymbol.mSprite;
        mSymbolImage.rectTransform.sizeDelta = mSymbol.size;
        transform.name = mSymbol.mReelFigure.ToString();
    }
}
