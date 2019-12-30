using System.Collections;
using UnityEngine;

public class PlankRotation : MonoBehaviour
{
    public Transform activePivot = null;
    public Transform surrogatePivot = null;

    public bool canRotateClockwiseR = true;
    public bool canRotateCounterclockwiseR = true;
    public bool canRotateClockwiseL = true;
    public bool canRotateCounterclockwiseL = true;
    public bool isConnectedFront = false; // Only used if surrogate pivot is assigned
    public bool isConnectedBack = false;  // Only used if surrogate pivot is assigned

    private ActivePivotFX activePivotFX;
    private CameraRigRotation cameraRigRotation;
    private CollisionDetection collisionDetection;
    private InputManager inputManager;
    private MoveCounter moveCounter;
    private PauseManager pauseManager;
    private PivotOrientationDetection pivotOrientationDetection;
    private PlankAudioManager plankAudioManager;
    private PlankConnection plankConnection;
    private PlankManager plankManager;
    private PlankRotationManager plankRotationManager;
    private PlayerManager playerManager;

    private float inputBuffer = 0f;
    private float inputOffset = 0f;

    private Vector2 startPosMouse = Vector2.zero;
    private Vector2 startPosTouch = Vector2.zero;
    private Vector2 inputDirection = Vector2.zero;

    private void Start()
    {
        activePivotFX = GetComponent<ActivePivotFX>();
        cameraRigRotation = GameObject.Find("Camera Rig").GetComponent<CameraRigRotation>();
        collisionDetection = GetComponent<CollisionDetection>();
        inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();
        moveCounter = GameObject.Find("Game Manager").GetComponent<MoveCounter>();
        pauseManager = GameObject.Find("Game Manager").GetComponent<PauseManager>();
        plankAudioManager = GameObject.Find("SFX Manager").GetComponent<PlankAudioManager>();
        plankConnection = GetComponent<PlankConnection>();
        plankManager = GameObject.Find("Plank Manager").GetComponent<PlankManager>();
        plankRotationManager = GameObject.Find("Plank Manager").GetComponent<PlankRotationManager>();
        playerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
    }

    private void Update()
    {
        // If game is paused, accept no input
        if (pauseManager.isPaused) return;

        // If Plank is not colliding with Player, accept rotation input
        if (collisionDetection.isCollidingWithTarget == false) RotationInput();

        // Else if Plank is colliding with Player, Plank is current Plank
        else playerManager.currentPlank = transform;
    }

    private void RotationInput()
    {
        // If no pivots are given or camera rig is currently rotating, accept no input
        if (!activePivot || cameraRigRotation.isRotating) return;

        // If Player is on last Plank and this Plank is first or vice versa, accept no input
        if ((playerManager.currentPlank == plankManager.lastPlank && transform == plankManager.firstPlank)
         || (playerManager.currentPlank == plankManager.firstPlank && transform == plankManager.lastPlank)) return;

        // If level is over, accept no input
        if (plankManager.hasReachedGoal) return;

        // If Plank can rotate clockwise and using right pivot
        if (canRotateClockwiseR && activePivot.name.Equals("Pivot R"))
        {
            if (!inputManager.isUsingTouch && MouseInput() == 1) StartRotation(1);
            if (inputManager.isUsingTouch && TouchInput() == 1) StartRotation(1);
        }

        // If Plank can rotate clockwise and using left pivot
        if (canRotateClockwiseL && activePivot.name.Equals("Pivot L"))
        {
            if (!inputManager.isUsingTouch && MouseInput() == -1) StartRotation(1);
            if (inputManager.isUsingTouch && TouchInput() == -1) StartRotation(1);
        }

        //  If Plank can rotate counterclockwise and using right pivot
        if (canRotateCounterclockwiseR && activePivot.name.Equals("Pivot R"))
        {
            if (!inputManager.isUsingTouch && MouseInput() == -1) StartRotation(-1);
            if (inputManager.isUsingTouch && TouchInput() == -1) StartRotation(-1);
        }

        //  If Plank can rotate counterclockwise and using left pivot
        if (canRotateCounterclockwiseL && activePivot.name.Equals("Pivot L"))
        {
            if (!inputManager.isUsingTouch && MouseInput() == 1) StartRotation(-1);
            if (inputManager.isUsingTouch && TouchInput() == 1) StartRotation(-1);
        }
    }

