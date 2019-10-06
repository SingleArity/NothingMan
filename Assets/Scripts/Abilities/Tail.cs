using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : Ability
{
    //Animator anim;
    CharacterController cc;

    bool alreadyJumped;

    // Start is called before the first frame update
    void Start()
    {    
        cc = GetComponentInParent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //reset alreadyjumped flag if we are grounded
        if (alreadyJumped)
        {
            if (character.isGrounded) alreadyJumped = false;
        }
    }

    public override void HandleAbility()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryDoJump();
        }
    }

    public override void HandleAnimation()
    {

        //anim.SetBool("Moving", cc.moving);

    }

    void TryDoJump()
    {
        //if we are not on ground and haven't double jumped already
        if (!character.isGrounded && !alreadyJumped)
        {
            alreadyJumped = true;
            character.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 40f));
        }
    }
}
