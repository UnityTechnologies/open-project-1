using System;
using AV.Logic;
using UnityEngine;

namespace AI.States
{
    [Serializable]
    public struct ChaseState : IStateData
    {
        public Transform target;
    }
}