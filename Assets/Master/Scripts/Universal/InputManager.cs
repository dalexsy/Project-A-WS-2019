using UnityEngine;

public class InputManager : MonoBehaviour
{
    [HideInInspector] public bool isUsingTouch = true;
    public bool isSwiping = false;

    private void OnEnable()
    {
        if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer) isUsingTouch = true;

        if (Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.WindowsPlayer) isUsingTouch = false;

    }
}
