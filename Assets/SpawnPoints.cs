using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    public List<GameObject> ghostSpawnPoints = new List<GameObject>();
    public List<GameObject> hunterSpawnPoints = new List<GameObject>();

    private int ghostCounter;
    private int hunterCounter;

    void Start()
    {
        ghostCounter = ghostSpawnPoints.Count-1;
        hunterCounter = ghostSpawnPoints.Count-1;
    }

    public GameObject getSpawnPoint(Role role)
    {
        if(ghostCounter == 0)
        {
            ghostCounter = ghostSpawnPoints.Count-1 ;
        } else if (hunterCounter == 0)
        {
            hunterCounter = hunterSpawnPoints.Count-1;
        }

        if(role == Role.Ghost)
        {
            ghostCounter--;
            return ghostSpawnPoints[ghostCounter];
        } else
        {
            hunterCounter--;
            return hunterSpawnPoints[hunterCounter];
        }
    }
}
