using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A scriptable object for the HoverPhysics script.
/// Will create the hover properties for the script.
/// </summary>

[CreateAssetMenu(fileName = "Hover Properties", menuName = "Scriptable Objects/Hover Physics")]
public class HoverProperties : ScriptableObject {
    #region Editable values
    [Header("Rigidbody Properties")]
    [ReadOnly][SerializeField] float totalMass = 0;
    [Tooltip("Speed of slowing down movement.")]
    [MinValue(0)][SerializeField] float drag = 1;
    [Tooltip("Speed of slowing down rotational movement.")]
    [MinValue(0)][SerializeField] float angularDrag = 1;
    [Space(10)]
    [Header("Hover Physcis Properties")]
    [SerializeField] bool isHovering = true;
    [Tooltip("Locations of where the hover force is applied.")]
    [SerializeField] Transform[] hoverPoints;
    [Tooltip("Strength of the downward force.")]
    [AbsoluteValue][SerializeField] float hoverForce = 200;
    [Tooltip("Distance between the ground and the object.")]
    [AbsoluteValue][SerializeField] float hoverDistance = 0;
    #endregion

    #region Final Script Values
    public float TotalMass => totalMass;
    public float Drag => drag;
    public float AngularDrag => angularDrag;
    public bool IsHovering => isHovering;
    public Transform[] HoverPoints => hoverPoints;
    public float HoverForce => hoverForce;
    public float HoverDistance => hoverDistance;
    #endregion
}