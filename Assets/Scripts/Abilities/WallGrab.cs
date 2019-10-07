using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGrab : Ability
{

    Animator anim;
    CharacterController cc;

    public bool holdingKey, grabbingWall, canGrab = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        cc = GetComponentInParent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void HandleAbility()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            holdingKey = true;
        }
        else
        {
            holdingKey = false;
        }

        //if we are holding down the button
        if (holdingKey)
        {
            //and we are next to a wall
            if (cc.walled)
            {
                //if we are not already holding on, do so
                if (!grabbingWall && canGrab)
                {
                    //cc.moveDisabled = true;
                    grabbingWall = true;
                    cc.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

                    if (transform.parent.GetComponentInChildren<Hover>() != null)
                    {
                        transform.parent.GetComponentInChildren<Hover>().canUse = false;
                    }
                }
            }
        }
        else
        {
            //if we have hover, allow it again
            if (transform.parent.GetComponentInChildren<Hover>() != null)
            {
                transform.parent.GetComponentInChildren<Hover>().canUse = true;
            }
        }

        //if we are not holding key down and we are walled, we let go
        if(!holdingKey && cc.walled && grabbingWall)
        {
            grabbingWall = false;
            cc.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            
        }

        //if we are grabbing a wall and we have jump ability
        if(grabbingWall && transform.parent.GetComponentInChildren<Jump>() != null)
        {
            //if we do a jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //no longer grabbing wall
                grabbingWall = false;
                //unfreeze
                cc.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                //do the thing
                //direction to jump off
                float dir = 0f;
                //wall with tag "Left" is wall to right of player
                if (cc.currentlyCollidingTag == "Left") dir = -30f;
                else if (cc.currentlyCollidingTag == "Right") dir = 30f;

                Debug.Log("direction: " +  dir);
                //do the thing
                transform.parent.GetComponentInChildren<Jump>().DoJump(dir, 60f);
                //stop us from hanging on to current wall
                StartCoroutine(CantGrab());
            }
        }
    }

    IEnumerator CantGrab()
    {
        canGrab = false;
        yield return new WaitForSeconds(.2f);
        canGrab = true;
    }


    public override void HandleAnimation()
    {

        anim.SetBool("Walk", cc.moving);

    }
}
