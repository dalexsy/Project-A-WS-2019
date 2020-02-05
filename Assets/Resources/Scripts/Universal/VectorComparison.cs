using UnityEngine;

public class VectorComparison : MonoBehaviour
{
    [SerializeField] float tolerance = 0.9f;

    public bool V2Equal(Vector2 a, Vector2 b)
    {
        return Vector2.SqrMagnitude(a - b) < tolerance;
    }

    public bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < tolerance;
    }
}
