using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraRepositionEditmode : MonoBehaviour
{
    private Vector3 averagePlankPosition;
    private GameObject[] planks;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        planks = GameObject.FindGameObjectsWithTag("Plank");

        // Create new list of Plank positions
        List<Vector3> plankPositions = new List<Vector3>();

        // Add each Plank's position to plankPositions
        foreach (var plank in planks)
            plankPositions.Add(plank.transform.position);

        // Return average position of all Planks
        averagePlankPosition = GetMeanVector(plankPositions);

        transform.position = averagePlankPosition;
    }

    // Returns mean position of list of vectors
    private Vector3 GetMeanVector(List<Vector3> positions)
    {
        // If list is empty, return (0,0,0)
        if (positions.Count == 0) return Vector3.zero;

        // Reset mean vector
        Vector3 meanVector = Vector3.zero;

        // Add each position to mean vector
        foreach (Vector3 pos in positions)
            meanVector += pos;

        // Return mean vector divided by number of positions
        return (meanVector / positions.Count);
    }

}
