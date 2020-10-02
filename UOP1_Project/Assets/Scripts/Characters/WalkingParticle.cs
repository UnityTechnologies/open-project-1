using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingParticle : MonoBehaviour
{

    [Tooltip("Particle prefab to be attached to the charactor in the scene.")] public ParticleSystem walkingParticlePrefab;

    private Vector3 lastPosition;
    private Vector3 currentPosition;
    private ParticleSystem particleObject;

    private void Update()
    {
        if (lastPosition == null) lastPosition = transform.position;
        if (currentPosition == null) currentPosition = transform.position;
        if (particleObject == null && walkingParticlePrefab != null)
        {
            particleObject = Instantiate(walkingParticlePrefab, transform);
            particleObject.Stop();
        }
        if(currentPosition != null && lastPosition != null && particleObject != null)
        {
            currentPosition = transform.position;
            if(currentPosition.x * 1000 != lastPosition.x * 1000 || currentPosition.z * 1000 != lastPosition.z * 1000)
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
