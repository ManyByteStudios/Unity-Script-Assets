using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A scriptable object for the GuidedProjectile script.
/// Will create a projectile property for the script.
/// </summary>

[CreateAssetMenu(fileName = "Guided Projectile Properties", menuName = "Scriptable Objects/Guided Projectile")]
public class GuidedProjectileProperties : ScriptableObject {
    #region Editable values
    public enum GuidanceSystem { Controlled, Homming, Laser }
    public enum ForwardAxis { X_Axis, Y_Axis, Z_Axis }

    [Header("Projectile Properites")]
    [Tooltip("Player controlled or self guided projectile.")]
    [SerializeField] GuidanceSystem guidanceMethod = GuidanceSystem.Controlled;
    [Tooltip("Direction of the projectile (what is forward).")]
    [SerializeField] ForwardAxis forwardDirection = ForwardAxis.X_Axis;
    [Tooltip("Speed of the projectile's turn.")]
    [SerializeField][Range(0, 1)] float turnRate = 0.5f;
    [Space(5)]
    [SerializeField] bool setInitalVelocity = false;
    [Tooltip("Set the starting velocity.")]
    [SerializeField][ConditionalHide("setInitalVelocity", true)] float projectileVelocity = 0;
    [Tooltip("Force stop the guided projectile at start.")]
    [SerializeField] bool isTracking = false;

    [Header("Control Guided Values")]
    [Space(20)]
    [Tooltip("Used for input control.")]
    [SerializeField][ConditionalEnumHide("guidanceMethod", (int)GuidanceSystem.Controlled)] float inputSmoothing = 0;
    [Tooltip("Used for input control.")]
    [SerializeField][ConditionalEnumHide("guidanceMethod", (int)GuidanceSystem.Controlled)] float inputSensitivity = 0;

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
    #endregion

    #region Final Script Values
    public GuidanceSystem GuidanceMethod => guidanceMethod;
    public ForwardAxis ForwardDriection => forwardDirection;
    public float TurnRate => turnRate;
    public bool SetInitalVelocity => setInitalVelocity;
    public float ProjectileVelocity => projectileVelocity;
    public bool IsTracking => isTracking;
    public float InputSmoothing => inputSmoothing;
    public float InputSensitivity => inputSensitivity;
    #endregion
}