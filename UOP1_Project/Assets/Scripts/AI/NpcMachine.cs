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
            AttachComponent(GetComponent<NavMeshAgent>());
            AttachComponent(audioSource);
            AttachComponent(alertParticle);
            
            AttachData(data, new NpcState(data));
            AttachData(new PatrolState(waypoints));

            Initialize();
        }

        private void Update()
        {
            Run();
        }
    }
}