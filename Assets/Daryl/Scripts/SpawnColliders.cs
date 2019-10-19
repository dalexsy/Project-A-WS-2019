using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnColliders : MonoBehaviour
{
    private float xOffset = 0f;
    private float yOffset = 0f;
    private float zOffset = 0f;

    [SerializeField] string topColliderName = "Plank Collider T";
    [SerializeField] string bottomColliderName = "Plank Collider B";

    private void Start()
    {
        float x = GetComponent<Renderer>().bounds.size.x;
        float y = GetComponent<Renderer>().bounds.size.y;
        float z = GetComponent<Renderer>().bounds.size.z;


        BoxCollider topBoxCollider = gameObject.AddComponent<BoxCollider>();
        topBoxCollider.isTrigger = true;
        topBoxCollider.size = new Vector3(x + xOffset, y + yOffset, z + zOffset);
        topBoxCollider.center = new Vector3(0, .5f, 0);
        topBoxCollider.name = topColliderName;
    }
}
