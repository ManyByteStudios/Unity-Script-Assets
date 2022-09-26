using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A scriptable object for the DroneController script.
/// Will create a loot table for the script.
/// </summary>

[CreateAssetMenu(fileName = "Drone Properties", menuName = "Scriptable Objects/Drone")]
public class DroneProperties : ScriptableObject {
    #region Editable Drone Values
    [Header("Drone Components")]
    [Tooltip("Enable uniform tilt.")]
    [SerializeField] protected bool uniformTilt = true;
    [Tooltip("Drone Parts that will be tilting.")]
    [SerializeField] protected TiltComponents tiltComponents = null;
    [Space(15)]
    [Header("Drone Movement")]
    [Tooltip("Drone movement speed and tilt for forward and strafe movement is the same.")]
    [SerializeField] bool uniformMovement = true;
    [Tooltip("Enable correction of drone's up force while moving.")]
    [SerializeField] bool correctUpForce = true;
    [Tooltip("Drone's movement speed for forward and strafe.")]
    [SerializeField][ConditionalHide("uniformMovement", true)] float movementSpeed = 0;
    [Tooltip("Drone's movement speed for forward movement.")]
    [SerializeField][ConditionalHide("NotUniform", true)] float forwardSpeed = 0;
    [Tooltip("Drone's movement speed for strafe movement.")]
    [SerializeField][ConditionalHide("NotUniform", true)] float strafeSpeed = 0;
    [Space(5)]
    [Tooltip("Acceleration speed of the drone 'x', and the deceleration speed of the the drone 'y'.")]
    [SerializeField][AbsoluteValue] Vector2 accelNDecelSpeed = Vector2.zero;
    [Space(5)]
    [Tooltip("Allow to change the elevation of the drone by input.")]
    [SerializeField] bool enableElevationChange = true;
    [Tooltip("The speed of the drone moving up and down, 'x' is the speed going up, and 'y' is the speed going down.")]
    [SerializeField][ConditionalHide("enableElevationChange", true)] Vector2 elevationSpeed = Vector2.zero;
    [Space(10)]
    [Tooltip("The amount the drone rotates left and right.")]
    [SerializeField][AbsoluteValue] float droneRotationAmount = 0f;
    [Tooltip("The speed of the drone rotation.")]
    [SerializeField][AbsoluteValue] private float droneRotationSpeed = 0f;
    [Space(10)]
    [Tooltip("Drone's movement tilt for all directional movement.")]
    [SerializeField][ConditionalHide("uniformMovement", true)] float tiltAmount = 0;
    [Tooltip("Speed of drone's tilt when moving.")]
    [SerializeField][ConditionalHide("uniformMovement", true)] float tiltSpeed = 0;
    [Tooltip("Drone's movement tilt for forward movement.")]
    [SerializeField][ConditionalHide("NotUniform", true)] float forwardTilt = 0;
    [Tooltip("Drone's movement tilt for strafe movement.")]
    [SerializeField][ConditionalHide("NotUniform", true)] float strafeTilt = 0;
    [Space(5)]
    [Tooltip("Speed of drone's tilt when moving forward.")]
    [SerializeField][ConditionalHide("NotUniform", true)] float forwardTiltSpeed = 0;
    [Tooltip("Speed of drone's tilt when strafing.")]
    [SerializeField][ConditionalHide("NotUniform", true)] float strafeTiltSpeed = 0;

    [System.Serializable]
    protected class TiltComponents {
        [SerializeField] public Transform[] forwardTiltComponents = null;
        [SerializeField] public Transform[] strafeTiltComponents = null;
    }

    [SerializeField][HideInInspector] private bool NotUniform = false;

    [ExecuteInEditMode]
    private void OnValidate() {
        NotUniform = !uniformMovement;

        // Ensure all movement values are positive
        if (movementSpeed < 0) {
            movementSpeed = 0;
        }
        if (forwardSpeed < 0) {
            forwardSpeed = 0;
        }
        if (strafeSpeed < 0) {
            strafeSpeed = 0;
        }
        if (elevationSpeed.x < 0) {
            elevationSpeed.x = 0;
        }
        if (elevationSpeed.y < 0) {
            elevationSpeed.y = 0;
        }

        if (droneRotationAmount < 0) {
            droneRotationAmount = 0;
        }
        if (droneRotationSpeed > 1) {
            droneRotationSpeed = 1;
        }

        if (tiltAmount < 0) {
            tiltAmount = 0;
        }
        if (forwardTilt < 0) {
            forwardTilt = 0;
        }
        if (strafeTilt < 0) {
            strafeTilt = 0;
        }

        if (tiltSpeed < 0) {
            tiltSpeed = 0;
        }
        else if (tiltSpeed > 1) {
            tiltSpeed = 1;
        }
        if (forwardTiltSpeed < 0) {
            forwardTiltSpeed = 0;
        }
        else if (forwardTiltSpeed > 1) {
            forwardTiltSpeed = 1;
        }
        if (strafeTiltSpeed < 0) {
            strafeTiltSpeed = 0;
        }
        else if (strafeTiltSpeed > 1) {
            strafeTiltSpeed = 1;
        }
    }
    #endregion

    #region Drone Script Values
    public bool UniformTilt => uniformTilt;
    public Transform[] ForwardTiltComponents => ForwardTiltComponents;
    public Transform[] StrafeTiltComponents => StrafeTiltComponents;

    public bool UniformMovement => uniformMovement;
    public bool CorrectUpForce => correctUpForce;
    public float MovementSpeed => movementSpeed;
    public float ForwardSpeed => forwardSpeed;
    public float StrafeSpeed => strafeSpeed;
    public Vector2 AccelNDecelSpeed => accelNDecelSpeed;

    public bool EnableElevationChange => enableElevationChange;
    public Vector2 ElevationSpeed => elevationSpeed;

    public float DroneRotationAmount => droneRotationAmount;
    public float DroneRotationSpeed => droneRotationSpeed;

    public float TiltAmount => TiltAmount;
    public float TiltSpeed => tiltSpeed;
    public float ForwardTilt => forwardTilt;
    public float StrafeTilt => strafeTilt;
    public float ForwardTiltSpeed => forwardTiltSpeed;
    public float StrafeTiltSpeed => StrafeTiltSpeed;
    #endregion
}