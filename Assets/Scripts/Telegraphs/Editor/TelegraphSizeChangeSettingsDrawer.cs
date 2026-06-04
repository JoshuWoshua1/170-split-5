using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TelegraphSizeChangeSettings))]
public class TelegraphSizeChangeSettingsDrawer : PropertyDrawer
{
    private const float VerticalSpacing = 2f;

    private enum ResizeFieldSet
    {
        None,
        PrimaryOnly,
        PrimaryAndSecondary
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (ShouldHideForDonut(property))
        {
            return;
        }

        EditorGUI.BeginProperty(position, label, property);

        Rect rect = EditorGUI.IndentedRect(position);
        float y = rect.y;

        SerializedProperty mode = property.FindPropertyRelative("mode");
        SerializedProperty speed = property.FindPropertyRelative("sizeChangeSpeed");
        SerializedProperty maxPrimary = property.FindPropertyRelative("maxPrimaryValue");
        SerializedProperty maxSecondary = property.FindPropertyRelative("maxSecondaryValue");
        SerializedProperty stepCount = property.FindPropertyRelative("stepCount");
        SerializedProperty timeBetweenSteps = property.FindPropertyRelative("timeBetweenSteps");
        SerializedProperty lastMomentDelay = property.FindPropertyRelative("lastMomentDelay");

        y = DrawField(rect, y, mode, "Resize Mode");

        SizeChangeMode selectedMode = mode != null ? (SizeChangeMode)mode.enumValueIndex : SizeChangeMode.None;
        TelegraphShapeType shapeType = GetShapeType(property);
        ResizeFieldSet fieldSet = GetFieldSet(shapeType);
        string primaryLabel = GetPrimaryLabel(shapeType);
        string secondaryLabel = GetSecondaryLabel(shapeType);

        if (selectedMode == SizeChangeMode.Gradual)
        {
            y = DrawField(rect, y, speed, "Resize Speed");
            if (fieldSet == ResizeFieldSet.PrimaryOnly || fieldSet == ResizeFieldSet.PrimaryAndSecondary)
            {
                y = DrawField(rect, y, maxPrimary, primaryLabel);
            }

            if (fieldSet == ResizeFieldSet.PrimaryAndSecondary)
            {
                y = DrawField(rect, y, maxSecondary, secondaryLabel);
            }
        }
        else if (selectedMode == SizeChangeMode.LastMoment)
        {
            y = DrawField(rect, y, lastMomentDelay, "Last Moment Delay");
            if (fieldSet == ResizeFieldSet.PrimaryOnly || fieldSet == ResizeFieldSet.PrimaryAndSecondary)
            {
                y = DrawField(rect, y, maxPrimary, primaryLabel);
            }

            if (fieldSet == ResizeFieldSet.PrimaryAndSecondary)
            {
                y = DrawField(rect, y, maxSecondary, secondaryLabel);
            }
        }
        else if (selectedMode == SizeChangeMode.Stepped)
        {
            y = DrawField(rect, y, stepCount, "Step Count");
            y = DrawField(rect, y, timeBetweenSteps, "Time Between Steps");
            if (fieldSet == ResizeFieldSet.PrimaryOnly || fieldSet == ResizeFieldSet.PrimaryAndSecondary)
            {
                y = DrawField(rect, y, maxPrimary, primaryLabel);
            }

            if (fieldSet == ResizeFieldSet.PrimaryAndSecondary)
            {
                y = DrawField(rect, y, maxSecondary, secondaryLabel);
            }
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (ShouldHideForDonut(property))
        {
            return 0f;
        }

        float height = 0f;

        SerializedProperty mode = property.FindPropertyRelative("mode");
        SerializedProperty speed = property.FindPropertyRelative("sizeChangeSpeed");
        SerializedProperty maxPrimary = property.FindPropertyRelative("maxPrimaryValue");
        SerializedProperty maxSecondary = property.FindPropertyRelative("maxSecondaryValue");
        SerializedProperty stepCount = property.FindPropertyRelative("stepCount");
        SerializedProperty timeBetweenSteps = property.FindPropertyRelative("timeBetweenSteps");
        SerializedProperty lastMomentDelay = property.FindPropertyRelative("lastMomentDelay");

        height += GetHeight(mode);

        SizeChangeMode selectedMode = mode != null ? (SizeChangeMode)mode.enumValueIndex : SizeChangeMode.None;
        ResizeFieldSet fieldSet = GetFieldSet(GetShapeType(property));

        if (selectedMode == SizeChangeMode.Gradual)
        {
            height += GetHeight(speed);
            if (fieldSet == ResizeFieldSet.PrimaryOnly || fieldSet == ResizeFieldSet.PrimaryAndSecondary)
            {
                height += GetHeight(maxPrimary);
            }

            if (fieldSet == ResizeFieldSet.PrimaryAndSecondary)
            {
                height += GetHeight(maxSecondary);
            }
        }
        else if (selectedMode == SizeChangeMode.LastMoment)
        {
            height += GetHeight(lastMomentDelay);
            if (fieldSet == ResizeFieldSet.PrimaryOnly || fieldSet == ResizeFieldSet.PrimaryAndSecondary)
            {
                height += GetHeight(maxPrimary);
            }

            if (fieldSet == ResizeFieldSet.PrimaryAndSecondary)
            {
                height += GetHeight(maxSecondary);
            }
        }
        else if (selectedMode == SizeChangeMode.Stepped)
        {
            height += GetHeight(stepCount);
            height += GetHeight(timeBetweenSteps);
            if (fieldSet == ResizeFieldSet.PrimaryOnly || fieldSet == ResizeFieldSet.PrimaryAndSecondary)
            {
                height += GetHeight(maxPrimary);
            }

            if (fieldSet == ResizeFieldSet.PrimaryAndSecondary)
            {
                height += GetHeight(maxSecondary);
            }
        }

        return height;
    }

