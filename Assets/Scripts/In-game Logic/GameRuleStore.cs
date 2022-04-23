using UnityEngine;
using Mirror;

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

    [SyncVar]
    private int hunterCount;

    void Start ()
    {
        if(isServer)
        {
            hunterCount = 1;
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
