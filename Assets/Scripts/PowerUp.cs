using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using TMPro;

public class PowerUp : MonoBehaviour
{
    float startingYPos;

    //index is used by character controller to add the powerup
    public int index;

    public string[] soundParams;

    public string flavorText;

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        startingYPos = transform.localPosition.y;
        gm = GameManager.Instance;
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
            if(flavorText != null)
            {
                gm.StartCoroutine(gm.ShowFlavorText(flavorText));
            }
            foreach (string s in soundParams)
            {
                collider.GetComponent<StudioEventEmitter>().SetParameter(s, 1f);
            }

            collider.GetComponent<CharacterController>().AddPowerUp(this);
        }
    }


}
