using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingManager : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Player Count on Hanging: " + RoomManager.Instance.players.Count +" with name " + RoomManager.Instance.players[0].name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
