using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Transporting;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIControl : NetworkBehaviour
{
    public Button buttonHost, buttonClient;

    public TMP_InputField inputFieldAddress;

    public ConnManager connManager;


    private void Start()
    {
        inputFieldAddress.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        buttonHost.onClick.AddListener(ButtonHost);
        buttonClient.onClick.AddListener(ButtonClient);
    }

    public void ValueChangeCheck()
    {
        connManager.SetAddress(inputFieldAddress.text);
    }

    public void ButtonHost()
    {
        connManager.StartHost();
    }

     
    public void ButtonClient()
    {
        connManager.StartClient();
    }

}
