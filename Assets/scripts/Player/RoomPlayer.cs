using FishNet.Object;
using FishNet.Object.Synchronizing;

public class RoomPlayer : NetworkBehaviour {

    [SyncVar]
    public string playerName;

    [SyncVar]
    public Role role;

    [SyncVar]
    public bool ready = false;
}
