using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public AnimationCurve animationCurve = null;
    public Animator animator;
    public static PlayerAnimationManager instance = null;
    [Range(1, 10)] public float rotationSpeed = 1f;

    public bool isJumping = false;
    public bool isMoving = false;
    public bool isTransitioningPlanks = false;
    public bool isTurning = false;
    public bool isWalking = false;

    public AnimationClip sadWalk;
    protected AnimatorOverrideController animatorOverrideController;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;
    }

    private void Update()
    {
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isWalking", isWalking);

        // If move counter is over threshold, change walk animation and slow Player down
        if (MoveCounter.instance.moveCount >= 10)
        {
            animatorOverrideController["Player_Walk"] = sadWalk;
            animator.SetFloat("walkSpeed", .8f);
            PlayerManager.instance.moveSpeed = .8f;
        }
    }
}
