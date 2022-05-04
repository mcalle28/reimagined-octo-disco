using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    [SerializeField]
    private Transform Back;

    [SerializeField]
    private Transform Front;
    // Start is called before the first frame update

    public int GetSortingOrder(GameObject obj)
    {
        float objDist = Mathf.Abs(Back.position.y - obj.transform.position.y);
        float totalDist = Mathf.Abs(Back.position.y - Front.position.y);

        return (int)Mathf.Lerp(short.MinValue, short.MaxValue, objDist / totalDist);
    }
}
