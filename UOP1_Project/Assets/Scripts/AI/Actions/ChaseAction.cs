using System.Collections;
using System.Collections.Generic;
using AV.Logic;
using AI.States;
using UnityEngine;
using UnityEngine.AI;

namespace AI.States
{
    [CreateAssetMenu(menuName = "NPC/Actions/Chase")]
    public class ChaseAction : StateAction
    {
        protected override void OnUpdate()
        {
            var agent = GetAttachedComponent<NavMeshAgent>();
            
            if(TryGetData<ChaseState>(out var chase))
            {
                var targetPosition = chase.target.position;
                
                transform.rotation = Quaternion.LookRotation(
                    targetPosition - transform.position, Vector3.up);
                
                agent.SetDestination(targetPosition);
            }
        }
    }
}
