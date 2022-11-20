using UnityEngine;
using ByteAttributes;

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
    [NotNullable] [SerializeField] private GuidedProjectileProperties guidedProjectileProperty = null;

    Rigidbody ProjectileBody;
    GameObject TargetObj;
    Vector3 LaserTarget;
    bool IsTracking;

    Vector2 DefaultVector = Vector2.zero;
    Vector3 ControlledDirection, SmoothedVector;
    float InitalVelocity;
    #endregion

    /// <summary>
    /// This must be called in the fixed update method and is the core function of the script.
    /// </summary>
    /// <param name="InputVector"></param>
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

    /// <summary>
    /// Appiles the the forward movement for based on the object's forward direction.
    /// </summary>
    /// <param name="TargetRotation"></param>
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

    /// <summary>
    /// Set target for projectile
    /// </summary>
    /// <param name="NewTarget"></param>
    protected void SetTarget(GameObject NewTarget = null) {
        TargetObj = NewTarget;
    }

    /// <summary>
    /// Set position to target for projectile
    /// </summary>
    /// <param name="TargetPos"></param>
    protected void SetTargetPosition(Vector3 TargetPos) {
        LaserTarget = TargetPos;
    }

    /// <summary>
    /// Enable or disable the homing function.
    /// </summary>
    protected void EnableTracking() {
        IsTracking = !IsTracking;
    }
}