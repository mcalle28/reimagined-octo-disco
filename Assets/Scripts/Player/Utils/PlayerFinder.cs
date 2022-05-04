using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFinder : MonoBehaviour
{

    private CircleCollider2D playerFinder;
    public List<IngamePlayerController> targets = new List<IngamePlayerController>();

    private void Awake()
    {
        playerFinder = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<IngamePlayerController>();
        if(player && player.role == Role.Ghost)
        {
            if(!targets.Contains(player))
            {
                targets.Add(player);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<IngamePlayerController>();
        if (player && player.role == Role.Ghost)
        {
            if (!targets.Contains(player))
            {
                targets.Remove(player);
            }
        }
    }

    public IngamePlayerController GetFirstTarget()
    {
        IngamePlayerController closeTarget = targets[0];
        targets.Remove(closeTarget);
        return closeTarget;
    }
}
