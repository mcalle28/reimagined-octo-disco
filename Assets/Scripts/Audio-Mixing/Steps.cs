using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class Steps : NetworkBehaviour
{
    private AudioSource audioSource;

    [SyncVar]
    private bool hunterMoving;

    public Vector3 dir;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
            dir.x = Input.GetAxisRaw("Horizontal");
            dir.y = Input.GetAxisRaw("Vertical");
            dir = dir.normalized;
            bool isMoving = dir.magnitude != 0f;
            if(base.IsClient)CMDHunterMoving(isMoving);

            if (hunterMoving){
                audioSource.mute = false;
            }
            else{
                audioSource.mute=true;
            }
        
    }

    [ServerRpc]
    private void CMDHunterMoving(bool hmoving)
    {
        hunterMoving = hmoving;
    }


}
