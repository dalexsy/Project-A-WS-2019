using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private bool isUsingGravity = true;

    public bool isMoving = false;
    public bool isUsingInvertedGravity = false;
    public float moveSpeed;
    [HideInInspector] public float gravity = 10f;
    [HideInInspector] public int gravityDirection = 1;
    public static PlayerManager instance;
    public Transform currentPlank = null;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Update()
    {
        if (!isUsingGravity) gravity = 0f;
        if (isUsingInvertedGravity) gravityDirection = -1;
        else gravityDirection = 1;
    }
}
