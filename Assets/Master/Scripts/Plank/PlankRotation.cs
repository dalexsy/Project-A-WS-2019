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
    private CollisionDetection collisionDetection;
    private InputManager inputManager;
    private PlankConnection plankConnection;
    private PlankRotationManager plankRotationManager;
    private PlayerManager playerManager;

    private Vector2 startPos = Vector2.zero;
    private Vector2 inputDirection = Vector2.zero;

    private void Start()
    {
        activePivotFX = GetComponent<ActivePivotFX>();
        collisionDetection = GetComponent<CollisionDetection>();
        inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();
        plankConnection = GetComponent<PlankConnection>();
        plankRotationManager = GameObject.Find("Plank Manager").GetComponent<PlankRotationManager>();
        playerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
    }

    private void Update()
    {
        // If Plank is not colliding with Player, accept rotation input
        if (collisionDetection.isCollidingWithTarget == false) RotationInput();
    }

    private void RotationInput()
    {
        // If no pivots are given, accept no input
        if (!activePivot) return;

        // If Plank can rotate clockwise
        if (canRotateClockwiseR && activePivot.name.Equals("Pivot R") ||
            canRotateClockwiseL && activePivot.name.Equals("Pivot L"))
        {
            if (!inputManager.isUsingTouch && MouseInput() == 1) StartRotation(1);
            if (inputManager.isUsingTouch && TouchInput() == 1) StartRotation(1);
        }

        //  If Plank can rotate counterclockwise
        if (canRotateCounterclockwiseR && activePivot.name.Equals("Pivot R") ||
            canRotateCounterclockwiseL && activePivot.name.Equals("Pivot L"))
        {
            if (!inputManager.isUsingTouch && MouseInput() == -1) StartRotation(-1);
            if (inputManager.isUsingTouch && TouchInput() == -1) StartRotation(-1);
        }
    }

    // Returns direction of input
    private int TouchInput()
    {
        if (inputManager.isDoubleSwiping == true) return 0;

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;

                case TouchPhase.Moved:

                    // Set input buffer to prevent input oversensitivity
                    float inputBuffer = Screen.height * .1f * Mathf.Sign(inputDirection.y);

                    if (Mathf.Abs(touch.position.y - startPos.y) > inputBuffer) inputManager.isSwiping = true;
                    break;

                case TouchPhase.Ended:

                    inputDirection = touch.position - startPos;
                    
                    inputManager.isSwiping = false;

                    // Set input buffer to prevent input oversensitivity
                    inputBuffer = Screen.height * .1f * Mathf.Sign(inputDirection.y);

                    if (inputDirection.y > inputBuffer && inputDirection.y != 0) return 1;
                    if (inputDirection.y < inputBuffer && inputDirection.y != 0) return -1;

                    break;
            }
        }

        // If no valid input is given, return zero
        return 0;
    }

    // Returns direction of input
    private int MouseInput()
    {
        if (Input.GetMouseButton(0))
        {
            float moveY = Input.GetAxis("Mouse Y");

            // Set input buffer to prevent input oversensitivity
            float inputBuffer = Screen.height * .1f * Mathf.Sign(moveY);

            // If input is over input buffer, return direction of input
            if (moveY < inputBuffer && moveY != 0) return 1;
            if (moveY > inputBuffer && moveY != 0) return -1;
        }

        // If no valid input is given, return zero
        return 0;
    }

    // Starts Plank rotation
    // Uses direction from MouseInput/TouchInput
    private void StartRotation(int direction)
    {
        if (direction == 0) return;
        if (plankRotationManager.isRotating || playerManager.isMoving) return;

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
    // Requires direction and pivot (lPivot, rPivot)
    IEnumerator RotatePlank(int direction, Transform pivot)
    {
        // Save local variable rotationPivot from active pivot
        // Needed in case Player leaves range of pivot during coroutine and pivot is unassigned (should no longer happen)
        Transform rotationPivot = pivot;
        Vector3 rotationAxis = rotationPivot.transform.right;

        if (activePivotFX.pulse) activePivotFX.pulse.GetComponent<ParticleSystem>().Stop();

        // Variable used to move through animation curve
        float lerpTime = 1f;

        // Reset current lerp time
        float currentLerpTime = 0f;

        // Reset current rotation
        float currentRotation = 0f;

        //if (activePivotFX.pulse) activePivotFX.DespawnPulse();

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

            float maxAngleCorrection = 0f;

            // If current rotation exceeds max rotation, set max angle correction to difference
            // Will only be used for last frame of animation
            if (currentRotation > plankRotationManager.maxRotation)
                maxAngleCorrection = currentRotation - plankRotationManager.maxRotation;

            // Rotate plank around given pivot in given direction
            transform.RotateAround(rotationPivot.position, rotationAxis * direction, plankRotationManager.animationCurve.Evaluate(t) - maxAngleCorrection);

            // Returns to top of while loop
            yield return null;
        }

        // Disconnect all connected planks
        plankConnection.DisconnectPlanks(transform);

        // If active pulse FX is paused, restart
        if (activePivotFX.pulse) activePivotFX.pulse.GetComponent<ParticleSystem>().Play();

        // Sets isRotating to false after Plank has reached max rotation
        plankRotationManager.isRotating = false;
    }
}
