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
}
