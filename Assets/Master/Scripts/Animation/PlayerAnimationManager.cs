using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] public AnimationCurve animationCurve = null;
    [SerializeField] public Animator animator;
    public static PlayerAnimationManager instance = null;
    [SerializeField] [Range(1, 10)] public float rotationSpeed = 1f;

    public bool isJumping = false;
    public bool isWalking = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Update()
    {
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isWalking", isWalking);
    }
}
