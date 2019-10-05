using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{

    public Transform baseMap;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform tile in baseMap)
        {
            if (tile.tag != "powerup" && tile.tag != "Exit")
            {
                tile.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
