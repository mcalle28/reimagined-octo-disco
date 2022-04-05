using DilmerGames.Core.Singletons;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;


public class RelayManager : Singleton<RelayManager>
{
    [SerializeField]
    private string environment = "production";
    [SerializeField]
    private int maxConnections = 7;

    public bool IsRelayEnabled => Transport != null && Transport.Protocol == UnityTransport.ProtocolType.RelayUnityTransport;
    public UnityTransport Transport => NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();


    public async Task<RelayHostData> SetupRelay(){

        Logger.Instance.LogInfo($"Relay Server Starting With Max Connections: {maxConnections}");

        InitializationOptions options = new InitializationOptions().SetEnvironmentName(environment);
        await UnityServices.InitializeAsync(options);

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        Allocation allocation = await Unity.Services.Relay.Relay.Instance.CreateAllocationAsync(maxConnections);

        RelayHostData data = new RelayHostData
        {
            Key = allocation.Key,
            Port = (ushort)allocation.RelayServer.Port,
            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            ConnectionData = allocation.ConnectionData,
            IPv4Address = allocation.RelayServer.IpV4
        };

        data.JoinCode = await Unity.Services.Relay.Relay.Instance.GetJoinCodeAsync(data.AllocationID);

        Transport.SetRelayServerData(data.IPv4Address, data.Port, data.AllocationIDBytes,
                data.Key, data.ConnectionData);

        Logger.Instance.LogInfo($"Relay Server Generated Join Code: {data.JoinCode}");
        Debug.Log($"Relay Server Generated Join Code: {data.JoinCode}");

        return data;
    }

    public async Task<RelayJoinData> JoinRelay(string joinCode)
    {
        Logger.Instance.LogInfo($"Client Joining Game With Join Code: {joinCode}");
        await UnityServices.InitializeAsync();
        //Always autheticate your users beforehand
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            //If not already logged, log the user in
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        //Ask Unity Services for allocation data based on a join code
        JoinAllocation allocation = await Unity.Services.Relay.Relay.Instance.JoinAllocationAsync(joinCode);

        //Populate the joining data
        RelayJoinData data = new RelayJoinData
        {
            Key = allocation.Key,
            Port = (ushort)allocation.RelayServer.Port,
            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            ConnectionData = allocation.ConnectionData,
            HostConnectionData = allocation.HostConnectionData,
            IPv4Address = allocation.RelayServer.IpV4
        };

        Transport.SetRelayServerData(data.IPv4Address, data.Port, data.AllocationIDBytes,
           data.Key, data.ConnectionData, data.HostConnectionData);

        Logger.Instance.LogInfo($"Client Joined Game With Join Code: {joinCode}");

        return data;
    }
}