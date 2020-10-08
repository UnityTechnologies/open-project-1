using AI.Navigation;
using AV.Logic;
using UnityEngine;
using UnityEngine.AI;

namespace AI.States
{
    [CreateAssetMenu(menuName = "NPC/Actions/Patrol")]
    public class PatrolAction : StateAction
    {
        protected override void OnUpdate()
        {
            var agent = GetComponent<NavMeshAgent>();
            var patrol = GetData<PatrolState>();
            
            if (!patrol.waypoints) 
                return;

            var position = transform.position;
            var distanceToPoint = 0f;

            if (patrol.currentPoint == -1)
            {
                var nearestPoint = patrol.waypoints.GetNearestPoint(position, out distanceToPoint, out patrol.currentPoint);
                agent.SetDestination(nearestPoint);
            }
                
            distanceToPoint = Vector3.Distance(position, agent.destination);
                    
            if (distanceToPoint <= agent.stoppingDistance)
            {
                var nextPoint = patrol.waypoints.GetNextPoint(ref patrol.currentPoint);
                agent.SetDestination(nextPoint);
            }
        }
    }
}