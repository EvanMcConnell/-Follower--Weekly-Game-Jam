using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement2D : MonoBehaviour
{
    [Header("Particle Effect")]
    [SerializeField]
    private ParticleSystem dust;

    [Header("Horizontal Movement")]
    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField]
    private Vector2 direction;
    [SerializeField]
    private bool facingRight = true;

    [Header("Vertical Movement")]
    [SerializeField]
    private bool canJump;
    [SerializeField]
    private bool toggleJump = false;
    [SerializeField]
    private bool jumping = false;
    [SerializeField]
    private float jumpSpeed = 15f;
    [SerializeField]
    private float jumpDelay = 0.25f;
    [SerializeField]
    private float jumpTimer;

    [Header("Physics")]
    [SerializeField]
    private float maxSpeed = 4f;
    [SerializeField]
    private float linearDrag = 4f;
    [SerializeField]
    private float gravity = 1;
    [SerializeField]
    private float fallMultiplier = 5;
    [SerializeField]
    private bool changingDirections;

    [Header("Collision")]
    [SerializeField]
    private bool onGround = false;
    [SerializeField]
    private bool groundable = true;
    [SerializeField]
    private float groundLength = 0.3f;

    [Header("Components")]
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private LayerMask groundLayer;

    public static CharacterMovement2D CreateComponent(GameObject where, Rigidbody2D rb, float moveSpeed, bool canJump, float jumpSpeed, float jumpDelay, LayerMask groundLayer, float groundLength)
    {
        CharacterMovement2D cm2d = where.AddComponent<CharacterMovement2D>();
        cm2d.rb = rb;
        cm2d.moveSpeed = moveSpeed;
        cm2d.canJump = canJump;
        cm2d.jumpSpeed = jumpSpeed;
        cm2d.jumpDelay = jumpDelay;
        cm2d.groundLayer = groundLayer;
        cm2d.groundLength = groundLength;
        return cm2d;
    }


    void Update()
    {
        onGround = Physics2D.Raycast(transform.position, Vector2.down, groundLength, groundLayer);

        if (canJump)
        {
            if (Input.GetButtonDown("Jump"))
            {
                jumpTimer = Time.time + jumpDelay;
            }

            if (jumpTimer > Time.time && onGround)
            {
                toggleJump = true;
            }
        }

        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }


    void FixedUpdate()
    {
        MoveCharacter(direction.x);
        if (toggleJump == true)
        {
            Jump();
        }

        modifyPhysics();
    }

    public bool getOnGround() { return onGround; }

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
    }


    void Jump()
    {
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        jumpTimer = 0;
        if (dust) { dust.Play(); }
        toggleJump = false;
    }


    public void setGroundable(bool g) { groundable = g; }


    void modifyPhysics()
    {
        changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);
        if (groundable)
        {
            if (onGround)
            {
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
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
        if (dust) { dust.Play(); }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.right * Input.GetAxis("R_Horizontal") + Vector3.up * Input.GetAxis("R_Vertical")));
    }
}
