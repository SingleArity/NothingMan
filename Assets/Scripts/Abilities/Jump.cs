using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : Ability
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void HandleAbility()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoJump();
        }
    }

    void DoJump()
    {
        if (character.isGrounded)
        {
            character.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 40f));
        }
    }
}
