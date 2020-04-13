using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delta : MonoBehaviour
{

    float startingYPos;
    GameManager gm;
    public bool isFloating;

    // Start is called before the first frame update
    void Start()
    {
        startingYPos = transform.localPosition.y;
        isFloating = true;
        gm = GameManager.Instance;
    }

    private void FixedUpdate()
    {
        if(isFloating)
            transform.localPosition = new Vector3(transform.localPosition.x, startingYPos + -.5f + (Mathf.Sin(Time.time * 3f) / 3f), 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gm.PlaySound(gm.powerUpSound);
            gm.DeltaGain(this);
        }
    }
}
