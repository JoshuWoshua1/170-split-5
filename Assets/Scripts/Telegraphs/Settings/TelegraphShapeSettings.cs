using System;
using UnityEngine;

public enum TelegraphShapeType
{
    Circle,
    Square,
    Donut,
    Line,
    Cone
}

[Serializable]
public class CircleShapeSettings
{
    [Min(0f)] public float radius = 2f;
}

[Serializable]
public class DonutShapeSettings
{
    [Min(0f)] public float innerRadius = 1f;
    [Min(0f)] public float outerRadius = 3f;
}

[Serializable]
public class LineShapeSettings
{
    [Min(0f)] public float length = 5f;
    [Min(0f)] public float width = 1f;
}

[Serializable]
public class ConeShapeSettings
{
    [Min(0f)] public float radius = 3f;
    [Range(0f, 360f)] public float angle = 45f;
}

[Serializable]
public class SquareShapeSettings
{
    [Min(0f)] public float sideLength = 3f;
}

[Serializable]
public class TelegraphShapeSettings
{
    [Header("Shape Type")]
    public TelegraphShapeType shapeType = TelegraphShapeType.Circle;

    [Header("Circle")]
    public CircleShapeSettings circle = new CircleShapeSettings();

    [Header("Square")]
    public SquareShapeSettings square = new SquareShapeSettings();

    [Header("Donut")]
    public DonutShapeSettings donut = new DonutShapeSettings();

    [Header("Line")]
    public LineShapeSettings line = new LineShapeSettings();

    [Header("Cone")]
    public ConeShapeSettings cone = new ConeShapeSettings();
}
