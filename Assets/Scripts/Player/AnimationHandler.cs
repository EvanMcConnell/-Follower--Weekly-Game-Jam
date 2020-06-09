using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private Animator animator;
    private CharacterMovement2D cm2d;

    public static AnimationHandler CreateComponent(GameObject where, Animator animator,CharacterMovement2D cm2d)
    {
        AnimationHandler ah = where.AddComponent<AnimationHandler>();
        ah.animator = animator;
        ah.cm2d = cm2d;
        return ah;
    }

    void Update()
    {
        animator.SetFloat("Horizontal", Mathf.Abs(Input.GetAxis("Horizontal")));
        animator.SetBool("Grounded", cm2d.getOnGround());
    }
}
