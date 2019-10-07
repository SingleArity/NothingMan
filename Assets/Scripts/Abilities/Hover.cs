using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Ability
{
    //Animator anim;
    CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
        cc = GetComponentInParent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void HandleAbility()
    {
        
    }

    public override void HandleAnimation()
    {

        

    }
}
