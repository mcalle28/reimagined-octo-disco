using FishNet;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Transporting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnManager : NetworkBehaviour
{
    public void SetAddress(string add) {
        InstanceFinder.TransportManager.Transport.SetServerBindAddress(add, IPAddressType.IPv4);
    }

    public void StartClient(){
        InstanceFinder.ClientManager.StartConnection();
    }

    public void StartHost() {
        InstanceFinder.ServerManager.StartConnection();
        InstanceFinder.ClientManager.StartConnection();
    }

    public void ChangeSceneToRoom()
    {
        SceneLoadData sld = new SceneLoadData("Room");
        InstanceFinder.NetworkManager.SceneManager.LoadGlobalScenes(sld);

        SceneUnloadData sud = new SceneUnloadData("Main Screen");
        InstanceFinder.NetworkManager.SceneManager.UnloadGlobalScenes(sud);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        ChangeSceneToRoom();
    }
}
