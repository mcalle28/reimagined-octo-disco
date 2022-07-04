using UnityEngine;
using FishNet.Object;
using FishNet.Managing;
using FishNet.Object.Synchronizing;
using System.Linq;

public class RoomManager : NetworkBehaviour {
    
    public GameRuleData gameRuleData;
    public int minPlayerCount;
    public int hunterCount = 1;

    public static RoomManager Instance { get; private set; }

    [SyncObject]
    public readonly SyncList<RoomPlayer> players = new SyncList<RoomPlayer>();

    [SyncVar]
    public bool canStart=false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!IsServer)return;

        canStart = players.All(player => player.ready);

        Debug.Log(canStart);
    }

    public RoomPlayer GetMyRoomPlayer()
    {
        foreach(var player in players)
        {
            Debug.Log(player);
            if (player.IsOwner) return player;
        }
        return null;
    }
}
