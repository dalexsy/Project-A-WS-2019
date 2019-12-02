using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePivotFX : MonoBehaviour
{
    public GameObject pulse;
    PlankRotationManager plankRotationManager;

    private void Start()
    {
        plankRotationManager = GameObject.Find("Plank Manager").GetComponent<PlankRotationManager>();
    }

    // Creates pulse FX
    public void SpawnPulse(Vector3 position)
    {
        pulse = Instantiate(plankRotationManager.activePivotParticlePrefab, position, Quaternion.identity);
    }

    // Despawn pulse FX
    public void DespawnPulse()
    {
        pulse.GetComponent<ParticleSystem>().Stop();
        Destroy(pulse, 5);
    }
}
