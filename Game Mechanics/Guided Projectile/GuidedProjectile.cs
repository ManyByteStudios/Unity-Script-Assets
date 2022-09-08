using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a simple script that allows any projectile based object
/// to be guided manually to a destination, or even have it set as
/// a homing missle.

/// If you wish to use it as a laser guided projectile, you can 
/// use a ray cast and set the target as a new transform when the ray
/// cast hits something or use the position of the ray cast as a way to 
/// direct the projectile.
/// </summary>

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class GuidedProjectile : MonoBehaviour {
    #region Variables
    private enum GuidanceSystem { Controlled, Homming }
    private enum ForwardAxis { X_Axis, Y_Axis, Z_Axis }

    [Header("Projectile Properites")]
    [Tooltip("Player controlled or self guided projectile.")]
    [SerializeField] private GuidanceSystem guidanceMethod = GuidanceSystem.Controlled;
    [Tooltip("Direction of the projectile (what is forward).")]
    [SerializeField] private ForwardAxis forwardDirection = ForwardAxis.X_Axis;
    [Tooltip("Speed of the projectile's turn.")]
    [SerializeField] [Range(0, 1)] private float turnRate = 0.5f;
    [Space(5)]
    [SerializeField] private bool setInitalVelocity = false;
    [Tooltip("Set the starting velocity.")]
    [SerializeField] [ConditionalHide("setInitalVelocity", true)] private float projectileVelocity = 0;
    [Tooltip("Force stop the guided projectile at start.")]
    [SerializeField] private bool isTracking = false;

    [Header("Control Guided Values")]
    [Space(20)]
    [Tooltip("Used for input control.")]
    [SerializeField] [ConditionalEnumHide("guidanceMethod", (int)GuidanceSystem.Controlled)] private float inputSmoothing = 0;
    [Tooltip("Used for input control.")]
    [SerializeField] [ConditionalEnumHide("guidanceMethod", (int)GuidanceSystem.Controlled)] private float inputdSensitivity = 0;

    Rigidbody ProjectileBody;
    GameObject TargetObj;
    bool IsTracking;

    Vector2 DefaultVector = Vector2.zero;
    Vector3 ControlledDirection, SmoothedVector;
    float InitalVelocity;
    #endregion

    [ExecuteInEditMode]
    private void OnValidate() {
        if (projectileVelocity < 0) {
            projectileVelocity = 0;
        }
        if (inputSmoothing < 0) {
            inputSmoothing = 0;
        }
        if (inputdSensitivity < 0) {
            inputdSensitivity = 0;
        }
    }

    // Main meathod of the script, should be used during fixed update
    protected void ControlProjectile(Vector2 InputVector = default(Vector2)) {
        IsTracking = isTracking;
        if (setInitalVelocity) {
            InitalVelocity = projectileVelocity;
        }
        else {
            InitalVelocity = ProjectileBody.velocity.magnitude;
        }

        // Ensure that all the referances to the body is there
        if (ProjectileBody == null) {
            ProjectileBody = GetComponent<Rigidbody>();
            ProjectileBody.useGravity = false;
        }
        else {
            if (IsTracking) {
                switch (guidanceMethod) {
                    case GuidanceSystem.Homming:
                        Quaternion TargetDirection = Quaternion.LookRotation(TargetObj.transform.position - this.transform.position);

                        ApplyNewDirection(TargetDirection);
                        break;
                    case GuidanceSystem.Controlled:
                        var NewDirection = new Vector2(-InputVector.y, InputVector.x);

                        NewDirection = Vector2.Scale(NewDirection, new Vector2(inputdSensitivity * inputSmoothing, inputdSensitivity * inputSmoothing));
                        SmoothedVector.x = Mathf.Lerp(SmoothedVector.x, NewDirection.x, 1f / inputSmoothing);
                        SmoothedVector.y = Mathf.Lerp(SmoothedVector.y, NewDirection.y, 1f / inputSmoothing);
                        ControlledDirection += SmoothedVector;

                        ApplyNewDirection(Quaternion.Euler(ControlledDirection));
                        break;
                }
            }
        }
    }

    // Applies the change in position or direction by slowly rotating the projectile while moving "forward"
    void ApplyNewDirection(Quaternion TargetRotation) {
        ProjectileBody.MoveRotation(Quaternion.RotateTowards(transform.rotation, TargetRotation, turnRate));

        switch (forwardDirection) {
            case ForwardAxis.X_Axis:
                ProjectileBody.velocity = transform.right * InitalVelocity;
                break;
            case ForwardAxis.Y_Axis:
                ProjectileBody.velocity = transform.up * InitalVelocity;
                break;
            case ForwardAxis.Z_Axis:
                ProjectileBody.velocity = transform.forward * InitalVelocity;
                break;
        }
    }

    // Change target for the projectile
    protected void SetTarget(GameObject NewTarget = null) {
        TargetObj = NewTarget;
    }

    // Enables or disables tracking of the projectile
    protected void EnableTracking() {
        IsTracking = !IsTracking;
    }
}