using UnityEngine;
using Mirror;

public class GHNetworkManager : NetworkRoomManager{

    public GameRuleData gameRuleData;

    public int minPlayerCount;
    public int hunterCount = 1;

    public override void OnRoomServerConnect(NetworkConnectionToClient conn)
    {
        base.OnRoomServerConnect(conn);
    }

    public override void OnRoomServerPlayersReady()
    {
        gameRuleData = FindObjectOfType<GameRuleStore>().GetGameRuleData();
        base.OnRoomServerPlayersReady();
    }
}
