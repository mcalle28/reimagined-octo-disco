using System;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.Events;

public class OnlineManager : MonoBehaviour
{

    public static OnlineManager _instance;
    public static OnlineManager Instance => _instance;

    public TMP_Text playerID;
    public TMP_InputField username;
    public TMP_InputField code;
    public TMP_Text loggedPlayers;
    public TMP_Text joinCodeText;

    public GameObject oopsieScreen;
    public GameObject inScreen;
    public GameObject joinScreen;


    private RelayHostData _hostData;
    private RelayJoinData _joinData;

    private Guid hostAllocationId;
    private Guid playerAllocationId;
    private string allocationRegion = "";
    private string joinCode = "n/a";
    private string playerId = "Not signed in";
    private string _lobbyId;


    async void Start()
    {
        await UnityServices.InitializeAsync();
        NetworkManager.Singleton.OnClientConnectedCallback += ClientConnected;
    }


    private void ClientConnected(ulong id)
    {

        Debug.Log("Connected player with id: " + id);
        updateLobbyList(id.ToString());
    }


    private void updateLobbyList(string player)
    {
        Debug.Log("Se ha unido" + player);
        loggedPlayers.text =  "Players ingame \n :" + player;
    }

    //Logs In and creates a Match
    public async void createMatch() {
        //Needed for Log in on network services
        Debug.Log("Signing On As Host");
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        playerId = "Host Maria";
        //playerID.text = playerId;

        //Allocation
        Allocation allocation = await Relay.Instance.CreateAllocationAsync(6);
        hostAllocationId = allocation.AllocationId;
        allocationRegion = allocation.Region;

        _hostData = new RelayHostData
        {
            Key = allocation.Key,
            Port = (ushort)allocation.RelayServer.Port,
            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            ConnectionData = allocation.ConnectionData,
            IPv4Address = allocation.RelayServer.IpV4
        };


        Debug.Log("Host Allocation ID: " + hostAllocationId.ToString());

        //Creating join code
        try
        {
            joinCode = await RelayService.Instance.GetJoinCodeAsync(hostAllocationId);
            Debug.Log("Host - Got join code: " + joinCode);
            joinCodeText.text = "Code: " + joinCode;

            CreateLobbyOptions options = new CreateLobbyOptions();
            options.IsPrivate = false;
            options.Data = new Dictionary<string, DataObject>()
            {
                {
                    "joinCode", new DataObject(
                        visibility: DataObject.VisibilityOptions.Member,
                        value: joinCode)
                }
            };

            var lobby = await Lobbies.Instance.CreateLobbyAsync("Test", 6, options);
            StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));
            Debug.Log("Created lobby: " + lobby.Id);


            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
                _hostData.IPv4Address,
                _hostData.Port,
                _hostData.AllocationIDBytes,
                _hostData.Key,
                _hostData.ConnectionData);

            NetworkManager.Singleton.StartHost();
        }
        catch (RelayServiceException ex)
        {
            Debug.LogError(ex.Message + "\n" + ex.StackTrace);
        }

        

    }

    public async void joinMatch(){
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        playerId = username.text;
        Debug.Log("Signing On As "+ playerId);

        try
        {
            Debug.Log("Username :" + playerId + " está intentando entrar al match : " + code.text);
            Lobby lobby = await Lobbies.Instance.JoinLobbyByIdAsync("LvaixpVXTkHYVdZhp36ygZ");
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(code.text);

            _joinData = new RelayJoinData
            {
                Key = allocation.Key,
                Port = (ushort)allocation.RelayServer.Port,
                AllocationID = allocation.AllocationId,
                AllocationIDBytes = allocation.AllocationIdBytes,
                ConnectionData = allocation.ConnectionData,
                HostConnectionData = allocation.HostConnectionData,
                IPv4Address = allocation.RelayServer.IpV4,
            };


            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
                _joinData.IPv4Address,
                _joinData.Port,
                _joinData.AllocationIDBytes,
                _joinData.Key,
                _joinData.ConnectionData,
                _joinData.HostConnectionData);

            // Finally start the client
            NetworkManager.Singleton.StartClient();

            playerAllocationId = allocation.AllocationId;
            joinScreen.SetActive(false);
            inScreen.SetActive(true);
            Debug.Log("Client Allocation ID: " + playerAllocationId.ToString());
        }
        catch (RelayServiceException ex)
        {
            joinScreen.SetActive(false);
            oopsieScreen.SetActive(true);
            Debug.LogError(ex.Message + "\n" + ex.StackTrace);
        }
     }

    IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)
    {
        var delay = new WaitForSecondsRealtime(waitTimeSeconds);
        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }

    public struct RelayHostData
    {
        public string JoinCode;
        public string IPv4Address;
        public ushort Port;
        public Guid AllocationID;
        public byte[] AllocationIDBytes;
        public byte[] ConnectionData;
        public byte[] Key;
    }

    /// <summary>
    /// RelayHostData represents the necessary informations
    /// for a Host to host a game on a Relay
    /// </summary>
    public struct RelayJoinData
    {
        public string JoinCode;
        public string IPv4Address;
        public ushort Port;
        public Guid AllocationID;
        public byte[] AllocationIDBytes;
        public byte[] ConnectionData;
        public byte[] HostConnectionData;
        public byte[] Key;
    }
}
