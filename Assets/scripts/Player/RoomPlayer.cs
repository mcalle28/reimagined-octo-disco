using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class RoomPlayer : NetworkBehaviour {

    [SyncVar]
    public string playerName;

    [SyncVar]
    public Role role;

    [SyncVar]
    public bool ready = false;

    public override void OnStartServer()
    {
        base.OnStartServer();
        RoomManager.Instance.players.Add(this);
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        RoomManager.Instance.players.Remove(this);
    }

    [ServerRpc]
    public void ServerSetIsReady(bool value)
    {
        ready = value;
    }
}
