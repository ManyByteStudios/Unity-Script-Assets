using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This script manipulates the forces applied on any
    game objects with the "GravityBody" script. The 
    script will be dynamic enough to change both gravity
    direction and force along with the effected area.
*/

[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SphereCollider))]
public class GravityManipulator : MonoBehaviour {
    #region Gravity Manipulation Properties
    [Header("Gravity Manipulation Properties")]
    [SerializeField] GravityProperties gravityProperties = null;

    float CurrentGravity = 0;
    GravityBody[] tempBodies;
    Vector3 GravityDirection;
    Vector3 ObjectUp;
    Quaternion ObjectRotation;
    #endregion

    [ExecuteInEditMode]
    private void OnValidate() {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;

        if (gravityProperties.GravityType == Gravity.Directional) {
            boxCollider.enabled = true;
            sphereCollider.enabled = false;
        }
        else {
            boxCollider.enabled = false;
            sphereCollider.enabled = true;
        }

        boxCollider.size = gravityProperties.DirectGravity.gravitySize;
        sphereCollider.radius = gravityProperties.SphereGravity.gravityRadius;

        if (gravityProperties.DirectGravity.gravitySize.x < 0) {
            gravityProperties.DirectGravity.gravitySize.x = 0;
        }
        if (gravityProperties.DirectGravity.gravitySize.y < 0) {
            gravityProperties.DirectGravity.gravitySize.y = 0;
        }
        if (gravityProperties.DirectGravity.gravitySize.z < 0) {
            gravityProperties.DirectGravity.gravitySize.z = 0;
        }
        if (gravityProperties.SphereGravity.gravityRadius < 0) {
            gravityProperties.SphereGravity.gravityRadius = 0;
        }
    }

    // Start is called before the first frame update
    public virtual void Start() {
        CurrentGravity = gravityProperties.GravityForce;
    }

    // Late Update is called at the end of each frame
    public virtual void LateUpdate() {
        if (!gravityProperties.LimitedGravityArea) {
            // Finds every object with "GravityBody"
            tempBodies = FindObjectsOfType<GravityBody>();
            foreach (GravityBody i in tempBodies) {
                i.AddManipulator(this);
            }
        }
    }

    // Simple use for those who don't want to override
    public void GravityUpdate() {
        if (!gravityProperties.LimitedGravityArea) {
            // Finds every object with "GravityBody"
            tempBodies = FindObjectsOfType<GravityBody>();
            foreach (GravityBody i in tempBodies) {
                i.AddManipulator(this);
            }
        }
    }

    // This script will be called by the "GravityBody" 
    // script to apply gravitational force
    public void ApplyGravity(Rigidbody ObjectBody, Transform ObjectTransform) {
        switch (gravityProperties.GravityType) {
            case Gravity.Directional:
                // Apply gravitational force by axis
                if (gravityProperties.GravAxis.axis.HasFlag(ForceAxis.X)) {
                    ObjectBody.AddForce(ObjectTransform.right * -CurrentGravity);
                }
                if (gravityProperties.GravAxis.axis.HasFlag(ForceAxis.Y)) {
                    ObjectBody.AddForce(ObjectTransform.up * -CurrentGravity);
                }
                if (gravityProperties.GravAxis.axis.HasFlag(ForceAxis.Z)) {
                    ObjectBody.AddForce(ObjectTransform.forward * -CurrentGravity);
                }

                // Rotate object to align with the gravity source
                ObjectTransform.rotation = Quaternion.Slerp(ObjectTransform.rotation, transform.localRotation, gravityProperties.RotationSpeed * Time.deltaTime);
                break;
            case Gravity.Spherical:
                // Determine the direction of the gravity and object "up" direction
                GravityDirection = (ObjectTransform.position - transform.position).normalized;
                ObjectUp = ObjectTransform.up;

                // Apply gravity
                ObjectBody.AddForce(GravityDirection * -CurrentGravity);

                // Rotate the object based on the position realative to the gravity source
                ObjectRotation = Quaternion.FromToRotation(ObjectUp, GravityDirection) * ObjectTransform.rotation;
                ObjectTransform.rotation = Quaternion.Slerp(ObjectTransform.rotation, ObjectRotation, gravityProperties.RotationSpeed * Time.deltaTime);
                break;
        }
    }

    #region Gravity Manipulator Adjustments
    public void ChangeGravityForce(float NewGravForce = 0) {
        CurrentGravity = NewGravForce;
    }
    public void AdjustGravityForce(float GravAdjustment = 0) {
        CurrentGravity += GravAdjustment;
    }
    public void ChangeGravityRangeBox(Vector3 BoxSize) {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = gravityProperties.DirectGravity.gravitySize;
    }
    public void ChangeGravityRadius(float SphereRadius) {
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = gravityProperties.SphereGravity.gravityRadius;
    }
    #endregion

    public virtual void OnTriggerEnter(Collider obj) {
        obj.GetComponent<GravityBody>().AddManipulator(this);
    }
    public virtual void OnTriggerExit(Collider obj) {
        obj.GetComponent<GravityBody>().RemoveManipulator(this);
    }
}