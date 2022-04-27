using Mirror;
using UnityEngine;

public class IngamePlayerController : PlayerControl
{
    [SyncVar]
    public Role role;

    [SyncVar]
    public bool isCaptured = false;

    private float captureRange = 0.4f;

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
        playerName = roomPlayerName;
    }

    /*
    public void Update()
    {
        base.Update();
        if (hasAuthority)
        {
            if(role == Role.Hunter)
            {
                DetectGhost();
            }
            //animator.SetBool("capturing", false);
        }
    }

    private void DetectGhost()
    {
        foreach (IngamePlayerController ghost in GameSystem.Instance.GetPlayerList())
        {
            if (ghost.role == Role.Ghost && ghost.isCaptured == false &&
                Vector3.Distance(ghost.transform.position, transform.position) <= captureRange)
            {
                CmdCatchGhost(ghost);
            }
        }
    }

    [ClientRpc]
    public void RpcTeleport(Vector3 position)
    {
        transform.position = position;
    }

    [Command]
    private void CmdCatchGhost(IngamePlayerController target)
    {
        RpcTeleport(target.gameObject.transform.position);
        animator.SetBool("capturing", true);
        RpcGetCaught(target);
    }

    [ClientRpc]
    private void RpcGetCaught(IngamePlayerController target)
    {
        target.isCaptured = true;
        target.gameObject.transform.GetComponent<Collider2D>().isTrigger = false;
        Color color = target.gameObject.transform.GetComponent<SpriteRenderer>().color;
        color.a = 150;
        target.gameObject.transform.GetComponent<SpriteRenderer>().color = color;
    }
    */
}
