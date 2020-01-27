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

    // Detects valid touch input
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

    // Detects valid mouse input
    private void MouseInput()
    {
        // Set starting mouse position on mouse down
        if (Input.GetMouseButtonDown(0)) startInputPos = Input.mousePosition;

        // If distance moved was smaller than threshold, select clicked waypoint
        if (Input.GetMouseButtonUp(0) && (Vector3.Distance(Input.mousePosition, startInputPos) < 2f))
        {
            SelectWaypoint();
        }
    }

    // Selects waypoint based on player input
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

            // Find direction between target and current waypoint
            arrayDirection = Math.Sign(targetIndex - currentIndex);

            // If level is connected
            if (PlankManager.instance.isLevelConnected)
            {
                // Find shortest distance between current and target waypoints
                arrayDirection = ShortestDirection(currentIndex, targetIndex);

                // If current waypoint is last waypoint and direction is forwards in array, next waypoint is first waypoint
                if (currentWaypoint == lastWaypoint && arrayDirection == 1) nextWaypoint = firstWaypoint;

                // If current waypoint is first waypoint and direction is backwards in array, next waypoint is last waypoint
                else if (currentWaypoint == firstWaypoint && arrayDirection == -1) nextWaypoint = lastWaypoint;

                // Otherwise, set next waypoint as next waypoint in array using direction
                else nextWaypoint = waypoints[currentIndex + arrayDirection];
            }

            // Otherwise, level is not connected
            else
            {
                // Set next waypoint as next waypoint in array using direction
                nextWaypoint = waypoints[currentIndex + arrayDirection];
            }

            // Play waypoint selection VFX
            InputVFXManager.instance.WaypointSelectionVFX(targetWaypoint.transform);

            // Start transitioning between waypoints
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

    // Moves Player between waypoints
    // Requires direction to move through waypoint array
    IEnumerator TransitionWaypoints(int arrayDirection)
    {
        // If no next waypoint is given, exit coroutine
        if (nextWaypoint == null) yield break;

        // Flag Player as moving
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

        // Else if next waypoint is not aligned with current waypoint, rotate Player towards next waypoint
        else
        {
            // Flag Player as transitioning planks
            PlayerAnimationManager.instance.isTransitioningPlanks = true;

            // Rotate Player towards pivot
            RotatePlayer();

            // Set jump angle and execute jump
            SetJumpAngle();

            // While Player is transitioning planks, pause coroutine
            while (PlayerAnimationManager.instance.isTransitioningPlanks == true) yield return null;

            // Position Player at next waypoint's position
            transform.position = nextWaypoint.transform.position;

            // Set Player's up as next waypoint's up using gravity direction
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
            else
            {
                transform.rotation = nextWaypoint.transform.rotation;
            }
        }

        // Set current position as Player's position
        var currentPosition = transform.position;

        // Reset distance
        distance = 100f;

        // While Player has not reached next waypoint's postion
        while (Vector3.Distance(transform.position, nextWaypoint.transform.position) < distance &&
           Vector3.Distance(transform.position, nextWaypoint.transform.position) >= .001f)
        {
            // Flag Player as walking
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

        // Unflag Player to exit animation
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

    // Jumps Player from plank to plank
    // Requires pivot as rotation reference and angle to rotate
    IEnumerator JumpPlanks(Transform pivot, int angle)
    {
        // Set rotation position as distance from Player to given pivot
        Vector3 rotationPosition = transform.position - pivot.position;

        // If Player is rotating using right pivot, inverse direction
        int direction = 1;
        if (pivot.name.Equals(PlankManager.instance.rightPivotName)) direction *= -1;

        // Reset current rotation
        int currentRotation = 0;

        // Set rotation rate
        int rotationRate = 6 * direction;

        // If Player is moving up a plank, inverse model rotation direction
        int flip = 1;
        if (angle == 90) flip = -1;

        // Disable box collider to prevent displacement from plank
        GetComponent<BoxCollider>().enabled = false;

        // While Player has not reached target angle
        while (Math.Abs(currentRotation) < angle)
        {
            // Set rotation position as angle from 
            rotationPosition = Quaternion.Euler(pivot.right.x * rotationRate, pivot.right.y * rotationRate, pivot.right.z * rotationRate) * rotationPosition;

            // Set playerRotation as current rotation
            Quaternion playerRotation = transform.rotation;

            // Adjust player rotation around X-axis using rotation rate
            playerRotation *= Quaternion.Euler((90f / angle * flip) * rotationRate * direction, 0, 0);

            // Set Player rotation as playerRotation
            transform.rotation = playerRotation;

            // Increase current rotation by rotation rate
            currentRotation += rotationRate;

            // Move Player to pivot's position plus rotationPosition
            transform.position = pivot.position + rotationPosition;

            // Re-enable box collider if jump is over 80% complete
            if (Math.Abs(currentRotation) > angle * .8f) GetComponent<BoxCollider>().enabled = true;

            yield return new WaitForFixedUpdate();
        }

        transform.position = currentWaypoint.transform.position;
        PlayerAnimationManager.instance.isTransitioningPlanks = false;
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

    // Rotates Player towards active pivot
    public void RotatePlayer()
    {
        if (PlankManager.instance.hasReachedGoal && currentWaypoint == lastWaypoint) 
        
        { 
            var pivot = PlayerManager.instance.activePivot.parent.Find("Goal");
            transform.LookAt(pivot, currentWaypoint.transform.up * PlayerManager.instance.gravityDirection);
        }

        else transform.LookAt(PlayerManager.instance.activePivot, currentWaypoint.transform.up * PlayerManager.instance.gravityDirection);
    }

    // Sets angle at which Player should jump
    private void SetJumpAngle()
    {
        int angle = 270;
        Transform pivot = PlayerManager.instance.activePivot;
        PivotAssignment pivotAssignment = pivot.GetComponent<PivotAssignment>();
        PlankRotation parentRotation = pivot.parent.GetComponent<PlankRotation>();

        if (!PlayerManager.instance.isUsingInvertedGravity)
        {
            // If top collider of current plank is colliding with another top collider, Player should rotate 90 degrees 
            if ((pivot.name.Equals(PlankManager.instance.rightPivotName) && pivotAssignment.hasMixedCollisionTop)
             || (pivot.name.Equals(PlankManager.instance.leftPivotName) && !parentRotation.canRotateCounterclockwiseL)
             || (pivot.name.Equals(PlankManager.instance.rightPivotName) && !parentRotation.canRotateClockwiseR))
                angle = 90;
        }

        else
        {
            // If top collider of current plank is colliding with another top collider, Player should rotate 90 degrees 
            if ((pivot.name.Equals(PlankManager.instance.leftPivotName) && pivotAssignment.hasMixedCollisionTop)
             || (pivot.name.Equals(PlankManager.instance.rightPivotName) && !parentRotation.canRotateCounterclockwiseR)
             || (pivot.name.Equals(PlankManager.instance.leftPivotName) && !parentRotation.canRotateClockwiseL))
                angle = 90;
        }

        if (PlankManager.instance.hasReachedGoal && currentWaypoint == lastWaypoint) pivot = pivot.parent.Find("Goal");

        // Start jumping planks with given pivot and angle
        StartCoroutine(JumpPlanks(pivot, angle));
    }

    private void OnTriggerStay(Collider collider)
    {
        // Set current waypoint as last waypoint Player has collided with
        if (collider.gameObject.tag == "Waypoint")
        {
            currentWaypoint = collider.gameObject;

            // Set current plank as parent of current waypoint
            PlayerManager.instance.currentPlank = currentWaypoint.transform.parent;
        }
    }

    // Compares rounded Vector3s
    private bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.9;
    }
}
