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

    private bool hasBecomeSad = false;
    private bool hasCelebrated = false;

    public AnimationClip[] idleCycles;
    public AnimationClip[] jumpCycles;
    public AnimationClip[] walkCycles;

    public Texture[] faceTextures;

    private Renderer rend;

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
        rend = GameObject.Find("Face_Plane").GetComponent<Renderer>();
    }

    private void Update()
    {
        animator.SetBool("isWalking", isWalking);

        SetAnimations();
    }

    private void SetAnimations()
    {
        // If move counter is over threshold, change walk animation and slow Player down
        if (MoveCounter.instance.moveCount >= ScoreManager.instance.sadnessThreshold && hasBecomeSad == false)
        {
            animatorOverrideController["Player_Walk"] = walkCycles[1];
            animatorOverrideController["Player_Idle"] = idleCycles[1];

            // Replace face texture
            rend.material.SetTexture("_BaseMap", faceTextures[1]);

            animator.SetFloat("walkSpeed", .8f);
            hasBecomeSad = true;
        }

        // If level is completed, change walk animation and speed Player up
        if (PlankManager.instance.hasReachedGoal && !hasCelebrated)
        {
            animatorOverrideController["Player_Walk"] = walkCycles[0];
            animator.SetFloat("walkSpeed", 1.1f);
            PlayerManager.instance.moveSpeed *= 1.1f;

            // Replace face texture
            rend.material.SetTexture("_BaseMap", faceTextures[2]);
            hasCelebrated = true;
        }

        // If jump angle is 90 degrees, use shorter jump animation, otherwise use longer animation
        if (PlayerMovement.instance.jumpAngle == 90) animatorOverrideController["Player_Jump"] = jumpCycles[0];
        else animatorOverrideController["Player_Jump"] = jumpCycles[1];
    }
}
