using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationLimitation : MonoBehaviour
{
    private PlayerPlankDetection playerPlankDetection;
    private PlankManager plankManager;

    private Transform currentPivot;
    private Transform plankTransitionPoint;
    public Vector3 pivotDirection;
    public Vector3 playerDifference;

    private void Start()
    {
        plankManager = GameObject.Find("PlankManager").GetComponent<PlankManager>();
        playerPlankDetection = GetComponent<PlayerPlankDetection>();
    }

    private void Update()
    {
        if (playerPlankDetection.currentPlank)
        {
            if (playerPlankDetection.currentPlank.GetComponent<PlankRotation>().activePivot)
                currentPivot = playerPlankDetection.currentPlank.GetComponent<PlankRotation>().activePivot;

            plankTransitionPoint = playerPlankDetection.currentPlank.Find("Plank Transition Point");

            var pivotDifference = plankTransitionPoint.position - currentPivot.position;
            var pivotDistance = pivotDifference.magnitude;
            pivotDirection = pivotDifference / pivotDistance;
            Debug.DrawRay(this.transform.position, pivotDirection, Color.green);
            //Debug.DrawLine(this.transform.position, plankTransitionPoint.position, Color.green);

            playerDifference = this.transform.position - plankTransitionPoint.position;



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
