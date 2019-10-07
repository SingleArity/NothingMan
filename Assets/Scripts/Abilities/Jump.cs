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
            character.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 80f));
            StartCoroutine(WaitToJumpAgain());
        }
    }

    public void DoJump(float direction = 0f, float yForce = 80f)
    {
        if (canJump)
        {
            character.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction, yForce));
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
