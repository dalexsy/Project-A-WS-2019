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
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        // Single swipe
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
