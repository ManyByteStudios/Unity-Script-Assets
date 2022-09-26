using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a universal timer script, the script allows for the
/// change in timer function, reset, pause, and unpause.
/// </summary>

public class UniversalTimer : MonoBehaviour {
    #region Timer Properties
    [Header("Timer Properties")]
    [Tooltip("Timer scriptable object.")]
    [SerializeField] private TimerProperties timerProperty;

    TimerProperties.TimerFunction CurrentFunction;
    bool isRunning = false;
    float CurrentTime = 0;
    string Minutes;
    string Seconds;

    [ExecuteInEditMode]

    private void OnValidate() {
        CurrentFunction = timerProperty.Function;
    }
    #endregion

    #region Core Timer Functions
    // Timer function that can be used in other class in the update method
    protected void RunTimer() {
        if (!timerProperty.TimerIsPause) {
            switch (timerProperty.Function) {
                case TimerProperties.TimerFunction.CountingUp:
                    CurrentTime += Time.deltaTime;

                    Minutes = ((int)CurrentTime / 60).ToString();
                    Seconds = (CurrentTime % 60).ToString("f2");
                    break;
                case TimerProperties.TimerFunction.CountingDown:
                    CurrentTime -= Time.deltaTime;

                    if (CurrentTime <= 0) {
                        CurrentTime = 0;
                    }

                    Minutes = ((int)CurrentTime / 60).ToString();
                    Seconds = (CurrentTime % 60).ToString("f2");
                    break;
            }
        }
    }

    // A simple way to pause and unpause the timer script
    protected void PauseUnpauseTimer() {
        isRunning = !timerProperty.TimerIsPause;
    }

    // Resetting the timer based on the current function, the TotalTime variable is only required for the CountingDown function
    protected void ResetTimer(int TotalTime = 0) {
        isRunning = true;

        switch (timerProperty.Function) {
            case TimerProperties.TimerFunction.CountingUp:
                CurrentTime = 0;

                Minutes = ((int)CurrentTime / 60).ToString();
                Seconds = (CurrentTime % 60).ToString("f2");
                break;
            case TimerProperties.TimerFunction.CountingDown:
                if (TotalTime > 0) {
                    CurrentTime = TotalTime;
                }
                else {
                    CurrentTime = timerProperty.AllowedTime;
                }

                Minutes = ((int)CurrentTime / 60).ToString();
                Seconds = (CurrentTime % 60).ToString("f2");
                break;
        }
    }

    // Method to get the timer string
    protected string TimerText() {
        string ReturnText = Minutes + ":" + Seconds;
        return ReturnText;
    }
    #endregion

    #region Changing Function
    // Alternates between the various timer function
    protected void SwitchTimerFunction(int TotalTime = 0) {
        isRunning = true;

        switch (CurrentFunction) {
            case TimerProperties.TimerFunction.CountingUp:
                CurrentFunction = TimerProperties.TimerFunction.CountingDown;

                CurrentTime = TotalTime;

                Minutes = ((int)CurrentTime / 60).ToString();
                Seconds = (CurrentTime % 60).ToString("f2");
                break;
            case TimerProperties.TimerFunction.CountingDown:
                CurrentFunction = TimerProperties.TimerFunction.CountingUp;

                CurrentTime = 0;

                Minutes = ((int)CurrentTime / 60).ToString();
                Seconds = (CurrentTime % 60).ToString("f2");
                break;
        }
    }

    // Setter functions to change to the specific timer function
    protected void SetTimerCountUp() {
        isRunning = true;
        CurrentFunction = TimerProperties.TimerFunction.CountingUp;
        CurrentTime = 0;

        Minutes = ((int)CurrentTime / 60).ToString();
        Seconds = (CurrentTime % 60).ToString("f2");
    }
    protected void SetTimerCountDown(int TotalTime = 0) {
        isRunning = true;
        CurrentFunction = TimerProperties.TimerFunction.CountingDown;
        CurrentTime = TotalTime;

        Minutes = ((int)CurrentTime / 60).ToString();
        Seconds = (CurrentTime % 60).ToString("f2");
    }
    #endregion
}