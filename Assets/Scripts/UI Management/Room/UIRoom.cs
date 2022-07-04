using FishNet.Object;
using UnityEngine;
using UnityEngine.UI;
public class UIRoom : NetworkBehaviour
{
    public Button buttonDisconnect,buttonReady;

    private void Start()
    {
        buttonDisconnect.onClick.AddListener(ButtonDisconnect);
        buttonReady.onClick.AddListener(ButtonReady);
    }
    public void ButtonDisconnect()
    {
        //NetworkManager.singleton.StopHost();
    }
    public void ButtonReady()
    {
        RoomPlayer player = RoomManager.Instance.GetMyRoomPlayer();
        player.ServerSetIsReady(true);
    }

}
