using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameSystem : NetworkBehaviour
{
    public static GameSystem Instance;

    private List<IngamePlayerController> players = new List<IngamePlayerController>();

    public void AddPlayer(IngamePlayerController player)
    {
        if(!players.Contains(player))
        {
            players.Add(player);
        }
    }

    public void RemovePlayer(IngamePlayerController player)
    {
        if (players.Contains(player))
        {
            players.Remove(player);
        }
    }

    private IEnumerator GameReady()
    {
        var manager = NetworkManager.singleton as GHNetworkManager;
        while (manager.roomSlots.Count != players.Count)
        {
            yield return null;
        }
        for (int i = 0; i < manager.hunterCount; i++)
        {
            var player = players[Random.Range(0, players.Count)];
            if(player.role != Role.Hunter)
            {
                NetworkIdentity hunterNetIdentity = player.GetComponent<NetworkIdentity>();
                GameObject hunterPrefab = manager.spawnPrefabs[1];

                NetworkConnectionToClient conn = hunterNetIdentity.connectionToClient;

                GameObject oldPlayer = conn.identity.gameObject;
                SetHunterPlayer(conn, hunterPrefab, oldPlayer);
                yield return new WaitForSeconds(0.005f);
                NetworkServer.Destroy(oldPlayer);
            } else
            {
                i--;
            }
        }
        yield return new WaitForSeconds(1f);
    }

    public List<IngamePlayerController> GetPlayerList()
    {
        return players;
    }

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {
            StartCoroutine(GameReady());
        }
    }

    public void SetHunterPlayer(NetworkConnectionToClient conn, GameObject hunterPrefab, GameObject oldPlayer)
    {
        Vector3 originalPos = oldPlayer.transform.position;
        RemovePlayer(oldPlayer.GetComponent<IngamePlayerController>());
        GameObject newHunter = Instantiate(hunterPrefab);
        NetworkServer.ReplacePlayerForConnection(conn, newHunter, true);
        newHunter.transform.position = originalPos;
        AddPlayer(newHunter.GetComponent<IngamePlayerController>());
    }
}
