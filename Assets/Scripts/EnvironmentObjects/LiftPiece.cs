using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftPiece : MonoBehaviour
{

    //global timer used by all lifts
    public static float loopTime;


    public float loopLength, loopPercent, startOffsetPercent, yPosition, maxHeight;

    float startY;

    // Start is called before the first frame update
    void Start()
    {
        startY = transform.localPosition.y;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //how far into the current loop we are
        //startOffsetPercent * loopLength represents our actual time offset for the loop
        loopPercent = (((loopTime+(startOffsetPercent*loopLength)) % loopLength) / loopLength);

        //the lift loop is an up and down motion, halfway through means it is at its top apex
        if(loopPercent < .5f)
        {
            yPosition = (2f * loopPercent) * maxHeight;
        }
        else
        {
            yPosition = Mathf.Abs((2f * loopPercent) - 2f) * maxHeight;
        }
        transform.localPosition = new Vector3(transform.localPosition.x, startY + yPosition, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //enabled vision on object if touched by player
        //if(collision.gameObject.tag == "Player")
        //{
        //    GetComponent<SpriteRenderer>().enabled = true;
        //}
    }
}
