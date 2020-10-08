using System;
using AI.Navigation;
using AV.Logic;
using AI.States;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    [AddComponentMenu("Logic/NPC")]
    [RequireComponent(typeof(NavMeshAgent))]
    public class NpcMachine : StateMachine
    {
        public NpcData data;
        public Waypoints waypoints;
        [Space]
        public AudioSource audioSource;
        public ParticleSystem alertParticle;
        
        private void Awake()
        {
            SetComponent(GetComponent<NavMeshAgent>());
            SetComponent(audioSource);
            SetComponent(alertParticle);
            
            SetData(data, new NpcState(data));
            SetData(new PatrolState(waypoints));

            Initialize();
        }

        private void Update() => Run();
    }
}