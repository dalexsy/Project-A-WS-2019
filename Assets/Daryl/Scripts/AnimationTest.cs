using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    public AnimationCurve animationCurve;

    float lerpTime = 4f;
    float currentLerpTime;

    float moveDistance = 8f;

    Vector3 startPos;
    Vector3 endPos;

    private void Start()
    {
        startPos = transform.position;
        endPos = transform.position + transform.up * moveDistance;
    }

    private void Update()
    {
        MoveUpward();
    }

    private void MoveUpward()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentLerpTime = 0f;
        }

        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        float t = currentLerpTime / lerpTime;

        transform.position = Vector3.Lerp(startPos, endPos, animationCurve.Evaluate(t));
    }

    private void Rotate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentLerpTime = 0f;
        }

        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        float t = currentLerpTime / lerpTime;

        // transform.RotateAround() = Vector3.Lerp(startPos, endPos, animationCurve.Evaluate(t));
    }
}
