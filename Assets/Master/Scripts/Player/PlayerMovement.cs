using System.Collections;
using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private float distance;
    [SerializeField] private GameObject currentWaypoint;
    private GameObject targetWaypoint;
    private GameObject nextWaypoint;
    private GameObject firstWaypoint;
    private GameObject lastWaypoint;
    private GameObject leftWaypoint;
    private GameObject rightWaypoint;
    private int arrayDirection;
    private InputManager inputManager;
    private InputVFXManager inputVFXManager;
    private PauseManager pauseManager;
    private PlankManager plankManager;
    private PlankRotationManager plankRotationManager;
    private PlayerAudioManager playerAudioManager;
    private PlayerManager playerManager;
    private Vector3 startInputPos;

    private void Start()
    {
        inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();
        inputVFXManager = GameObject.Find("VFX Manager").GetComponent<InputVFXManager>();
        pauseManager = GameObject.Find("Game Manager").GetComponent<PauseManager>();
        plankManager = GameObject.Find("Plank Manager").GetComponent<PlankManager>();
        plankRotationManager = GameObject.Find("Plank Manager").GetComponent<PlankRotationManager>();
        playerAudioManager = GameObject.Find("SFX Manager").GetComponent<PlayerAudioManager>();
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

        if (plankManager.hasReachedGoal) RunCircles();

        // If Player is moving, Plank is rotating, or game is paused, accept no input
        if (playerManager.isMoving || plankRotationManager.isRotating || inputManager.isSwiping || pauseManager.isPaused) return;

        if (!inputManager.isUsingTouch || Application.platform == RuntimePlatform.WebGLPlayer) MouseInput();

        if (inputManager.isUsingTouch && Application.platform != RuntimePlatform.WebGLPlayer) TouchInput();
    }

    private void TouchInput()
    {
        if (inputManager.isDoubleSwiping == true || inputManager.isSwiping == true) return;

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startInputPos = touch.position;
                    break;

                // If touch distance is smaller than threshold, select tapped waypoint
                case TouchPhase.Ended:
                    if (Vector3.Distance(touch.position, startInputPos) < 2f) SelectWaypoint();
                    break;
            }
        }
    }

    private void MouseInput()
    {
        // Set starting mouse position on mouse down
        if (Input.GetMouseButtonDown(0)) startInputPos = Input.mousePosition;

        // If distance moved was smaller than threshold, select clicked waypoint
        if (Input.GetMouseButtonUp(0) && (Vector3.Distance(Input.mousePosition, startInputPos) < 2f))
            SelectWaypoint();
    }

    private void SelectWaypoint()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = LayerMask.GetMask("Waypoint Triggers");

        // If any waypoints are tapped
        if (Physics.Raycast(ray, out hit, 1000f, layerMask))
        {
            // Set target waypoint as tapped waypoint
            targetWaypoint = hit.transform.gameObject;

            // If target is same as current waypoint, exit method
            if (targetWaypoint == currentWaypoint) return;

            // Play selection SFX
            playerAudioManager.WaypointSelectionSFX(targetWaypoint.transform);

            // Find array position of current waypoint
            var currentIndex = Array.FindIndex(waypoints, item => item.transform.name.Equals(currentWaypoint.name));

            // Find array position of target waypoint
            var targetIndex = Array.FindIndex(waypoints, item => item.transform.name.Equals(targetWaypoint.name));

            // Find direction between target and current waypoint
            arrayDirection = Math.Sign(targetIndex - currentIndex);

            // If level is connected
            if (plankManager.isLevelConnected)
            {
                // Find shortest distance between current and target waypoints
                arrayDirection = ShortestDirection(currentIndex, targetIndex);

                // If current waypoint is last waypoint and direction is forwards in array, next waypoint is first waypoint
                if (currentWaypoint == lastWaypoint && arrayDirection == 1) nextWaypoint = firstWaypoint;

                // If current waypoint is first waypoint and direction is backwards in array, next waypoint is last waypoint
                if (currentWaypoint == firstWaypoint && arrayDirection == -1) nextWaypoint = lastWaypoint;
            }

            // If level is not connected
            else
            {
                // Set next waypoint as next waypoint in array using direction
                nextWaypoint = waypoints[currentIndex + arrayDirection];
            }

            // Play waypoint selection VFX
            inputVFXManager.WaypointSelectionVFX(targetWaypoint.transform);

            // Start transitioning
            StartCoroutine(TransitionWaypoints(arrayDirection));
        }
    }

    // Finds shortest direction between two waypoints
    // Only used if level is fully connected
    private int ShortestDirection(int currentIndex, int targetIndex)
    {
        // Set mean distance as half of array length
        var meanDistance = waypoints.Length / 2;

        // If distance between current index and target is over mean distance, flip direction
        if (Math.Abs(currentIndex - targetIndex) > meanDistance) return Math.Sign(targetIndex - currentIndex) * -1;

        // Otherwise, business as usual
        return Math.Sign(targetIndex - currentIndex);
    }

    IEnumerator TransitionWaypoints(int arrayDirection)
    {
        // If no next waypoint is given, exit coroutine
        if (nextWaypoint == null) yield break;

        playerManager.isMoving = true;

        // If level is connected
        if (plankManager.isLevelConnected)
        {
            // If current waypoint is last waypoint and direction is forward in array, next waypoint is first waypoint
            if (currentWaypoint == lastWaypoint && arrayDirection == 1)
            {
                nextWaypoint = firstWaypoint;

                // If level is connected successfully, flip gravity
                if (plankManager.hasReachedGoal)
                    playerManager.isUsingInvertedGravity = !playerManager.isUsingInvertedGravity;
            }

            // If current waypoint is first waypoint and direction is forward in array, next waypoint is last waypoint
            if (currentWaypoint == firstWaypoint && arrayDirection == -1)
            {
                nextWaypoint = lastWaypoint;

                // If level is connected successfully, flip gravity
                if (plankManager.hasReachedGoal)
                    playerManager.isUsingInvertedGravity = !playerManager.isUsingInvertedGravity;
            }
        }

        // If current waypoint is flagged as transitional and next waypoint hasn't been set
        if (currentWaypoint.GetComponent<WaypointMarker>().isTransitional == true && currentWaypoint == nextWaypoint)
        {
            // Set next waypoint as next waypoint in array using previous array direction
            var currentIndex = Array.FindIndex(waypoints, item => item.transform.name.Equals(currentWaypoint.name));
            nextWaypoint = waypoints[currentIndex + arrayDirection];
        }

        // Set target position as next waypoint's position with Player's Y position
        Vector3 targetPosition = nextWaypoint.transform.position + transform.up * .05f;

        // If next waypoint is aligned with current waypoint, rotate Player towards target position
        if (V3Equal(transform.up, nextWaypoint.transform.up * playerManager.gravityDirection))
        {
            transform.LookAt(targetPosition, nextWaypoint.transform.up * playerManager.gravityDirection);
        }

        // Else if next waypoint is not aligned with current waypoint, teleport Player to next waypoint
        else
        {
            yield return new WaitForSeconds(.12f);
            transform.position = nextWaypoint.transform.position;
            transform.up = nextWaypoint.transform.up * playerManager.gravityDirection;
        }

        // Set current position as Player's position
        var currentPosition = transform.position;

        distance = 100f;

        // While Player has not reached next waypoint's postion
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

        // If next waypoint is not target waypoint, flag next waypoint as transitional
        if (nextWaypoint != targetWaypoint) nextWaypoint.GetComponent<WaypointMarker>().isTransitional = true;

        // Otherwise if Player has reached target waypoint, unflag all waypoints
        else foreach (var waypoint in waypoints) waypoint.GetComponent<WaypointMarker>().isTransitional = false;

        // If next waypoint is flagged as transitional
        if (nextWaypoint.GetComponent<WaypointMarker>().isTransitional == true)
        {
            // Set next waypoint as current waypoint
            currentWaypoint = nextWaypoint;

            // Restart coroutine with new current waypoint
            StartCoroutine(TransitionWaypoints(arrayDirection));
        }

        yield return null;
    }

    // Moves Player around level on completion
    private void RunCircles()
    {
        while (!playerManager.isMoving)
        {
            // Set next waypoint as next waypoint in array using previous array direction
            var currentIndex = Array.FindIndex(waypoints, item => item.transform.name.Equals(currentWaypoint.name));

            // If current waypoint is last waypoint, next waypoint is first waypoint
            if (currentWaypoint == lastWaypoint) nextWaypoint = firstWaypoint;

            // Otherwise, next and target waypoint is next waypoint in array
            else nextWaypoint = waypoints[currentIndex + arrayDirection];
            targetWaypoint = nextWaypoint;

            StartCoroutine(TransitionWaypoints(1));
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        // Set current waypoint as last waypoint Player has collided with
        if (collider.gameObject.tag == "Waypoint")
            currentWaypoint = collider.gameObject;
    }

    // Compares rounded Vector3s because MonoBehaviour doesn't do it well
    private bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.9;
    }
}
