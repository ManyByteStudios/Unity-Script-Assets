using System;
using UnityEngine;

namespace ByteAttributes {

    /// <summary>
    /// This script does nothing it is only used for the attribute tag.
    /// </summary>

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ReadOnlyAttribute : PropertyAttribute {

    }
}