using Mirror;
using UnityEngine;

namespace OneNightFollow
{
    public class AutoHostClient : MonoBehaviour
    {

        [SerializeField] NetworkManager networkManager;

        void Start()
        {
            if (!Application.isBatchMode)
            { //Headless build
                Debug.Log($"=== Client and player Build ===");
                networkManager.StartClient();
            }
            else
            {
                Debug.Log($"=== Dedicated Server build ===");
            }
        }

    }
}
