using DilmerGames.Core.Singletons;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine;
using Unity.Services.Relay;
using UnityEngine.SceneManagement;

public class PlayersManager : NetworkSingleton<PlayersManager>
{
    NetworkVariable<int> playersInGame = new NetworkVariable<int>();
    public GameObject playerPrefab,hunterPrefab;

    public int PlayersInGame
    {
        get
        {
            return playersInGame.Value;
        }
    }

    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
                playersInGame.Value++;
                GameObject go = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
                go.GetComponent<NetworkObject>().SpawnWithOwnership(id);
                DontDestroyOnLoad(go);
        };

        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
                playersInGame.Value--;
        };
    }

}