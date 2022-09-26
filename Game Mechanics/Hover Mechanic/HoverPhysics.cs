using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script allows for any object with ot have
/// hover like physcis. The force applied on the 
/// object with vary depending on the force applied
/// on the object and the distance between the object
/// and ground.
/// </summary>

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class HoverPhysics : MonoBehaviour {
    #region Hover Properites
    [Header("Hover Properties")]
    [Tooltip("Hover scriptable object.")]
    [SerializeField] private HoverProperties hoverProperty;

    float TotalMass;
    bool IsHovering;

    [ExecuteInEditMode]
    private void OnValidate() {
        Rigidbody HoverBody = GetComponent<Rigidbody>();

        IsHovering = hoverProperty.IsHovering;

        HoverBody.drag = hoverProperty.Drag;
        HoverBody.angularDrag = hoverProperty.AngularDrag;

        TotalMass = HoverBody.mass;
    }
    #endregion

    // Hover function must be called in FixedUpdate
    public void Hover() {
        if (hoverProperty.IsHovering) {
            RaycastHit hit;
            foreach (Transform HoverPoint in hoverProperty.HoverPoints) {
                Vector3 DownwardForce;
                float ForcePercentage;

                if (Physics.Raycast(HoverPoint.position, HoverPoint.up * -1, out hit, hoverProperty.HoverDistance)) {
                    // Determine the current distance between the object and ground
                    ForcePercentage = 1 - (hit.distance / hoverProperty.HoverDistance);

                    // Calculate the amount of force to apply
                    DownwardForce = transform.up * hoverProperty.HoverForce * ForcePercentage;

                    // Include the force applied on the object via mass
                    DownwardForce *= Time.deltaTime * TotalMass;

                    // Apply force
                    Rigidbody HoverBody = GetComponent<Rigidbody>();
                    HoverBody.AddForceAtPosition(DownwardForce, HoverPoint.position);
                }
            }
        }
    }

    public void AllowHover () {
        IsHovering = !IsHovering;
    }

    void OnCollisionEnter(Collision Obj) {
        if (Obj.gameObject.GetComponent<Rigidbody>() != null) {
            TotalMass += Obj.gameObject.GetComponent<Rigidbody>().mass;
        }
    }

    private void OnCollisionExit(Collision Obj) {
        if (Obj.gameObject.GetComponent<Rigidbody>() != null) {
            TotalMass -= Obj.gameObject.GetComponent<Rigidbody>().mass;
        }
    }
}