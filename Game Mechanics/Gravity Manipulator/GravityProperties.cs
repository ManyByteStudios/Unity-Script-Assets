using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This script is a scriptable object for the Gravity Manipulator script.
    It contains all of the variables needed for the script.
*/

public enum Gravity { Directional, Spherical}
[System.Flags] public enum ForceAxis { X = (1 << 0), Y = (1 << 1), Z = (1 << 2)}
[CreateAssetMenu(fileName = "Gravity Properties", menuName = "Scriptable Objects/Gravity")]
public class GravityProperties : ScriptableObject {
    #region Editable Values
    [Header("Gravity Properties")]
    [SerializeField] Gravity gravityType = Gravity.Directional;
    [SerializeField] float gravityForce = 9.81f;
    [SerializeField] float rotationSpeed = 50f;
    [SerializeField] bool limitedGravityArea = true;
    [Space(10)]
    [SerializeField] [ConditionalEnumHide("gravityType", (int)Gravity.Directional)] GravityAxis gravityAxis = null;
    [SerializeField] [ConditionalHide("limitedGravityArea", true)] DirectionalGravity directionalGravity = null;
    [SerializeField] [ConditionalHide("limitedGravityArea", true)] SphericalGravity sphericalGravity = null;

    [System.Serializable]
    public class GravityAxis {
        [EnumFlags] public ForceAxis axis = ForceAxis.X;
    }

    [System.Serializable]
    public class DirectionalGravity {
        [ConditionalEnumHideAttribute("gravityType", (int)Gravity.Directional)] public Vector3 gravitySize = new Vector3(1, 1, 1);
    }

    [System.Serializable]
    public class SphericalGravity {
        [ConditionalEnumHideAttribute("gravityType", (int)Gravity.Spherical)] public float gravityRadius = 1;
    }
    #endregion

    #region Final Values
    public Gravity GravityType => gravityType;
    public float GravityForce => gravityForce;
    public float RotationSpeed => rotationSpeed;
    public bool LimitedGravityArea => limitedGravityArea;
    public GravityAxis GravAxis => gravityAxis;
    public DirectionalGravity DirectGravity => directionalGravity;
    public SphericalGravity SphereGravity => sphericalGravity;
    #endregion
}