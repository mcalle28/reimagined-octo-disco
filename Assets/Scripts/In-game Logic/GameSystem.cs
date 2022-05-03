using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class GameSystem : NetworkBehaviour
{
    [SerializeField]
    public static GameSystem Instance;

    private List<IngamePlayerController> players = new List<IngamePlayerController>();

    [SerializeField]
    private GameObject hunterSpawnPoint;

    [SerializeField]
    private TMP_Text victory_text;

    [SerializeField]
    private TMP_Text captured_count;

    [SerializeField]
    private TMP_Text ghost_count;

    [SerializeField]
    private TMP_Text player_count;

    [SerializeField]
    private TMP_Text hunter_count;

    public void AddPlayer(IngamePlayerController player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);
        }
    }

    [ClientRpc]
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
            if (player.role != Role.Hunter)
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
            RemovePlayer(oldPlayer.GetComponent<IngamePlayerController>());
            GameObject newHunter = Instantiate(hunterPrefab, hunterSpawnPoint.transform.position, hunterSpawnPoint.transform.rotation);
            NetworkServer.ReplacePlayerForConnection(conn, newHunter, true);
            AddPlayer(newHunter.GetComponent<IngamePlayerController>());
    }

    [ClientRpc]
    public void RpcCheckHunterWinCon(IngamePlayerController newTarget)
    {
        var manager = NetworkManager.singleton as GHNetworkManager;
        player_count.SetText("Player: " + players.Count.ToString());
        hunter_count.SetText("Hunter: " + manager.hunterCount.ToString());
        int ghostCount = players.Count - manager.hunterCount;
        ghost_count.SetText("Ghost: " + ghostCount.ToString());
        int captured = 0;

        foreach (IngamePlayerController player in players)
        {
            if (player.role == Role.Ghost && player.isCaptured) {
                captured++;
                captured_count.SetText("Captured: " + captured.ToString());
            } else if ((newTarget.netId == player.netId) && !player.isCaptured){
                player.isCaptured = true;
                captured++;
                captured_count.SetText("Captured: " + captured.ToString());
            }
        }

        Debug.Log("Captured :" + captured + " Ghost Count: " + ghostCount);
        if (captured == ghostCount)
        {
            victory_text.SetText("Hunters Win!!");
        }
    }

    [ClientRpc]
    public void RpcCheckGhostWinCon()
    {
        var manager = NetworkManager.singleton as GHNetworkManager;
        int ghostCount = players.Count - manager.hunterCount;

        int captured = 0;

        foreach (IngamePlayerController player in players)
        {
            if (player.role == Role.Ghost && player.isCaptured) captured++;
        }

        if (captured != ghostCount)
        {
            Debug.Log("Timer Condition Meet");
            victory_text.SetText("Ghosts Win!!");
        }
    }
}
