using DG.Tweening;
using SlotMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReelView : MonoBehaviour
{
    [Header("Model")]
    private ReelModel lReelModel;

    [Header("Symbol")]
    public ReelFigures mReelFigure;
    public List<int> mSymbolId = new List<int>();

    [Header("UI")]
    public RectTransform mRectTransform;
    public ScrollRect mScrollRect;

    [Header("PublicVariables")]
    public float finalPosY;

    [Header("PrivateVariables")]
    private Tweener lMainTween;
    private Tweener lStopTween;
    private Tweener lBounceTween;
    private readonly float differenceValue = 140;
    private float lLastScrollValue;

    [Header("Debug")]
    private SymbolModel mForcePrizeSymbolModel;

    #region SetData_PlayReel

    float Progress(float aCurrentXp, float aFromXp, float aToXp)
    {
        aCurrentXp = Mathf.Clamp(aCurrentXp, aFromXp, aToXp);
        aCurrentXp = (aCurrentXp - aFromXp) / (aToXp - aFromXp);
        return Mathf.Lerp(1, 0, aCurrentXp);
    }

    public void SetData(ReelModel aModel, float value = 0, float finalY = 0)
    {
        lReelModel = aModel;

        if (PlayerDataController.Controller.currentStripModel.reelstrip.Count > 0)
        {
            Debug.Log(value);

            lLastScrollValue = value;

            finalPosY = finalY;

            Invoke(nameof(SetScrollValue), 0.01f);
        }
        else
        {
            lMainTween = DOTween.To(() => mScrollRect.verticalScrollbar.value, x => mScrollRect.verticalScrollbar.value = x, 0f, 0.2f);
            lMainTween.OnComplete(OnComplete);
        }
    }

    void SetScrollValue()
    {
        mScrollRect.verticalScrollbar.value = lLastScrollValue;
    }

    public void OnComplete()
    {
        Debug.Log(mRectTransform.anchoredPosition.y);

        finalPosY = mRectTransform.anchoredPosition.y;

        float yPos = finalPosY - differenceValue;

        float value = Progress(yPos, 0, finalPosY);

        DOTween.To(() => mScrollRect.verticalScrollbar.value, x => mScrollRect.verticalScrollbar.value = x, value, 0.2f);

        lMainTween = null;
    }

    public void PlayAnimation()
    {
        if (lMainTween == null)
        {
            lMainTween = DOTween.To(() => mScrollRect.verticalScrollbar.value, x => mScrollRect.verticalScrollbar.value = x, 1f, lReelModel.mTimeDelay).SetEase(Ease.Linear).SetLoops(lReelModel.mLoop);
            lMainTween.OnComplete(OnCompleteMainTween);
            lMainTween.OnStepComplete(OnStepCompleteMainTween);
            lMainTween.SetAutoKill(false);
        }
        else
            lMainTween.Restart();
    }

    #endregion

    #region MainTweenCallBacks

    public IEnumerator CompleteAllTweens()
    {
        lMainTween.Complete();
        yield return new WaitForSeconds(0.1f);
        lStopTween.Complete();
        yield return new WaitForSeconds(0.1f);
        lBounceTween.Complete();
    }

    void OnStepCompleteMainTween()
    {
        for (int i = 0; i < mRectTransform.childCount - 2; i++)
        {
            mRectTransform.GetChild(mRectTransform.childCount - 1).SetAsFirstSibling();
        }
    }

    void OnCompleteMainTween()
    {
        mScrollRect.verticalScrollbar.value = 0f;

        GameController.Controller.mSlotMachineView.OnStoppingReels();

        if (lStopTween == null)
        {
            float value = Progress(differenceValue, 0, finalPosY);
            value += 0.01f;

            lStopTween = DOTween.To(() => mScrollRect.verticalScrollbar.value, x => mScrollRect.verticalScrollbar.value = x, value, lReelModel.mStopTimeDelay);

        #if Debug
            if (mForcePrizeSymbolModel != null)
            {
                Debug.Log("ForcePrize");
                mRectTransform.GetChild(1).GetComponent<SymbolView>().mSymbol = mForcePrizeSymbolModel;
                mRectTransform.GetChild(1).GetComponentInChildren<Image>().rectTransform.sizeDelta = mForcePrizeSymbolModel.size;
                mRectTransform.GetChild(1).GetComponentInChildren<Image>().sprite = mForcePrizeSymbolModel.mSprite;
                mForcePrizeSymbolModel = null;
            }
        #endif

            lStopTween.OnComplete(OnCompleteStopTween);
            lStopTween.SetAutoKill(false);
        }
        else
            lStopTween.Restart();
    }

    #endregion

    #region StopTweenCallBacks

    void OnCompleteStopTween()
    {
        mSymbolId.Clear();

        mReelFigure = mRectTransform.GetChild(1).GetComponent<SymbolView>().mSymbol.mReelFigure;
        mSymbolId.Add((int)mRectTransform.GetChild(1).GetComponent<SymbolView>().mSymbol.mReelFigure);

        GameController.Controller.mSlotMachineView.OnStopReels();
        
        lStopTween = null;

        if (lBounceTween == null)
        {
            float value = Progress(differenceValue, 0, finalPosY);

            lBounceTween = DOTween.To(() => mScrollRect.verticalScrollbar.value, x => mScrollRect.verticalScrollbar.value = x, value, lReelModel.mBounceTimeDelay);
            lBounceTween.SetAutoKill(false);
        }
        else
            lBounceTween.Restart();
    }

    #endregion

    #if Debug

    public void SetForcePrizeSymbols(SymbolModel aSymbols)
    {
        mForcePrizeSymbolModel = aSymbols;
    }

    #endif
}
