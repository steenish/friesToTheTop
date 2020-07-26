using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions {
    

    public static Vector3 RandomPointInCircle(Vector3 center, float radius) {
        Vector3 spherePoint = RandomPointInSphere(center, radius);
        spherePoint.y = center.y;
        return spherePoint;
    }

    // Find point using Las Vegas random algorithm.
    public static Vector3 RandomPointInSphere(Vector3 center, float radius) {
        return RandomPointInSphereExcludeInner(center, radius, 0.0f);
    }

    public static Vector3 RandomPointInSphereExcludeInner(Vector3 center, float outerRadius, float innerRadius) {
        bool foundPoint = false;
        Vector3 point = Vector3.zero;

        while (!foundPoint) {
            float x = Random.Range(center.x - outerRadius, center.x + outerRadius);
            float y = Random.Range(center.y - outerRadius, center.y + outerRadius);
            float z = Random.Range(center.z - outerRadius, center.z + outerRadius);

            Vector3 candidate = new Vector3(x, y, z);
            float distance = Vector3.Distance(candidate, center);

            if (innerRadius <= distance && distance <= outerRadius) {
                point = candidate;
                foundPoint = true;
            }
        }
        return point;
    }
}
