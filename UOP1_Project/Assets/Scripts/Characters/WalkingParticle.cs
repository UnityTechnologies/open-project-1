using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingParticle : MonoBehaviour
{

    [Tooltip("Particle prefab to be attached to the charactor in the scene.")] public ParticleSystem walkingParticlePrefab;

    private Vector2 lastPosition;
    private Vector2 currentPosition;
    private ParticleSystem particleObject;

    private void OnEnable()
    {
        lastPosition = new Vector2(transform.position.x, transform.position.z);
        currentPosition = new Vector2(transform.position.x, transform.position.z);
    }

    private void Update()
    {
        if (!particleObject && walkingParticlePrefab)
        {
            particleObject = Instantiate(walkingParticlePrefab, transform);
            particleObject.Stop();
        }
        if(particleObject)
        {
            currentPosition = new Vector2(transform.position.x, transform.position.z);
            if(Vector2.Distance(currentPosition, lastPosition) > 0)
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
