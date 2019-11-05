using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{

    public Transform baseMap;

    //full invis
    public bool fullInvis = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform tile in baseMap)
        {
                tile.GetComponent<SpriteRenderer>().enabled = false;
        }
        baseMap.GetComponent<TilemapRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //update global timer for Lift Pieces
        LiftPiece.loopTime += Time.deltaTime;

        if (!fullInvis)
        {
            fullInvis = true;
            foreach (Transform tile in baseMap)
            {
                Debug.Log("invis tile:" + tile.gameObject.name);
                tile.GetComponent<SpriteRenderer>().enabled = false;
            }
            baseMap.GetComponent<TilemapRenderer>().enabled = false;
        }
    }

    public void EnableFullSight()
    {
        foreach (Transform tile in baseMap)
        {
            tile.GetComponent<SpriteRenderer>().enabled = true;
        }
        baseMap.GetComponent<TilemapRenderer>().enabled = true;

    }
}
