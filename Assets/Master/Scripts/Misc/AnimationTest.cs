using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    public AnimationCurve animationCurve;

    float lerpTime = 4f;
    float currentLerpTime;
    float startRotation;

    float moveDistance = 8f;

    Vector3 startPos;
    Vector3 endPos;

    [SerializeField] Transform rotationPivot = null;

    PlankManager plankManager;

    private void Start()
    {
        startPos = transform.position;
        endPos = transform.position + transform.up * moveDistance;

        StartCoroutine(RotateOnce());
    }

    private void Update()
    {
        //MoveUpward();
        //Rotate();
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
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }


        float t = currentLerpTime / lerpTime;

        float endRotation = Math.Abs(Mathf.RoundToInt(transform.rotation.eulerAngles.x - startRotation));

        Vector3 targetRot = new Vector3(startRotation + animationCurve.Evaluate(t), 0, 0);

        transform.eulerAngles = targetRot;

        //transform.RotateAround(rotationPivot.position, rotationPivot.transform.right, animationCurve.Evaluate(t));
    }

    IEnumerator RotateOnce()
    {
        startRotation = transform.rotation.eulerAngles.x;
        float currentRotation = 0;

        while (currentRotation < 90f)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            float t = currentLerpTime / lerpTime;

            transform.RotateAround(rotationPivot.position, rotationPivot.transform.right, animationCurve.Evaluate(t));
            currentRotation = Math.Abs(Mathf.RoundToInt(transform.rotation.eulerAngles.x - startRotation));
            if (currentRotation > 90f)
            {
                transform.eulerAngles = new Vector3(startRotation + 90, transform.eulerAngles.y, transform.eulerAngles.z);
            }
            yield return null;
        }
    }
}
