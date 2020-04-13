using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{

    public float moveSpeed, moveX, prevMoveX, accelTimer, momentum;

    Animator anim;
    Rigidbody2D rigidBody;

    public Sprite mask_ringright, mask_ringleft, mask_ringtop, mask_ringbottom, mask_full;

    //0 - legs
    //1 - arms
    //2 - ?
    //3 - ?
    public bool[] upgrades;

    public GameObject[] upgradePrefabs;

    public List<Ability> abilities;

    public GameObject walkingTriggerGO;

    private bool isPlayerInput;
    public bool moving, isGrounded, isWalking, facingRight, walled, cornerWalled, moveDisabled = false;
    public LayerMask groundMask, wallMask;

    public string currentlyCollidingTag, currentlyCollidingName;

    List<String> currentCollisions;

    private InputAction moveAction;
    [SerializeField] private InputActionAsset playerActions;

    private void Awake()
    {
        playerActions.Enable();
        InputActionMap gameplayActionMap = playerActions.FindActionMap("Player");
        moveAction = gameplayActionMap.FindAction("TestAction", true);
        moveAction.performed += OnMove;
        moveAction.started += OnMoveStarted;
        moveAction.canceled += OnMoveCancelled;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        abilities = new List<Ability>();
        isPlayerInput = true;
        currentCollisions = new List<String>();
        accelTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(moveAction.phase == InputActionPhase.Started)
        {
            Debug.Log("holding button?");
        }
        //movement info
        if (!moveDisabled)
            moveX = Input.GetAxisRaw("Horizontal");
        else
            moveX = 0;

        if (isPlayerInput == false)
        {
            moveX = 0;
        }


        //check grounding
        isGrounded = GetComponent<Collider2D>().IsTouchingLayers(groundMask);

        //check if against wall
        walled = GetComponent<Collider2D>().IsTouchingLayers(wallMask) || cornerWalled;


        //if we aren't on a wall
        if (!walled)
        {
            if (GetComponentInChildren<WallGrab>() == null)
            {
                TryWalking();
            }
            else
            {
                //don't set rigidbody velocity if just jumped off a wall
                if (!GetComponentInChildren<WallGrab>().canGrab) KeepVelocity();
                else TryWalking();
            }
                
        }
        //or if we are walled but also grounded
        else if (isGrounded)
        {
            TryWalking();
        }


        anim.SetFloat("Move", moveX);
        if (moving && moveX == 0f)
        {
            moving = false;
            anim.SetBool("Moving", false);
        }
        else if (!moving && moveX != 0f)
        {
            moving = true;
            anim.SetBool("Moving", true);
        }

        //walking trigger
        //turn off
        if (isWalking && moveX == 0f)
        {
            isWalking = false;
            walkingTriggerGO.SetActive(false);
        }
        //turn on
        if (!isWalking && moveX != 0f)
        {
            isWalking = true;
            walkingTriggerGO.SetActive(true);
        }

        //check grounding
        isGrounded = GetComponent<Collider2D>().IsTouchingLayers(groundMask);

        //handle abilities
        if (isPlayerInput) {
            foreach (Ability a in abilities)
            {
                a.HandleAbility();
            }
        }
    }

    private void FixedUpdate()
    {
        //for the case in which we quickly shift from moving right to left or vice versa
        if (moveX != prevMoveX && moveX != 0f) accelTimer = 0f;

        if (moveX != 0f) accelTimer = Mathf.Clamp(accelTimer + (Time.deltaTime * 2f), 0f, 1f);
        else accelTimer = Mathf.Clamp(accelTimer - (Time.deltaTime * 2f), 0f, 1f);

        //momentum factor of .08f for some reason. seems arbitrary. but then again, my definition of momentum is arbitrary.oops
        if (moveX != 0f)
            momentum = Mathf.Clamp((moveX * accelTimer * .08f), -1f, 1f);
        else {
            if (momentum < 0)
                momentum = Mathf.Clamp(momentum + Time.deltaTime*2f, -1f, 0f);
            else if (momentum >= 0)
                momentum = Mathf.Clamp(momentum - Time.deltaTime*2f, 0f, 1f);
        }

        prevMoveX = moveX;
    }

    //IPlayerActions Implementation
    public void OnMove(InputAction.CallbackContext context)
    {
        // 'Move' code here.
        Debug.Log("move action performed!");
        Debug.Log(context.ReadValue<Vector2>());
    }

    public void OnMoveStarted(InputAction.CallbackContext context)
    {
        // 'Move' code here.
        Debug.Log("move action start");
    }

    public void OnMoveCancelled(InputAction.CallbackContext context)
    {
        // 'Move' code here.
        Debug.Log("move action cancelled");
    }


    void TryWalking()
    {
        //if (facingRight)
        Debug.Log("accel:"+accelTimer);
        Debug.Log("momentum:" + momentum);

        float moveDir = 1;
        if (!facingRight) moveDir = -1f;

        rigidBody.velocity = new Vector2((moveDir * moveSpeed * accelTimer) + momentum, rigidBody.velocity.y);

       //transform.Translate(new Vector3(moveX * moveSpeed, 0f, 0f));
       // else
           // transform.Translate(new Vector3(moveX * moveSpeed * -1f, 0f, 0f));
    }

    void KeepVelocity()
    {
        //Debug.Log("keeping velocity");
    }

    //animation handling

    private void LateUpdate()
    {
        if (isPlayerInput == true)
        {
            HandleAnimation();
            foreach (Ability a in abilities)
            {
                a.HandleAnimation();
            }
        }
    }
    
    void HandleAnimation()
    {

        if (walled) anim.SetBool("Moving", false);

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
                                + Convert.ToInt32(upgrades[3])
                                + Convert.ToInt32(upgrades[4]);

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

    public void Death()
    {
        isPlayerInput = false;
        anim.SetBool("Death", true);
    }

    public void Respawn()
    {
        Debug.Log("Respawning!!!");
        GameManager.Instance.RestartLevel();
    }

    //environment tile collision

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentCollisions.Add(collision.gameObject.name);

        cornerWalled = CollisionEffect(collision.gameObject.tag, collision.transform);
        if(collision.gameObject.name != "Base" && GetComponentInChildren<See>() == null)
            StartCoroutine(TempVis(collision.gameObject.GetComponent<SpriteRenderer>()));
        //set the tag, so things can know what we are colliding with at any given time 
        currentlyCollidingTag = collision.gameObject.tag;
        currentlyCollidingName = collision.gameObject.name;
    }

    public IEnumerator TempVis(SpriteRenderer sr)
    {
        sr.enabled = true;
        float newA = 1f;
        while(newA > 0f)
        {
            newA -= .05f;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, newA);
            yield return new WaitForSeconds(.01f);
        }
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        if(GetComponentInChildren<See>() == null)
            sr.enabled = false;
        else
            sr.enabled = true;
        yield return null;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        cornerWalled = CollisionEffect(collision.gameObject.tag, collision.transform);
        if(GetComponentInChildren<WallGrab>() != null)
        {
            if (!GetComponentInChildren<WallGrab>().canJump) cornerWalled = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        currentCollisions.Remove(collision.gameObject.name);

        MaskRing();
        cornerWalled = false;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }

    public void Exit(Transform exitObj)
    {
        print("collided with exit.playery:" + transform.position.y + "exit posy:"+exitObj.position.y);
        isPlayerInput = false;
        transform.position = new Vector3(transform.position.x, exitObj.position.y, transform.position.z);
        if (!facingRight) Flip();
        GetComponent<CircleCollider2D>().offset = new Vector2(0f, 0f);
        anim.SetBool("Exit", true);

        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        foreach(Ability a in GetComponentsInChildren<Ability>())
        {
            a.GetComponent<PolygonCollider2D>().enabled = true;

            Rigidbody2D rbod = a.GetComponent<Rigidbody2D>();
            rbod.bodyType = RigidbodyType2D.Dynamic;
            rbod.simulated = true;
            rbod.AddForce(new Vector2(UnityEngine.Random.Range(-20f,0f), UnityEngine.Random.Range(20f, 30f)));
        }
    }

    //return - whether we are touching the wall side of a corner
    public bool CollisionEffect(string tag, Transform tf)
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
            case "corner":
                //Debug.Log("Colliding with corner!");
                if ((tf.position.y + .9f) < transform.position.y)
                    transform.Find("Ring").GetComponent<SpriteMask>().sprite = mask_ringbottom;
                else if (tf.position.x < transform.position.x)
                {
                    transform.Find("Ring").GetComponent<SpriteMask>().sprite = mask_ringleft;
                    if((tf.position.y + .6f > transform.position.y) && (transform.position.y > tf.position.y -.6f))
                        return true;
                }
                else if (tf.position.x > transform.position.x)
                {
                    transform.Find("Ring").GetComponent<SpriteMask>().sprite = mask_ringright;
                    if ((tf.position.y + .6f > transform.position.y) && (transform.position.y > tf.position.y - .6f))
                    {
                        return true;
                    }
                }
                break;
            case "Floor":
                transform.Find("Ring").GetComponent<SpriteMask>().sprite = mask_ringbottom;
                break;
            
            default:
                break;
        }
        return false;
    }

    public void MaskRing()
    {
        transform.Find("Ring").GetComponent<SpriteMask>().sprite = mask_full;
    }


}
