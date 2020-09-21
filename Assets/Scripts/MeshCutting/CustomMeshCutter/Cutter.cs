using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter
{
    public static void SplitOBJ(GameObject obj, Transform selection, Vector3 direction)
    {
        float[] coeffs = ParallelismCoefficient(selection, direction, 0.0f);
        float x = coeffs[0];
        float y = coeffs[1];
        float z = coeffs[2];

        // Debug.Log(string.Format("x:{0}, y:{1}, z{2} => Shortest: {3}", x, y, z, ClosetTo(1, coeffs)));
        // Debug.Log(string.Format("x:{0}, y:{1}, z{2} => MIN: {3}", x, y, z, Mathf.Min(Mathf.Min(x, y), z)));

        GameObject plane = GameObject.Find("PlaneCutter");
        plane.transform.position = selection.position;
        plane.transform.rotation = Quaternion.LookRotation(direction);
        // {
        // }
        GameObject left = new GameObject(),
            right = new GameObject();

        Plane cutPlane = new Plane();
        cutPlane.SetNormalAndPosition(plane.transform.up, selection.position);

        int count = 0;
        foreach (Transform child in obj.transform)
        {
            Transform child1 = Object.Instantiate(child);
            if (cutPlane.GetSide(child.transform.position) || selection.position.Equals(child.transform.position))
            {
                child1.parent = left.transform;
            }
            else
            {
                child1.parent = right.transform;
            }

            count++;
        }

        GameObject.Destroy(obj.gameObject);
    }

    /// <summary>
    /// Check which axis the direction vector in closely parallel with
    /// We return the axis whose product with the direction is the closest to 1
    /// </summary>
    /// <param name="postion"></param>
    /// <param name="direction"></param>
    /// <param name="bias"></param>
    private static float[] ParallelismCoefficient(Transform position, Vector3 direction, float bias)
    {
        // Calculate coefficients
        float xCoeff = Vector3.Dot(position.right.normalized, direction.normalized); // Along the x axis
        float yCoeff = Vector3.Dot(position.up.normalized, direction.normalized); // Along the y axis
        float zCoeff = Vector3.Dot(position.forward.normalized, direction.normalized); // Along the z axis

        Debug.Log(xCoeff + ", " + yCoeff + ", " + zCoeff);

        return new float[] { xCoeff, yCoeff, zCoeff };
    }

    /// <summary>
    /// Find the value closest to val among the list of values
    /// </summary>
    /// <param name="val"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    private static float ClosetTo(float val, float[] values)
    {
        // float closest = 0.0f;
        float distance = Mathf.Abs(values[0]);
        int idx = 0;
        for(int i = 1; i < values.Length; i++)
        {
            // Compute the intermediary distance between the given value and the value
            // which correspond to the current index
            // if this distance is shorter than current distance, the newly computed distance
            // becomes the current shortest distance
            float cDistance = Mathf.Abs(1 - values[i]);

            if(cDistance < distance)
            {
                idx = i;
                distance = cDistance;
            }
        }

        return values[idx];
    }
}
