using System;
using UnityEngine;

namespace ByteAttribute {
    public interface ByteAttribute { }

    public class ValidatorAttribute : PropertyAttribute, ByteAttribute { }

    public class DrawerAttribute : PropertyAttribute, ByteAttribute { }
}