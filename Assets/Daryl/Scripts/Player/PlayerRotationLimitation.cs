using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerRotationLimitation : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    public bool isMoving = false;
    private float moveSpeed = 1f;
    private GameObject currentWaypoint;
    private GameObject nextWaypoint;
    private GameObject firstWaypoint;
    private GameObject lastWaypoint;
    private PlayerTransitionPlanks playerTransitionPlanks;

    private void Start()
    {
        playerTransitionPlanks = GetComponent<PlayerTransitionPlanks>();

        // Find all waypoints in scene
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        // Sort waypoints alphabetically
        Array.Sort(waypoints, (x, y) => String.Compare(x.transform.name, y.transform.name));

        // Set first and last waypoint
        firstWaypoint = waypoints[0];
        lastWaypoint = waypoints[waypoints.Length - 1];
    }

    private void Update()
    {
        if (isMoving) return;
        if (Input.GetKeyDown(KeyCode.W)) StartCoroutine(TransitionWaypoints(1));
        if (Input.GetKeyDown(KeyCode.S)) StartCoroutine(TransitionWaypoints(-1));
    }

    IEnumerator TransitionWaypoints(int direction)
    {
        // If Player tries to move past first or last waypoint, exit coroutine
        if ((currentWaypoint == firstWaypoint && direction == -1) ||
            (currentWaypoint == lastWaypoint && direction == 1)) yield break;

        // Find index of current waypoint
        var currentIndex = Array.FindIndex(waypoints, item => item.transform.name.Equals(currentWaypoint.name));

        // Set next waypoint as previous or next element in array based on given direction
        nextWaypoint = waypoints[currentIndex + direction];

        // If first and last waypoint are used to transition between, exit coroutine
        if ((currentWaypoint == firstWaypoint && nextWaypoint == lastWaypoint) ||
            (currentWaypoint == lastWaypoint && nextWaypoint == firstWaypoint)) yield break;

        isMoving = true;

        // Set target position as next waypoint's position with player's Y position
        // Will probably stop working
        Vector3 targetPosition = nextWaypoint.transform.position;

        // Rotate Player towards target position
        // Should be a coroutine
        this.transform.LookAt(targetPosition);

        // Set current position as Player's position
        var currentPosition = transform.position;

        // While Player has not reached target postion
        while (Vector3.Distance(this.transform.position, nextWaypoint.transform.position) >= .06f)
        {
            while (playerTransitionPlanks.isRotating) yield return null;

            Debug.Log(Vector3.Distance(this.transform.position, nextWaypoint.transform.position));

            // Set rate as move speed over time
            float rate = moveSpeed * Time.deltaTime;

            // Move Player towards target position
            //

            transform.Translate(0, 0, rate);

            // Return to top of while loop
            yield return null;
        }

        isMoving = false;
        yield return null;
    }

    private void OnTriggerStay(Collider collider)
    {
        // Set current waypoint as last waypoint Player has collided with
        if (collider.gameObject.tag == "Waypoint")
            currentWaypoint = collider.gameObject;
    }
}
