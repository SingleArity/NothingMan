using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : Ability
{

    Animator anim;
    CharacterController cc;
    bool canJump = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        cc = GetComponentInParent<CharacterController>();
        cc.GetComponent<CircleCollider2D>().offset = new Vector2(0f, -.3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void HandleAbility()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            TryGroundedJump();
        }
    }

    public override void HandleAnimation()
    {
        
        anim.SetBool("Moving", cc.moving);

    }

    void TryGroundedJump()
    {
        if (character.isGrounded)
        {
            if (character.walled)
            {
                //if walled, jump if we don't have arms
                if (character.GetComponentInChildren<WallGrab>() == null)
                {
                    character.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 80f));
                    StartCoroutine(WaitForNextJump());
                }
                //if we do have arms, only jump if we aren't grabbing the wall (we might be standing in a corner)
                else
                {
                    if (!character.GetComponentInChildren<WallGrab>().grabbingWall)
                    {
                        //check to see if this is happening alongside wallgrab
                        character.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 80f));
                        StartCoroutine(WaitForNextJump());
                    }
                }
            }
            else
            {
                character.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 80f));
                StartCoroutine(WaitForNextJump());
            }
        }
    }

    public void DoJump(float direction = 0f, float yForce = 80f)
    {
        if (canJump)
        {
            character.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction, yForce));
            StartCoroutine(WaitForNextJump());
        }
    }

    public IEnumerator WaitForNextJump()
    {
        canJump = false;
        yield return new WaitForSeconds(.3f);
        canJump = true;
    }
}
