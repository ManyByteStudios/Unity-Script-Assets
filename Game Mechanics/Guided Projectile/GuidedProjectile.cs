using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a simple script that allows any projectile based object
/// to be guided manually to a destination, set as a homing missle,
/// or even direct it manually with a laser.
/// </summary>

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class GuidedProjectile : MonoBehaviour {
    #region Variables
    [Header("Guided Projectile Properties")]
    [Tooltip("Guided projectile scriptable object.")]
    [SerializeField] private GuidedProjectileProperties guidedProjectileProperty = null;

    Rigidbody ProjectileBody;
    GameObject TargetObj;
    Vector3 LaserTarget;
    bool IsTracking;

    Vector2 DefaultVector = Vector2.zero;
    Vector3 ControlledDirection, SmoothedVector;
    float InitalVelocity;
    #endregion

    // Main meathod of the script, should be used during fixed update
    protected void ControlProjectile(Vector2 InputVector = default(Vector2)) {
        IsTracking = guidedProjectileProperty.IsTracking;
        if (guidedProjectileProperty.SetInitalVelocity) {
            InitalVelocity = guidedProjectileProperty.ProjectileVelocity;
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
                switch (guidedProjectileProperty.GuidanceMethod) {
                    case GuidedProjectileProperties.GuidanceSystem.Homming:
                        Quaternion HomingDirection = Quaternion.LookRotation(TargetObj.transform.position - this.transform.position);

                        ApplyNewDirection(HomingDirection);
                        break;
                    case GuidedProjectileProperties.GuidanceSystem.Controlled:
                        var NewDirection = new Vector2(-InputVector.y, InputVector.x);

                        NewDirection = Vector2.Scale(NewDirection, new Vector2(guidedProjectileProperty.InputSensitivity * guidedProjectileProperty.InputSmoothing, guidedProjectileProperty.InputSensitivity * guidedProjectileProperty.InputSmoothing));
                        SmoothedVector.x = Mathf.Lerp(SmoothedVector.x, NewDirection.x, 1f / guidedProjectileProperty.InputSmoothing);
                        SmoothedVector.y = Mathf.Lerp(SmoothedVector.y, NewDirection.y, 1f / guidedProjectileProperty.InputSmoothing);
                        ControlledDirection += SmoothedVector;

                        ApplyNewDirection(Quaternion.Euler(ControlledDirection));
                        break;
                    case GuidedProjectileProperties.GuidanceSystem.Laser:
                        Quaternion LaserDirection = Quaternion.LookRotation(LaserTarget - this.transform.position);

                        ApplyNewDirection(LaserDirection);
                        break;
                }
            }
        }
    }

    // Applies the change in position or direction by slowly rotating the projectile while moving "forward"
    void ApplyNewDirection(Quaternion TargetRotation) {
        ProjectileBody.MoveRotation(Quaternion.RotateTowards(transform.rotation, TargetRotation, guidedProjectileProperty.TurnRate));

        switch (guidedProjectileProperty.ForwardDriection) {
            case GuidedProjectileProperties.ForwardAxis.X_Axis:
                ProjectileBody.velocity = transform.right * InitalVelocity;
                break;
            case GuidedProjectileProperties.ForwardAxis.Y_Axis:
                ProjectileBody.velocity = transform.up * InitalVelocity;
                break;
            case GuidedProjectileProperties.ForwardAxis.Z_Axis:
                ProjectileBody.velocity = transform.forward * InitalVelocity;
                break;
        }
    }

    // Change target for the projectile
    protected void SetTarget(GameObject NewTarget = null) {
        TargetObj = NewTarget;
    }

    // Set target location
    protected void SetTargetPosition(Vector3 TargetPos) {
        LaserTarget = TargetPos;
    }

    // Enables or disables tracking of the projectile
    protected void EnableTracking() {
        IsTracking = !IsTracking;
    }
}