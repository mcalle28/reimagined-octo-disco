using FishNet;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Transporting;

public class MainManager : NetworkBehaviour
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
        sld.ReplaceScenes = ReplaceOption.All;
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        ChangeSceneToRoom();
    }
}
