using Mirror;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class IngamePlayerController : PlayerControl
{
    [SyncVar]
    public Role role;

    [SyncVar]
    public bool isCaptured = false;

    [SerializeField]
    private SpriteRenderer renderer;

    [SerializeField]
    private PlayerFinder playerFinder;

    private Volume v;
    private Vignette vg;

    public override void Start()
    {
        base.Start();

        if(hasAuthority && role == Role.Hunter)
        {
            v = FindObjectOfType<Volume>();
            v.profile.TryGet(out vg);
            vg.intensity.value = 0.6f;
            vg.smoothness.value = 0.6f;
        }
        GameSystem.Instance.AddPlayer(this);
    }

    [ClientRpc]
    public void RpcTeleport(Vector3 position)
    {
        transform.position = position;
    }

    [ClientRpc]
    public void RpcAnimateCapture()
    {
        animator.Play("capturing");
    }


    protected void FixedUpdate()
    {
        base.FixedUpdate();
        if (hasAuthority)
        {
            if (role == Role.Hunter)
            {
                DetectGhost();
            }
        }
    }

    private void DetectGhost()
    {
        if(playerFinder.targets.Count > 0)
        {
            CmdCatchGhost(playerFinder.GetFirstTarget().netId);
        }
    }

    [Command]
    private void CmdCatchGhost(uint targetNetId)
    {
        IngamePlayerController target = null;
        foreach(var player in GameSystem.Instance.GetPlayerList())
        {
            if(player.netId == targetNetId)
            {
                target = player;
            }
        }

        if(target != null && !target.isCaptured)
        {
            RpcTeleport(target.transform.position);
            RpcAnimateCapture();
            target.isCaptured = true;
            target.RpcGetCaught();
            GameSystem.Instance.RpcCheckHunterWinCon(target);
        }
    }

    [ClientRpc]
    private void RpcGetCaught()
    {
        if (hasAuthority)
        {
            int WispLayer = LayerMask.NameToLayer("Wisp");
            gameObject.layer = WispLayer;
            animator.SetBool("captured", true);
        }
        else {
            renderer.color = new Color(1f, 1f, 1f, 0);
        }
    }

}
