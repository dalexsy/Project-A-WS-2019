using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankRotationManager : MonoBehaviour
{
    public AnimationCurve animationCurve;
    public float maxRotation = 90f;
    [Range(0, 1)] public float rotationSpeed = 1f;
    public GameObject pulseParticlePrefab = null;
}
