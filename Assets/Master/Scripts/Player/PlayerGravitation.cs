using UnityEngine;

public class PlayerGravitation : MonoBehaviour
{
    private PlayerManager playerManager;
    private PlayerPlankDetection playerPlankDetection;
    private Rigidbody rigid;

    private void Start()
    {
        playerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        playerPlankDetection = GetComponent<PlayerPlankDetection>();
        rigid = GetComponent<Rigidbody>();
        rigid.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        // If current Plank has been assigned add force downwards towards current Plank
        if (playerPlankDetection.currentPlank)
            rigid.AddForce(-playerManager.gravity * rigid.mass * playerPlankDetection.currentPlank.up * playerManager.gravityDirection);
    }
}
