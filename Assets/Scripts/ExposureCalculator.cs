using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExposureCalculator : MonoBehaviour
{
    public const float bias = 0.09f;

    public static float ComputeExposure(int face, int material) {
        return face * material * bias;
    }
}
