using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpinsView : MonoBehaviour
{
    [Header("Bet")]
    public BetsView mBet;

    [Header("gameUI")]
    public TextMeshProUGUI mSpinsText;
    public Image mSpinPercentageFill;

    [Header("PrivateVariables")]
    private readonly int lTotalSpins = 50;
    private int lCurrentSpins;
    private int mBetAmount;
    private float mCurrentPercentage;

    public const string lSpinString = "{0}/";

    void Start()
    {
        lCurrentSpins = lTotalSpins;
        //lCurrentSpins = PlayerModel.Spins;
    }

    /// <summary>
    /// Debug Region For Developers
    /// on S key Fills The Spins to Full
    /// </summary>
    private void LateUpdate()
    {
//DEV Debug
#if Debug
        if (Input.GetKeyDown(KeyCode.S))
        {
            lCurrentSpins = lCurrentSpins + 50;
            if(lCurrentSpins > 50)
            {
                lCurrentSpins = 50;
            }
            
            PlayerDataController.Controller.playerItems.Spins = lCurrentSpins;
        }
#endif
    }

    /// <summary>
    /// returns Bool on whether player can Spin or not.
    /// </summary>
    /// <returns></returns>
    public bool CanSpin()
    {
        mBetAmount = mBet.GetBetsAmount();
        return mBetAmount <= lCurrentSpins;
    }

    /// <summary>
    /// On Spin Updates the Spin amount Accordingly and Update the Player Item
    /// </summary>
    public void UpdateSpin()
    {
        lCurrentSpins -= mBetAmount;

        PlayerDataController.Controller.playerItems.Spins = lCurrentSpins;

        Percentage();

        if(mCurrentPercentage != mSpinPercentageFill.fillAmount)
            DOTween.To(() => mSpinPercentageFill.fillAmount, x => mSpinPercentageFill.fillAmount = x, mCurrentPercentage, 1f);

        mSpinsText.text = string.Format(lSpinString + lTotalSpins, lCurrentSpins.ToString("F0"));
        Debug.Log("Spins Remaining :" + lCurrentSpins); ;
    }

    void Percentage()
    {
        mCurrentPercentage = lCurrentSpins * 100 / lTotalSpins;
        mCurrentPercentage /= 100;
    }
}
