﻿using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool isUsingTouch = true;
    public bool isSwiping = false;
    public bool isDoubleSwiping = false;
    public readonly float inputBuffer = .3f;
    public static InputManager instance;
    public string debugMessage;

    private float timer = 0f;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void OnEnable()
    {
        if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer) isUsingTouch = true;

        if (Application.platform == RuntimePlatform.WindowsPlayer) isUsingTouch = false;

        debugMessage = GameObject.Find("UI").GetComponentInChildren<UIDebugLog>().debugMessage; // Set message with debugMessage
    }

    private void Update()
    {
        if (isUsingTouch) DetectSwipe();
    }

    /// <summary>
    /// Resets swiping and double swiping bools after a cooldown.
    /// </summary>
    /// <param name="swipeCount">Number of touches</param>
    public IEnumerator ResetBool(int swipeCount)
    {
        yield return new WaitForSeconds(.5f);
        if (swipeCount == 1) isSwiping = false;
        if (swipeCount == 2) isDoubleSwiping = false;
        else yield break;
    }

    /// <summary>
    /// Detects swipes based on touch duration.
    /// </summary>
    private void DetectSwipe()
    {
        // Double swipe
        // If swipe lasts longer than threshold, swipe is valid
        if (Input.touchCount == 2)
        {
            Touch touch = Input.GetTouch(1);

            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                timer += Time.deltaTime;
                if (timer > .1f) isDoubleSwiping = true;
                else isDoubleSwiping = false;
            }

            // If touch has ended, reset swiping bool after cooldown
            // Prevents single swipes from being falsely executed
            if (touch.phase == TouchPhase.Ended)
            {
                StartCoroutine(ResetBool(2));
                timer = 0f;
            }
        }
    }
}
