using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public struct GameRuleData
{
    public float movespeed;
    public float hunterMoveSpeed;
    public float hunterSight;
}

public class GameRuleStore : NetworkBehaviour
{
    [SyncVar]
    private float movespeed;
    [SyncVar]
    private float hunterMoveSpeed;
    [SyncVar]
    private float hunterSight;

    private void SetRecommendRule()
    {
        movespeed = 1f;
        hunterMoveSpeed = 2f;
        hunterSight = 1f;
    }

    void Start ()
    {
        if(IsServer)
        {
            SetRecommendRule();
        }
    }

    public GameRuleData GetGameRuleData()
    {
        return new GameRuleData()
        {
            movespeed = movespeed,
            hunterMoveSpeed = hunterMoveSpeed,
            hunterSight = hunterSight
        };
    }
}
