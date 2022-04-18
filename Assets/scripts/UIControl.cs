using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIControl : MonoBehaviour
{
    public GameObject MainScreen;
    public GameObject PanelStart;
    public GameObject PanelStop;
    public GameObject PanelJoin;

    public Button buttonHost, buttonClient;

    public TMP_InputField inputFieldAddress;
    public TMP_InputField inputFieldName;
    public TMP_InputField inputFieldHostName;


    private void Start()
    {
        //Update the canvas text if you have manually changed network managers address from the game object before starting the game scene
        if (NetworkManager.singleton.networkAddress != "localhost") { inputFieldAddress.text = NetworkManager.singleton.networkAddress; }

        //Adds a listener to the main input field and invokes a method when the value changes.
        inputFieldAddress.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        inputFieldName.onValueChanged.AddListener(delegate { NameChangeCheck(); });
        inputFieldHostName.onValueChanged.AddListener(delegate { HostNameChangeCheck(); });
        //Make sure to attach these Buttons in the Inspector
        buttonHost.onClick.AddListener(ButtonHost);
        buttonClient.onClick.AddListener(ButtonClient);
        //This updates the Unity canvas, we have to manually call it every change, unlike legacy OnGUI.
        SetupCanvas();
    }

    // Invoked when the value of the text field changes.
    public void ValueChangeCheck()
    {
        NetworkManager.singleton.networkAddress = inputFieldAddress.text;
    }

    public void NameChangeCheck()
    {
        LocalStore.instance.setPlayerName(inputFieldName.text);
    }

    public void HostNameChangeCheck()
    {
        LocalStore.instance.setPlayerName(inputFieldHostName.text);
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
        // Here we will dump majority of the canvas UI that may be changed.

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
