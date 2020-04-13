using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : Ability
{
    Animator anim;
    CharacterController cc;

    bool alreadyJumped;

    // Start is called before the first frame update
    void Start()
    {    
        cc = GetComponentInParent<CharacterController>();
        anim = GetComponent<Animator>();
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

        anim.SetBool("Walk", cc.moving);

    }

    void TryDoJump()
    {
        //if we are not on ground and haven't double jumped already
        if (!character.isGrounded && !alreadyJumped)
        {
            if (HasArms()) {
                if (!transform.parent.GetComponentInChildren<WallGrab>().grabbingWall && transform.parent.GetComponentInChildren<WallGrab>().canJump)
                {
                    DoJump();
                }
            }
            else
            {
                DoJump();
            }
        }
    }

    void DoJump()
    {
        alreadyJumped = true;
        character.GetComponent<Rigidbody2D>().velocity = new Vector3(character.GetComponent<Rigidbody2D>().velocity.x, 0f, 0f);
        character.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 80f));
        anim.SetTrigger("Bounce");
    }

    bool HasArms()
    {
        return transform.parent.GetComponentInChildren<WallGrab>() != null;
    }
}
