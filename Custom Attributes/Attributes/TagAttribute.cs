using System;
using UnityEngine;

namespace ByteAttributes {
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class TagAttribute : PropertyAttribute {
        public bool UseDefaultTagFieldDrawer = false;
    }
}