using UnityEngine;

public class PlayerGravitation : MonoBehaviour
{
    private PlayerManager playerManager;
    private Rigidbody rigid;

    private void Start()
    {
        playerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        rigid = GetComponent<Rigidbody>();
        rigid.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        // If current Plank has been assigned add force downwards towards current Plank
        if (playerManager.currentPlank)
            rigid.AddForce(-playerManager.gravity * rigid.mass * playerManager.currentPlank.up * playerManager.gravityDirection);
    }
}