    // Returns direction of input
    private int TouchInput()
    {
        // If player is double swiping, input is not valid
        if (inputManager.isDoubleSwiping == true) return 0;

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                // Set start position to touch position and reset offset
                case TouchPhase.Began:
                    startPosTouch = touch.position;
                    inputOffset = 0;
                    break;

                case TouchPhase.Ended:

                    pivotOrientationDetection = activePivot.GetComponent<PivotOrientationDetection>();

                    // If Plank is vertical, look for vertical input
                    if (pivotOrientationDetection.isVertical())
                    {
                        // Calculate offset from start position on X axis in screenspace
                        var currentPosition = touch.position.x;
                        inputOffset = currentPosition - startPosTouch.x;

                        // Set input buffer to prevent input oversensitivity
                        inputBuffer = Screen.width * .1f;

                        // If Plank is flipped vertically, input must be reversed
                        var plankOrientation = 1;
                        if (pivotOrientationDetection.isTopRight()) plankOrientation = -1;

                        // If input is over input buffer, return direction of input
                        var direction = Mathf.Sign(inputOffset);

                        // If input offset is over input buffer, return input direction corrected for Plank orientation
                        if (Mathf.Abs(inputOffset) > inputBuffer && direction == 1) return 1 * plankOrientation;
                        if (Mathf.Abs(inputOffset) > inputBuffer && direction == -1) return -1 * plankOrientation;
                    }

                    // Else if Plank is horizontal, look for horizontal input
                    else
                    {
                        // Calculate offset from start position on Y axis in screenspace
                        var currentPosition = touch.position.y;
                        inputOffset = currentPosition - startPosTouch.y;

                        // Set input buffer to prevent input oversensitivity
                        inputBuffer = Screen.height * .1f;

                        // If Plank is flipped horizontally, input must be reversed
                        var plankOrientation = 1;
                        if (!pivotOrientationDetection.isTopTop()) plankOrientation = -1;

                        // If input is over input buffer, return direction of input
                        var direction = Mathf.Sign(inputOffset);

                        // If input offset is over input buffer, return input direction corrected for Plank orientation
                        if (Mathf.Abs(inputOffset) > inputBuffer && direction == 1) return 1 * plankOrientation;
                        if (Mathf.Abs(inputOffset) > inputBuffer && direction == -1) return -1 * plankOrientation;
                    }
                    break;
            }
        }

        // If no valid input is given, return zero
        return 0;
    }

    // Returns direction of input
    private int MouseInput()
    {
        // On left mouse button down, set start position and reset input offset
        if (Input.GetMouseButtonDown(0))
        {
            startPosMouse = Input.mousePosition;
            inputOffset = 0;
        }

        // On left mouse button up, determine input direction if over input buffer
        if (Input.GetMouseButtonUp(0))
        {
            pivotOrientationDetection = activePivot.GetComponent<PivotOrientationDetection>();

            // If Plank is vertical, look for vertical input
            if (pivotOrientationDetection.isVertical())
            {
                var currentPosition = Input.mousePosition.x;
                inputOffset = currentPosition - startPosMouse.x;

                // Set input buffer to prevent input oversensitivity
                inputBuffer = Screen.width * .1f;

                // If Plank is flipped vertically, input must be reversed
                var plankOrientation = 1;
                if (pivotOrientationDetection.isTopRight()) plankOrientation = -1;

                // If input is over input buffer, return direction of input
                var direction = Mathf.Sign(inputOffset);

                // If input offset is over input buffer, return input direction corrected for plank orientation
                if (Mathf.Abs(inputOffset) > inputBuffer && direction == 1) return 1 * plankOrientation;
                if (Mathf.Abs(inputOffset) > inputBuffer && direction == -1) return -1 * plankOrientation;
            }

            // Else if Plank is horizontal, look for horizontal input
            else
            {
                var currentPosition = Input.mousePosition.y;
                inputOffset = currentPosition - startPosMouse.y;

                // Set input buffer to prevent input oversensitivity
                inputBuffer = Screen.height * .1f;

                // If Plank is flipped horizontally, input must be reversed
                var plankOrientation = 1;
                if (!pivotOrientationDetection.isTopTop()) plankOrientation = -1;

                // If input is over input buffer, return direction of input
                var direction = Mathf.Sign(inputOffset);

                // If input offset is over input buffer, return input direction corrected for plank orientation
                if (Mathf.Abs(inputOffset) > inputBuffer && direction == 1) return 1 * plankOrientation;
                if (Mathf.Abs(inputOffset) > inputBuffer && direction == -1) return -1 * plankOrientation;
            }
        }

        // If no valid input is given, return zero
        return 0;
    }

    // Starts Plank rotation
    // Uses direction from MouseInput/TouchInput
    private void StartRotation(int direction)
    {
        if (direction == 0 || plankRotationManager.isRotating || playerManager.isMoving) return;

        if (isConnectedFront)
        {
            // Set isRotating to true to prevent multiple rotations
            plankRotationManager.isRotating = true;

            // Rotate Plank clockwise
            // Will rotate from surrogate pivot's position
            StartCoroutine(RotatePlank(direction, activePivot));
        }

        else
        {
            // Set isRotating to true to prevent multiple rotations
            plankRotationManager.isRotating = true;

            // Rotate Plank counterclockwise 
            // Will rotate from active pivot's position
            StartCoroutine(RotatePlank(direction * -1, activePivot));
        }
    }

    // Rotates Plank
    // Requires direction (1, -1) and pivot (lPivot, rPivot)
    IEnumerator RotatePlank(int direction, Transform pivot)
    {
        // Save local variable rotationPivot from active pivot
        // Needed in case Player leaves range of pivot during coroutine and pivot is unassigned (should no longer happen)
        Transform rotationPivot = pivot;
        Vector3 rotationAxis = rotationPivot.transform.right;

        if (activePivotFX.pulse) activePivotFX.pulse.GetComponent<ParticleSystem>().Stop();

        plankAudioManager.ActivationSuccessSFX(pivot);

        // Variable used to move through animation curve
        float lerpTime = 1f;

        // Reset current lerp time
        float currentLerpTime = 0f;

        // Reset current rotation
        float currentRotation = 0f;

        // Start coroutine to connect planks using rotation pivot
        plankConnection.ConnectPlanks(rotationPivot);

        // If using a surrogate pivot
        if (surrogatePivot)
        {
            // Assign surrogate pivot as rotationpivot
            rotationPivot = surrogatePivot;

            // Redefines rotationAxis using surrogate pivot
            rotationAxis = rotationPivot.transform.right;
        }

        // Create visual feedback on pivot to be rotated from
        GameObject pulse = Instantiate(plankRotationManager.rotateParticlePrefab, rotationPivot.transform.position, plankRotationManager.rotateParticlePrefab.transform.rotation);

        // Destroy particle system once system has run once
        Destroy(pulse, pulse.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);

        // While the Plank has not reached max rotation
        while (currentRotation < plankRotationManager.maxRotation)
        {
            // Increase currentLerpTime per frame
            // Rotation speed adjusts animation curve frame rate
            currentLerpTime += Time.deltaTime * plankRotationManager.rotationSpeed;

            // Gate maximum lerp time
            if (currentLerpTime > lerpTime) currentLerpTime = lerpTime;

            // Define t as percentage of lerpTime
            // Used to move through frames of animation curve
            float t = currentLerpTime / lerpTime;

            // Increase current rotation by value from animation curve
            currentRotation += plankRotationManager.animationCurve.Evaluate(t);

            // If current rotation exceeds max rotation, set max angle correction to difference
            // Will only be used for last frame of animation
            float maxAngleCorrection = 0f;

            if (currentRotation > plankRotationManager.maxRotation)
                maxAngleCorrection = currentRotation - plankRotationManager.maxRotation;

            // Rotate plank around given pivot in given direction
            transform.RotateAround(rotationPivot.position, rotationAxis * direction, plankRotationManager.animationCurve.Evaluate(t) - maxAngleCorrection);

            // Returns to top of while loop after fixed update
            yield return new WaitForFixedUpdate();
        }

        // Disconnect all connected planks
        plankConnection.DisconnectPlanks(transform);

        // If active pulse FX is paused, restart
        if (activePivotFX.pulse) activePivotFX.pulse.GetComponent<ParticleSystem>().Play();

        // Increase move counter
        moveCounter.moveCount += 1;

        // Sets isRotating to false after Plank has reached max rotation
        plankRotationManager.isRotating = false;
    }
}
