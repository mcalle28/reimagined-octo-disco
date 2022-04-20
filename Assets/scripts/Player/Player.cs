using UnityEngine;
using Mirror;
using TMPro;

public class Player : NetworkBehaviour {
    [SyncVar (hook = nameof(SetPlayerName_Hook))]
    public string playerName;

    [SerializeField]
    private TMP_Text text;

    public void SetPlayerName_Hook(string _, string value)
    {
        text.text = value;
    }
}
