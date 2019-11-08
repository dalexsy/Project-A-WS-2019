using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{
    [SerializeField] bool shouldRotate = true;
    [SerializeField] float distance = 10.0f;
    [SerializeField] float height = 5.0f;
    [SerializeField] float heightDamping = 2.0f;
    [SerializeField] float rotationDamping = 3.0f;
    [SerializeField] Transform target = null;

    private float wantedRotationAngle;
    private float wantedHeight;
    private float currentRotationAngle;
    private float currentHeight;

    private Quaternion currentRotation;

    void LateUpdate()
    {
        if (!target) return;

        if (target)
        {
            // Set wanted rotation angle as target's y rotation
            wantedRotationAngle = target.eulerAngles.y + 45;

            // Set wanted height as target's y position plus height modifier
            wantedHeight = target.position.y + height;

            // Set current rotation angle as camera's current y rotation
            currentRotationAngle = this.transform.eulerAngles.y;

            // Set current height as camera's current y position
            currentHeight = transform.position.y;

            // Damp camera's rotation around its y-axis
            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

            // Damp the height from camera's current height to wanted height
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

            // Convert the angle into a rotation
            currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            // Set camera's position to target's position
            this.transform.position = target.position;

            // Move camera's position based on distance from object variable
            this.transform.position -= currentRotation * Vector3.forward * distance;

            // Set camera's y position based on damped height
            this.transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

            // Redefines camera's position based on vertical offset from target
            //var offsetFromTarget = target.position + target.forward;

            // If the camera should rotate
            if (shouldRotate)

                // Look at target with vertical offset
                transform.LookAt(target);
        }
    }
}
