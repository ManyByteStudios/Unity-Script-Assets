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
    #region Gravity Manipulator Values
    protected enum Gravity { Directional, Spherical}
    protected enum ForceAxis { X, Y, Z }

    [Header("Gravity Properties")]
    [Tooltip("Plantary gravity (Spherical), Specific direction (Directional)")] [SerializeField] Gravity gravityType = Gravity.Directional;
    [Tooltip("Gravity strength.")] [SerializeField] float gravityForce = 9.81f;
    [Tooltip("The speed of the object rotatating upwards relative to gravity direction.")] [SerializeField] float rotationSpeed = 50f;
    [Tooltip("Limited Gravity Range.")] [SerializeField] bool limitedGravityArea = true;
    [Space(10)]
    [Tooltip("Direction of gravity.")] [SerializeField] [ConditionalEnumHide("gravityType", (int)Gravity.Directional)] protected GravityAxis gravityAxis = null;
    [Tooltip("Area for gravity.")] [SerializeField] [ConditionalHide("limitedGravityArea", true)] protected DirectionalGravity directionalGravity = null;
    [Tooltip("Area for gravity.")] [SerializeField] [ConditionalHide("limitedGravityArea", true)] protected SphericalGravity sphericalGravity = null;

    [System.Serializable]
    protected class GravityAxis {
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

        if (gravityType == Gravity.Directional) {
            boxCollider.enabled = true;
            sphereCollider.enabled = false;
        }
        else {
            boxCollider.enabled = false;
            sphereCollider.enabled = true;
        }

        boxCollider.size = directionalGravity.gravitySize;
        sphereCollider.radius = sphericalGravity.gravityRadius;

        if (directionalGravity.gravitySize.x < 0) {
            directionalGravity.gravitySize.x = 0;
        }
        if (directionalGravity.gravitySize.y < 0) {
            directionalGravity.gravitySize.y = 0;
        }
        if (directionalGravity.gravitySize.z < 0) {
            directionalGravity.gravitySize.z = 0;
        }
        if (sphericalGravity.gravityRadius < 0) {
            sphericalGravity.gravityRadius = 0;
        }
    }

    // Start is called before the first frame update
    public void GravityStart() {
        CurrentGravity = gravityForce;
    }

    // Late Update is called at the end of each frame
    public virtual void LateUpdate() {
        if (!limitedGravityArea) {
            // Finds every object with "GravityBody"
            tempBodies = FindObjectsOfType<GravityBody>();
            foreach (GravityBody i in tempBodies) {
                i.AddManipulator(this);
            }
        }
    }

    // Simple use for those who don't want to override
    public void GravityUpdate() {
        if (!limitedGravityArea) {
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
        switch (gravityType) {
            case Gravity.Directional:
                // Apply gravitational force by axis
                if (gravityAxis.axis.HasFlag(ForceAxis.X)) {
                    ObjectBody.AddForce(ObjectTransform.right * -CurrentGravity);
                }
                if (gravityAxis.axis.HasFlag(ForceAxis.Y)) {
                    ObjectBody.AddForce(ObjectTransform.up * -CurrentGravity);
                }
                if (gravityAxis.axis.HasFlag(ForceAxis.Z)) {
                    ObjectBody.AddForce(ObjectTransform.forward * -CurrentGravity);
                }

                // Rotate object to align with the gravity source
                ObjectTransform.rotation = Quaternion.Slerp(ObjectTransform.rotation, transform.localRotation, rotationSpeed * Time.deltaTime);
                break;
            case Gravity.Spherical:
                // Determine the direction of the gravity and object "up" direction
                GravityDirection = (ObjectTransform.position - transform.position).normalized;
                ObjectUp = ObjectTransform.up;

                // Apply gravity
                ObjectBody.AddForce(GravityDirection * -CurrentGravity);

                // Rotate the object based on the position realative to the gravity source
                ObjectRotation = Quaternion.FromToRotation(ObjectUp, GravityDirection) * ObjectTransform.rotation;
                ObjectTransform.rotation = Quaternion.Slerp(ObjectTransform.rotation, ObjectRotation, rotationSpeed * Time.deltaTime);
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
        boxCollider.size = directionalGravity.gravitySize;
    }
    public void ChangeGravityRadius(float SphereRadius) {
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = sphericalGravity.gravityRadius;
    }
    #endregion

    public virtual void OnTriggerEnter(Collider obj) {
        obj.GetComponent<GravityBody>().AddManipulator(this);
    }
    public virtual void OnTriggerExit(Collider obj) {
        obj.GetComponent<GravityBody>().RemoveManipulator(this);
    }
}