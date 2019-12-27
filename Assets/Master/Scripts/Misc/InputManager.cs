using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool isUsingTouch = true;
    public bool isSwiping = false;
    public bool isDoubleSwiping = false;
    public readonly float inputBuffer = .3f;

    public UIDebugLog debugLog;

    private void OnEnable()
    {
        if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer) isUsingTouch = true;

        if (Application.platform == RuntimePlatform.WindowsPlayer) isUsingTouch = false;

        debugLog = GameObject.Find("UI").GetComponentInChildren<UIDebugLog>();
    }

    private void Update()
    {
        if (isUsingTouch) DetectSwipe();
        else DetectMouse();
    }

    private void DetectMouse()
    {
        var startTime = Time.time;

        // Left click swipe
        // If swipe lasts longer than half a second, swipe is valid
        if (Input.GetMouseButtonDown(0)) startTime = Time.time;

        if (Input.GetMouseButton(0))
        {
            var timeElapsed = Time.time - startTime;
            if (timeElapsed > .5) isSwiping = true;
            else isSwiping = false;
        }

        if (Input.GetMouseButtonUp(0)) isSwiping = false;

        // Right click swipe
        // If swipe lasts longer than half a second, swipe is valid
        if (Input.GetMouseButtonDown(1)) startTime = Time.time;

        if (Input.GetMouseButton(1))
        {
            var timeElapsed = Time.time - startTime;
            if (timeElapsed > .5) isDoubleSwiping = true;
            else isDoubleSwiping = false;
        }

        if (Input.GetMouseButtonUp(1)) isDoubleSwiping = false;
    }

    private void DetectSwipe()
    {
        // Single swipe
        // If swipe lasts longer than half a second, swipe is valid
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            var startTime = Time.time;

            if (touch.phase == TouchPhase.Began) startTime = Time.time;

            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                var timeElapsed = Time.time - startTime;
                if (timeElapsed > .5) isSwiping = true;
                else isSwiping = false;
            }

            if (touch.phase == TouchPhase.Ended) isSwiping = false;
        }

        // Double swipe
        // If swipe lasts longer than half a second, swipe is valid
        if (Input.touchCount == 2)
        {
            Touch touch = Input.GetTouch(1);
            var startTime = Time.time;

            if (touch.phase == TouchPhase.Began) startTime = Time.time;

            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                var timeElapsed = Time.time - startTime;
                if (timeElapsed > .5) isDoubleSwiping = true;
                else isDoubleSwiping = false;
            }

            if (touch.phase == TouchPhase.Ended) isDoubleSwiping = false;
        }
    }
}
