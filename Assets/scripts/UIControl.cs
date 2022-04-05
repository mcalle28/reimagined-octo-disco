using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public GameObject Lobby,MainScreen;
    public GameObject PanelStart;
    public GameObject PanelStop;
    public GameObject PanelJoin;

    public Button buttonHost, buttonClient;

    public TMP_InputField inputFieldAddress;


    private void Start()
    {
        //Update the canvas text if you have manually changed network managers address from the game object before starting the game scene
        if (NetworkManager.singleton.networkAddress != "localhost") { inputFieldAddress.text = NetworkManager.singleton.networkAddress; }

        //Adds a listener to the main input field and invokes a method when the value changes.
        inputFieldAddress.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
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
                Debug.Log("Server Start");
                PanelStart.SetActive(false);
                MainScreen.SetActive(false);
                PanelJoin.SetActive(false);
                Lobby.SetActive(true);
                PanelStop.SetActive(true);
            }
            else
            {
                Debug.Log("Server Not connect");

                PanelStart.SetActive(true);
                PanelJoin.SetActive(false);
                MainScreen.SetActive(true);
                PanelStop.SetActive(false);
                Lobby.SetActive(false);
            }
        }
        else
        {
            PanelStart.SetActive(false);
            PanelJoin.SetActive(false);
            MainScreen.SetActive(false);
            Lobby.SetActive(true);
            PanelStop.SetActive(true);

            Debug.Log("Server Setting Active Lobby");
        }
    }
}
