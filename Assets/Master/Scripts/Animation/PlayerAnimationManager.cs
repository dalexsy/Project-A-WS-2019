using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public static PlayerAnimationManager instance;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }
}
