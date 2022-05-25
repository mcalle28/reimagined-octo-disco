using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class RoomPlayerController : PlayerControl
{
    /*
    [SyncVar (OnChange = nameof(SetOwnerObjectId_Hook))]
    public uint ownerObjectId;

    public void SetOwnerObjectId_Hook(uint _, uint newOwnerId)
    {
        var players = FindObjectsOfType<RoomPlayer>();
        foreach (var player in players)
        {
            if(newOwnerId == player.ObjectId)
            {
                player.lobbyPlayerCharacter = this;
                break;
            }
        }
    }
    */

    public void CompleteSpawn()
    {
        if (IsOwner)
        {
            isMoveable = true;
        }
    }
}
