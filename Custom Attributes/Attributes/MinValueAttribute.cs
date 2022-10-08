using System;
using UnityEngine;

namespace ByteAttribute {

    /// <summary>
    /// Min Value Attribute clamps the loweset possible value of any 
    /// numerical variable that is specified.
    /// </summary>

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MinValueAttribute : ValidatorAttribute {
        public float minValue { get; private set; }

        public MinValueAttribute(float MainVlaue) {
            minValue = MainVlaue;
        }

        public MinValueAttribute(int MinVlaue) {
            minValue = MinVlaue;
        }
    }
}