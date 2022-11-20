using UnityEngine;
using ByteAttributes;

/// <summary>
/// This script allows for an object to have
/// the movement and chacteristics of a drone.
/// Taking in input, the script will manipulate
/// position, rotation and even audio, similar
/// to a flying drone. Any method that is called
/// must be used in the FixedUpdate method.
/// </summary>

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class DroneController : MonoBehaviour {
    [Header("Drone Controller Properties")]
    [Tooltip("Drone scriptable object.")]
    [NotNullable] [SerializeField] private DroneProperties droneProperty = null;
    
    //Private values
    Rigidbody DroneBody;
    AudioSource DroneSound;
    float SplitSpeedLimit;
    float E_Input, F_Input, S_Input, R_Input;
    float UpForce;
    float ForwardTilt, StrafeTilt;
    float TiltRefForward, TiltRefStrafe;
    float DroneRotation;
    float DroneRotationRef;
    float DesiredYRotation;

    // Awake is called when the script instance is being loaded
    void Awake() {
        DroneBody = GetComponent<Rigidbody>();
        DroneSound = GetComponent<AudioSource>();

        SplitSpeedLimit = new Vector2(droneProperty.ForwardSpeed, droneProperty.StrafeSpeed).magnitude;
    }

    // Change drone elevation, must be called during FixedUpdate
    protected void ElevationChange(float ElevationInput) {
        // Up force correction during movement
        if (droneProperty.CorrectUpForce) {
            if (Mathf.Abs(F_Input) != 0 || Mathf.Abs(S_Input) != 0) {
                if (E_Input != 0) {
                    DroneBody.velocity = DroneBody.velocity;
                }
                else {
                    DroneBody.velocity = new Vector3(DroneBody.velocity.x, -DroneBody.velocity.y, DroneBody.velocity.z);
                }

            }
        }

        // Changing drone elevation
        if (droneProperty.EnableElevationChange) {
            E_Input = Mathf.Clamp(ElevationInput, -1, 1);

            if (E_Input != 0) {
                if (E_Input > 0) {
                    UpForce = droneProperty.ElevationSpeed.x * DroneBody.mass;

                    // Go up, upwards speed will be faster than going down
                    DroneBody.AddRelativeForce(Vector3.up * UpForce * (E_Input * 2));
                }
                else {
                    UpForce = droneProperty.ElevationSpeed.y * DroneBody.mass;

                    // Go down, downwards speed will be slower that going up
                    DroneBody.AddRelativeForce(Vector3.up * UpForce * (E_Input / 4));
                }
            }
            else {
                // Hover
                DroneBody.AddRelativeForce(Vector3.up * Vector3.Magnitude(Physics.gravity) * DroneBody.mass);
            }
        }

        DroneAudio();
    }

    // Drone forward movement, must be called during FixedUpdate
    protected void ForwardMovement(float ForwardInput) {
        F_Input = Mathf.Clamp(ForwardInput, -1, 1);

        // Movement and tilt calculation
        switch (droneProperty.UniformMovement) {
            case true:
                DroneBody.AddRelativeForce(Vector3.forward * F_Input * droneProperty.MovementSpeed);
                ForwardTilt = Mathf.SmoothDamp(ForwardTilt, droneProperty.TiltAmount * F_Input, ref TiltRefForward, droneProperty.TiltSpeed);
                break;

            case false:
                DroneBody.AddRelativeForce(Vector3.forward * F_Input * droneProperty.ForwardSpeed);
                ForwardTilt = Mathf.SmoothDamp(ForwardTilt, droneProperty.ForwardTilt * F_Input, ref TiltRefForward, droneProperty.ForwardTiltSpeed);
                break;
        }

        DroneTilt();
        SpeedClamp();
        DroneAudio();
    }

    // Drone strafe movement, must be called during FixedUpdate
    protected void StrafeMovement(float StrafeInput) {
        S_Input = Mathf.Clamp(StrafeInput, -1, 1);

        // Movement and tilt calculation
        switch (droneProperty.UniformMovement) {
            case true:
                DroneBody.AddRelativeForce(Vector3.right * S_Input * droneProperty.MovementSpeed);
                StrafeTilt = Mathf.SmoothDamp(StrafeTilt, droneProperty.TiltAmount * -S_Input, ref TiltRefStrafe, droneProperty.TiltSpeed);
                break;

            case false:
                DroneBody.AddRelativeForce(Vector3.right * S_Input * droneProperty.StrafeSpeed);
                StrafeTilt = Mathf.SmoothDamp(StrafeTilt, droneProperty.StrafeTilt * -S_Input, ref TiltRefStrafe, droneProperty.StrafeTiltSpeed);
                break;
        }

        DroneTilt();
        SpeedClamp();
        DroneAudio();
    }

    // Drone Rotation
    protected void RotateDrone(float RotateInput) {
        R_Input = Mathf.Clamp(RotateInput, -1, 1);

        if (R_Input != 0) {
            DesiredYRotation +=(droneProperty.DroneRotationAmount * R_Input);
        }

        DroneRotation = Mathf.SmoothDamp(DroneRotation, DesiredYRotation, ref DroneRotationRef, droneProperty.DroneRotationSpeed);
    }

    // Drone tilt and rotation depending on input
    private void DroneTilt() {
        switch (droneProperty.UniformTilt) {
            case true:
                DroneBody.rotation = Quaternion.Euler(ForwardTilt, DroneRotation, StrafeTilt);
                break;

            case false:
                DroneBody.rotation = Quaternion.Euler(DroneBody.rotation.x, DroneRotation, DroneBody.rotation.z);

                foreach (Transform F_Tilit in droneProperty.ForwardTiltComponents) {
                    F_Tilit.localRotation = Quaternion.Euler(ForwardTilt, F_Tilit.rotation.y, F_Tilit.rotation.z);
                }

                foreach (Transform S_Tilit in droneProperty.StrafeTiltComponents) {
                    S_Tilit.localRotation = Quaternion.Euler(S_Tilit.rotation.x, S_Tilit.rotation.y, StrafeTilt);
                }
                break;
        }
    }

    // Limit speed of drone
    private void SpeedClamp() {
        switch (droneProperty.UniformMovement) {
            case true:
                if (Mathf.Abs(F_Input) != 0 && Mathf.Abs(S_Input) != 0) {
                    DroneBody.velocity = Vector3.ClampMagnitude(DroneBody.velocity, Mathf.Lerp(DroneBody.velocity.magnitude, droneProperty.MovementSpeed, Time.deltaTime * droneProperty.AccelNDecelSpeed.x));
                }
                else if (Mathf.Abs(F_Input) != 0 && Mathf.Abs(S_Input) < 0.2) {
                    DroneBody.velocity = Vector3.ClampMagnitude(DroneBody.velocity, Mathf.Lerp(DroneBody.velocity.magnitude, droneProperty.MovementSpeed, Time.deltaTime * droneProperty.AccelNDecelSpeed.x));
                }
                else if (Mathf.Abs(F_Input) == 0 && Mathf.Abs(S_Input) != 0) {
                    DroneBody.velocity = Vector3.ClampMagnitude(DroneBody.velocity, Mathf.Lerp(DroneBody.velocity.magnitude, droneProperty.MovementSpeed, Time.deltaTime * droneProperty.AccelNDecelSpeed.x));
                }
                else{
                    DroneBody.velocity = Vector3.ClampMagnitude(DroneBody.velocity, Mathf.Lerp(DroneBody.velocity.magnitude, 0, Time.deltaTime * droneProperty.AccelNDecelSpeed.y));
                }
                break;

            case false:
                if (Mathf.Abs(F_Input) != 0 && Mathf.Abs(S_Input) != 0) {
                    DroneBody.velocity = Vector3.ClampMagnitude(DroneBody.velocity, Mathf.Lerp(DroneBody.velocity.magnitude, SplitSpeedLimit, Time.deltaTime * droneProperty.AccelNDecelSpeed.x));
                }
                else if (Mathf.Abs(F_Input) != 0 && Mathf.Abs(S_Input) == 0) {
                    DroneBody.velocity = Vector3.ClampMagnitude(DroneBody.velocity, Mathf.Lerp(DroneBody.velocity.magnitude, droneProperty.ForwardSpeed, Time.deltaTime * droneProperty.AccelNDecelSpeed.x));
                }
                else if (Mathf.Abs(F_Input) == 0 && Mathf.Abs(S_Input) != 0) {
                    DroneBody.velocity = Vector3.ClampMagnitude(DroneBody.velocity, Mathf.Lerp(DroneBody.velocity.magnitude, droneProperty.StrafeSpeed, Time.deltaTime * droneProperty.AccelNDecelSpeed.x));
                }
                else {
                    DroneBody.velocity = Vector3.ClampMagnitude(DroneBody.velocity, Mathf.Lerp(DroneBody.velocity.magnitude, 0, Time.deltaTime * droneProperty.AccelNDecelSpeed.y));
                }
                break;
        }
    }

    // Audio during movement
    private void DroneAudio() {
        DroneSound.pitch = 1 + (DroneBody.velocity.magnitude / 100);
    }
}