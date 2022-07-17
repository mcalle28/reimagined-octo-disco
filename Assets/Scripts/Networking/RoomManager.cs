using UnityEngine;
using FishNet.Object;
using FishNet.Managing;
using FishNet.Object.Synchronizing;
using System.Linq;
using FishNet.Managing.Scened;
using FishNet;
using System.Collections.Generic;

public class RoomManager : NetworkBehaviour {
    
    public GameRuleData gameRuleData;
    public int minPlayerCount;
    public int hunterCount = 1;
    private bool starting = false;

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
        if (players.Count == 0) return;
        canStart = players.All(player => player.ready);
        if (canStart && !starting)
        {
            starting = true;
            assignRoles();
            ChangeSceneToGame();
        }
    }

    public RoomPlayer GetMyRoomPlayer()
    {
        foreach(var player in players)
        {
            if (player.IsOwner) return player;
        }
        return null;
    }

    public void ChangeSceneToGame()
    {
        List<NetworkObject> loadData = new List<NetworkObject>();
        string[] scenes = new string[1];
        foreach (var player in players)
        {
            loadData.Add(player.GetComponent<NetworkObject>());
        }
        scenes[0] = "Hanging Corridor";
        SceneLoadData sld = new SceneLoadData(scenes, loadData.ToArray());
        object[] roomParams = new object[] { players };
        LoadParams loadParams = new LoadParams { ServerParams = roomParams };
        sld.Params = loadParams;
        sld.ReplaceScenes = ReplaceOption.All;
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
    }

    public void assignRoles()
    {
        foreach (RoomPlayer rp in players)
        {
            rp.role = Role.Ghost;
        }
        gameRuleData = FindObjectOfType<GameRuleStore>().GetGameRuleData();
        for (int i = 0; i < hunterCount; i++)
        {
            var player = players[Random.Range(0, players.Count)];
            if (player.role != Role.Hunter)
            {
                player.role = Role.Hunter;
            }
            else
            {
                i--;
            }
        }
    }
}
