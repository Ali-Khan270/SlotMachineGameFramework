using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using SlotMachine;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using DG.Tweening;

public class SlotMachineView : MonoBehaviour
{
    [Header("SlotMachineState")]
    public MachineState mState;

    [Header("JsonData")]
    public TextAsset mReelStrip;
    public TextAsset mPlayersVillageList;

    [Header("Model")]
    private ReelStripModel stripModel;

    [Header("PlayerVillageView")]
    public PlayerVillageView mPlayerVillageView;

    [Header("PlayerProgress")]
    public PlayerProgressView mPlayerProgress;

    [Header("Bets/Spins")]
    public BetsView mBets;
    public PlayerSpinsView mSpinView;

    [Header("Texts")]
    public TextAnimationView mCoinText;
    public TextAnimationView mDiamondText;
    public TextMeshProUGUI mSpinButtonText;

    [Header("SymbolsData")]
    public SymbolModel[] mSymbols;

    [Header("ReelsData")]
    public ReelModel[] mReels;

    [Header("ReelViews")]
    public ReelView[] mReelViews;

    [Header("Prefabs")]
    public SymbolView mSymbolObj;

    [Header("Animations")]
    public AnimationView mAnimation;

    private int mStopReels = 0;
    private int mTotalAmount, mBetMultiplier;

    private readonly Dictionary<string, SymbolModel> lSymbols = new Dictionary<string, SymbolModel>();

    private SymbolView lSymbolView;
    private PlayerProgressModel.Level lLevel;
    private ReelRewardsModel.ReelReward lReelReward;
    private ReelRewardsModel.MatchType lMatchType;
    private ReelRewardsModel.Reward lRewards;
    private MatchMaking mMatchmaking;

    readonly WinCalculation mWinlineCalculation = new WinCalculation();

    #region Unity_Callbacks

    void Start()
    {
        mState = MachineState.Idle;

        ShowData();

        GenerateSymbols();

        mMatchmaking = MatchMaking.GetInstance();

        mMatchmaking.LoadPlayerjson(mPlayersVillageList);

        mPlayerVillageView.SetPlayer(mMatchmaking.GetPlayerForMatch());

        PlayerDataController.Controller.playerVillage = mPlayerVillageView.playerVillage;
    }

    void ShowData()
    {
        mCoinText.ShowValue(PlayerDataController.Controller.playerItems.Coins);
        mDiamondText.ShowValue(PlayerDataController.Controller.playerItems.Diamonds);

        lLevel = PlayerDataController.Controller.GetLevel();
        mPlayerProgress.ShowData(lLevel.level, PlayerDataController.Controller.playerItems.Xp, lLevel.fromXP, lLevel.toXP);
    }

    void GenerateSymbols()
    {
        //Debug.Log(mReelStrip.text);

        for (int j = 0; j < mSymbols.Length; j++)
        {
            lSymbols.Add(mSymbols[j].mReelFigure.ToString(), mSymbols[j]);
        }

        if (PlayerDataController.Controller.currentStripModel.reelstrip.Count > 0)
            stripModel = PlayerDataController.Controller.currentStripModel;
        else
            stripModel = JsonConvert.DeserializeObject<ReelStripModel>(mReelStrip.text);

        //Debug.Log(stripModel.reelstrip.Count);

        for (int i = 0; i < stripModel.reelstrip.Count; i++)
        {
            for (int j = 0; j < stripModel.reelstrip[i].Count; j++)
            {
                //Debug.Log(stripModel.reelstrip[i][j]);
                lSymbolView = Instantiate(mSymbolObj, mReelViews[i].transform);
                lSymbolView.BindData(lSymbols.Single(s => s.Key == stripModel.reelstrip[i][j]).Value);
                lSymbolView.transform.SetAsFirstSibling();
            }

            if (PlayerDataController.Controller.saveReelStrip.currentStopReelScrollValues.Count > 0)
                mReelViews[i].SetData(mReels[i], PlayerDataController.Controller.saveReelStrip.currentStopReelScrollValues[i], PlayerDataController.Controller.saveReelStrip.finalPosY[i]);
            else
                mReelViews[i].SetData(mReels[i]);
        }
    }

