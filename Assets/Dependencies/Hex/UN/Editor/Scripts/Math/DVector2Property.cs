
using HexUN.Math;
using UnityEditor;
using UnityEngine;

namespace HexUN.Math
{
    [CustomPropertyDrawer(typeof(DVector2))]
    public class DVector2Property : PropertyDrawer
    {
        private readonly GUIContent rXLabel = new GUIContent("X");
        private readonly GUIContent rYLabel = new GUIContent("Y");

        private const float cLabelWidth = 15;

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            float halfWidth = position.width / 2;

            // X
            SerializedProperty propX = property.FindPropertyRelative("X");
            EditorGUI.LabelField(
                new Rect(position.x, position.y, cLabelWidth, position.height), 
                rXLabel
            );

            propX.intValue = EditorGUI.IntField(
                new Rect(position.x + cLabelWidth, position.y, halfWidth - cLabelWidth, position.height),
                propX.intValue
            );

            // Y
            SerializedProperty propY = property.FindPropertyRelative("Y");
            EditorGUI.LabelField(
                new Rect(position.x + halfWidth, position.y, cLabelWidth, position.height),
                rYLabel
            );

            propY.intValue = EditorGUI.IntField(
                new Rect(position.x + halfWidth + cLabelWidth, position.y, halfWidth - cLabelWidth, position.height),
                propY.intValue
            );


            EditorGUI.EndProperty();
        }
    }
}
