using UnityEngine;

public class MoveCounter : MonoBehaviour
{
    /// <summary>
    /// The amount of moves the player has taken in a level. A plank rotation counts as a move.
    /// </summary>
    public int moveCount = 0;

    public static MoveCounter instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }
}
