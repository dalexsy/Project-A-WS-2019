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
        if (Input.touchCount == 2)
            Invoke("SetDoubleSwipeBool", .2f);

        /*
                if (Input.touchCount == 1)
                    Invoke("SetSingleSwipeBool", .2f);
                    */

        if (Input.touchCount == 0)
            Invoke("ResetBools", .5f);

        if (Input.GetMouseButtonUp(0)) isSwiping = false;
        if (Input.GetMouseButtonUp(1)) isDoubleSwiping = false;

        if (Input.GetMouseButton(0)) isSwiping = true;
        if (Input.GetMouseButton(1)) isDoubleSwiping = true;
    }

    private void SetDoubleSwipeBool()
    {
        isDoubleSwiping = true;
    }

    private void SetSingleSwipeBool()
    {
        isSwiping = true;
    }

    private void ResetBools()
    {
        isDoubleSwiping = false;
        isSwiping = false;
    }
}
