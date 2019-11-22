using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerRotationLimitation : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float distance;
    public GameObject currentWaypoint;
    public GameObject nextWaypoint;
    private GameObject firstWaypoint;
    private GameObject lastWaypoint;
    private PlayerManager playerManager;
    private PlayerTransitionPlanks playerTransitionPlanks;

    private void Start()
    {
        playerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
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
        if (playerManager.isMoving) return;
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

        playerManager.isMoving = true;


        // Set target position as next waypoint's position with player's Y position
        // Will probably stop working
        Vector3 targetPosition = nextWaypoint.transform.position + this.transform.up * .05f;

        //Vector3 targetPosition = nextWaypoint.transform.position;

        // Rotate Player towards target position
        if (this.transform.up == nextWaypoint.transform.up) this.transform.LookAt(targetPosition);
        //else if ((direction == -1) && (this.transform.up != nextWaypoint.transform.forward)) transform.RotateAround(transform.position, transform.up, 180f);
        //this.transform.LookAt(targetPosition);

        // Set current position as Player's position
        var currentPosition = transform.position;

        distance = 100f;

        // While Player has not reached target postion
        while (Vector3.Distance(this.transform.position, nextWaypoint.transform.position) < distance &&
           Vector3.Distance(this.transform.position, nextWaypoint.transform.position) >= .001f)
        {
            distance = Vector3.Distance(this.transform.position, nextWaypoint.transform.position);

            // Pause movement while Player is rotating
            while (playerManager.isRotating) yield return null;

            // Set rate as move speed over time
            float rate = playerManager.moveSpeed * Time.deltaTime;

            // Translate Player forward
            transform.Translate(0, 0, rate);

            // Return to top of while loop
            yield return null;
        }

        this.transform.position = nextWaypoint.transform.position + (this.transform.up * .05f);
        //this.transform.up = nextWaypoint.transform.forward;
        playerManager.isMoving = false;

        yield return null;
    }

    private void OnTriggerStay(Collider collider)
    {
        // Set current waypoint as last waypoint Player has collided with
        if (collider.gameObject.tag == "Waypoint")
            currentWaypoint = collider.gameObject;
    }
}
