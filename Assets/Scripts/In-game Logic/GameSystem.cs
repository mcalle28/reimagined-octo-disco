using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using TMPro;
using FishNet.Object.Synchronizing;
using FishNet.Managing.Scened;
using FishNet.Connection;
using FishNet;
using System;

public class GameSystem : NetworkBehaviour
{
    [SerializeField]
    public static GameSystem Instance;

    [SyncVar]
    private bool winCondition = false;

    private int hunterCount = 0;

    private readonly List<IngamePlayerController> players = new List<IngamePlayerController>();

    [SerializeField]
    private TMP_Text victory_text;

    [SerializeField]
    private TMP_Text returning_text;

    [SerializeField]
    private SpawnPoints spawnPoints;

    public GameObject hunterPrefab, ghostPrefab;

    [SyncVar]
    private bool sceneHasLoaded = false;



    private void Update(){
        if (IsServer && winCondition) {
            ChangeSceneToRoom();
            winCondition = false;
        }
    }

    public void ChangeSceneToRoom()
    {
        SceneLoadData sld = new SceneLoadData("Room");
        sld.ReplaceScenes = ReplaceOption.All;
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
    }

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
        InstanceFinder.SceneManager.OnLoadEnd += GetPlayerData;
    }

    public void GetPlayerData(SceneLoadEndEventArgs args)
    {
        if (sceneHasLoaded || !IsServer) return;
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Hanging Corridor")
        {
            RoomPlayer[] roomPlayers = FindObjectsOfType<RoomPlayer>();
            foreach (RoomPlayer r in roomPlayers)
            {
                spawnPlayerCharacter(r);
            }
        }
        sceneHasLoaded = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (IsServer)
        {

            StartCoroutine(GameReady());
        }
    }

    
    public void spawnPlayerCharacter(RoomPlayer roomPlayer)
    {
        RoomPlayer rp = roomPlayer.GetComponent<RoomPlayer>();
        GameObject spawnPoint = spawnPoints.getSpawnPoint(rp.role);
        GameObject go;
        if (rp.role != Role.Hunter)
        {

            go = Instantiate(ghostPrefab,spawnPoint.transform.position,spawnPoint.transform.rotation);
        }
        else
        {
            hunterCount++;
            go = Instantiate(hunterPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
        InstanceFinder.ServerManager.Spawn(go, roomPlayer.Owner);
    }

    [ObserversRpc]
    public void RpcCheckHunterWinCon(IngamePlayerController newTarget)
    {
        int captured = 0;
        int ghostCount = players.Count - hunterCount;

        foreach (IngamePlayerController player in players)
        {
            if (player.role == Role.Ghost && player.isCaptured) {
                captured++;
            } else if ((newTarget.ObjectId == player.ObjectId) && !player.isCaptured){
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

    [ObserversRpc]
    public void RpcCheckGhostWinCon()
    {
        int captured = 0;
        int ghostCount = players.Count - hunterCount;

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
        returning_text.SetText("Returning Soon...");
        yield return new WaitForSeconds(10f);
        ReturnToRoomServer();

    }


    [ServerRpc(RequireOwnership = false)]
    private void ReturnToRoomServer()
    {
        winCondition = true;
    }
}
