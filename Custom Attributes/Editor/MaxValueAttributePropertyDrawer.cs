using UnityEngine;
using UnityEditor;

namespace ByteAttributes.Editor {

    /// <summary>
    /// This script contains the logic for the "Max Value" attribute.
    /// Clamping the value to the maxiumum possible value.
    /// </summary>

    [CustomPropertyDrawer(typeof(MaxValueAttribute))]
    public class MaxValueAttributePropertyDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            MaxValueAttribute Max = (MaxValueAttribute)attribute;

            if (property.propertyType == SerializedPropertyType.Integer) {
                if (property.intValue > Max.maxValue) {
                    property.intValue = (int)Max.maxValue;
                }

                property.intValue = EditorGUI.IntField(position, label, property.intValue);
            }
            else if (property.propertyType == SerializedPropertyType.Float) {
                if (property.floatValue > Max.maxValue) {
                    property.floatValue = (float)Max.maxValue;
                }

                property.floatValue = EditorGUI.FloatField(position, label, property.floatValue);
            }
            else {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}