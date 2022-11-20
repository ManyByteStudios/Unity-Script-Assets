using UnityEngine;
using ByteAttributes;

/// <summary>
/// This is a universal timer script, the script allows for the
/// change in timer function, reset, pause, and unpause.
/// </summary>

public class UniversalTimer : MonoBehaviour {
    #region Timer Properties
    [Header("Timer Properties")]
    [Tooltip("Timer scriptable object.")]
    [NotNullable] [SerializeField] TimerProperties timerProperty;

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
    /// <summary>
    /// Core timer function.
    /// </summary>
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

    /// <summary>
    /// Pause and Unpause timer.
    /// </summary>
    protected void PauseUnpauseTimer() {
        isRunning = !timerProperty.TimerIsPause;
    }

    /// <summary>
    /// Reset timer based on the current function.
    /// </summary>
    /// <param name="TotalTime"></param>
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

    /// <summary>
    /// Get the text for the timer.
    /// </summary>
    /// <returns></returns>
    protected string TimerText() {
        string ReturnText = null;
        switch (timerProperty.OutputInclusion) {
            case TimerProperties.TimerOutputInclusion.Minutes:
                ReturnText = Minutes;
                break;
            case TimerProperties.TimerOutputInclusion.Seconds:
                ReturnText = Minutes + ":" + Seconds;
                break;
        }
        
        return ReturnText;
    }
    #endregion

    #region Changing Function
    /// <summary>
    /// Swap timer function.
    /// </summary>
    /// <param name="TotalTime"></param>
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

    /// <summary>
    /// Change timer function to count up.
    /// </summary>
    protected void SetTimerCountUp() {
        isRunning = true;
        CurrentFunction = TimerProperties.TimerFunction.CountingUp;
        CurrentTime = 0;

        Minutes = ((int)CurrentTime / 60).ToString();
        Seconds = (CurrentTime % 60).ToString("f2");
    }

    /// <summary>
    /// Change timer function to count down.
    /// </summary>
    protected void SetTimerCountDown(int TotalTime = 0) {
        isRunning = true;
        CurrentFunction = TimerProperties.TimerFunction.CountingDown;
        CurrentTime = TotalTime;

        Minutes = ((int)CurrentTime / 60).ToString();
        Seconds = (CurrentTime % 60).ToString("f2");
    }
    #endregion
}