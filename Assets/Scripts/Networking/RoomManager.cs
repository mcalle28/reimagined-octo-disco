using UnityEngine;
using FishNet.Object;
using FishNet.Managing;

public class RoomManager : NetworkBehaviour {
    
    public GameRuleData gameRuleData;

    public int minPlayerCount;
    public int hunterCount = 1;

    /*
    public override void OnRoomServerConnect(NetworkConnectionToClient conn)
    {
        base.OnRoomServerConnect(conn);
    }

    public override void OnRoomServerPlayersReady()
    {
        foreach (RoomPlayer rp in  roomSlots)
        {
            rp.role = Role.Ghost;
        }
        gameRuleData = FindObjectOfType<GameRuleStore>().GetGameRuleData();
        for (int i = 0; i < hunterCount; i++)
        {
            var player = roomSlots[Random.Range(0, roomSlots.Count)] as RoomPlayer;
            if (player.role != Role.Hunter)
            {
                player.role = Role.Hunter;
            } else
            {
                i--;
            }
        }
        base.OnRoomServerPlayersReady();
    }

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        Transform startPos = GetStartPosition();
        HunterSpawnPoint hunterSpawnPoint = FindObjectOfType<HunterSpawnPoint>();
        RoomPlayer rp = roomPlayer.GetComponent<RoomPlayer>();
        GameObject gamePlayer;
        if (rp.role != Role.Hunter)
        {
            gamePlayer = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        } else
        {
            gamePlayer = hunterSpawnPoint != null
            ? Instantiate(spawnPrefabs[1], hunterSpawnPoint.transform.position, hunterSpawnPoint.transform.rotation)
            : Instantiate(spawnPrefabs[1], Vector3.zero, Quaternion.identity);
        }

        return gamePlayer;
    }*/
}
