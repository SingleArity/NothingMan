using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    public float moveSpeed, moveX;

    Animator anim;

    public Sprite mask_ringright, mask_ringleft, mask_ringtop, mask_ringbottom, mask_full;

    //0 - legs
    //1 - arms
    //2 - ?
    //3 - ?
    public bool[] upgrades;

    public GameObject[] upgradePrefabs;

    public List<Ability> abilities;

    public GameObject walkingTriggerGO;

    public bool isGrounded, isWalking, facingRight;
    public LayerMask groundMask;
    

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        abilities = new List<Ability>();
    }

    // Update is called once per frame
    void Update()
    {
        //movement info
        moveX = Input.GetAxis("Horizontal");
        if(facingRight)
        transform.Translate(new Vector3(moveX * moveSpeed, 0f, 0f));
        else
            transform.Translate(new Vector3(moveX * moveSpeed * -1f, 0f, 0f));

        anim.SetFloat("Move", moveX);

        //walking trigger
        //turn off
        if (isWalking && moveX == 0f)
        {
            isWalking = false;
            walkingTriggerGO.SetActive(false);
        }
        //turn on
        if(!isWalking && moveX != 0f)
        {
            isWalking = true;
            walkingTriggerGO.SetActive(true);
        }

        //check grounding
        isGrounded = GetComponent<Collider2D>().IsTouchingLayers(groundMask);

        //handle abilities
        foreach(Ability a in abilities)
        {
            a.HandleAbility();
        }
    }



    //animation handling

    private void LateUpdate()
    {
        HandleAnimation();
        foreach(Ability a in abilities)
        {
            a.HandleAnimation();
        }
    }

    void HandleAnimation()
    {
        //flip or not?
        if (moveX > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveX < 0 && facingRight)
        {
            Flip();
        }

        string upgradeBits = "" + Convert.ToInt32(upgrades[0])
                                + Convert.ToInt32(upgrades[1])
                                + Convert.ToInt32(upgrades[2])
                                + Convert.ToInt32(upgrades[3]);
        Sprite[] bodySprites = Resources.LoadAll<Sprite>("character/body/body_" + upgradeBits);

        string currentSpriteName = GetComponent<SpriteRenderer>().sprite.name;

        Sprite newSprite = Array.Find(bodySprites, x => x.name == currentSpriteName);

        GetComponent<SpriteRenderer>().sprite = newSprite;

    }

    public void Flip()
    {
        facingRight = !facingRight;
        //Vector3 theScale = transform.localScale;
        //theScale.x *= -1;
        //transform.localScale = theScale;
        Vector3 rot = transform.localEulerAngles;
        if (rot.y == 0) rot = new Vector3(0, 180, 0);
        else if (rot.y == 180) rot = new Vector3(0, 0, 0);
        transform.localEulerAngles = rot;
        //rotate barrier detection ring as well, the extra 180 if rot is 180, 0 if the rot is 0
        transform.Find("Ring").localEulerAngles = rot;

    }

    //objects/pickups

    public void AddPowerUp(PowerUp toAdd)
    {
        Debug.Log("Picked Up");
        upgrades[toAdd.index] = true;
        GameObject abilityGO = Instantiate(upgradePrefabs[toAdd.index], transform);
        abilityGO.GetComponent<Ability>().SetCharacter(this);
        abilities.Add(abilityGO.GetComponent<Ability>());
        Destroy(toAdd.gameObject);
    }

    //environment tile collision

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionEffect(collision.gameObject.tag);
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        CollisionEffect(collision.gameObject.tag);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        MaskRing();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Exit")
        {
            print("collided with exit.");
            anim.SetBool("Exit", true);
        }
    }

    public void CollisionEffect(string tag)
    {
        switch (tag)
        {
            case "Left":
                transform.Find("Ring").GetComponent<SpriteMask>().sprite = mask_ringright;
                break;
            case "Right":
                transform.Find("Ring").GetComponent<SpriteMask>().sprite = mask_ringleft;
                break;
            case "Top":
                transform.Find("Ring").GetComponent<SpriteMask>().sprite = mask_ringtop;
                break;
            case "Floor":
                transform.Find("Ring").GetComponent<SpriteMask>().sprite = mask_ringbottom;
                break;

            default:
                break;
        }
    }

    public void MaskRing()
    {
        transform.Find("Ring").GetComponent<SpriteMask>().sprite = mask_full;
    }


}
