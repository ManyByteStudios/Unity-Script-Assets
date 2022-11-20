using System;
using UnityEngine;

namespace ByteAttributes { 
    public interface ByteAttributes { }

    public class ValidatorAttribute : PropertyAttribute, ByteAttributes { }

    public class DrawerAttribute : PropertyAttribute, ByteAttributes { }
}