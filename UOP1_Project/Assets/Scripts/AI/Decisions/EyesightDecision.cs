using AV.Logic;
using UnityEngine;

namespace AI.States
{
    [CreateAssetMenu(menuName = "NPC/Decisions/Eyesight")]
    public class EyesightDecision : StateDecision
    {
        public LayerMask rayMask;
        public string targetTag;
        
        private readonly RaycastHit[] hits = new RaycastHit[8];
        private RaycastHit targetHit;
        
        public override bool OnDecide()
        {
            var data = GetSharedData<NpcData>();
            var forward = transform.forward;
            var position = transform.position;
            
            var hitCount = Physics.SphereCastNonAlloc(position, data.sightRadius, forward, hits,
                data.sightRadius, rayMask);

            var seesTarget = false;
            for(int i = 0; i < hitCount; i++)
            {
                if (hits[i].transform.CompareTag(targetTag))
                {
                    seesTarget = true;
                    targetHit = hits[i];
                    break;
                }
            }
            return seesTarget;
        }

        public override void AfterDecision(bool decided)
        {
            if(decided)
                SetData(new ChaseState { target = targetHit.transform });
        }
    }
}