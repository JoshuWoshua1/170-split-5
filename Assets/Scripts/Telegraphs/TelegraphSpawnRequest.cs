using System;
using UnityEngine;

[Serializable]
public class TelegraphSpawnRequest
{
    [Header("Who/Where")]
    public Transform origin;
    public Vector3 worldPosition;

    [Header("Core")]
    [Min(0f)] public float duration = 1f;
    public int damage = 1;

    [Header("Shape")]
    public TelegraphShapeSettings shape = new TelegraphShapeSettings();

    [Header("Resize")]
    public TelegraphSizeChangeSettings sizeChange = new TelegraphSizeChangeSettings();

    [Header("Cascade")]
    public TelegraphCascadeSettings cascade = new TelegraphCascadeSettings();
}
