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
                player.role = Role.Hunter;
                GameObject hunterPrefab = manager.spawnPrefabs[1];
                player.spriteRenderer = hunterPrefab.GetComponent<SpriteRenderer>();
                player.animator.runtimeAnimatorController = hunterPrefab.GetComponent<Animator>().runtimeAnimatorController;
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
        StartCoroutine(GameReady());
    }
}
