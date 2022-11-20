using UnityEngine;
using UnityEditor;

namespace ByteAttributes.Editor {

    /// <summary>
    /// This script contains the logic for the "Absolute Value" attribute.
    /// The script will check for the type of numerical variable and using
    /// Mathf.abs, it will force the value to be positive.
    /// </sumary>

    [CustomPropertyDrawer(typeof(AbsoluteValueAttribute))]
    public class AbsoluteValuePropertyDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (property.propertyType == SerializedPropertyType.Integer) {
                property.intValue = Mathf.Abs(EditorGUI.IntField(position, label, property.intValue));
            }
            else if (property.propertyType == SerializedPropertyType.Float) {
                property.floatValue = Mathf.Abs(EditorGUI.FloatField(position, label, property.floatValue));
            }
            else {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}