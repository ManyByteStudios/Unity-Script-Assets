using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A scriptable object for the UniversalTimer script.
/// Will create the timer properties for the script.
/// </summary>

[CreateAssetMenu(fileName = "Timer Properties", menuName = "Scriptable Objects/Timer")]
public class TimerProperties : ScriptableObject {
    #region Editable Values
    public enum TimerFunction { CountingUp, CountingDown }

    [Header("Timer Properties")]
    [Tooltip("Causes the timer to pause or resume its process.")]
    bool timerIsPause = false;
    [Tooltip("Determines if the timer will count up or down.")]
    [SerializeField] TimerFunction function = TimerFunction.CountingUp;
    [Tooltip("What is the total amount of time the timer has to count down in seconds.")]
    [ConditionalEnumHide("function", (int)TimerFunction.CountingDown)][SerializeField] int allowedTime = 0;
    #endregion

    #region Final Script Values
    public bool TimerIsPause => timerIsPause;
    public TimerFunction Function => function;
    public int AllowedTime => allowedTime;
    #endregion
}