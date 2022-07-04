using FishNet.Object;
using TMPro;
using UnityEngine.UI;

public class MainScreenUI : NetworkBehaviour
{
    public Button buttonHost, buttonClient;

    public TMP_InputField inputFieldAddress;

    public MainManager mainManager;


    private void Start()
    {
        inputFieldAddress.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        buttonHost.onClick.AddListener(ButtonHost);
        buttonClient.onClick.AddListener(ButtonClient);
    }

    public void ValueChangeCheck()
    {
        mainManager.SetAddress(inputFieldAddress.text);
    }

    public void ButtonHost()
    {
        mainManager.StartHost();
    }

     
    public void ButtonClient()
    {
        mainManager.StartClient();
    }

}
