using System;
using UnityEngine;

namespace ByteAttribute {

    /// <summary>
    /// Absolute Value Attribute forces any numerical value
    /// to be a positive value.
    /// </summary>

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class AbsoluteValueAttribute : ValidatorAttribute { }
}