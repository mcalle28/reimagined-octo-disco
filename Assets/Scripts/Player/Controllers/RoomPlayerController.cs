using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RoomPlayerController : PlayerControl
{
    [SyncVar (hook = nameof(SetOwnerNetId_Hook))]
    public uint ownerNetId;

    public void SetOwnerNetId_Hook(uint _, uint newOwnerId)
    {
        var players = FindObjectsOfType<RoomPlayer>();
        foreach (var player in players)
        {
            if(newOwnerId == player.netId)
            {
                player.lobbyPlayerCharacter = this;
                break;
            }
        }
    }

    public void CompleteSpawn()
    {
        if (hasAuthority)
        {
            isMoveable = true;
        }
    }
}
