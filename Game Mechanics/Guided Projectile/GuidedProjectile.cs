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
    private enum GuidanceSystem { Controlled, Homming, Laser }
    private enum ForwardAxis { X_Axis, Y_Axis, Z_Axis }

    #region Variables
    [Header("Guided Projectile Properties")]
    [Tooltip("Player controlled or self guided projectile.")]
    [SerializeField] GuidanceSystem guidanceMethod = GuidanceSystem.Controlled;
    [Tooltip("Direction of the projectile (what is forward).")]
    [SerializeField] ForwardAxis forwardDirection = ForwardAxis.X_Axis;
    [Tooltip("Speed of the projectile's turn.")]
    [SerializeField] [Range(0, 1)] float turnRate = 0.5f;
    [Space(5)]
    [SerializeField] bool setInitalVelocity = false;
    [Tooltip("Set the starting velocity.")]
    [SerializeField] [ConditionalHide("setInitalVelocity", true)] float projectileVelocity = 0;
    [Tooltip("Force stop the guided projectile at start.")]
    [SerializeField] bool isTracking = false;
    [Space(5)]
    [LineDivider(4, color: LineColors.Black)]
    [Header("Control Guided Values")]
    [Tooltip("Used for input control.")]
    [SerializeField] [ConditionalEnumHide("guidanceMethod", (int)GuidanceSystem.Controlled)] float inputSmoothing = 0;
    [Tooltip("Used for input control.")]
    [SerializeField] [ConditionalEnumHide("guidanceMethod", (int)GuidanceSystem.Controlled)] float inputSensitivity = 0;

    [ExecuteInEditMode]
    private void OnValidate() {
        if (projectileVelocity < 0) {
            projectileVelocity = 0;
        }
        if (inputSmoothing < 0) {
            inputSmoothing = 0;
        }
        if (inputSensitivity < 0) {
            inputSensitivity = 0;
        }
    }


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
    public void ControlProjectile(Vector2 InputVector = default(Vector2)) {
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
                        Quaternion HomingDirection = Quaternion.LookRotation(TargetObj.transform.position - this.transform.position);

                        ApplyNewDirection(HomingDirection);
                        break;
                    case GuidanceSystem.Controlled:
                        var NewDirection = new Vector2(-InputVector.y, InputVector.x);

                        NewDirection = Vector2.Scale(NewDirection, new Vector2(inputSensitivity * inputSmoothing, inputSensitivity * inputSmoothing));
                        SmoothedVector.x = Mathf.Lerp(SmoothedVector.x, NewDirection.x, 1f / inputSmoothing);
                        SmoothedVector.y = Mathf.Lerp(SmoothedVector.y, NewDirection.y, 1f / inputSmoothing);
                        ControlledDirection += SmoothedVector;

                        ApplyNewDirection(Quaternion.Euler(ControlledDirection));
                        break;
                    case GuidanceSystem.Laser:
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

    #region Getters and Setters
    /// <summary>
    /// Change the input smoothing.
    /// </summary>
    /// <param name="NewInputSmoothing"></param>
    public void SetInputSmoothing(float NewInputSmoothing) {
        inputSmoothing = NewInputSmoothing;
    }
    /// <summary>
    /// Get the input smoothing value.
    /// </summary>
    /// <returns></returns>
    public float GetInputSmoothing() {
        return inputSmoothing;
    }
    /// <summary>
    /// Change the input sensitivity.
    /// </summary>
    public void SetInputSensitivity(float NewInputSensitivity) {
        inputSensitivity = NewInputSensitivity;
    }
    /// <summary>
    /// Get the input sensitivity value.
    /// </summary>
    /// <returns></returns>
    public float GetInputSensitivity() {
        return inputSensitivity;
    }
    /// <summary>
    /// Change the turn speed.
    /// </summary>
    public void SetTurnSpeed(float NewTurnSpeed) {
        turnRate = NewTurnSpeed;
    }
    /// <summary>
    /// Get the turn rate for the projectile.
    /// </summary>
    /// <returns></returns>
    public float GetTurnSpeed() {
        return turnRate;
    }
    /// <summary>
    /// Change the projectiles velocity.
    /// </summary>
    /// <param name="VelocityChange"></param>
    public void ChangeInitalVelocity(float VelocityChange) {
        float Speed = ProjectileBody.velocity.magnitude;
        Speed += VelocityChange;

        switch (forwardDirection) {
            case ForwardAxis.X_Axis:
                ProjectileBody.velocity = transform.right * Speed;
                break;
            case ForwardAxis.Y_Axis:
                ProjectileBody.velocity = transform.up * Speed;
                break;
            case ForwardAxis.Z_Axis:
                ProjectileBody.velocity = transform.forward * Speed;
                break;
        }
    }
    /// <summary>
    /// Get the projectile's speed.
    /// </summary>
    /// <returns></returns>
    public float GetVelocity() {
        return ProjectileBody.velocity.magnitude;
    }
    


    /// <summary>
    /// Set target for projectile
    /// </summary>
    /// <param name="NewTarget"></param>
    public void SetTarget(GameObject NewTarget = null) {
        TargetObj = NewTarget;
    }
    /// <summary>
    /// Returns to targeted object
    /// </summary>
    /// <returns></returns>
    public GameObject GetTarget() {
        return TargetObj;
    }

    /// <summary>
    /// Set position to target for projectile
    /// </summary>
    /// <param name="TargetPos"></param>
    public void SetTargetPosition(Vector3 TargetPos) {
        LaserTarget = TargetPos;
    }
    /// <summary>
    /// Returns a vector 3 position for laser targeting
    /// </summary>
    /// <returns></returns>
    public Vector3 GetTargetedPosition() {
        return LaserTarget;
    }

    /// <summary>
    /// Enable or disable the homing function.
    /// </summary>
    public void EnableTracking() {
        IsTracking = !IsTracking;
    }
    #endregion
}