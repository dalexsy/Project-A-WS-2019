using UnityEngine;

public class UniversalVFXManager : MonoBehaviour
{
    public static UniversalVFXManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    /// <summary>
    /// Creates or plays VFX.
    /// </summary>
    /// <param name="vfxSource">Position to create VFX and parent of created VFX.</param>
    /// <param name="particlePrefab">Particle system prefab used for VFX.</param>
    /// <param name="offset">Distance to offset VFX from VFX source.</param>
    public void PlayVFX(Transform vfxSource, GameObject particlePrefab, Vector3 offset)
    {
        var existingParticle = vfxSource.transform.Find(particlePrefab.name);

        // If no particle exists, create a new particle system
        if (!existingParticle)
        {
            // Create new particle system as child of VFX source
            GameObject particle = Instantiate(particlePrefab,
                                                    vfxSource.position,
                                                    vfxSource.transform.rotation * particlePrefab.transform.rotation);

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

    /// <summary>
    /// Creates or plays VFX with rotation.
    /// </summary>
    /// <param name="vfxSource">Position to create VFX and parent of created VFX.</param>
    /// <param name="particlePrefab">Particle system prefab used for VFX.</param>
    /// <param name="offset">Distance to offset VFX from VFX source.</param>
    /// <param name="rotation">Amount to rotate VFX.</param>
    public void PlayRotatedVFX(Transform vfxSource, GameObject particlePrefab, Vector3 offset, Quaternion rotation = default)
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

    /// <summary>
    /// Stops VFX from playing.
    /// </summary>
    /// <param name="vfxSource">Parent of VFX.</param>
    /// <param name="particlePrefab">Particle system prefab used for VFX.</param>
    /// <param name="shouldFade">Should particle system fade out or not.</param>
    public void StopVFX(Transform vfxSource, GameObject particlePrefab, bool shouldFade)
    {
        Transform particle = vfxSource.transform.Find(particlePrefab.name);

        if (!particle) return;

        ParticleSystem pS = particle.GetComponent<ParticleSystem>();

        if (shouldFade)
        {
            Renderer rend = pS.GetComponent<Renderer>();
            float r = rend.material.color.r;
            float g = rend.material.color.g;
            float b = rend.material.color.b;
            float a = rend.material.color.a;
            float startA = a;

            while (a > 0)
            {
                a -= .2f;
                rend.material.color = new Color(r, g, b, a);
            }

            pS.Stop();
            a = startA;
            rend.material.color = new Color(r, g, b, a);
        }

        else pS.Stop();
    }
}
