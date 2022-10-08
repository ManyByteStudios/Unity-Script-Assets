using System;
using UnityEngine;

namespace ByteAttribute {

    /// <summary>
    /// Absolute Value Attribute forces any numerical value
    /// to be a negative value.
    /// </summary>

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class NegativeValueAttribute : ValidatorAttribute { }
}