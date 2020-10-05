using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingParticle : MonoBehaviour
{

    [Tooltip("Particle prefab to be attached to the charactor in the scene.")] public ParticleSystem walkingParticlePrefab;
    [Tooltip("Minimum walking distance to play the particle.")] public float walkingDistance;

    private Vector2 lastPosition;
    private Vector2 currentPosition;
    private ParticleSystem particleObject;

    private void OnEnable()
    {
        lastPosition = new Vector2(transform.position.x, transform.position.z);
        currentPosition = new Vector2(transform.position.x, transform.position.z);
        if (!particleObject && walkingParticlePrefab)
        {
            particleObject = Instantiate(walkingParticlePrefab, transform);
            particleObject.Stop();
        }
    }

    private void Update()
    {
        if(particleObject)
        {
            currentPosition = new Vector2(transform.position.x, transform.position.z);
            if(Vector2.Distance(currentPosition, lastPosition) > walkingDistance)
            {
                if(!particleObject.isPlaying) particleObject.Play();
            }
            else
            {
                particleObject.Stop();
            }
            lastPosition = currentPosition;
        }
    }
}
