﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab = null;

    [System.Serializable]
    public class Settings
    {
        [Tooltip("Half the length of the box to spawn grass, centered on the game object")]
        public int areaHalfLength;
        [Tooltip("The number of rings to calculate. Put more than you think.")]
        public int numRings;
        [Tooltip("The amount to increase the ring radius each iteration")]
        public float ringRadiusIncrement;
        [Tooltip("If ringIndex % staggerRingModulo == 0, apply a radius offset")]
        public int staggerRingModulo;
        [Tooltip("The radius offset to apply every few rings")]
        public float staggerRingOffset;
        [Tooltip("The amount to offset to the circle centers to the left of the spawn box")]
        public float circleCenterOffset;
    }
    [SerializeField] private Settings settings = null;

    private float CalcRingRadius(int ringIndex, int otherCircleRingIndex)
    {
        // Every X rings of the other circle, offset our radius
        return ringIndex * settings.ringRadiusIncrement +
            (otherCircleRingIndex % settings.staggerRingModulo == 0 ? settings.staggerRingOffset : 0);
    }

    private bool DoCirclesIntersect(float centerDistance, float radiusA, float radiusB)
    {
        // There will only be intersections if the distance between circles is less than the sum of their radii
        // and more than the difference between their radii
        return radiusA + radiusB > centerDistance && centerDistance > Mathf.Abs(radiusA - radiusB);
    }

    private Vector2 CalcIntersectionPoint(Vector2 circleACenter, Vector2 centerDelta, float centerDistance, float radiusA, float radiusB)
    {
        // Math to calculate the intersection between two circles
        float lengthMultiplier = (radiusA * radiusA - radiusB * radiusB + centerDistance * centerDistance) / (2 * centerDistance);
        float heightMultiplier = Mathf.Sqrt(radiusA * radiusA - lengthMultiplier * lengthMultiplier);
        float lDivD = lengthMultiplier / centerDistance;
        float hDivD = heightMultiplier / centerDistance;

        // The intersection point
        Vector2 pointA = new Vector2(lDivD * centerDelta.x + hDivD * centerDelta.y + circleACenter.x,
                                    lDivD * centerDelta.y - hDivD * centerDelta.x + circleACenter.y);
        // This math is for the second interestion point. It's always outside the bounds, so I ignore it
        //float2 pointB = new float2(lDivD * circleCenterDiff.x - hDivD * circleCenterDiff.y + circleACenter.x,
        //                            lDivD * circleCenterDiff.y + hDivD * circleCenterDiff.x + circleACenter.y);

        return pointA;
    }

    private bool IsPointInBounds(Rect areaBounds, Vector2 point)
    {
        return areaBounds.Contains(point);
    }

    public void Start()
    {
        // Create a list to hold positions to spawn grass
        List<Vector2> spawnPoints = new List<Vector2>();

        var s = settings;

        // Math to initialize some information about the circles and the spawn area
        Vector2 circleACenter = new Vector2(-s.areaHalfLength - s.circleCenterOffset, -s.areaHalfLength);
        Vector2 centerDelta = new Vector2(0, s.areaHalfLength * 2);
        float centerDistance = centerDelta.magnitude;
        Rect areaBounds = new Rect(-s.areaHalfLength, -s.areaHalfLength, s.areaHalfLength * 2, s.areaHalfLength * 2);

        // Loop through each ring
        for (int ringIndexA = 0; ringIndexA < s.numRings; ringIndexA++)
        {
            for (int ringIndexB = 0; ringIndexB < s.numRings; ringIndexB++)
            {
                float radiusA = CalcRingRadius(ringIndexA, ringIndexB);
                float radiusB = CalcRingRadius(ringIndexB, ringIndexA);

                if (DoCirclesIntersect(centerDistance, radiusA, radiusB))
                {
                    // Find the intersection point
                    Vector2 pointA = CalcIntersectionPoint(circleACenter, centerDelta, centerDistance, radiusA, radiusB);

                    // If the point is inside the bounds, add it to the spawn list
                    if (IsPointInBounds(areaBounds, pointA))
                    {
                        spawnPoints.Add(pointA);
                    }
                }
            }
        }

        // Spawn the grass prefab at every determined point
        var centerPos = transform.position;
        foreach (var point in spawnPoints)
        {
            GameObject.Instantiate(prefab,
                new Vector3(point.x, 0, point.y) + centerPos,
                Quaternion.Euler(0, Random.value * 360, 0),
                transform);
            ;
        }
    }
}
