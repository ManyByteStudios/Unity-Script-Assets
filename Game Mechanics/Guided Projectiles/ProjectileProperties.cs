using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This script is a scriptable object for the guided projectile script.
    It contains all of the variables needed for the script.
*/

public enum GuidanceSystem { Controlled, Homming}
public enum ForwardAxis { X_Axis, Y_Axis, Z_Axis}
[CreateAssetMenu(fileName = "Projectile Properties", menuName = "Scriptable Objects/Projectile")]
public class ProjectileProperties : ScriptableObject {
    #region Editable Values
    [Header("Projectile Properites")]
    [SerializeField] GuidanceSystem guidanceMethod = GuidanceSystem.Controlled;
    [Tooltip("This is used to set which way the projectile is facing")] [SerializeField] ForwardAxis forwardDirection = ForwardAxis.X_Axis;
    [SerializeField] [Range(0, 1)] float turnRate = 0.5f;
    [Space(5)]
    [SerializeField] bool setInitalVelocity = false;
    [SerializeField] [ConditionalHide("setInitalVelocity", true)] float projectileVelocity = 0;
    [SerializeField] bool isTracking = false;

    [Header("Control Guided Values")] [Space(20)]
    [SerializeField] [ConditionalEnumHide("guidanceMethod", (int)GuidanceSystem.Controlled)] float inputSmoothing = 0;
    [SerializeField] [ConditionalEnumHide("guidanceMethod", (int)GuidanceSystem.Controlled)] float inputdSensitivity = 0;
    #endregion

    #region Final Values
    public GuidanceSystem GuidanceMethod => guidanceMethod;
    public ForwardAxis ForwardDirection => forwardDirection;
    public float TurnRate => turnRate;
    public bool SetInitalVelocity => setInitalVelocity;
    public float ProjectileVelocity => projectileVelocity;
    public bool IsTracking => isTracking;
    public float InputSmoothing => inputSmoothing;
    public float InputSensitivity => inputdSensitivity;
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
}