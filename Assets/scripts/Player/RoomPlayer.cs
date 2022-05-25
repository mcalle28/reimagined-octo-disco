using FishNet.Object;
using FishNet.Object.Synchronizing;

public class RoomPlayer : NetworkBehaviour {

    private static RoomPlayer myRoomPlayer;

    /*public static RoomPlayer MyRoomPlayer
    {
        get
        {
            if(myRoomPlayer == null)
            {
                var players = FindObjectsOfType<RoomPlayer>();
                foreach (var player in players)
                {
                    if(player.hasAuthority)
                    {
                        myRoomPlayer = player;
                    }
                }
            }
            return myRoomPlayer;
        }
    }*/

    [SyncVar]
    public string playerName;

    [SyncVar]
    public Role role;

    public RoomPlayerController lobbyPlayerCharacter;

    public void Start()
    {
        /*
        base.Start();

        if(base.IsServer)
        {
            spawnLobbyPlayer();
        }*/
    }

    /*
    public override void OnGUI() {
        base.OnGUI();
    }

    public void spawnLobbyPlayer()
    {
        var player = Instantiate(NetworkManager.singleton.spawnPrefabs[0]).GetComponent<RoomPlayerController>();
        NetworkServer.Spawn(player.gameObject, connectionToClient);
        player.ownerObjectId = ObjectId;
        lobbyPlayerCharacter.CompleteSpawn();
    }*/
}
