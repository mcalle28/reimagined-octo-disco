using FishNet.Object;
using UnityEngine.UI;
public class UIRoom : NetworkBehaviour
{
    public Button buttonDisconnect;

    private void Start()
    {
        buttonDisconnect.onClick.AddListener(ButtonDisconnect);
    }
    public void ButtonDisconnect()
    {
        NetworkManager.singleton.StopHost();
    }

}
