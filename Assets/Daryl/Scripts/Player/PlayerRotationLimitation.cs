using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerRotationLimitation : MonoBehaviour
{
    private PlayerPlankDetection playerPlankDetection;
    private PlankManager plankManager;

    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private List<Vector3> waypointPositions = new List<Vector3>();


    private Transform currentPivot;
    private Transform waypoint;
    public Vector3 pivotDirection;
    public Vector3 playerDifference;

    private void Start()
    {
        plankManager = GameObject.Find("PlankManager").GetComponent<PlankManager>();
        playerPlankDetection = GetComponent<PlayerPlankDetection>();
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        Array.Sort(waypoints, (x, y) => String.Compare(x.transform.name, y.transform.name));

        foreach (var waypoint in waypoints)
            waypointPositions.Add(waypoint.transform.position);
    }

    private void Update()
    {
        return;
        if (playerPlankDetection.currentPlank)
        {
            if (playerPlankDetection.currentPlank.GetComponent<PlankRotation>().activePivot)
                currentPivot = playerPlankDetection.currentPlank.GetComponent<PlankRotation>().activePivot;

            waypoint = playerPlankDetection.currentPlank.Find("Waypoint");

            var pivotDifference = waypoint.position - currentPivot.position;
            var pivotDistance = pivotDifference.magnitude;
            pivotDirection = pivotDifference / pivotDistance;
            Debug.DrawRay(this.transform.position, pivotDirection, Color.green);
            //Debug.DrawLine(this.transform.position, plankTransitionPoint.position, Color.green);

            playerDifference = this.transform.position - waypoint.position;

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
