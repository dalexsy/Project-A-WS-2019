using System.Collections;
using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    [SerializeField] private GameObject[] waypoints;
    private float distance;
    [SerializeField] private GameObject currentWaypoint;
    private GameObject targetWaypoint;
    private GameObject nextWaypoint;
    private GameObject firstWaypoint;
    private GameObject lastWaypoint;
    private GameObject leftWaypoint;
    private GameObject rightWaypoint;
    private int arrayDirection = 1;
    private Vector3 startInputPos;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

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
        // If player has reached goal, move Player around level
        if (PlankManager.instance.hasReachedGoal) RunCircles();

        // If Player is moving, Plank is rotating, or game is paused, accept no input
        if (PlayerAnimationManager.instance.isMoving || PlankRotationManager.instance.isRotating || InputManager.instance.isSwiping || PauseManager.instance.isPaused) return;

        if (!InputManager.instance.isUsingTouch || Application.platform == RuntimePlatform.WebGLPlayer) MouseInput();

        if (InputManager.instance.isUsingTouch && Application.platform != RuntimePlatform.WebGLPlayer) TouchInput();
    }

    private void TouchInput()
    {
        if (InputManager.instance.isDoubleSwiping == true || InputManager.instance.isSwiping == true) return;

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
                    if (Vector3.Distance(touch.position, startInputPos) < 50f) SelectWaypoint();
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
            PlayerAudioManager.instance.WaypointSelectionSFX(targetWaypoint.transform);

            // Find array position of current waypoint
            var currentIndex = Array.FindIndex(waypoints, item => item.transform.name.Equals(currentWaypoint.name));

            // Find array position of target waypoint
            var targetIndex = Array.FindIndex(waypoints, item => item.transform.name.Equals(targetWaypoint.name));

            int previousDirection = arrayDirection;

            // Find direction between target and current waypoint
            arrayDirection = Math.Sign(targetIndex - currentIndex);

            // If Player is moving in a new direction, Player is turning
            if (previousDirection != arrayDirection)
            {
                //PlayerAnimationManager.instance.animator.SetTrigger("isTurning");
                StartCoroutine(TurnPlayer(180, 1));
            }

            // If level is connected
            if (PlankManager.instance.isLevelConnected)
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
            InputVFXManager.instance.WaypointSelectionVFX(targetWaypoint.transform);

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

    private IEnumerator TurnPlayer(int angle, int direction)
    {
        PlayerAnimationManager.instance.isTurning = true;

        // Set target rotation to given angle around Player's y-axis in given direction
        Vector3 targetRotation = new Vector3(0, angle * direction, 0);

        // Set start rotation as Player's current rotation
        Quaternion startRotation = transform.rotation;

        // Set end rotation as start rotation plus target rotation
        Quaternion endRotation = startRotation * Quaternion.Euler(targetRotation);

        // Reset time
        float t = 0f;

        // While running
        while (t < 1f)
        {
            // Increase time by rotation speed
            t += Time.deltaTime * PlayerAnimationManager.instance.rotationSpeed;

            // Rotate towards end rotation using animation curve
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, PlayerAnimationManager.instance.animationCurve.Evaluate(t));

            // Return to top of while loop
            yield return null;
        }

        PlayerAnimationManager.instance.isTurning = false;
        yield return null;
    }

    // Requires direction (1, -1) and pivot (lPivot, rPivot)
    IEnumerator TransitionPlanks(int direction, Transform pivot)
    {
        // Save local variable rotationPivot from active pivot
        // Needed in case Player leaves range of pivot during coroutine and pivot is unassigned (should no longer happen)
        Transform rotationPivot = pivot;
        Vector3 rotationAxis = rotationPivot.transform.right;

        // Variable used to move through animation curve
        float lerpTime = 1f;

        // Reset current lerp time
        float currentLerpTime = 0f;

        // Reset current rotation
        float currentRotation = 0f;

        // While the Plank has not reached max rotation
        while (currentRotation < PlankRotationManager.instance.maxRotation)
        {
            // Increase currentLerpTime per frame
            // Rotation speed adjusts animation curve frame rate
            currentLerpTime += Time.deltaTime * PlankRotationManager.instance.rotationSpeed;

            // Gate maximum lerp time
            if (currentLerpTime > lerpTime) currentLerpTime = lerpTime;

            // Define t as percentage of lerpTime
            // Used to move through frames of animation curve
            float t = currentLerpTime / lerpTime;

            // Increase current rotation by value from animation curve
            currentRotation += PlankRotationManager.instance.animationCurve.Evaluate(t);

            // If current rotation exceeds max rotation, set max angle correction to difference
            // Will only be used for last frame of animation
            float maxAngleCorrection = 0f;

            if (currentRotation > PlankRotationManager.instance.maxRotation)
                maxAngleCorrection = currentRotation - PlankRotationManager.instance.maxRotation;

            // Rotate plank around given pivot in given direction
            transform.RotateAround(rotationPivot.position, rotationAxis * direction, PlayerAnimationManager.instance.animationCurve.Evaluate(t) - maxAngleCorrection);

            // Returns to top of while loop after fixed update
            yield return new WaitForFixedUpdate();
        }

        PlayerAnimationManager.instance.isTransitioningPlanks = false;
    }

    IEnumerator TransitionWaypoints(int arrayDirection)
    {
        // If no next waypoint is given, exit coroutine
        if (nextWaypoint == null) yield break;

        // Pause coroutine until Player is done turning
        while (PlayerAnimationManager.instance.isTurning == true) yield return null;

        PlayerAnimationManager.instance.isMoving = true;

        // If level is connected
        if (PlankManager.instance.isLevelConnected)
        {
            // If current waypoint is last waypoint and direction is forward in array, next waypoint is first waypoint
            if (currentWaypoint == lastWaypoint && arrayDirection == 1)
            {
                nextWaypoint = firstWaypoint;

                // If level is connected successfully, flip gravity
                if (PlankManager.instance.hasReachedGoal)
                    PlayerManager.instance.isUsingInvertedGravity = !PlayerManager.instance.isUsingInvertedGravity;
            }

            // If current waypoint is first waypoint and direction is forward in array, next waypoint is last waypoint
            if (currentWaypoint == firstWaypoint && arrayDirection == -1)
            {
                nextWaypoint = lastWaypoint;

                // If level is connected successfully, flip gravity
                if (PlankManager.instance.hasReachedGoal)
                    PlayerManager.instance.isUsingInvertedGravity = !PlayerManager.instance.isUsingInvertedGravity;
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
        if (V3Equal(transform.up, nextWaypoint.transform.up * PlayerManager.instance.gravityDirection))
        {
            transform.LookAt(targetPosition, nextWaypoint.transform.up * PlayerManager.instance.gravityDirection);
        }

        // Else if next waypoint is not aligned with current waypoint, teleport Player to next waypoint
        else
        {
            //PlayerAnimationManager.instance.isJumping = true;
            //PlayerAnimationManager.instance.isTransitioningPlanks = true;
            //StartCoroutine(TransitionPlanks(1, PlayerManager.instance.activePivot));
            //while (PlayerAnimationManager.instance.isTransitioningPlanks == true) yield return null;
            //PlayerAnimationManager.instance.isJumping = false;

            yield return new WaitForSeconds(.2f);

            transform.position = nextWaypoint.transform.position;
            transform.up = nextWaypoint.transform.up * PlayerManager.instance.gravityDirection;

            // If Player is moving backwards, rotate Player backwards
            if (arrayDirection == -1)
            {
                Quaternion rotationOffset = Quaternion.Euler(0, 180, 0);
                transform.rotation = nextWaypoint.transform.rotation * rotationOffset;
            }

            // If gravity is flipped, rotate Player upsidedown
            else if (PlayerManager.instance.gravityDirection == -1)
            {
                Quaternion rotationOffset = Quaternion.Euler(0, 0, 180);
                transform.rotation = nextWaypoint.transform.rotation * rotationOffset;
            }

            // Otherwise, Player takes rotation of next waypoint
            else transform.rotation = nextWaypoint.transform.rotation;
        }
        // Set current position as Player's position
        var currentPosition = transform.position;

        distance = 100f;

        // While Player has not reached next waypoint's postion
        while (Vector3.Distance(transform.position, nextWaypoint.transform.position) < distance &&
           Vector3.Distance(transform.position, nextWaypoint.transform.position) >= .001f)
        {
            PlayerAnimationManager.instance.isWalking = true;

            // Take distance from Player's position to next waypoint's position
            distance = Vector3.Distance(transform.position, nextWaypoint.transform.position);

            // Pause movement while Player is rotating
            while (PauseManager.instance.isPaused) yield return null;

            // Set rate as move speed over time
            float rate = PlayerManager.instance.moveSpeed * Time.deltaTime;

            // Translate Player forward
            transform.Translate(0, 0, rate);

            // Return to top of while loop
            yield return null;
        }

        PlayerAnimationManager.instance.isMoving = false;
        PlayerAnimationManager.instance.isWalking = false;

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
        while (!PlayerAnimationManager.instance.isMoving)
        {
            // Set next waypoint as next waypoint in array using previous array direction
            var currentIndex = Array.FindIndex(waypoints, item => item.transform.name.Equals(currentWaypoint.name));

            // If current waypoint is last waypoint, next waypoint is first waypoint
            if (currentWaypoint == lastWaypoint) nextWaypoint = firstWaypoint;

            // Otherwise, next and target waypoint is next waypoint in array
            else nextWaypoint = waypoints[currentIndex + 1];
            targetWaypoint = nextWaypoint;

            StartCoroutine(TransitionWaypoints(1));
        }
    }

    public void RotatePlayer()
    {
        transform.LookAt(PlayerManager.instance.activePivot, currentWaypoint.transform.up * PlayerManager.instance.gravityDirection);
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
