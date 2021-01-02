using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RaidVillageView : MonoBehaviour
{
    [Header("PlayerVillageView")]
    public PlayerVillageView playerVillageView;

    [Header("UI")]
    public TextAnimationView stoleView;
    public TextMeshProUGUI stoleVillageText;
    public Image[] raidUI;
    public Image[] digUI;
    public Image[] attackUI;
    public GameObject congratsUI;
    public GameObject stoleUI;
    public GameObject attackObjects;
    public GameObject headerObj;
    public GameObject attack_UI;

    private int lCounter = 3;
    private int rewardCoins = 0;

    private readonly string stoleVillageString = "You Stole {0} Coins from {1}.";
    private readonly string attackVillageString = "You attacked {0}'s village and won {1}.";

    private void Start()
    {
        playerVillageView.SetPlayer(PlayerDataController.Controller.playerVillage);

        PlayerDataController.Controller.playerVillage.SetRandomPrice();

        attackObjects.SetActive(PlayerDataController.Controller.mReelFigure == SlotMachine.ReelFigures.eAttack);

        headerObj.SetActive(PlayerDataController.Controller.mReelFigure == SlotMachine.ReelFigures.eRaid);

        attack_UI.SetActive(PlayerDataController.Controller.mReelFigure == SlotMachine.ReelFigures.eAttack);

        if (PlayerDataController.Controller.mReelFigure == SlotMachine.ReelFigures.eAttack)
        {
            foreach (Image go in attackUI)
                go.enabled = true;

            foreach (Image go in digUI)
                go.enabled = false;

            foreach (Image go in raidUI)
                go.enabled = false;
        }
        else if(PlayerDataController.Controller.mReelFigure == SlotMachine.ReelFigures.eRaid)
        {
            foreach (Image go in digUI)
                go.enabled = true;

            foreach (Image go in attackUI)
                go.enabled = false;
        }
    }

    public void DigClick(int digPlace)
    {
        if (lCounter == 0)
            return;

        stoleUI.SetActive(true);

        digUI[digPlace].enabled = false;

        lCounter--;

        raidUI[lCounter].enabled = false;

        int coins = PlayerDataController.Controller.playerVillage.randomPrice[digPlace];

        PlayerDataController.Controller.playerVillage.Coins = coins;

        playerVillageView.SetPlayer(PlayerDataController.Controller.playerVillage);

        PlayerDataController.Controller.playerItems.Coins = coins;

        rewardCoins += coins;

        stoleView.ShowValue(rewardCoins);

        if(lCounter == 0)
        {
            foreach (Image go in digUI)
                go.enabled = false;

            congratsUI.SetActive(true);

            stoleVillageText.text = string.Format(stoleVillageString, rewardCoins.ToString("#,#"), PlayerDataController.Controller.playerVillage.PlayerName);
        }
    }

    public void AttackClick(int attackPlace)
    {
        foreach (Image go in attackUI)
            go.enabled = false;

        int coins = PlayerDataController.Controller.playerVillage.randomPrice[attackPlace];

        PlayerDataController.Controller.playerItems.Coins = coins;

        attack_UI.SetActive(false);

        congratsUI.SetActive(true);

        stoleVillageText.text = string.Format(attackVillageString, PlayerDataController.Controller.playerVillage.PlayerName, coins.ToString("#,#"));
    }

    public void OkClick()
    {
        SceneManager.LoadScene("GamePlay");
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}
