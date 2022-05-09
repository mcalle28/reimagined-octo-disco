using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class GameSystem : NetworkBehaviour
{
    [SerializeField]
    public static GameSystem Instance;

    private readonly List<IngamePlayerController> players = new List<IngamePlayerController>();

    [SerializeField]
    private TMP_Text victory_text;

    public void AddPlayer(IngamePlayerController player)
    {
        if (!players.Contains(player))
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

    [ClientRpc]
    public void RpcCheckHunterWinCon(IngamePlayerController newTarget)
    {
        var manager = NetworkManager.singleton as GHNetworkManager;
        int captured = 0;
        int ghostCount = players.Count - manager.hunterCount;

        foreach (IngamePlayerController player in players)
        {
            if (player.role == Role.Ghost && player.isCaptured) {
                captured++;
            } else if ((newTarget.netId == player.netId) && !player.isCaptured){
                player.isCaptured = true;
                captured++;
            }
        }

        if (captured == ghostCount)
        {
            victory_text.SetText("Hunters Win!!");
            ChangeToRoom();
        }
    }

    [ClientRpc]
    public void RpcCheckGhostWinCon()
    {
        var manager = NetworkManager.singleton as GHNetworkManager;
        int captured = 0;
        int ghostCount = players.Count - manager.hunterCount;

        foreach (IngamePlayerController player in players)
        {
            if (player.role == Role.Ghost && player.isCaptured) captured++;
        }

        if (captured != ghostCount)
        {
            victory_text.SetText("Ghosts Win!");
            ChangeToRoom();
        }
    }

    private void ChangeToRoom()
    {
        StartCoroutine(ReturnToRoomWait());

    }


    private IEnumerator ReturnToRoomWait()
    {
            yield return new WaitForSeconds(10f);
            if (isServer) ReturnToRoomServer();

    }

    [ClientRpc]
    private void ReturnToRoomServer()
    {
        var manager = NetworkManager.singleton as GHNetworkManager;
        manager.ServerChangeScene(manager.RoomScene);
    }
}
