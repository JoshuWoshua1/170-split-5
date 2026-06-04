using System;
using UnityEngine;

[Serializable]
public class TelegraphSizeChangeSettings
{
    [Header("Resize")]
    public SizeChangeMode mode = SizeChangeMode.None;

    [Min(0f)] public float sizeChangeSpeed = 1f;

    [Tooltip("Used as max radius/side length/line length depending on shape.")]
    [Min(0f)] public float maxPrimaryValue;

    [Tooltip("Used as max width/angle when the selected shape needs a second value.")]
    [Min(0f)] public float maxSecondaryValue;

    [Min(1)] public int stepCount = 3;
    [Min(0f)] public float timeBetweenSteps = 0.15f;
    [Min(0f)] public float lastMomentDelay = 0.5f;
}
