using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIControl : MonoBehaviour
{
    public Button buttonHost, buttonClient;
    public ClientStartup clientStartup;

    public TMP_InputField inputFieldAddress;
    public TMP_InputField inputFieldName;
    public TMP_InputField inputFieldHostName;


    private void Start()
    {
        if (NetworkManager.singleton.networkAddress != "localhost") { inputFieldAddress.text = NetworkManager.singleton.networkAddress; }
        inputFieldAddress.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        inputFieldName.onValueChanged.AddListener(delegate { NameChangeCheck(); });
        inputFieldHostName.onValueChanged.AddListener(delegate { HostNameChangeCheck(); });
        buttonHost.onClick.AddListener(ButtonHost);
        buttonClient.onClick.AddListener(ButtonClient);
    }

    public void ValueChangeCheck()
    {
        NetworkManager.singleton.networkAddress = inputFieldAddress.text;
    }

    public void NameChangeCheck()
    {
        LocalStore.playerName = inputFieldName.text;
    }

    public void HostNameChangeCheck()
    {
        LocalStore.playerName = inputFieldHostName.text;
    }

    public void ButtonHost()
    {
        NetworkManager.singleton.StartHost();
    }


    public void ButtonClient()
    {
        Debug.Log("Client Button Active");
        clientStartup.StartPlayFabClient();
    }

}
