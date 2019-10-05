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

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        transform.Translate(new Vector3(moveX * moveSpeed, 0f, 0f));
        anim.SetFloat("Move", moveX);
        
    }



    //animation handling

    private void LateUpdate()
    {
        HandleAnimation();
    }

    void HandleAnimation()
    {
        string upgradeBits = "" + Convert.ToInt32(upgrades[0])
                                + Convert.ToInt32(upgrades[1])
                                + Convert.ToInt32(upgrades[2])
                                + Convert.ToInt32(upgrades[3]);
        Sprite[] bodySprites = Resources.LoadAll<Sprite>("character/body/body_" + upgradeBits);

        string currentSpriteName = GetComponent<SpriteRenderer>().sprite.name;

        Sprite newSprite = Array.Find(bodySprites, x => x.name == currentSpriteName);

        GetComponent<SpriteRenderer>().sprite = newSprite;

    }

    //object/pickup collision

    private void OnTriggerEnter2D(Collider2D collider)
    {
        
    }

    //environment tile collision

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("player pos" + transform.position);
        Debug.Log(collision.gameObject.name + " pos:" + collision.transform.position);
        CollisionEffect(collision.gameObject.tag);
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        MaskRing();
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
            default:
                break;
        }
    }

    public void MaskRing()
    {
        transform.Find("Ring").GetComponent<SpriteMask>().sprite = mask_full;
    }
}
