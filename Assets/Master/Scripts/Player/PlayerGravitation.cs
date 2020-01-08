using UnityEngine;

public class PlayerGravitation : MonoBehaviour
{
    private Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        // If current Plank has been assigned add force downwards towards current Plank
        if (PlayerManager.instance.currentPlank)
            rigid.AddForce(-PlayerManager.instance.gravity * rigid.mass * PlayerManager.instance.currentPlank.up * PlayerManager.instance.gravityDirection);
    }
}
