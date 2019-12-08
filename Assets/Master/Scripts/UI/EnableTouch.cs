using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableTouch : MonoBehaviour
{
    private InputManager inputManager;

    private void Start()
    {
        inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();

    }
    public void TouchToggle()
    {
        inputManager.isUsingTouch = !inputManager.isUsingTouch;
    }
}
