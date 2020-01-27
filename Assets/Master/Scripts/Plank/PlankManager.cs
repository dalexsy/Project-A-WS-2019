using UnityEngine;

public class PlankManager : MonoBehaviour
{
    public static PlankManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

#pragma warning disable IDE0051 // Remove unused private members

    [Header("First & Last Plank")]
    [TextArea(0, 5)]
    [SerializeField]
    private string usageNotes1 = "The first and last Plank are the start and end Plank in the level. Assign their transforms here.";

    public Transform firstPlank;
    public Transform lastPlank;

    [Header("Pivot Names")]
    [TextArea(0, 5)]
    [SerializeField]
    private string usageNotes2 = "Pivots must be named exactly as listed here.";

    public string leftPivotName = "Pivot L";
    public string rightPivotName = "Pivot R";

    [Header("Plank Collider Names")]
    [TextArea(0, 5)]
    [SerializeField]
    private string usageNotes3 = "Plank colliders must be named exactly as listed here.";

    public string topColliderName = "Plank Collider Top";
    public string bottomColliderName = "Plank Collider Bottom";
    public string frontColliderName = "Plank Collider Front";
    public string backColliderName = "Plank Collider Back";

    [Header("Plank Transition Point Names")]
    [TextArea(0, 5)]
    [SerializeField]
    private string usageNotes4 = "Plank transition points must be named exactly as listed here.";

    public string leftTransitionPointName = "Plank Transition Point L";
    public string rightTransitionPointName = "Plank Transition Point R";

    [Header("Plank Collider Tags")]
    [TextArea(0, 5)]
    [SerializeField]
    private string usageNotes5 = "Top, bottom, front, and back colliders must be tagged with their respective sides.";

    public string leftColliderTag = "Collider L";
    public string rightColliderTag = "Collider R";

    [Header("Goal Detection")]
    public bool hasReachedGoal = false;

    [Header("Level Connected")]
    public bool isLevelConnected = false;

#pragma warning restore IDE0051 // Remove unused private members

}
