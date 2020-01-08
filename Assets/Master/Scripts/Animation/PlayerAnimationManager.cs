using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public static PlayerAnimationManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }
}
