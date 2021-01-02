using System.Collections.Generic;
using UnityEngine;
using SlotMachine;

public class MatchMaking
{
    [Header("List Player")]
    public List<PlayerVillage> mPlayers = new List<PlayerVillage>();

    [Header("Model")]
    PlayersModel PlayersModel;

    [Header("variables")]
    public static MatchMaking sInstance = null;

    public int mPlayerFrequency = 0;
    public int mSpins = 0;

    private MatchMaking()
    { 

    }

    public static MatchMaking GetInstance()
    {
        if (sInstance == null)
            return sInstance = new MatchMaking();
        else
            return sInstance;
    }

    /// <summary>
    /// loads the fake Player info to the Json
    /// </summary>
    /// <param name="aPlayerjson"></param>
    public void LoadPlayerjson(TextAsset aPlayerjson)
    {
        PlayersModel = JsonUtility.FromJson<PlayersModel>(aPlayerjson.text);
    }

    /// <summary>
    /// Gets random Player from the PlayerModel and return that Players details
    /// </summary>
    /// <returns></returns>
    public PlayerVillage GetPlayerForMatch()
    {
        int MaxRange = PlayersModel.PlayersList.Count;
        PlayerVillage PlayerFormatch = PlayersModel.PlayersList[Random.Range(1, MaxRange)];
        GetPlayerFrequency();
        return PlayerFormatch;
    }

    /// <summary>
    /// Sets a Random Frequency for change for the Selected Player
    /// </summary>
    void GetPlayerFrequency()
    {
        mPlayerFrequency = Random.Range(PlayersModel.MinimumMatchFrequency,PlayersModel.MaximumMatchFrequency);
        mSpins = 0;

        //Debug.Log("PlayerFrequency: " + mPlayerFrequency);
    }
}

