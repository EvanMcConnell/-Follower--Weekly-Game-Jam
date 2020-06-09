using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour {

    [Header("Movement Handler")]
    public Rigidbody2D rb;
    public float moveSpeed;
    public bool canJump;
    public float jumpSpeed;
    public float jumpDelay;
    public LayerMask groundLayer;
    public float groundLength;
    private CharacterMovement2D characterMovement2D;

    [Header("Launch Controller")]
    public bool createLauncher;
    public PhysicsMaterial2D[] materials;
    public SpriteRenderer launchCursor;

    [Header("Trigger Handler")]
    public int loadWaitTime;
    public string nextScene;
    public bool quitAfterLevel;

    [Header("Animation Handler")]
    public Animator animator;

    void Awake() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        characterMovement2D = CharacterMovement2D.CreateComponent(gameObject, rb, moveSpeed, canJump, jumpSpeed, jumpDelay, groundLayer, groundLength);
        if (createLauncher) { LaunchController.CreateComponent(gameObject, rb, materials, launchCursor, characterMovement2D); }
        AnimationHandler.CreateComponent(gameObject, animator, characterMovement2D);
        TriggerHandler.CreateComponent(gameObject, loadWaitTime, nextScene, quitAfterLevel);
    }

    void Update() {}
}