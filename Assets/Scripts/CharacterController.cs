using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    public float moveSpeed, moveX;

    Animator anim;

    public Sprite mask_ringright, mask_ringleft, mask_ringtop, mask_ringbottom, mask_full;

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
