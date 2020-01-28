using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public AnimationCurve animationCurve = null;
    public Animator animator;
    public static PlayerAnimationManager instance = null;
    [Range(1, 10)] public float rotationSpeed = 1f;

    public bool isMoving = false;
    public bool isTransitioningPlanks = false;
    public bool isTurning = false;
    public bool isWalking = false;

    public AnimationClip[] walkCycles;
    public AnimationClip[] idleCycles;

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
        animator.SetBool("isWalking", isWalking);

        SetAnimations();
    }

    private void SetAnimations()
    {
        // If move counter is over threshold, change walk animation and slow Player down
        if (MoveCounter.instance.moveCount >= 6)
        {
            animatorOverrideController["Player_Walk"] = walkCycles[1];
            animatorOverrideController["Player_Idle"] = idleCycles[1];

            animator.SetFloat("walkSpeed", .8f);
            PlayerManager.instance.moveSpeed = .8f;
        }

        // If level is completed, change walk animation and speed Player up
        if (PlankManager.instance.hasReachedGoal)
        {
            animatorOverrideController["Player_Walk"] = walkCycles[0];

            animator.SetFloat("walkSpeed", 1.1f);
            PlayerManager.instance.moveSpeed = 1.1f;
        }
    }
}
