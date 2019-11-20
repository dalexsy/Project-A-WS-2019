using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerRotationLimitation : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private bool isMoving = false;
    private float moveSpeed = 1f;
    private GameObject currentWaypoint;
    private GameObject nextWaypoint;
    public GameObject firstWaypoint;
    public GameObject lastWaypoint;

    private void Start()
    {
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
        Vector3 targetPosition = new Vector3(nextWaypoint.transform.position.x,
                                               this.transform.position.y,
                                               nextWaypoint.transform.position.z);


        // Rotate Player towards target position
        // Should be a coroutine
        this.transform.LookAt(targetPosition);

        // Set current position as Player's position
        var currentPosition = transform.position;

        // While Player has not reached target postion
        while (Vector3.Distance(currentPosition, targetPosition) >= .1f)
        {
            // Update current position
            currentPosition = this.transform.position;

            // Set rate as move speed over time
            float rate = moveSpeed * Time.deltaTime;

            // Move Player towards target position
            transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, rate);

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
