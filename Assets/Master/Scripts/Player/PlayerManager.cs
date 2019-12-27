using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public bool isMoving = false;
    public bool isRotating = false;
    public bool isUsingInvertedGravity = false;
    public float moveSpeed;
    [HideInInspector] public float gravity = 10f;
    [HideInInspector] public int gravityDirection = 1;
    public Transform currentPlank = null;

    private bool isUsingGravity = true;

    private void Update()
    {
        if (!isUsingGravity) gravity = 0f;
        if (isUsingInvertedGravity) gravityDirection = -1;
        else gravityDirection = 1;
    }
}
