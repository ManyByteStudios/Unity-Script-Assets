using UnityEngine;
using ByteAttributes;

/// <summary>
/// This script manipulates the forces applied on any
/// game objects with the "GravityBody" script. The 
/// script will be dynamic enough to change both gravity
/// direction and force along with the effected area.
/// </summary>

[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SphereCollider))]
public class GravityManipulator : MonoBehaviour {
    #region Gravity Manipulator Values
    [Header("Gravity Manipulation Properties")]
    [Tooltip("Gravity manipulator scriptable object.")]
    [NotNullable] [SerializeField] private GravityProperties gravProperty = null;

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

        if (gravProperty.GravityType == GravityProperties.Gravity.Directional) {
            boxCollider.enabled = true;
            sphereCollider.enabled = false;
        }
        else {
            boxCollider.enabled = false;
            sphereCollider.enabled = true;
        }

        boxCollider.size = gravProperty.DirectionalGrav.gravitySize;
        sphereCollider.radius = gravProperty.SphericalGrav.gravityRadius;

        if (gravProperty.DirectionalGrav.gravitySize.x < 0) {
            gravProperty.DirectionalGrav.gravitySize.x = 0;
        }
        if (gravProperty.DirectionalGrav.gravitySize.y < 0) {
            gravProperty.DirectionalGrav.gravitySize.y = 0;
        }
        if (gravProperty.DirectionalGrav.gravitySize.z < 0) {
            gravProperty.DirectionalGrav.gravitySize.z = 0;
        }
        if (gravProperty.SphericalGrav.gravityRadius < 0) {
            gravProperty.SphericalGrav.gravityRadius = 0;
        }
    }

    // Start is called before the first frame update
    protected virtual void Start() {
        CurrentGravity = gravProperty.GravityForce;
    }

    // Late Update is called at the end of each frame
    protected virtual void LateUpdate() {
        if (!gravProperty.LimitedGravityArea) {
            // Finds every object with "GravityBody"
            tempBodies = FindObjectsOfType<GravityBody>();
            foreach (GravityBody i in tempBodies) {
                if (gravProperty.SelectiveObjects) {
                    for (int a = 0; a < gravProperty.ObjectTags.Count; a++) {
                        if (i.gameObject.CompareTag(gravProperty.ObjectTags[a].objectTag)) {
                            i.AddManipulator(this);
                        }
                    }
                }
                else {
                    i.AddManipulator(this);
                }
            }
        }
    }

    // Simple use for those who don't want to override
    protected void GravityUpdate() {
        if (!gravProperty.LimitedGravityArea) {
            // Finds every object with "GravityBody"
            tempBodies = FindObjectsOfType<GravityBody>();
            foreach (GravityBody i in tempBodies) {
                if (gravProperty.SelectiveObjects) {
                    for (int a = 0; a < gravProperty.ObjectTags.Count; a++) {
                        if (i.gameObject.CompareTag(gravProperty.ObjectTags[a].objectTag)) {
                            i.AddManipulator(this);
                        }
                    }
                }
                else {
                    i.AddManipulator(this);
                }
            }
        }
    }

    /// <summary>
    /// Applies the gravity force on objects with the GravBody script.
    /// </summary>
    /// <param name="ObjectBody"></param>
    /// <param name="ObjectTransform"></param>
    public void ApplyGravity(Rigidbody ObjectBody, Transform ObjectTransform) {
        switch (gravProperty.GravityType) {
            case GravityProperties.Gravity.Directional:
                // Apply gravitational force by axis
                if (gravProperty.GravAxis.axis.HasFlag(GravityProperties.ForceAxis.X)) {
                    ObjectBody.AddForce(ObjectTransform.right * -CurrentGravity);
                }
                if (gravProperty.GravAxis.axis.HasFlag(GravityProperties.ForceAxis.Y)) {
                    ObjectBody.AddForce(ObjectTransform.up * -CurrentGravity);
                }
                if (gravProperty.GravAxis.axis.HasFlag(GravityProperties.ForceAxis.Z)) {
                    ObjectBody.AddForce(ObjectTransform.forward * -CurrentGravity);
                }

                // Rotate object to align with the gravity source
                ObjectTransform.rotation = Quaternion.Slerp(ObjectTransform.rotation, transform.localRotation, gravProperty.RotationSpeed * Time.deltaTime);
                break;
            case GravityProperties.Gravity.Spherical:
                // Determine the direction of the gravity and object "up" direction
                GravityDirection = (ObjectTransform.localPosition - transform.localPosition).normalized;
                ObjectUp = ObjectTransform.up;

                // Apply gravity
                ObjectBody.AddForce(GravityDirection * -CurrentGravity);

                // Rotate the object based on the position realative to the gravity source
                ObjectRotation = Quaternion.FromToRotation(ObjectUp, GravityDirection) * ObjectTransform.rotation;
                ObjectTransform.rotation = Quaternion.Slerp(ObjectTransform.rotation, ObjectRotation, gravProperty.RotationSpeed * Time.deltaTime);
                break;
        }
    }

    #region Gravity Manipulator Adjustments
    protected void ChangeGravityForce(float NewGravForce = 0) {
        CurrentGravity = NewGravForce;
    }
    protected void AdjustGravityForce(float GravAdjustment = 0) {
        CurrentGravity += GravAdjustment;
    }
    protected void ChangeGravityRangeBox(Vector3 BoxSize) {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = gravProperty.DirectionalGrav.gravitySize;
    }
    protected void ChangeGravityRadius(float SphereRadius) {
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = gravProperty.SphericalGrav.gravityRadius;
    }
    #endregion

    #region Gravity manipulator Collision Triggers
    protected virtual void OnTriggerEnter(Collider obj) {
        if (gravProperty.SelectiveObjects) {
            for (int a = 0; a < gravProperty.ObjectTags.Count; a++) {
                if (obj.gameObject.CompareTag(gravProperty.ObjectTags[a].objectTag)) {
                    obj.GetComponent<GravityBody>().AddManipulator(this);
                }
            }
        }
        else {
            obj.GetComponent<GravityBody>().AddManipulator(this);
        }
    }
    protected virtual void OnTriggerExit(Collider obj) {
        obj.GetComponent<GravityBody>().RemoveManipulator(this);
    }
    #endregion
}