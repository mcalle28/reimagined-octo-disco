using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostAbilityBtn : MonoBehaviour
{

    [SerializeField]
    private Button abilityBtn;

    private IngamePlayerController player;

    public void Show(bool enabled)
    {
        if(player.role == Role.Ghost) gameObject.SetActive(enabled);
    }

    public void LinkPlayer(IngamePlayerController playerController)
    {
        player = playerController;
    }

    public void UseAbility()
    {
        InGameUIManager.Instance.GhostAbilityBtn.Show(false);
        player.CmdScreamAbility();
    }
}