    #endregion

//DEV DEBUG
#if Debug

    public void ForcePrize(int aSymbolId)
    {
        for (int i = 0; i < stripModel.reelstrip.Count; i++)
        {
            mReelViews[i].SetForcePrizeSymbols(mSymbols[aSymbolId - 1]);
        }
    }

#endif

    #region ClickEvents

    public void SpinClick()
    {
        if (mState == MachineState.Rotating)
        {
            for (int i = 0; i < mReelViews.Length; i++)
            {
                StartCoroutine(mReelViews[i].CompleteAllTweens());
            }
            return;
        }

        if (mState != MachineState.Idle)
            return;

        if (!mSpinView.CanSpin())
            return;

        mSpinView.UpdateSpin();

        mState = mBets.mState = MachineState.Rotating;

        SpinStopButton();

        Debug.Log(mState);

        for (int i = 0; i < mReelViews.Length; i++)
        {
            mReelViews[i].PlayAnimation();
        }
    }

    void SpinStopButton()
    {
        if (mState == MachineState.Idle)
        {
            mSpinButtonText.text = "SPIN";
        }
        else
        {
            mSpinButtonText.text = "STOP";
        }
    }

    #endregion

    #region MachineStateEvents

    public void OnStoppingReels()
    {
        if (mState == MachineState.Stopping)
            return;

        mState = MachineState.Stopping;

        Debug.Log(mState);
    }

    public void OnStopReels()
    {
        mStopReels++;

        if (mStopReels >= mReelViews.Length)
        {
            mState = MachineState.Stopped;

            Debug.Log(mState);

            Invoke(nameof(SaveStopReels), 0.2f);

            Invoke(nameof(OnReelWinner), 0.2f);
        }
    }

    void SaveStopReels()
    {
        PlayerDataController.Controller.currentStripModel.reelstrip = new List<List<string>>();

        PlayerDataController.Controller.saveReelStrip.currentStopReelScrollValues = new List<float>();

        PlayerDataController.Controller.saveReelStrip.finalPosY = new List<float>();

        for (int i = 0; i < mReelViews.Length; i++)
        {
            List<string> reels = new List<string>();

            for (int j = mReelViews[i].transform.childCount - 1; j >= 0; j--)
            {
                reels.Add(mReelViews[i].transform.GetChild(j).GetComponent<SymbolView>().mSymbol.mReelFigure.ToString());
            }

            PlayerDataController.Controller.currentStripModel.reelstrip.Add(reels);

            PlayerDataController.Controller.saveReelStrip.currentStopReelScrollValues.Add(mReelViews[i].mScrollRect.verticalScrollbar.value);

            PlayerDataController.Controller.saveReelStrip.finalPosY.Add(mReelViews[i].finalPosY);
        }

        //Debug.Log(PlayerDataController.Controller.currentStripModel.reelstrip.Count);
    }

    void OnReelWinner()
    {
        mState = MachineState.ShowingWin;

        //matrix Build
        MatrixBuilding();
        WinningRewards();

        Debug.Log(mState);
    }

    void OnReelIdle()
    {
        mState = mBets.mState = MachineState.Idle;

        SpinStopButton();

        mMatchmaking.mSpins++;

        if (mMatchmaking.mSpins >= mMatchmaking.mPlayerFrequency)
            mPlayerVillageView.SetPlayer(mMatchmaking.GetPlayerForMatch());

        PlayerDataController.Controller.playerVillage = mPlayerVillageView.playerVillage;

        mStopReels = 0;

        Debug.Log(mState);
    }

    #endregion

    #region WinRewards

