using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProgressView : MonoBehaviour
{
    [Header("UI")]
    public Image mProgressFill;
    public TextMeshProUGUI mLevelText;
    public TextMeshProUGUI mPercentageText;
    public TextMeshProUGUI mPlayerSpins;

    public const string lLevelString = "Level {0}";
    public const string lPercentageString = "{0}%";

    [Header("PrivateVariables")]
    private float lTargetFill;
    private float lTargetPercentage;
    private float lCurrentPercentage = 0;
    private Tweener lPercentageTween;
    private Tweener lProgressTween;

    #region ShowData

    public void ShowData(long aCurrentLevel, long aCurrentXp, long aFromXp, long aToXp)
    {
        lTargetFill = Progress(aCurrentXp, aFromXp, aToXp);
        lTargetPercentage = Percentage(aCurrentXp, aFromXp, aToXp);
        mLevelText.text = string.Format(lLevelString, aCurrentLevel);
        

        if (mProgressFill.fillAmount != lTargetFill && mProgressFill.fillAmount < lTargetFill)
            DOTween.To(() => mProgressFill.fillAmount, x => mProgressFill.fillAmount = x, lTargetFill, 1f);
        else if (mProgressFill.fillAmount != lTargetFill && mProgressFill.fillAmount > lTargetFill)
        {
            lProgressTween = DOTween.To(() => mProgressFill.fillAmount, x => mProgressFill.fillAmount = x, 1, 1f);
            lProgressTween.OnComplete(OnCompleteTween);
        }

        if (lCurrentPercentage != lTargetPercentage && lCurrentPercentage < lTargetPercentage)
        {
            lPercentageTween = DOTween.To(() => lCurrentPercentage, x => lCurrentPercentage = x, lTargetPercentage, 1f);
            lPercentageTween.OnUpdate(OnUpdateTween);
        }
        else if (lCurrentPercentage != lTargetPercentage && lCurrentPercentage > lTargetPercentage)
        {
            lPercentageTween = DOTween.To(() => lCurrentPercentage, x => lCurrentPercentage = x, 100, 1f);
            lPercentageTween.OnUpdate(OnUpdateTween);
            lPercentageTween.OnComplete(OnCompletePercentageTween);
        }
    }

    float Progress(float aCurrentXp, float aFromXp, float aToXp)
    {
        aCurrentXp = Mathf.Clamp(aCurrentXp, aFromXp, aToXp);
        aCurrentXp = (aCurrentXp - aFromXp) / (aToXp - aFromXp);
        return Mathf.Lerp(0, 1, aCurrentXp);
    }

    float Percentage(float aCurrentXp, float aFromXp, float aToXp)
    {
        aCurrentXp = Mathf.Clamp(aCurrentXp, aFromXp, aToXp);
        aCurrentXp = (aCurrentXp - aFromXp) / (aToXp - aFromXp);
        return Mathf.Lerp(0, 100, aCurrentXp);
    }

    #endregion

    #region PercentageTweenCallbacks

    void OnUpdateTween()
    {
        mPercentageText.text = string.Format(lPercentageString, lCurrentPercentage.ToString("F0"));
    }

    void OnCompletePercentageTween()
    {
        lPercentageTween = DOTween.To(() => 0, x => lCurrentPercentage = x, lTargetPercentage, 1f);
        lPercentageTween.OnUpdate(OnUpdateTween);
    }

    #endregion

    #region ProgressTweenCallbacks

    void OnCompleteTween()
    {
        DOTween.To(() => 0, x => mProgressFill.fillAmount = x, lTargetFill, 1f);
    }

    #endregion
}
