using UnityEngine;

public class PlayerPlankDetection : MonoBehaviour
{
    public Transform currentPlank;
    public Transform nextPlank;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Plank"))
        {
            if (!currentPlank) currentPlank = collision.transform;
            else nextPlank = collision.transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Plank") &&
           (collision.transform == currentPlank && nextPlank)) currentPlank = nextPlank;
    }
}
