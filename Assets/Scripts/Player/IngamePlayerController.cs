using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class IngamePlayerController : PlayerControl
{
    [SyncVar]
    public Role role;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        if (hasAuthority)
        {
            var myRoomPlayer = RoomPlayer.MyRoomPlayer;

            CmdSetPlayerCharacter(myRoomPlayer.playerName);
        }
        GameSystem.Instance.AddPlayer(this);
    }

    [Command]
    private void CmdSetPlayerCharacter(string roomPlayerName)
    {
        this.playerName = roomPlayerName;
    }
}
