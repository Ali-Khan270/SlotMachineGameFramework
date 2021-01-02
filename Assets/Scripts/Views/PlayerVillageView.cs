using SlotMachine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerVillageView : MonoBehaviour
{
    [Header("PlayerUI")]
    public Image mPlayerImage;
    public TextMeshProUGUI mPlayerName;
    public TextMeshProUGUI mPlayerLevel;
    public TextMeshProUGUI mPlayerCoins;

    [Header("Model")]
    public PlayerVillage playerVillage;

    public void SetPlayer(PlayerVillage aPlayer)
    {
        playerVillage = aPlayer;

        mPlayerImage.sprite = Resources.Load<Sprite>(aPlayer.PlayerImage);
        mPlayerName.text = aPlayer.PlayerName;
        mPlayerLevel.text = "Level " + aPlayer.PlayerLevel.ToString();
        mPlayerCoins.text = aPlayer.Coins.ToString("#,#");
    }
}
