using UnityEngine;
using Mirror;

public class GHNetworkManager : NetworkRoomManager{

    public int minPlayerCount;
    public int hunterCount;

    public override void OnRoomServerConnect(NetworkConnectionToClient conn)
    {
        base.OnRoomServerConnect(conn);
    }
}
