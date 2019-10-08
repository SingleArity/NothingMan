using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    GameManager gm;

    bool alreadyDead = false;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CharacterController player = collision.gameObject.GetComponent<CharacterController>();
        if(player && !alreadyDead)
        {
            //dont accidentally load scene more than once
            alreadyDead = true;
            player.Death();
        }
        
    }
}
