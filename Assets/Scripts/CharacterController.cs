using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    [Header ("Visuals")]
    public ParticleSystem dust;
    public GameObject cursorParent;
    public GameObject cursorAnchor;
    public GameObject cursor;
    public int screenWidth, screenHeight;
    public Vector2 centreScreen;
    public GameObject wintext;



    [Header("Horizontal Movement")]
    public float moveSpeed = 10f;
    public Vector2 direction;
    public bool facingRight = true;

    [Header("Vertical Movement")]
    public bool toggleJump = false;
    public bool jumping = false;
    public float jumpSpeed = 15f;
    public float jumpDelay = 0.25f;
    public float jumpTimer;


    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public LayerMask groundLayer;

    [Header("Physics")]
    public float maxSpeed = 4f;
    public float linearDrag = 4f;
    public float gravity = 1;
    public float fallMultiplier = 5;
    public bool changingDirections;
    public PhysicsMaterial2D playerMaterial;
    public PhysicsMaterial2D[] materials;

    [Header("Launcher")]
    public SpriteRenderer launchCursorSprite;
    public float launchForce = 100f;
    public float flickWindow = 0.5f;
    private Vector2 flickDir;
    private Vector2 finalFlickDir;
    private float flickTime = 0f;
    private bool flickable = false;
    public bool launched = false;
    public Vector2 trajectoryCorrection;
    public bool launchReady;
    public int bouncesRemaining;

    [Header("Aiming")]
    public LineRenderer lineA;
    private TrajectoryPredictor lineAFinder;
    public LineRenderer lineARaw;
    private RawTrajectoryPredictor lineARawFinder;

    [Header ("Collision")]
    public bool onGround = false;
    public float groundLength = 0.3f;

    [Header("Velocity")]
    public float xvelocity;

    [Header("Respawn")]
    public Vector3 spawnPoint;


    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position;

        Cursor.visible = false;
        lineAFinder = lineA.GetComponent<TrajectoryPredictor>();
        lineARawFinder = lineARaw.GetComponent<RawTrajectoryPredictor>();

        screenWidth = Display.main.renderingWidth;
        screenHeight = Display.main.renderingHeight;
        centreScreen = new Vector2(screenWidth/2, screenHeight/2);
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.Raycast(transform.position, Vector2.down, groundLength, groundLayer);

        if(Input.GetButtonDown("Jump"))
        {
            jumpTimer = Time.time + jumpDelay;
        }

        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        launchCursor();

        if (Input.GetKey(KeyCode.JoystickButton4) && GetComponent<BoxCollider2D>().sharedMaterial.Equals(materials[1]) || Input.GetKey(KeyCode.Q) && GetComponent<BoxCollider2D>().sharedMaterial.Equals(materials[1]))
        {
            modifyBounciness();
        }


        AnimationHandler(animator);
    }

    void FixedUpdate()
    {
        MoveCharacter(direction.x);
        if(toggleJump == true)
        {
            Jump();
        }

        modifyPhysics();
    }

    void MoveCharacter(float horizontal)
    {
        if (Mathf.Abs(rb.velocity.x) < maxSpeed)
        {
            rb.AddForce(Vector2.right * horizontal * moveSpeed);
        }

        if ((rb.velocity.x > 0 && !facingRight) || (rb.velocity.x < 0 && facingRight))
        {
            Flip();
        }

        /*if(Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            //rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
            rb.AddForce(new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y));
        }*/
    }

    void Jump()
    {
        //rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        jumpTimer = 0;
        dust.Play();
        print("jumped");
        toggleJump = false;
        //animator.SetBool("Jumping", false);
    }


    /*void modifyTime()
    {
        if (Input.GetKey(KeyCode.JoystickButton4))
        {
            Time.timeScale = 0.2f;
            Time.fixedDeltaTime = 0.2f * 0.02f;
        } else
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
        }
    }*/

    void modifyBounciness()
    {
        /*if (Input.GetKey(KeyCode.JoystickButton4) && GetComponent<BoxCollider2D>().sharedMaterial.Equals(materials[1])  || Input.GetKey(KeyCode.Q) && GetComponent<BoxCollider2D>().sharedMaterial.Equals(materials[1]))
        {
            //animator.SetBool("Bouncing", false);
            launched = false;
            GetComponent<BoxCollider2D>().sharedMaterial = materials[0];
            print(playerMaterial.bounciness);
        }*/

        launched = false;
        GetComponent<BoxCollider2D>().sharedMaterial = materials[0];
        print(playerMaterial.bounciness);
    }

    void modifyPhysics()
    {
        changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if (onGround && !launched)
        {
            launched = false;
            linearDrag = 10;

            if (Mathf.Abs(direction.x) < 0.4f || changingDirections && onGround)
            {
                rb.drag = linearDrag;
            }
            else
            {
                rb.drag = 0;
            }
            rb.gravityScale = 0;

            launchReady = true;
        }
        else if(launched == true)
        {
            rb.gravityScale = gravity * fallMultiplier;
            linearDrag = 0;
        }
        else
        {
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;

            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fallMultiplier;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.gravityScale = gravity * (fallMultiplier / 1.5f);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Ground")
        {
            print("collided with water");
            if (launched == false)
            {
                launchReady = true;
            }
        }

        if (coll.gameObject.name == "Grid")
        {
            print("collided with griddy water");
        }

        print("collided");
        if(launched == true) {
            //launchReady = true;
            if (bouncesRemaining == 0) { 
                launched = false;
                modifyBounciness();
            }
            else { bouncesRemaining--; }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "endzone")
        {
            wintext.SetActive(true);
        }

        if(coll.tag == "killzone")
        {
            transform.position = spawnPoint;
            print("dead");
        }
    }

    void launchCursor()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            flickDir = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - centreScreen;
            flickDir.Normalize();
            print("flickDir: " + flickDir);
            print("Mag: " + flickDir.magnitude);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            flickDir = new Vector2(Input.GetAxis("R_Horizontal"), Input.GetAxis("R_Vertical"));
            Cursor.visible = false;
        }


        if (launchReady == true)
        {
            if (flickDir.magnitude > 0.8)
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    finalFlickDir = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - centreScreen;
                    finalFlickDir.Scale(new Vector2(-1, -1));
                    finalFlickDir.Normalize();

                    launchCursorSprite.enabled = true;
                    Quaternion cursorDir = Quaternion.LookRotation((Vector3.right * finalFlickDir.x + Vector3.up * finalFlickDir.y), Vector3.back);
                    launchCursorSprite.gameObject.GetComponent<Rigidbody2D>().SetRotation(cursorDir);
                    launchCursorSprite.gameObject.transform.position = transform.position;
                }
                else
                {
                    finalFlickDir = new Vector2(Input.GetAxis("R_Horizontal"), Input.GetAxis("R_Vertical"));

                    launchCursorSprite.enabled = true;
                    Quaternion cursorDir = Quaternion.LookRotation((Vector3.right * flickDir.x + Vector3.up * flickDir.y), Vector3.back);
                    launchCursorSprite.gameObject.GetComponent<Rigidbody2D>().SetRotation(cursorDir);
                    launchCursorSprite.gameObject.transform.position = transform.position;
                }

                

                flickTime = Time.time;

                //slow time
                Time.timeScale = 0.2f;
                Time.fixedDeltaTime = 0.2f * 0.02f;

                flickable = true;

                lineA.enabled = true;
                lineA.transform.position = transform.position;
                lineAFinder.setVelocity(finalFlickDir);
                lineAFinder.setForce(launchForce);

                lineARaw.enabled = true;
                //lineARaw.transform.position = transform.position;
                //finalFlickDir.Scale(new Vector2(launchForce, launchForce));
                lineARawFinder.setVelocity(finalFlickDir*launchForce);
                //lineARawFinder.setForce(launchForce);


                //animator.SetBool("Bouncing", true);
                //animator.SetBool("Launching", true);

            }
            else if (flickDir.magnitude >= 0.2)
            {
                flickable = true;
                //animator.SetBool("Bouncing", true);
                //animator.SetBool("Launching", true);

                Time.timeScale = 1;
                Time.fixedDeltaTime = 0.02f;

                launchCursorSprite.enabled = false;
            }
            else if (flickDir.magnitude < 0.2 && (Time.time - flickWindow) < flickTime && flickable == true)
            {
                rb.velocity = new Vector2(0, 0);
                launch(finalFlickDir);
                bouncesRemaining = 1;
                print("flicked");

                //set material to bouncy
                GetComponent<BoxCollider2D>().sharedMaterial = materials[1];

                Time.timeScale = 1;
                Time.fixedDeltaTime = 0.02f;

                launchCursorSprite.enabled = false;

                flickable = false;
                //animator.SetBool("Launching", false);
            }
            else
            {
                launchCursorSprite.enabled = false;
                lineA.enabled = false;
                lineARaw.enabled = false;

                //resume time
                Time.timeScale = 1;
                Time.fixedDeltaTime = 0.02f;
            }
        }
    }

    void launch(Vector2 launchDir)
    {
        rb.AddForce(launchDir*launchForce*trajectoryCorrection  , ForceMode2D.Impulse);
        print("trajectory: " + launchDir * launchForce);

        launched = true;
        launchReady = false;

        rb.drag = 0.5f;
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
        dust.Play();
    }

    void AnimationHandler(Animator anim)
    {
        anim.SetBool("Grounded", onGround);
        anim.SetFloat("Horizontal", Mathf.Abs(Input.GetAxis("Horizontal")));

        if (jumpTimer > Time.time && onGround)
        {
            //anim.SetBool("Jumping", true);
            toggleJump = true;
        }
    }

    /*void AnimationHandler(Animator anim)
    {
        anim.SetFloat("Horizontal", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("Vertical", rb.velocity.y);
        anim.SetBool("Changing Directions", changingDirections);
        anim.SetFloat("Horizontal Input", Mathf.Abs(Input.GetAxis("Horizontal")));

        (anim.GetBool("Jumping"))
        {
            anim.SetBool("Jumping", false);
        }

        if (jumpTimer > Time.time && onGround)
        {
            anim.SetBool("Jumping", true);
            jumping = true;
        }

        if (rb.velocity.y < 0)
        {
            jumping = false;
        }

        if (onGround && !jumping)
        {
            anim.SetBool("Grounded", true);
            jumping = false;
        }

        if (jumping)
        {
            anim.SetBool("Jumping", true);
        }
        else
        {
            anim.SetBool("Jumping", false);
        }
    }*/

    IEnumerator stopJumpAnimation()
    {
        jumping = true;
        yield return new WaitForSecondsRealtime(0.5f);
        jumping = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.right * Input.GetAxis("R_Horizontal") + Vector3.up*Input.GetAxis("R_Vertical")));
    }
}
