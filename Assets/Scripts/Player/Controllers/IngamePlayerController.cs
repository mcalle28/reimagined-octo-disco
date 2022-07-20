using System.Collections;
using FishNet.Object;
using FishNet.Object.Synchronizing;
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

        if (IsClient)
        {
            v = FindObjectOfType<Volume>();
            v.profile.TryGet(out vg);
            if (role == Role.Hunter)
            {
                vg.intensity.value = 0.6f;
                vg.smoothness.value = 0.6f;
            }
            else if (role == Role.Ghost)
            {
                vg.active = false;

            }
            InGameUIManager.Instance.GhostAbilityBtn.LinkPlayer(this);
            InGameUIManager.Instance.GhostAbilityBtn.Show(true);
        }

        GameSystem.Instance.AddPlayer(this);
    }

    [ObserversRpc]
    public void RpcTeleport(Vector3 position)
    {
        transform.position = position;
    }

    [ObserversRpc]
    public void RpcAnimateCapture()
    {
        animator.Play("capturing");
    }


    protected void FixedUpdate()
    {
        base.FixedUpdate();
        if (base.IsOwner)
        {
            if (role == Role.Hunter)
            {
                DetectGhost();
            }
        }
    }

    private void DetectGhost()
    {
        if (playerFinder.targets.Count > 0)
        {
            CmdCatchGhost(playerFinder.GetFirstTarget().ObjectId);
        }
    }

    [ServerRpc]
    private void CmdCatchGhost(int targetObjectId)
    {
        IngamePlayerController target = null;
        foreach (var player in GameSystem.Instance.GetPlayerList())
        {
            if (player.ObjectId == targetObjectId)
            {
                target = player;
            }
        }

        if (target != null && !target.isCaptured)
        {
            RpcTeleport(target.transform.position);
            RpcAnimateCapture();
            target.isCaptured = true;
            target.RpcGetCaught();
            GameSystem.Instance.RpcCheckHunterWinCon(target);
        }
    }

    [ObserversRpc]
    private void RpcGetCaught()
    {
        if (base.IsOwner)
        {
            int WispLayer = LayerMask.NameToLayer("Wisp");
            gameObject.layer = WispLayer;
            animator.SetBool("captured", true);
        }
        else
        {
            renderer.color = new Color(1f, 1f, 1f, 0);
        }
    }

    [ServerRpc]
    public void CmdScreamAbility()
    {
        foreach (var player in GameSystem.Instance.GetPlayerList())
        {
            if (player.role == Role.Hunter)
            {
                player.RpcScreamAbility();
            }
        }
    }

    [ObserversRpc]
    private void RpcScreamAbility()
    {
        if (base.IsOwner && role == Role.Hunter)
        {
            StartCoroutine(StunnedTimer(2f));
        }
    }

    private IEnumerator StunnedTimer(float secs)
    {
        isMoveable = false;
        yield return new WaitForSeconds(secs);
        isMoveable = true;
    }
}