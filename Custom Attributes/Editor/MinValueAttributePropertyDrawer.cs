using UnityEngine;
using UnityEditor;

namespace ByteAttributes.Editor {

    /// <summary>
    /// This script contains the logic for the "Min Value" attribute.
    /// Clamping the value to the minimum possible value.
    /// </summary>

    [CustomPropertyDrawer(typeof(MinValueAttribute))]
    public class MinValueAttributePropertyDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            MinValueAttribute Min = (MinValueAttribute)attribute;

            if (property.propertyType == SerializedPropertyType.Integer) {
                if (property.intValue < Min.minValue) {
                    property.intValue = (int)Min.minValue;
                }

                property.intValue = EditorGUI.IntField(position, label, property.intValue);
            }
            else if (property.propertyType == SerializedPropertyType.Float) {
                if (property.floatValue < Min.minValue) {
                    property.floatValue = (float)Min.minValue;
                }

                property.floatValue = EditorGUI.FloatField(position, label, property.floatValue);
            }
            else {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}