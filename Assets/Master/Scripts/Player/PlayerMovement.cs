using System.Collections;
using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float distance;
    public GameObject currentWaypoint;
    public GameObject nextWaypoint;
    private GameObject firstWaypoint;
    private GameObject lastWaypoint;
    private GameObject leftWaypoint;
    private GameObject rightWaypoint;
    private PlankRotationManager plankRotationManager;
    private PlayerManager playerManager;

    private void Start()
    {
        plankRotationManager = GameObject.Find("Plank Manager").GetComponent<PlankRotationManager>();
        playerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();

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
        // If Player is moving or Plank is rotating, accept no input
        if (playerManager.isMoving || plankRotationManager.isRotating) return;

        if (playerManager.horizontalInput > 0) StartCoroutine(TransitionWaypoints(1));
        if (playerManager.horizontalInput < 0) StartCoroutine(TransitionWaypoints(-1));
    }

    IEnumerator TransitionWaypoints(int direction, int? arrayDirection = null)
    {
        // If using transitional waypoint, use last direction in array to find next waypoint
        int aD = 0;
        if (arrayDirection == null)
        {
            aD = ScreenToWaypoint(direction);
        }

        else
        {
            var currentIndex = Array.FindIndex(waypoints, item => item.transform.name.Equals(currentWaypoint.name));
            nextWaypoint = waypoints[currentIndex + (int)arrayDirection];
        }

        // If no next waypoint is given, exit coroutine
        if (nextWaypoint == null) yield break;

        playerManager.isMoving = true;

        // Set target position as next waypoint's position with player's Y position
        Vector3 targetPosition = nextWaypoint.transform.position + transform.up * .05f;

        // If next waypoint is aligned with current waypoint, rotate Player towards target position
        if (V3Equal(transform.up, nextWaypoint.transform.up))
        {
            this.transform.LookAt(targetPosition, nextWaypoint.transform.up);
        }

        // Else if next waypoint is not aligned with current waypoint, teleport Player to next waypoint
        else
        {
            yield return new WaitForSeconds(.5f); // Needed to stop coroutine from restarting
            transform.position = nextWaypoint.transform.position;
            transform.up = nextWaypoint.transform.up;
            playerManager.isMoving = false;
            yield break;
        }

        // Set current position as Player's position
        var currentPosition = transform.position;

        distance = 100f;

        // While Player has not reached target postion
        while (Vector3.Distance(transform.position, nextWaypoint.transform.position) < distance &&
           Vector3.Distance(transform.position, nextWaypoint.transform.position) >= .001f)
        {
            // Take distance from Player's position to next waypoint's position
            distance = Vector3.Distance(transform.position, nextWaypoint.transform.position);

            // Pause movement while Player is rotating
            while (playerManager.isRotating) yield return null;

            // Set rate as move speed over time
            float rate = playerManager.moveSpeed * Time.deltaTime;

            // Translate Player forward
            transform.Translate(0, 0, rate);

            // Return to top of while loop
            yield return null;
        }

        playerManager.isMoving = false;

        // If next waypoint is flagged as transitional
        if (nextWaypoint.GetComponent<WaypointMarker>().isTransitional == true)
        {
            // Set next waypoint as current waypoint
            currentWaypoint = nextWaypoint;

            // Restart coroutine with new current waypoint
            StartCoroutine(TransitionWaypoints(direction, aD));
        }

        yield return null;
    }

    private int ScreenToWaypoint(int direction)
    {
        // Find index of current waypoint
        var currentIndex = Array.FindIndex(waypoints, item => item.transform.name.Equals(currentWaypoint.name));
        var nextWaypointInArray = waypoints[currentIndex];

        // If current waypoint is not last waypoint, set nextWaypointInArray as next waypoint in array
        if (currentWaypoint != lastWaypoint) nextWaypointInArray = waypoints[currentIndex + 1];

        var currentWaypointScreenPosition = Camera.main.WorldToScreenPoint(currentWaypoint.transform.position);
        var nextWaypointScreenPosition = Camera.main.WorldToScreenPoint(nextWaypointInArray.transform.position);

        // Set screenPosition as current waypoint's position minus next waypoint's position along the x-axis
        var screenPosition = currentWaypointScreenPosition.x - nextWaypointScreenPosition.x;

        // If screen position is positive, next waypoint is to the left (if given)
        if (screenPosition > 0)
        {
            leftWaypoint = nextWaypointInArray;
            if (currentIndex > 0) rightWaypoint = waypoints[currentIndex - 1];
            else rightWaypoint = null;
        }

        // If screen position is negative, next waypoint is to the right (if given)
        else
        {
            rightWaypoint = nextWaypointInArray;
            if (currentIndex > 0) leftWaypoint = waypoints[currentIndex - 1];
            else leftWaypoint = null;
        }

        // If horizontal input is to the left, next waypoint is left waypoint
        if (direction < 0) nextWaypoint = leftWaypoint;

        // If horizontal input is to the right, next waypoint is right waypoint
        else nextWaypoint = rightWaypoint;

        if (nextWaypoint == currentWaypoint) nextWaypoint = null;

        if (nextWaypointInArray == nextWaypoint) return 1;
        else return -1;
    }

    private void OnTriggerStay(Collider collider)
    {
        // Set current waypoint as last waypoint Player has collided with
        if (collider.gameObject.tag == "Waypoint")
            currentWaypoint = collider.gameObject;
    }

    private bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.9;
    }
}
