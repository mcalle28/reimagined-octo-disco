using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostAbilityBtn : MonoBehaviour
{

    [SerializeField]
    private Button abilityBtn;

    [SerializeField]
    private int abilityUses = 1;

    private IngamePlayerController player;

    public void Show(bool enabled)
    {
        gameObject.SetActive(enabled);
    }

    public void LinkPlayer(IngamePlayerController playerController)
    {
        player = playerController;
    }

    public void UseAbility()
    {
        if (abilityUses > 0)
        {
            abilityUses = abilityUses-1;
            if (abilityUses <= 0)
            {
                InGameUIManager.Instance.GhostAbilityBtn.Show(false);
            }
            player.CmdScreamAbility();
        }
    }
}