    private static float DrawField(Rect rect, float y, SerializedProperty prop, string label)
    {
        if (prop == null)
        {
            return y;
        }

        float height = EditorGUI.GetPropertyHeight(prop, true);
        EditorGUI.PropertyField(new Rect(rect.x, y, rect.width, height), prop, new GUIContent(label), true);
        return y + height + VerticalSpacing;
    }

    private static float GetHeight(SerializedProperty prop)
    {
        if (prop == null)
        {
            return 0f;
        }

        return EditorGUI.GetPropertyHeight(prop, true) + VerticalSpacing;
    }

    private static bool ShouldHideForDonut(SerializedProperty sizeChangeProperty)
    {
        if (sizeChangeProperty == null)
        {
            return false;
        }

        string propertyPath = sizeChangeProperty.propertyPath;
        const string suffix = ".sizeChange";
        int suffixIndex = propertyPath.LastIndexOf(suffix);
        if (suffixIndex < 0)
        {
            return false;
        }

        string shapeTypePath = propertyPath.Substring(0, suffixIndex) + ".shape.shapeType";
        SerializedProperty shapeTypeProperty = sizeChangeProperty.serializedObject.FindProperty(shapeTypePath);
        if (shapeTypeProperty == null)
        {
            return false;
        }

        return (TelegraphShapeType)shapeTypeProperty.enumValueIndex == TelegraphShapeType.Donut;
    }

    private static TelegraphShapeType GetShapeType(SerializedProperty sizeChangeProperty)
    {
        if (sizeChangeProperty == null)
        {
            return TelegraphShapeType.Circle;
        }

        string propertyPath = sizeChangeProperty.propertyPath;
        const string suffix = ".sizeChange";
        int suffixIndex = propertyPath.LastIndexOf(suffix);
        if (suffixIndex < 0)
        {
            return TelegraphShapeType.Circle;
        }

        string shapeTypePath = propertyPath.Substring(0, suffixIndex) + ".shape.shapeType";
        SerializedProperty shapeTypeProperty = sizeChangeProperty.serializedObject.FindProperty(shapeTypePath);
        if (shapeTypeProperty == null)
        {
            return TelegraphShapeType.Circle;
        }

        return (TelegraphShapeType)shapeTypeProperty.enumValueIndex;
    }

    private static ResizeFieldSet GetFieldSet(TelegraphShapeType shapeType)
    {
        switch (shapeType)
        {
            case TelegraphShapeType.Circle:
            case TelegraphShapeType.Square:
                return ResizeFieldSet.PrimaryOnly;
            case TelegraphShapeType.Line:
            case TelegraphShapeType.Cone:
                return ResizeFieldSet.PrimaryAndSecondary;
            default:
                return ResizeFieldSet.None;
        }
    }

    private static string GetPrimaryLabel(TelegraphShapeType shapeType)
    {
        switch (shapeType)
        {
            case TelegraphShapeType.Circle:
                return "Max Radius";
            case TelegraphShapeType.Square:
                return "Max Side Length";
            case TelegraphShapeType.Line:
                return "Max Length";
            case TelegraphShapeType.Cone:
                return "Max Radius";
            default:
                return "Max Primary Value";
        }
    }

    private static string GetSecondaryLabel(TelegraphShapeType shapeType)
    {
        switch (shapeType)
        {
            case TelegraphShapeType.Line:
                return "Max Width";
            case TelegraphShapeType.Cone:
                return "Max Angle";
            default:
                return "Max Secondary Value";
        }
    }
}
