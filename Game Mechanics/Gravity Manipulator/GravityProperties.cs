using UnityEngine;
using ByteAttributes;
using System.Collections.Generic;

/// <summary>
/// A scriptable object for the GravityManipulator script.
/// Will create a gravity property for the script.
/// </summary>

[CreateAssetMenu(fileName = "Gravity Properties", menuName = "Scriptable Objects/Gravity Manipulator")]
public class GravityProperties : ScriptableObject {
    #region Editable variables
    public enum Gravity { Directional, Spherical }
    public enum ForceAxis { X, Y, Z }

    [Header("Gravity Properties")]
    [Tooltip("Plantary gravity (Spherical), Specific direction (Directional)")]
    [SerializeField] Gravity gravityType = Gravity.Directional;
    [Tooltip("Gravity strength.")]
    [SerializeField] float gravityForce = 9.81f;
    [Tooltip("The speed of the object rotatating upwards relative to gravity direction.")]
    [SerializeField] float rotationSpeed = 50f;
    [Space(5)]
    [LineDivider(4, color: LineColors.Black)]
    [Tooltip("Limited Gravity Range.")]
    [SerializeField] bool limitedGravityArea = true;
    [Space(10)]
    [Tooltip("Direction of gravity.")]
    [SerializeField][ConditionalEnumHide("gravityType", (int)Gravity.Directional)] GravityAxis gravityAxis = null;
    [Tooltip("Area for gravity.")]
    [SerializeField][ConditionalHide("limitedGravityArea", true)] DirectionalGravity directionalGravity = null;
    [Tooltip("Area for gravity.")]
    [SerializeField][ConditionalHide("limitedGravityArea", true)] SphericalGravity sphericalGravity = null;
    [Space(5)]
    [LineDivider(4, color: LineColors.Black)]
    [Tooltip("Gravity manipulator affects only listed objects with selective tags.")]
    [SerializeField] bool selectiveObjects = false;
    [Tooltip("All tags that will be affect by the gravity manipulator.")]
    [SerializeField] List<SelectiveTags> objectTags = null;

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

    [System.Serializable]
    public class SelectiveTags {
        [Tag] public string objectTag = null;
    }
    #endregion

    #region Final Script Values
    public Gravity GravityType => gravityType;
    public float GravityForce => gravityForce;
    public float RotationSpeed => rotationSpeed;
    public bool LimitedGravityArea => limitedGravityArea;

    public GravityAxis GravAxis => gravityAxis;
    public DirectionalGravity DirectionalGrav => directionalGravity;
    public SphericalGravity SphericalGrav => sphericalGravity;

    public bool SelectiveObjects => selectiveObjects;
    public List<SelectiveTags> ObjectTags => objectTags;
    #endregion
}