using FishNet;
using FishNet.Object;
using FishNet.Transporting;
using TMPro;
using UnityEngine.UI;
public class UIControl : NetworkBehaviour
{
    public Button buttonHost, buttonClient;

    public TMP_InputField inputFieldAddress;

    private void Start()
    {
        inputFieldAddress.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        buttonHost.onClick.AddListener(ButtonHost);
        buttonClient.onClick.AddListener(ButtonClient);
    }

    public void ValueChangeCheck()
    {
        InstanceFinder.TransportManager.Transport.SetServerBindAddress(inputFieldAddress.text, IPAddressType.IPv4);
    }

    public void ButtonHost()
    {
        InstanceFinder.ServerManager.StartConnection();
        InstanceFinder.ClientManager.StartConnection();
    }


    public void ButtonClient()
    {
        InstanceFinder.ClientManager.StartConnection();
    }
}
