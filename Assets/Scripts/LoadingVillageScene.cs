using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingVillageScene : MonoBehaviour
{
    public TextMeshProUGUI text;

    private readonly string loadingString = "Loading {0} Village...";

    IEnumerator Start()
    {
        text.text = string.Format(loadingString, PlayerDataController.Controller.playerVillage.PlayerName);

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("Village");
    }
}
