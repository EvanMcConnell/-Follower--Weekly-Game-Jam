using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    [Header("Horizontal Movement")]
    public float moveSpeed = 10f;
    public Vector2 direction;
    private bool facingRight = true;

    [Header("Vertical Movement")]
    public bool jumping = false;
    public float jumpSpeed = 15f;
    public float jumpDelay = 0.25f;
    private float jumpTimer;


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
    private bool launched = false;
    public Vector2 trajectoryCorrection;

    [Header ("Collision")]
    public bool onGround = false;
    public float groundLength = 0.3f;

    [Header("Velocity")]
    public float xvelocity;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
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

        AnimationHandler(animator);

        modifyBounciness();
    }

    void FixedUpdate()
    {
        MoveCharacter(direction.x);
        if(jumpTimer > Time.time && onGround)
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

        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
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
        print("jumped");
    }


    /*
    void modifyTime()
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
        if (Input.GetKey(KeyCode.JoystickButton4) && GetComponent<BoxCollider2D>().sharedMaterial.Equals(materials[1]))
        {
            GetComponent<BoxCollider2D>().sharedMaterial = materials[0];
            print(playerMaterial.bounciness);
        }
    }

    void modifyPhysics()
    {
        changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if (onGround)
        {
            launched = false;
            linearDrag = 4;

            if (Mathf.Abs(direction.x) < 0.4f || changingDirections && onGround)
            {
                rb.drag = linearDrag;
            }
            else
            {
                rb.drag = 0;
            }
            rb.gravityScale = 0;
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
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
    }

    void launchCursor()
    {
        flickDir = new Vector2(Input.GetAxis("R_Horizontal"), Input.GetAxis("R_Vertical"));

        if (flickDir.magnitude > 0.8)
        {
            finalFlickDir = new Vector2(Input.GetAxis("R_Horizontal"), Input.GetAxis("R_Vertical"));

            launchCursorSprite.enabled = true;
            Quaternion cursorDir = Quaternion.LookRotation((Vector3.right * flickDir.x + Vector3.up * flickDir.y), Vector3.back);
            launchCursorSprite.gameObject.GetComponent<Rigidbody2D>().SetRotation(cursorDir);
            launchCursorSprite.gameObject.transform.position = transform.position;

            flickTime = Time.time;

            //slow time
            Time.timeScale = 0.2f;
            Time.fixedDeltaTime = 0.2f * 0.02f;

            flickable = true;

        } else if(flickDir.magnitude >= 0.2)
        {
            flickable = true;

            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;

            launchCursorSprite.enabled = false;
        }
        else if (flickDir.magnitude < 0.2 && (Time.time - flickWindow) < flickTime && flickable == true)
        {
            rb.velocity = new Vector2(0, 0);
            launch(finalFlickDir);
            print("flicked");

            //set material to bouncy
            GetComponent<BoxCollider2D>().sharedMaterial = materials[1];

            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;

            launchCursorSprite.enabled = false;

            flickable = false;
        }
        else
        {
            launchCursorSprite.enabled = false;

            //resume time
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
        }
    }

    void launch(Vector2 launchDir)
    {
        rb.AddForce(launchDir*launchForce*trajectoryCorrection  , ForceMode2D.Impulse);
        print("trajectory: " + launchDir * launchForce);

        launched = true;
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }

    void AnimationHandler(Animator anim)
    {
        anim.SetFloat("Horizontal", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("Vertical", rb.velocity.y);

        if (anim.GetBool("Jumping"))
        {
            anim.SetBool("Jumping", false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            StartCoroutine(stopJumpAnimation());
        }

        if (onGround)
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
    }
  
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
