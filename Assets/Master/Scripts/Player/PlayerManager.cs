using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public bool isUsingGravity = true;
    public bool isUsingInvertedGravity = false;
    public float moveSpeed;
    [HideInInspector] public float gravity = 10f;
    [HideInInspector] public int gravityDirection = 1;
    public static PlayerManager instance;
    public Transform currentPlank = null;
    public Transform activePivot = null;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Update()
    {
        if (currentPlank) activePivot = currentPlank.GetComponent<PlankRotation>().activePivot;
        if (!isUsingGravity) gravity = 0f;
        if (isUsingInvertedGravity) gravityDirection = -1;
        else gravityDirection = 1;
    }
}
