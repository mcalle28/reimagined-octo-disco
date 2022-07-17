using FishNet;
using FishNet.Object;
using UnityEngine;
using UnityEngine.UI;
public class UIRoom : NetworkBehaviour
{
    public Button buttonDisconnect,buttonReady;

    public RoomManager roomManager;

    private void Start()
    {
        buttonDisconnect.onClick.AddListener(ButtonDisconnect);
        buttonReady.onClick.AddListener(ButtonReady);
    }
    public void ButtonDisconnect()
    {
        InstanceFinder.ClientManager.StopConnection();
        if (IsServer)
        {
            InstanceFinder.ServerManager.StopConnection(true);
        }
        roomManager.ChangeSceneToMain();
    }
    public void ButtonReady()
    {
        RoomPlayer player = RoomManager.Instance.GetMyRoomPlayer();
        player.ServerSetIsReady(true);
    }

}
