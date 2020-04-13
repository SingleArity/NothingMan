using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hover : Ability
{
    Animator anim;
    CharacterController cc;

    bool holdingButton = false;
    public bool canUse = true;

    [SerializeField]
    float timer = 1f;

    float coolDownInc = .03f;

    Image bar;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponentInParent<CharacterController>();
        bar = GetComponentInChildren<Image>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void HandleAbility()
    {
        //by default, don't show timer bar
        bar.enabled = false;

        if (canUse)
        {
            //are we holding down the button?
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                holdingButton = true;
            }
            else
            {
                holdingButton = false;
            }

            //if we are, and within window, do hover
            if (holdingButton)
            {
                if (timer != 0f)
                {
                    //hover
                    cc.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
                    anim.SetBool("Flying", true);
                    //subtract timer by change in time, not less than 0
                    timer = Mathf.Max(timer - Time.deltaTime, 0f);
                    //show ui image
                    bar.enabled = true;
                    bar.fillAmount = timer;
                }
                else
                {
                    //out of timer, not hovering
                    cc.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    anim.SetBool("Flying", false);
                }

            }
            else
            {
                //if we let go of hover button this frame, set the physics back to normal
                if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
                {
                    cc.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    anim.SetBool("Flying", false);

                }
                //if we are on the ground and not hovering, refill timer
                if (cc.isGrounded && !(timer >= 1f))
                {
                    timer = Mathf.Min(timer + coolDownInc, 1f);
                    //show ui image
                    bar.enabled = true;
                    bar.fillAmount = timer;
                }
            }
        }


    }

    public override void HandleAnimation()
    {

        

    }
}
