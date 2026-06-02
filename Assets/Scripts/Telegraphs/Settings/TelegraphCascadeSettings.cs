using System;
using UnityEngine;

[Serializable]
public class TelegraphCascadeSettings
{
    [Header("Cascade Toggle")]
    public bool cascades;

    [Header("Cascade Timing")]
    [Min(0f)] public float cascadeDelay = 1f;
    [Min(1)] public int cascadeCount = 3;

    [Header("Next Spawn Shape")]
    public bool useCascadeShape;
    public TelegraphShapeSettings cascadeShape = new TelegraphShapeSettings();

    [Header("Per-Cascade Size Steps")]
    public float circleRadiusStep;
    public float squareSideLengthStep;
    public float donutInnerRadiusStep;
    public float donutOuterRadiusStep;
    public float lineLengthStep;
    public float lineWidthStep;
    public float coneRadiusStep;
    public float coneAngleStep;
}
