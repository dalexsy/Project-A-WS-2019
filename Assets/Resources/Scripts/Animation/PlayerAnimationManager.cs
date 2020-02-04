using UnityEngine.SceneManagement;
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
    public bool shouldCelebrate = true;

    private bool hasBecomeSad = false;
    private bool hasCelebrated = false;
    private bool canBlink = false;

    private float blinkTimer = 0f;

    public AnimationClip[] idleCycles;
    public AnimationClip[] jumpCycles;
    public AnimationClip[] walkCycles;

    public Texture[] faceTextures;
    public Texture[] happyBlink;

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

        // Populate happy blink array with resources from level's blink texture folder
        happyBlink = Resources.LoadAll<Texture>("Textures/Player/" + SceneManager.GetActiveScene().name + "/Blink");
        faceTextures[0] = Resources.Load<Texture>("Textures/Player/" + SceneManager.GetActiveScene().name + "/Sad/sad");
        faceTextures[1] = Resources.Load<Texture>("Textures/Player/" + SceneManager.GetActiveScene().name + "/Happy/happy");

        rend = GameObject.Find("Face_Plane").GetComponent<Renderer>();
    }

    private void Update()
    {
        animator.SetBool("isWalking", isWalking);

        SetAnimations();
    }

    private void FixedUpdate()
    {
        Blink();
    }

    private void SetAnimations()
    {
        // If move counter is over threshold, change walk animation and slow Player down
        if (MoveCounter.instance.moveCount >= ScoreManager.instance.sadnessThreshold && hasBecomeSad == false)
        {
            animatorOverrideController["Player_Walk"] = walkCycles[1];
            animatorOverrideController["Player_Idle"] = idleCycles[1];

            // Replace face texture
            rend.material.SetTexture("_BaseMap", faceTextures[0]);

            animator.SetFloat("walkSpeed", .8f);
            hasBecomeSad = true;
        }

        // If level is completed, change walk animation and speed Player up
        if (PlankManager.instance.hasReachedGoal && !hasCelebrated)
        {
            animatorOverrideController["Player_Walk"] = walkCycles[2];
            animator.SetFloat("walkSpeed", 1.7f);
            PlayerManager.instance.moveSpeed *= 1.1f;

            // Replace face texture
            rend.material.SetTexture("_BaseMap", faceTextures[1]);
            hasCelebrated = true;
        }

        // If jump angle is 90 degrees, use shorter jump animation, otherwise use longer animation
        if (PlayerMovement.instance.jumpAngle < 100)
        {
            animatorOverrideController["Player_Jump"] = jumpCycles[0];
            animator.SetFloat("jumpSpeed", 2.2f);
        }

        else
        {
            animatorOverrideController["Player_Jump"] = jumpCycles[1];
            animator.SetFloat("jumpSpeed", 1.0f);
        }
    }

    private void Blink()
    {
        // Increase blink timer
        blinkTimer += Time.deltaTime;

        // If blink timer is over threshold, Player can blink again
        if (blinkTimer > 5) canBlink = true;

        // If Player can blink, sequentially change face texture using texture array
        if (canBlink && !hasBecomeSad && !hasCelebrated)
        {
            int index = Mathf.FloorToInt(Time.time / .05f);
            index = index % happyBlink.Length;
            rend.material.SetTexture("_BaseMap", happyBlink[index]);

            // If end of animation is reached, reset bool and reset timer
            if (index == happyBlink.Length - 1)
            {
                canBlink = false;
                blinkTimer = 0;
            }
        }
    }
}
