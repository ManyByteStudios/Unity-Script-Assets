using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

/// <summary>
/// A scriptable object for the UniversalUIBar script.
/// Will create the UI bar properties for the script.
/// </summary>

[CreateAssetMenu(fileName = "UI Bar Properties", menuName = "Scriptable Objects/UI Bar")]
public class UI_BarProperties : ScriptableObject {
    #region Universal UI Bar Enums
    public enum FillMethod { Horizontal, Vertical, Radial360 }
    public enum H_FillOrigin { Right, Left }
    public enum V_FillOrigin { Bottom, Top }
    public enum R_FillOrigin { Bottom, Right, Top, Left }
    #endregion

    #region Editable Values
    [Header("UI Bar Properties")]
    [Tooltip("Image sprite.")]
    [SerializeField] Sprite imageSprite = null;

    [Space(10)]
    [Tooltip("Slowly fill the UI bar.")]
    [SerializeField] bool smoothFillBar = false;
    [Tooltip("Fill speed of the UI bar.")]
    [ConditionalHide("SmoothFillBar", true)][SerializeField] float fillSpeed = 0;
    [Tooltip("Set the start fill amount.")]
    [Range(0, 1)][SerializeField] float startFillPercent = 0.5f;

    [Space(10)]
    [Tooltip("Fill origion from the center.")]
    [SerializeField] bool fillFromCenter = false;
    [Tooltip("Flip the fill direction.")]
    [ConditionalHide("fillFromCenter", true)][SerializeField] bool flipFillDirection = false;
    [Tooltip("Have a custom radial UI amount")]
    [ConditionalEnumHide("fillType", (int)FillMethod.Radial360)][SerializeField] bool customRadialUI = false;
    [Tooltip("Set the custom radial amount for the UI.")]
    [ConditionalHide("customRadialUI", true)][SerializeField][Range(0, 360)] int customRadial = 360;

    [Space(5)]
    [Tooltip("Vertical fill bar or horizontal fill bar.")]
    [SerializeField] FillMethod fillType = FillMethod.Horizontal;
    [Tooltip("Fill direction of the UI image.")]
    [ShowIf("fillType", (int)FillMethod.Vertical)][SerializeField] V_FillOrigin verticalFillOrigin = V_FillOrigin.Bottom;
    [Tooltip("Fill direction of the UI image.")]
    [ShowIf("fillType", (int)FillMethod.Horizontal)][SerializeField] H_FillOrigin horizontalFillOrigin = H_FillOrigin.Left;
    [Tooltip("Fill direction of the UI image.")]
    [ShowIf("fillType", (int)FillMethod.Radial360)][SerializeField] R_FillOrigin radialFillOrigin = R_FillOrigin.Bottom;

    [Space(10)]
    [Tooltip("Allow for over fill.")]
    [SerializeField] bool canOverFill = false;
    [ShowIf("canOverFill", true)][SerializeField] Color overFillColor = Color.blue;

    [Space(5)]
    [Tooltip("Range for positive color on UI bar.")]
    [SerializeField][MinMaxRange(0, 1)] Vector2 positiveRange = new Vector2(0.75f, 1f);
    [Tooltip("Color of UI bar when high.")]
    [SerializeField] Color positiveColor = Color.green;

    [Space(5)]
    [Tooltip("Range for negative color on UI bar.")]
    [SerializeField][MinMaxRange(0, 1)] Vector2 negativeRange = new Vector2(0f, 0.25f);
    [Tooltip("Color of UI bar when low.")]
    [SerializeField] Color negativeColor = Color.red;

    [ExecuteInEditMode]
    private void OnValidate() {
        // Clamp values
        if (fillSpeed < 0) {
            fillSpeed = 0;
        }
    }
    #endregion

    #region Final Script Values
    public Sprite ImageSprite => imageSprite;

    public bool SmoothFillBar => smoothFillBar;
    public float FillSpeed => fillSpeed;
    public float StartFillPercent => startFillPercent;

    public bool FillFromCenter => fillFromCenter;
    public bool FlipFillDirection => flipFillDirection;
    public bool CustomRadialUI => customRadialUI;
    public int CustomRadial => customRadial;

    public FillMethod FillType => fillType;
    public V_FillOrigin VerticalFillOrigin => verticalFillOrigin;
    public H_FillOrigin HorizontalFillOrigin => horizontalFillOrigin;
    public R_FillOrigin RadialFillOrigin => radialFillOrigin;

    public bool CanOverFill => canOverFill;
    public Color OverFillColor => overFillColor;

    public Vector2 PositiveRange => positiveRange;
    public Vector2 NegativeRange => negativeRange;
    public Color PositiveColor => positiveColor;
    public Color NegativeColor => negativeColor;
    #endregion
}