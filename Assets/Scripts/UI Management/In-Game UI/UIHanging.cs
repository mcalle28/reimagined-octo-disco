using Mirror;
using UnityEngine.UI;
public class UIHanging : NetworkBehaviour
{
    public Button buttonRoom;

    private void Start()
    {
        buttonRoom.onClick.AddListener(ButtonRoom);
    }

    public void ButtonRoom()
    {
        var manager = NetworkManager.singleton as GHNetworkManager;
        manager.ServerChangeScene(manager.RoomScene);
     }

}

