using System;
using System.Collections;
using System.Collections.Generic;
using AI.Navigation;
using AV.Logic;
using UnityEngine;

namespace AI.States
{
    [Serializable]
    public class PatrolState : IStateData
    {
        public Waypoints waypoints;
        public int currentPoint;

        public PatrolState(Waypoints waypoints)
        {
            this.waypoints = waypoints;
            currentPoint = -1;
        }
    }
}
