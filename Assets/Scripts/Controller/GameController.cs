using SlotMachine;
using UnityEngine;

public enum MachineState
{
    Idle,
    Rotating,
    Stopping,
    Stopped,
    ShowingWin
}

public class GameController : MonoBehaviour
{
    public static GameController Controller;

    [Header("JsonData")]
    public TextAsset playerProgressJson;
    public TextAsset reelRewardsJson;

    [Header("Views")]
    public SlotMachineView mSlotMachineView;

    void Awake()
    {
        Controller = this;

        PlayerDataController.Controller.playerProgress = JsonUtility.FromJson<PlayerProgressModel>(playerProgressJson.text);

        PlayerDataController.Controller.reelRewardsModel = JsonUtility.FromJson<ReelRewardsModel>(reelRewardsJson.text);
        
    }
}
