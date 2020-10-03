using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Navigation
{
    public class Waypoints : MonoBehaviour
    {
        [SerializeField] private Vector3[] points = new Vector3[1];

        public Vector3 GetNearestPoint(Vector3 position, out float distanceToPoint, out int nearestPointIndex)
        {
            distanceToPoint = Mathf.Infinity;
            nearestPointIndex = 0;

            for (int i = 0; i < points.Length; i++)
            {
                var distance = Vector3.Distance(points[i], position);
                if (distance < distanceToPoint)
                {
                    distanceToPoint = distance;
                    nearestPointIndex = i;
                }
            }

            return points[nearestPointIndex];
        }

        public Vector3 GetNextPoint(ref int currentPointIndex)
        {
            currentPointIndex = (currentPointIndex + 1) % points.Length;

            return points[currentPointIndex];
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 previousPoint = points[0];
            for (int i = 0; i < points.Length; i++)
            {
                Gizmos.DrawLine(previousPoint, points[i]);
                previousPoint = points[i];
            }

            Gizmos.DrawLine(previousPoint, points[0]);
        }
    }
}