using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationLimitation : MonoBehaviour
{
    private PlayerPlankDetection playerPlankDetection;
    private PlankManager plankManager;
    public Transform currentLeftPivot;
    public Transform currentRightPivot;

    private void Start()
    {
        plankManager = GameObject.Find("PlankManager").GetComponent<PlankManager>();
        playerPlankDetection = GetComponent<PlayerPlankDetection>();
    }

    private void Update()
    {
        if (playerPlankDetection.currentPlank)
        {
            currentLeftPivot = playerPlankDetection.currentPlank.transform.Find(plankManager.leftPivotName);
            currentRightPivot = playerPlankDetection.currentPlank.transform.Find(plankManager.rightPivotName);

            var pivotDifference = currentLeftPivot.position - currentRightPivot.position;
            var pivotDistance = pivotDifference.magnitude;
            var pivotDirection = pivotDifference / pivotDistance;

            // Need current plank's connected planks
            // If pivots are in the same location (current R, next L), no rotation limitation needed
            // If pivots are in different locations, planks are not aligned and rotation limited is needed
            // Script could go on transition points

            if (playerPlankDetection.currentPlank != playerPlankDetection.nextPlank)
            {
                return;
            }
        }
    }
}
