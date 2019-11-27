using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankRotationManager : MonoBehaviour
{
    public AnimationCurve animationCurve;
    public float maxRotation = 90f;
    [Range(0, 1)] public float rotationSpeed = 1f;
    public bool isRotating = false;
    public GameObject rotateParticlePrefab = null;
    public GameObject activePivotParticlePrefab = null;
}
