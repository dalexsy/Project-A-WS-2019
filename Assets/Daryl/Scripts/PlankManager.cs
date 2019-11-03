using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankManager : MonoBehaviour
{
    // Disabled unused variable warning
#pragma warning disable 0414

    [Header("First & Last Plank")]
    [TextArea(0, 5)]
    [SerializeField]
    private string usageNotes1 = "The first and last Plank are the start and end Plank in the level. Assign their transforms here.";

    [SerializeField] public Transform firstPlank;
    [SerializeField] public Transform lastPlank;

    [Header("Pivot Names")]
    [TextArea(0, 5)]
    [SerializeField]
    private string usageNotes2 = "Pivots must be named exactly as listed here.";

    [SerializeField] public string leftPivotName = "Pivot L";
    [SerializeField] public string rightPivotName = "Pivot R";

    [Header("Plank Collider Names")]
    [TextArea(0, 5)]
    [SerializeField]
    private string usageNotes3 = "Plank colliders must be named exactly as listed here.";

    [SerializeField] public string topColliderName = "Plank Collider Top";
    [SerializeField] public string bottomColliderName = "Plank Collider Bottom";
    [SerializeField] public string frontColliderName = "Plank Collider Front";
    [SerializeField] public string backColliderName = "Plank Collider Back";

    [Header("Plank Collider Tags")]
    [TextArea(0, 5)]
    [SerializeField]
    private string usageNotes4 = "Top, bottom, front, and back colliders must be tagged with their respective sides.";

    [SerializeField] public string leftColliderTag = "Collider L";
    [SerializeField] public string rightColliderTag = "Collider R";
}