    //Matrix to check against winlines
    readonly int[][] mMatrix =
    {
        new int[]{-1,-1,-1},
        new int[]{-1,-1,-1},
        new int[]{-1,-1,-1},
        new int[]{-1,-1,-1},
        new int[]{-1,-1,-1}
    };

    /// <summary>
    /// Takes the reelview symbol ids and make a matrix to check against winlines
    /// </summary>
    void MatrixBuilding()
    {
        //Build Matrix To Check against Winlines
        for (int i = 0; i < mReelViews.Length; i++)
        {
            for (int j = 0; j < mReelViews[i].mSymbolId.Count; j++)
            {
                mMatrix[i][j] = mReelViews[i].mSymbolId[j];
            }
        }

        //Winline Calculation
        mWinlineCalculation.Views(mMatrix);

        //ReelFigure Reward
        for (int i = 0; i < mSymbols.Length; i++)
        {
            if ((int)mSymbols[i].mReelFigure == mWinlineCalculation.mMostRepeatedSymbol)
            {
                PlayerDataController.Controller.mReelFigure = mSymbols[i].mReelFigure;
            }
        }

        //Debug.Log("Winning Symbol and Count: " + PlayerDataController.Controller.mReelFigure + " " + mWinlineCalculation.mRepetitionCount);
    }

    public void WinningRewards()
    {
        lReelReward = PlayerDataController.Controller.reelRewardsModel.reelRewards.Single(s => s.reelFigure.Equals(PlayerDataController.Controller.mReelFigure.ToString()));

        //Debug.Log(mWinlineCalculation.mRepetitionCount);

        if (!lReelReward.HasMatchType(mWinlineCalculation.mRepetitionCount))
        {
            Invoke(nameof(OnReelIdle), 0.1f);
            return;
        }

        lMatchType = lReelReward.matchType.Single(s => s.matchSymbols.Equals(mWinlineCalculation.mRepetitionCount));

        //Debug.Log(lMatchType);

        switch (PlayerDataController.Controller.mReelFigure)
        {
            case ReelFigures.eCoin:
                mAnimation.PlayAnimation("Coin");
                lRewards = lMatchType.rewards.Single(s => s.type.Equals("eCoin"));
                mBetMultiplier = mBets.GetBetsAmount();
                mTotalAmount = lRewards.amount * mBetMultiplier;
                PlayerDataController.Controller.playerItems.Coins = mTotalAmount;
                break;
            case ReelFigures.eAttack:
                DOTween.KillAll();
                Invoke(nameof(AttackVillage), 1f);
                return;
            case ReelFigures.eRaid:
                DOTween.KillAll();
                Invoke(nameof(RaidVillage), 1f);
                return;
            default:

                break;
        }

        lRewards = lMatchType.rewards.Single(s => s.type.Equals("XP"));

        PlayerDataController.Controller.playerItems.Xp = lRewards.amount;

        Debug.Log("XP Reward : " + lRewards.amount);

        Debug.Log("PlayerItems.Xp : " + PlayerDataController.Controller.playerItems.Xp);

        PlayerDataController.Controller.IsNewLevelAchieved();

        Debug.Log("PlayerItems.Xp : " + PlayerDataController.Controller.playerItems.Xp);

        lLevel = PlayerDataController.Controller.GetLevel();

        mPlayerProgress.ShowData(lLevel.level, PlayerDataController.Controller.playerItems.Xp, lLevel.fromXP, lLevel.toXP);

        ShowData();
        
        if (PlayerDataController.Controller.mReelFigure == ReelFigures.eAttack || PlayerDataController.Controller.mReelFigure == ReelFigures.eCoin || PlayerDataController.Controller.mReelFigure == ReelFigures.eRaid)
        {
            Invoke(nameof(OnReelIdle), 0.1f);
            return;
        }

        
        Invoke(nameof(OnReelIdle), 0.1f);
    }

    void RaidVillage()
    {
        SceneManager.LoadScene("Loading");
    }

    void AttackVillage()
    {
        SceneManager.LoadScene("Loading");
    }

    #endregion
}
