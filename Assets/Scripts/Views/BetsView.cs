using System.Collections.Generic;
using UnityEngine;
using SlotMachine;
using TMPro;

public class BetsView : MonoBehaviour
{
    [Header("SlotMachineState")]
    public MachineState mState;

    [Header("Json")]
    public TextAsset BetsJson;

    [Header ("Model")]
    public BetsModel betsModel;

    [Header ("GameUI")]
    public TextMeshProUGUI mBetstext;

    private List<string> mBetsList = new List<string>();
    private int mCurrentBet = 1;
    private int CurrentBetIndex=0;

    // Start is called before the first frame update
    void Start()
    {
        betsModel = JsonUtility.FromJson<BetsModel>(BetsJson.text);
        PopulateBets();
    }

    //Populates the Bets List and Updates the Bets Button Text
    void PopulateBets()
    {
        foreach(var v in betsModel.mBets)
        {
            mBetsList.Add(v.bets.ToString());
        }
        mBetstext.text = ("BET x"+mBetsList[CurrentBetIndex]).ToString();
    }

    /// <summary>
    /// Return Current Bet Amount Which is Selected
    /// </summary>
    /// <returns></returns>
    public int GetBetsAmount() 
    {
        string[] mCurrentbetSplit = mBetstext.text.Split('x');
        mCurrentBet = int.Parse(mCurrentbetSplit[1]);
        return mCurrentBet;
    }

    /// <summary>
    /// Called From the Bets Button to Update
    /// </summary>
    public void OnButtonClick()
    {
        if (mState != MachineState.Idle) return;
        if(CurrentBetIndex < mBetsList.Count-1)
        {
            CurrentBetIndex++;
        }
        else
        {
            CurrentBetIndex = 0;
        }
        mBetstext.text = ("BET x" + mBetsList[CurrentBetIndex]).ToString();
        mCurrentBet = GetBetsAmount();
        Debug.Log("Current Bet :" + mCurrentBet);
    }
}
