using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : Ability
{

    Animator anim;
    CharacterController cc;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoJump();
        }
    }

    public override void HandleAnimation()
    {
        
        anim.SetBool("Moving", cc.moving);

    }

    void DoJump()
    {
        if (character.isGrounded)
        {
            character.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 80f));
        }
    }
}
