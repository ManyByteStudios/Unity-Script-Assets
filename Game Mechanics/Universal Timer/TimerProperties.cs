using UnityEngine;
using ByteAttributes;

/// <summary>
/// A scriptable object for the UniversalTimer script.
/// Will create the timer properties for the script.
/// </summary>

[CreateAssetMenu(fileName = "Timer Properties", menuName = "Scriptable Objects/Timer")]
public class TimerProperties : ScriptableObject {
    #region Editable Values
    public enum TimerFunction { CountingUp, CountingDown }
    public enum TimerOutputInclusion { Minutes, Seconds}

    [Header("Timer Properties")]
    [Tooltip("Causes the timer to pause or resume its process.")]
    bool timerIsPause = false;
    [Tooltip("Determines if the timer will count up or down.")]
    [SerializeField] TimerFunction function = TimerFunction.CountingUp;
    [Tooltip("What is the total amount of time the timer has to count down in seconds.")]
    [ConditionalEnumHide("function", (int)TimerFunction.CountingDown)][SerializeField] int allowedTime = 0;
    [Space(5)]
    [LineDivider(4, color: LineColors.Black)]
    [Tooltip("Accuracy of timer text.")]
    [SerializeField] TimerOutputInclusion outputInclusion = TimerOutputInclusion.Minutes;
    #endregion

    #region Final Script Values
    public bool TimerIsPause => timerIsPause;
    public TimerFunction Function => function;
    public int AllowedTime => allowedTime;
    public TimerOutputInclusion OutputInclusion => outputInclusion;
    #endregion
}