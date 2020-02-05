﻿using UnityEngine;

public class InputVFXManager : MonoBehaviour
{
    public GameObject waypointSelectPrefab;

    public static InputVFXManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public void WaypointSelectionVFX(Transform targetWaypoint)
    {
        UniversalVFXManager.instance.PlayVFX(targetWaypoint, waypointSelectPrefab, new Vector3(0, .06f, 0));
    }
}
