using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool isUsingTouch = true;
    public bool isSwiping = false;
    public bool isDoubleSwiping = false;
    public float inputBuffer = .5f;

    private void OnEnable()
    {
        if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer) isUsingTouch = true;

        if (Application.platform == RuntimePlatform.WindowsPlayer) isUsingTouch = false;
    }

    private void Update()
    {
        if (Input.touchCount == 2) isDoubleSwiping = true;
        if (Input.touchCount == 0) isDoubleSwiping = false;
    }

}
