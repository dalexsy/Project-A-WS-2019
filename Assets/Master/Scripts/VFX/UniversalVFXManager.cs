using UnityEngine;

public class UniversalVFXManager : MonoBehaviour
{
    public void PlayVFX(Transform vfxSource, GameObject particlePrefab)
    {
        GameObject particle = Instantiate(particlePrefab, vfxSource.position, Quaternion.identity);
        Destroy(particle, particlePrefab.GetComponent<ParticleSystem>().main.duration);
    }
}
