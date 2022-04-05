using DilmerGames.Core.Singletons;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Relay;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{

    [SerializeField]
    private Button joinButton;

    [SerializeField]
    private Button createButton;

    [SerializeField]
    private TextMeshProUGUI playersInGameText;

    [SerializeField]
    private TMP_InputField joinCodeInput;

    [SerializeField]
    private RelayHostData relaycreated;

    [SerializeField]
    public TMP_Text joinCodeText;
    public GameObject oopsieScreen;
    public GameObject inScreen;
    public GameObject joinScreen;



    void Update()
    {
        playersInGameText.text = $"Players in game: {PlayersManager.Instance.PlayersInGame}";
    }

    void Start(){  

        createButton?.onClick.AddListener(async () =>
        {
            if (RelayManager.Instance.IsRelayEnabled)
                relaycreated = await RelayManager.Instance.SetupRelay();

            if (NetworkManager.Singleton.StartHost()) {

                Logger.Instance.LogInfo("Host started...");
                joinCodeText.text = relaycreated.JoinCode.ToString();
                SceneManager.LoadScene(sceneName: "dungeon level");
            }
            else
                Logger.Instance.LogInfo("Unable to start host...");
        });

        joinButton?.onClick.AddListener(async () =>
        {
            try
            {
                if (RelayManager.Instance.IsRelayEnabled && !string.IsNullOrEmpty(joinCodeInput.text))
                    await RelayManager.Instance.JoinRelay(joinCodeInput.text);
                joinScreen.SetActive(false);
                inScreen.SetActive(true);
                SceneManager.LoadScene(sceneName: "dungeon level");
            }
            catch (RelayServiceException ex)
            {
                joinScreen.SetActive(false);
                oopsieScreen.SetActive(true);
                Logger.Instance.LogError("Wrong Code Maybe?...");
            }

            if (NetworkManager.Singleton.StartClient())
            {
                Logger.Instance.LogInfo("Client started...");
            }
            else {
                Logger.Instance.LogError("Unable to start client...");
            }
                
        });

        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            Logger.Instance.LogInfo($"{id} just connected...");
        };

    }


}
