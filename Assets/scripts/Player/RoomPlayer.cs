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

    /*
    [Command]
    public void CmdSetPlayerName(string name)
    {
        playerName = name;
        lobbyPlayerCharacter.playerName = name;
    }*/

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
        if(isLocalPlayer)
        {
            //CmdSetPlayerName(LocalStore.playerName);
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
