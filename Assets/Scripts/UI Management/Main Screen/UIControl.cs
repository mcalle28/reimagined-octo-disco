using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIControl : MonoBehaviour
{
    public Button buttonHost, buttonClient;

    public TMP_InputField inputFieldAddress;

    private void Start()
    {
        if (NetworkManager.singleton.networkAddress != "localhost") { inputFieldAddress.text = NetworkManager.singleton.networkAddress; }
        inputFieldAddress.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        buttonHost.onClick.AddListener(ButtonHost);
        buttonClient.onClick.AddListener(ButtonClient);
        SetupCanvas();
    }

    public void ValueChangeCheck()
    {
        NetworkManager.singleton.networkAddress = inputFieldAddress.text;
    }

    public void ButtonHost()
    {
        NetworkManager.singleton.StartHost();
        SetupCanvas();
    }


    public void ButtonClient()
    {
        Debug.Log("Client Button Active");
        NetworkManager.singleton.StartClient();
        SetupCanvas();
    }

    public void SetupCanvas()
    {
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            if (NetworkClient.active)
            {
                Debug.Log(NetworkManager.singleton);
                Debug.Log("Server Start");
            }
            else
            {
                Debug.Log("Server Not connect");
            }
        }
        else
        {
            Debug.Log("Server Setting Active Lobby");
        }
    }
}
