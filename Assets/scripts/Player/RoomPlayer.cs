using Mirror;

public class RoomPlayer : NetworkRoomPlayer {

    private static RoomPlayer myRoomPlayer;

    public static RoomPlayer MyRoomPlayer
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
    }

    [SyncVar]
    public string playerName;

    public RoomPlayerController lobbyPlayerCharacter;

    public void Start()
    {
        base.Start();

        if(isServer)
        {
            spawnLobbyPlayer();
        }
    }

    public override void OnGUI() {
        base.OnGUI();
    }

    public void spawnLobbyPlayer()
    {
        var player = Instantiate(NetworkManager.singleton.spawnPrefabs[0]).GetComponent<RoomPlayerController>();
        NetworkServer.Spawn(player.gameObject, connectionToClient);
        player.ownerNetId = netId;
        lobbyPlayerCharacter.CompleteSpawn();
    }
}
