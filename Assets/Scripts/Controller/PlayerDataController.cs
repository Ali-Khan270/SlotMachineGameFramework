using SlotMachine;
using UnityEngine;

public class PlayerDataController : MonoBehaviour
{
    public static PlayerDataController Controller { get; private set; } = null;

    [Header("ReelFigures")]
    public ReelFigures mReelFigure;

    [Header("ReelStripData")]
    public ReelStripModel currentStripModel;
    public SaveReelStrip saveReelStrip;

    [Header("Models")]
    public PlayerItemsModel playerItems;
    public PlayerProgressModel playerProgress;
    public ReelRewardsModel reelRewardsModel;

    [Header("Player's Village")]
    public PlayerVillage playerVillage;

    void Awake()
    {
        if (Controller != null && Controller != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Controller = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    #region GetData

    public PlayerProgressModel.Level GetLevel()
    {
        return playerProgress.levels[playerItems.Level - 1];
    }

    public PlayerProgressModel.Reward GetLevelRewards()
    {
        return playerProgress.levels[playerItems.Level - 1].rewards;
    }

    public void IsNewLevelAchieved()
    {
        if (playerItems.Xp >= GetLevel().toXP)
        {
            Debug.Log("NewLevelAchieved");

            playerItems.Diamonds = GetLevelRewards().diamonds;
            playerItems.Coins = GetLevelRewards().coins;

            Debug.Log("RewardDiamond : " + GetLevelRewards().diamonds);
            Debug.Log("RewardCoin : " + GetLevelRewards().coins);

            playerItems.ResetXp = GetLevel().toXP;

            Debug.Log("toXP : " + GetLevel().toXP);

            playerItems.Level = 1;
        }
    }

    #endregion
}
