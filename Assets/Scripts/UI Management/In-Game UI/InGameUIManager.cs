using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIManager : MonoBehaviour
{

    public static InGameUIManager Instance;

    [SerializeField]
    private GhostAbilityBtn ghostAbilityBtn;

    public GhostAbilityBtn GhostAbilityBtn { get { return ghostAbilityBtn; } }
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
}
