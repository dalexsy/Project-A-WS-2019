using UnityEngine;

public class VectorComparison : MonoBehaviour
{
    [SerializeField] float tolerance = 0.9f;

    /// <summary>
    /// Says if rounded Vector2s are equal or not.
    /// </summary>
    /// <param name="a">First Vector2 to be compared.</param>
    /// <param name="b">Second Vector2 to be compared.</param>
    /// <returns></returns>
    public bool V2Equal(Vector2 a, Vector2 b)
    {
        return Vector2.SqrMagnitude(a - b) < tolerance;
    }

    /// <summary>
    /// Says if rounded Vector3s are equal or not.
    /// </summary>
    /// <param name="a">First Vector3 to be compared.</param>
    /// <param name="b">Second Vector3 to be compared.</param>
    /// <returns></returns>
    public bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < tolerance;
    }
}
