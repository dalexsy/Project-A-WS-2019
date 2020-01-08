using UnityEngine;

public class UniversalVFXManager : MonoBehaviour
{
    public static UniversalVFXManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public void PlayVFX(Transform vfxSource, GameObject particlePrefab, Vector3 offset, Quaternion rotation = default(Quaternion))
    {
        var existingParticle = vfxSource.transform.Find(particlePrefab.name);

        // If no particle exists, create a new particle system
        if (!existingParticle)
        {
            // Create new particle system as child of VFX source
            GameObject particle = Instantiate(particlePrefab,
                                                    vfxSource.position,
                                                    vfxSource.transform.rotation * particlePrefab.transform.rotation * rotation);

            particle.transform.name = particlePrefab.name;
            particle.transform.parent = vfxSource;
            particle.transform.Translate(offset);
        }

        // Else restart existing particle system
        else
        {
            var pS = existingParticle.GetComponent<ParticleSystem>();

            if (!pS.isPlaying)
            {
                pS.time = 0;
                pS.Play();
            }
        }
    }

    public void StopVFX(Transform vfxSource, GameObject particlePrefab)
    {
        var particle = vfxSource.transform.Find(particlePrefab.name);

        if (!particle) return;

        var pS = particle.GetComponent<ParticleSystem>();
        pS.Stop();
    }
}
