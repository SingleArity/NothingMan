using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    float startingYPos;

    //index is used by character controller to add the powerup
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        startingYPos = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, startingYPos + (Mathf.Sin(Time.time*3f) / 3f), 0f);

        float yRot = ((Time.time * 300f) % 360f);
        transform.localRotation = Quaternion.Euler(0f, yRot, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.GetComponent<CharacterController>().AddPowerUp(this);
        }
    }
}
