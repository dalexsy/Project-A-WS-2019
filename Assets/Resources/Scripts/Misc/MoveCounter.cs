using UnityEngine;

public class MoveCounter : MonoBehaviour
{
    public int moveCount = 0;
    public readonly int perfectCount = 2; // Perfect solution
    public static MoveCounter instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }
}
