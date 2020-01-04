using UnityEngine;


public class UniversalVFXManager : MonoBehaviour
{
    public void PlayVFX(Transform vfxSource, GameObject particlePrefab)
    {
        GameObject particle = Instantiate(particlePrefab,
                                          vfxSource.position,
                                          vfxSource.transform.rotation * particlePrefab.transform.rotation);

        particle.transform.Translate(0, 0.06f, 0);

        Destroy(particle, particlePrefab.GetComponent<ParticleSystem>().main.duration);
    }
}
