using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TelegraphShapeSettings))]
public class TelegraphShapeSettingsDrawer : PropertyDrawer
{
    private const float VerticalSpacing = 2f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Rect contentRect = EditorGUI.IndentedRect(position);
        float y = contentRect.y;

        SerializedProperty shapeTypeProperty = property.FindPropertyRelative("shapeType");
        TelegraphShapeType shapeType = TelegraphShapeType.Circle;

        if (shapeTypeProperty != null)
        {
            float shapeTypeHeight = EditorGUI.GetPropertyHeight(shapeTypeProperty, true);
            Rect shapeTypeRect = new Rect(contentRect.x, y, contentRect.width, shapeTypeHeight);
            EditorGUI.PropertyField(shapeTypeRect, shapeTypeProperty, true);
            y = shapeTypeRect.yMax + VerticalSpacing;
            shapeType = (TelegraphShapeType)shapeTypeProperty.enumValueIndex;
        }

        DrawSelectedShapeFields(property, contentRect.x, contentRect.width, ref y, shapeType);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = 0f;

        SerializedProperty shapeTypeProperty = property.FindPropertyRelative("shapeType");
        TelegraphShapeType shapeType = TelegraphShapeType.Circle;

        if (shapeTypeProperty != null)
        {
            height += EditorGUI.GetPropertyHeight(shapeTypeProperty, true);
            shapeType = (TelegraphShapeType)shapeTypeProperty.enumValueIndex;
        }

        height += VerticalSpacing;
        height += GetSelectedShapeFieldsHeight(property, shapeType);

        return height;
    }

    private static void DrawSelectedShapeFields(
        SerializedProperty root,
        float x,
        float width,
        ref float y,
        TelegraphShapeType shapeType)
    {
        if (root == null)
        {
            return;
        }

        switch (shapeType)
        {
            case TelegraphShapeType.Circle:
                DrawField(root.FindPropertyRelative("circle")?.FindPropertyRelative("radius"), "Radius", x, width, ref y);
                break;

            case TelegraphShapeType.Square:
                DrawField(root.FindPropertyRelative("square")?.FindPropertyRelative("sideLength"), "Side Length", x, width, ref y);
                break;

            case TelegraphShapeType.Donut:
                DrawField(root.FindPropertyRelative("donut")?.FindPropertyRelative("innerRadius"), "Inner Radius", x, width, ref y);
                DrawField(root.FindPropertyRelative("donut")?.FindPropertyRelative("outerRadius"), "Outer Radius", x, width, ref y);
                break;

            case TelegraphShapeType.Line:
                DrawField(root.FindPropertyRelative("line")?.FindPropertyRelative("length"), "Length", x, width, ref y);
                DrawField(root.FindPropertyRelative("line")?.FindPropertyRelative("width"), "Width", x, width, ref y);
                break;

            case TelegraphShapeType.Cone:
                DrawField(root.FindPropertyRelative("cone")?.FindPropertyRelative("radius"), "Radius", x, width, ref y);
                DrawField(root.FindPropertyRelative("cone")?.FindPropertyRelative("angle"), "Angle", x, width, ref y);
                break;

            default:
                break;
        }
    }

    private static float GetSelectedShapeFieldsHeight(SerializedProperty root, TelegraphShapeType shapeType)
    {
        if (root == null)
        {
            return 0f;
        }

        float total = 0f;
        switch (shapeType)
        {
            case TelegraphShapeType.Circle:
                total += GetFieldHeight(root.FindPropertyRelative("circle")?.FindPropertyRelative("radius"));
                break;

            case TelegraphShapeType.Square:
                total += GetFieldHeight(root.FindPropertyRelative("square")?.FindPropertyRelative("sideLength"));
                break;

            case TelegraphShapeType.Donut:
                total += GetFieldHeight(root.FindPropertyRelative("donut")?.FindPropertyRelative("innerRadius"));
                total += GetFieldHeight(root.FindPropertyRelative("donut")?.FindPropertyRelative("outerRadius"));
                break;

            case TelegraphShapeType.Line:
                total += GetFieldHeight(root.FindPropertyRelative("line")?.FindPropertyRelative("length"));
                total += GetFieldHeight(root.FindPropertyRelative("line")?.FindPropertyRelative("width"));
                break;

            case TelegraphShapeType.Cone:
                total += GetFieldHeight(root.FindPropertyRelative("cone")?.FindPropertyRelative("radius"));
                total += GetFieldHeight(root.FindPropertyRelative("cone")?.FindPropertyRelative("angle"));
                break;

            default:
                break;
        }

        return total;
    }

    private static void DrawField(SerializedProperty field, string label, float x, float width, ref float y)
    {
        if (field == null)
        {
            return;
        }

        float height = EditorGUI.GetPropertyHeight(field, true);
        Rect rect = new Rect(x, y, width, height);
        EditorGUI.PropertyField(rect, field, new GUIContent(label), true);
        y = rect.yMax + VerticalSpacing;
    }

    private static float GetFieldHeight(SerializedProperty field)
    {
        if (field == null)
        {
            return 0f;
        }

        return EditorGUI.GetPropertyHeight(field, true) + VerticalSpacing;
    }
}
