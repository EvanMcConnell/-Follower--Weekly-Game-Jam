using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchController : MonoBehaviour
{
    [SerializeField]
    private Launcher launcher;
    [SerializeField]
    private CharacterMovement2D cm2d;
    [SerializeField]
    private Rigidbody2D rb;

    [Header("Materials")]
    public PhysicsMaterial2D[] materials;

    [Header("Sprite Renderer")]
    public SpriteRenderer launchCursorSprite;

    [Header("GameObjects")]
    public GameObject controllerWarningText;

    [Header("Vectors")]
    private Vector2 launchDir;
    private Vector2 finalLaunchDir;
    private Vector2 centreScreen;

    [Header("Integers")]
    public int bouncesRemaining;

    [Header("Floats")]
    public float flickWindow = 0.5f;
    private float flickTime = 0f;
    private float slowTimeSpeed = 0.2f;

    [Header("Bools")]
    [SerializeField]
    private bool flickable = false;
    public bool launched = false;
    public bool launchReady;

    public static LaunchController CreateComponent(GameObject where, Rigidbody2D rb, PhysicsMaterial2D[] materials, SpriteRenderer launchCursorSprite, CharacterMovement2D cm2d) {
        LaunchController lc = where.AddComponent<LaunchController>();
        lc.rb = rb;
        lc.materials = materials;
        lc.launchCursorSprite = launchCursorSprite;
        lc.cm2d = cm2d;
        return lc;
    }

    void Start()
    {
        launcher = Launcher.CreateComponent(gameObject, rb);

        centreScreen = new Vector2(Display.main.renderingWidth / 2, Display.main.renderingHeight / 2);
    }

    void Update()
    {
        launchCursor();
        prepareLaunch();
        //print(launchDir.magnitude);
    }


    void FixedUpdate()
    {

    }


    void takeInput()
    {
        if (Input.GetKey(KeyCode.Mouse0) && Input.GetJoystickNames().Length == 0)
        {
            finalLaunchDir = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - centreScreen;
            finalLaunchDir.Scale(new Vector2(-1, -1));
            finalLaunchDir.Normalize();

            launchCursorSprite.enabled = true;
            Quaternion cursorDir = Quaternion.LookRotation((Vector3.right * finalLaunchDir.x + Vector3.up * finalLaunchDir.y), Vector3.back);
            launchCursorSprite.gameObject.GetComponent<Rigidbody2D>().SetRotation(cursorDir);
            launchCursorSprite.gameObject.transform.position = transform.position;
        }
        else if(Input.GetKey(KeyCode.Mouse0) && Input.GetJoystickNames().Length != 0)
        {

        }
        else
        {
            finalLaunchDir = new Vector2(Input.GetAxis("R_Horizontal"), Input.GetAxis("R_Vertical"));

            launchCursorSprite.enabled = true;
            Quaternion cursorDir = Quaternion.LookRotation((Vector3.right * launchDir.x + Vector3.up * launchDir.y), Vector3.back);
            launchCursorSprite.gameObject.GetComponent<Rigidbody2D>().SetRotation(cursorDir);
            launchCursorSprite.gameObject.transform.position = transform.position;
        }
    }

    void launchCursor()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            finalLaunchDir = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - centreScreen;
            finalLaunchDir.Scale(new Vector2(-1, -1));
            finalLaunchDir.Normalize();

            launchCursorSprite.enabled = true;
            Quaternion cursorDir = Quaternion.LookRotation((Vector3.right * finalLaunchDir.x + Vector3.up * finalLaunchDir.y), Vector3.back);
            launchCursorSprite.gameObject.GetComponent<Rigidbody2D>().SetRotation(cursorDir);
            launchCursorSprite.gameObject.transform.position = transform.position;
        }
        else
        {
            finalLaunchDir = new Vector2(Input.GetAxis("R_Horizontal"), Input.GetAxis("R_Vertical"));

            launchCursorSprite.enabled = true;
            Quaternion cursorDir = Quaternion.LookRotation((Vector3.right * launchDir.x + Vector3.up * launchDir.y), Vector3.back);
            launchCursorSprite.gameObject.GetComponent<Rigidbody2D>().SetRotation(cursorDir);
            launchCursorSprite.gameObject.transform.position = transform.position;
        }
    }

    void prepareLaunch()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            launchDir = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - centreScreen;
            launchDir.Normalize();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            launchDir = new Vector2(Input.GetAxis("R_Horizontal"), Input.GetAxis("R_Vertical"));
        }


        if (launchDir.magnitude > 0.8)
        {

            launchCursor();

            flickable = true;

            flickTime = Time.time;
            print("final launch direction: " + finalLaunchDir);

            slowTime(slowTimeSpeed);
        }
        else if (launchDir.magnitude >= 0.2)
        {

            resumeTime();

            launchCursorSprite.enabled = false;
        }
        else if (launchDir.magnitude < 0.2 && (Time.time - flickWindow) < flickTime && flickable == true)
        {
            rb.velocity = new Vector2(0, 0);
            //finalLaunchDir = launchDir.normalized;
            print("final launch direction: " + finalLaunchDir);
            print("basic launch direction: " + launchDir);
            launcher.launch(launchDir);
            flickable = false;
            print("launched oh goodie");
            cm2d.setGroundable(false);
            bouncesRemaining = 1;

            //set material to bouncy
            GetComponent<BoxCollider2D>().sharedMaterial = materials[1];

            resumeTime();

            launchCursorSprite.enabled = false;

            //flickable = false;
            //animator.SetBool("Launching", false);
        }
        else
        {
            launchCursorSprite.enabled = false;

            resumeTime();
        }


        /*if (launchReady == true)
        {
            if (launchDir.magnitude > 0.8)
            {

                launchCursor();

                flickTime = Time.time;

                slowTime(slowTimeSpeed);
            }
            else if (launchDir.magnitude >= 0.2)
            {

                resumeTime();

                launchCursorSprite.enabled = false;
            }
            else if (launchDir.magnitude < 0.2 && (Time.time - flickWindow) < flickTime && flickable == true)
            {
                rb.velocity = new Vector2(0, 0);
                launcher.launch(finalLaunchDir);
                cm2d.setGroundable(false);
                bouncesRemaining = 1;

                //set material to bouncy
                GetComponent<BoxCollider2D>().sharedMaterial = materials[1];

                resumeTime();

                launchCursorSprite.enabled = false;

                //flickable = false;
                //animator.SetBool("Launching", false);
            }
            else
            {
                launchCursorSprite.enabled = false;

                resumeTime();
            }
        }*/
    }

    public void toggleBounciness()
    {
        launched = false;
        PhysicsMaterial2D x = new PhysicsMaterial2D(GetComponent<BoxCollider2D>().sharedMaterial.name);
        x = x == materials[0] ? materials[0] : materials[1];
        cm2d.setGroundable(true);
    }

    void slowTime(float t)
    {
        Time.timeScale = t;
        Time.fixedDeltaTime = t * 0.02f;
    }

    void resumeTime()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
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
        if (launched == true)
        {
            //launchReady = true;
            if (bouncesRemaining == 0)
            {
                launched = false;
                //modifyBounciness();
            }
            else { bouncesRemaining--; }
        }
    }
}
