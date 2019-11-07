using System.Collections;
using UnityEngine;

public class DPlayerController : MonoBehaviour
{
    private float moveSpeed = 1f;
    private float rotationSpeed = 100f;

    private void Update()
    {
        float translation = Input.GetAxis("kVertical") * moveSpeed;
        float rotation = Input.GetAxis("kHorizontal") * rotationSpeed;

        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // Move translation along Player's z-axis
        transform.Translate(0, 0, translation);

        // Rotate around Player's y-axis
        transform.Rotate(0, rotation, 0);
    }
}