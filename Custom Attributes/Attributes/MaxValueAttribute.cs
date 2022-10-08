using System;
using UnityEngine;

namespace ByteAttribute {

    /// <summary>
    /// Min Value Attribute clamps the highest possible value of any 
    /// numerical variable that is specified.
    /// </summary>

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MaxValueAttribute : ValidatorAttribute {
        public float maxValue { get; private set; }

        public MaxValueAttribute(float MaxVlaue) {
            maxValue = MaxVlaue;
        }

        public MaxValueAttribute(int MaxVlaue) {
            maxValue = MaxVlaue;
        }
    }
}