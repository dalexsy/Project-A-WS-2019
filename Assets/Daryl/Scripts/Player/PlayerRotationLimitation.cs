using System.Collections;
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
    private PlayerPlankDetection playerPlankDetection;

    private void Start()
    {
        playerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        playerPlankDetection = GetComponent<PlayerPlankDetection>();

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
        if (playerManager.verticalInput > 0) StartCoroutine(TransitionWaypoints(1));
        if (playerManager.verticalInput < 0) StartCoroutine(TransitionWaypoints(-1));
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
        Vector3 targetPosition = nextWaypoint.transform.position + this.transform.up * .05f;

        // Rotate Player towards target position
        if (V3Equal(this.transform.up, nextWaypoint.transform.up))
        {
            this.transform.LookAt(targetPosition, nextWaypoint.transform.up);
        }

        else
        {
            yield return new WaitForSeconds(.5f);
            this.transform.position = nextWaypoint.transform.position;
            this.transform.up = nextWaypoint.transform.up;
            playerManager.isMoving = false;
            yield break;
        }

        // Set current position as Player's position
        var currentPosition = transform.position;

        distance = 100f;

        // While Player has not reached target postion
        while (Vector3.Distance(this.transform.position, nextWaypoint.transform.position) < distance &&
           Vector3.Distance(this.transform.position, nextWaypoint.transform.position) >= .001f)
        {
            // Take distance from Player's position to next waypoint's position
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

        playerManager.isMoving = false;

        // If next waypoint is flagged as transitional
        if (nextWaypoint.GetComponent<WaypointMarker>().isTransitional == true)
        {
            // Set next waypoint as current waypoint
            currentWaypoint = nextWaypoint;

            // Restart coroutine with new current waypoint
            StartCoroutine(TransitionWaypoints(direction));
        }

        yield return null;
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
