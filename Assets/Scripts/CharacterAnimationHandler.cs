using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationHandler : MonoBehaviour
{
    private CharacterControllerAllInOne cc;
    private Animator anim;
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Vector2 rbVelocity;
    private Vector2 launcherInput;
    private bool jump;
    public enum animState
    {
        Idle,
        Walking,
        Jumping,
        Falling,
        Bouncing
    }

    public animState state;

    void Start()
    {
        //assign component variables
        cc = GetComponent<CharacterControllerAllInOne>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        AnimationHandler(anim);
    }



    void AnimationHandler(Animator anim)
    {
        resetAnimBools();

        convertInput();

        stateSetting();

        switch (state)
        {
            case animState.Idle:
                anim.SetBool("Idle", true);
                break;
            case animState.Walking:
                anim.SetBool("Running", true);
                break;
            case animState.Jumping:
                anim.SetBool("Jumping", true);
                break;
            case animState.Falling:
                anim.SetBool("Falling", true);
                break;
            case animState.Bouncing:
                anim.SetBool("Bouncing", true);
                break;
        }
    }

    private void resetAnimBools()
    {
        /*anim.SetBool(
            "Bouncing" +
            "Jumping" +
            "Idle" +
            "Running" +
            "Falling",
            false
            );*/
        anim.SetBool("Bouncing", false);
        anim.SetBool("Jumping", false);
        anim.SetBool("Idle", false);
        anim.SetBool("Running", false);
        anim.SetBool("Falling", false);
    }

    private void convertInput()
    {
        /*legacy animator parameters
        anim.SetFloat("Horizontal", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("Vertical", rb.velocity.y);
        anim.SetBool("Changing Directions", cc.changingDirections);
        anim.SetFloat("Horizontal Input", Mathf.Abs(Input.GetAxis("Horizontal")));
        anim.SetBool("Jumping", Input.GetButtonDown("Jump"));*/

        movementInput = new  Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rbVelocity = new Vector2(rb.velocity.x, rb.velocity.y);
        jump = Input.GetButton("Jump");

    }

    private int stateSetting()
    {
        //if jump button pressed: Jumping state
        if (jump || rb.velocity.y > 0 && !cc.launched) { state = animState.Jumping; return 0; }

        //Launched: launching state;
        if (cc.launched) { state = animState.Bouncing; return 0; }

        //on ground and no velocity: Idle state
        if (cc.onGround && rbVelocity.x < 0.05) { state = animState.Idle; return 0; }

        //on ground and positive velocity: Walking state
        if(cc.onGround && Mathf.Abs(rbVelocity.x) > 0.1) { state = animState.Walking; return 0; }

        //if vertical velocity < 0 & not launched: falling state
        if (!cc.onGround && rbVelocity.y < -0.1 && cc.launched == false) { state = animState.Falling; return 0; }

        state = animState.Idle;
        print("Player: Defaulted to Idle State");
        return 1;
    }

    /*animator method for original animation controller

    void AnimationHandler(Animator anim)
    {
        anim.SetFloat("Horizontal", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("Vertical", rb.velocity.y);
        anim.SetBool("Changing Directions", cc.changingDirections);
        anim.SetFloat("Horizontal Input", Mathf.Abs(Input.GetAxis("Horizontal")));

        commented out
        (anim.GetBool("Jumping"))
        {
            anim.SetBool("Jumping", false);
        }

    if (cc.jumpTimer > Time.time && cc.onGround)
    {
        anim.SetBool("Jumping", true);
        cc.jumping = true;
    }

    if (rb.velocity.y < 0)
    {
        cc.jumping = false;
    }

    if (cc.onGround && !cc.jumping)
    {
        anim.SetBool("Grounded", true);
        cc.jumping = false;
    }

    if (cc.jumping)
    {
        anim.SetBool("Jumping", true);
    }
    else
    {
        anim.SetBool("Jumping", false);
    }
    }*/
}
