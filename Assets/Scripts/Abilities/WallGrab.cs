using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGrab : Ability
{

    Animator anim, leganim, eyeanim, bodyanim;
    CharacterController cc;

    public bool holdingLeft, holdingRight, grabbingWall, canGrab = true, canJump = true;

    // Start is called before the first frame update
    void Start()
    {
        //get references to the various animators of the character
        anim = GetComponent<Animator>();
        if (transform.parent.GetComponentInChildren<Jump>() != null)
            leganim = transform.parent.GetComponentInChildren<Jump>().GetComponent<Animator>();
        if (transform.parent.GetComponentInChildren<See>() != null)
            eyeanim = transform.parent.GetComponentInChildren<See>().GetComponent<Animator>();
        bodyanim = GetComponentInParent<Animator>();

        cc = GetComponentInParent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void HandleAbility()
    {
       
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) holdingLeft = true;
        else holdingLeft = false;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) holdingRight = true;
        else holdingRight = false;

        //if we are holding down the left button
        if (holdingLeft)
        {
            //and we are next to a left wall
            if (cc.walled && (cc.currentlyCollidingTag == "Right" || cc.currentlyCollidingName.Contains("Right")))
            {
                //if we are not already holding on, do so
                if (!grabbingWall && canGrab)
                {

                    //cc.moveDisabled = true;
                    grabbingWall = true;
                    SetAnimatorsToState(true);
                    cc.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

                    if (transform.parent.GetComponentInChildren<Hover>() != null)
                    {
                        transform.parent.GetComponentInChildren<Hover>().canUse = false;
                    }
                }
            }
        }
        //if we are holding down the right button
        else if (holdingRight)
        {
            //and we are next to a left wall
            if (cc.walled && (cc.currentlyCollidingTag == "Left" || cc.currentlyCollidingName.Contains("Left")))
            {
                //if we are not already holding on, do so
                if (!grabbingWall && canGrab)
                {
                    //cc.moveDisabled = true;
                    grabbingWall = true;
                    SetAnimatorsToState(true);
                    cc.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

                    if (transform.parent.GetComponentInChildren<Hover>() != null)
                    {
                        transform.parent.GetComponentInChildren<Hover>().canUse = false;
                    }
                }
            }
        }
        //holding neither left nor right
        else
        {
            //if we have hover, allow it again
            if (transform.parent.GetComponentInChildren<Hover>() != null)
            {
                transform.parent.GetComponentInChildren<Hover>().canUse = true;
            }
        }

        //if we are not holding key down and we are walled, we let go
        if(!holdingRight && (cc.currentlyCollidingTag == "Left" || cc.currentlyCollidingName.Contains("Left")) && cc.walled && grabbingWall)
        {
            grabbingWall = false;
            SetAnimatorsToState(false);
            cc.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            if (transform.parent.GetComponentInChildren<Hover>() != null)
            {
                transform.parent.GetComponentInChildren<Hover>().canUse = true;
            }

        }
        if (!holdingLeft && (cc.currentlyCollidingTag == "Right" || cc.currentlyCollidingName.Contains("Right")) && cc.walled && grabbingWall)
        {
            grabbingWall = false;
            SetAnimatorsToState(false);
            cc.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            if (transform.parent.GetComponentInChildren<Hover>() != null)
            {
                transform.parent.GetComponentInChildren<Hover>().canUse = true;
            }
        }


        //if we are grabbing a wall and we have jump ability
        if (grabbingWall /*&& transform.parent.GetComponentInChildren<Jump>() != null*/)
        {
            //if we do a jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //no longer grabbing wall
                grabbingWall = false;
                SetAnimatorsToState(false);
                //unfreeze
                cc.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                //do the thing
                //direction to jump off
                float dir = 0f;
                //wall with tag "Left" is wall to right of player
                if (cc.currentlyCollidingTag == "Left") dir = -15f;
                else if (cc.currentlyCollidingTag == "Right") dir = 15f;
                else if (cc.currentlyCollidingName.Contains("Left")) dir = -15f;
                else if (cc.currentlyCollidingName.Contains("Right")) dir = 15f;

                //ensure 'cornerwalled' in charactercontroller is false
                cc.cornerWalled = false;

                //do the thing
                //doing it this way requires having legs
                //transform.parent.GetComponentInChildren<Jump>().DoJump(dir, 65f);
                DoJump(dir, 65f);

                //re-allow hovering if we have it
                if (transform.parent.GetComponentInChildren<Hover>() != null)
                {
                    transform.parent.GetComponentInChildren<Hover>().canUse = true;
                }

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

    void SetAnimatorsToState(bool state)
    {
        if(eyeanim !=null) eyeanim.SetBool("WallGrab", state);
        if(leganim != null) leganim.SetBool("WallGrab", state);

        if (state == true) bodyanim.SetBool("Moving", false);

        anim.SetBool("Grab", state);

    }

    //Jumping off wall stuff
    public void DoJump(float direction = 0f, float yForce = 80f)
    {        
        if (canJump)
        {
            character.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction, yForce));
            //if we have legs, make sure they can't jump right away also
            if (character.GetComponentInChildren<Jump>() != null)
            {
                character.GetComponentInChildren<Jump>().StartCoroutine(character.GetComponentInChildren<Jump>().WaitForNextJump());
            }
            StartCoroutine(WaitToJumpAgain());
          
        }
    }

    IEnumerator WaitToJumpAgain()
    {
        canJump = false;
        yield return new WaitForSeconds(.2f);
        canJump = true;
    }
}
