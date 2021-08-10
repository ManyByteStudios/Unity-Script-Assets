using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script manipulates a UI images to create
/// a visual bar mechanic such as a health bar, &
/// progress bar. This script will have multiple 
/// properties to accomidate for the different types
/// of fill directions, and method of the image
/// component.
/// </summary>

[DisallowMultipleComponent]
public class UniversalUIBar : MonoBehaviour {
    #region Universal UI Bar Enums
    enum FillMethod { Horizontal, Vertical, Radial360 }
    enum H_FillOrigin { Right, Left }
    enum V_FillOrigin { Bottom, Top }
    enum R_FillOrigin { Bottom, Right, Top, Left }
    #endregion

    #region Universal UI Bar Properties
    [Header("UI Bar Properties")]
    [Tooltip("Image sprite.")]
    [SerializeField] Sprite imageSprite = null;

    [Space(10)]
    [Tooltip("Slowly fill the UI bar.")]
    [SerializeField] bool SmoothFillBar = false;
    [Tooltip("Fill speed of the UI bar.")]
    [ConditionalHide("SmoothFillBar", true)] [SerializeField] float fillSpeed = 0;
    [Tooltip("Set the start fill amount.")]
    [Range(0, 1)] [SerializeField] float startFillPercent = 0.5f;

    [Space(10)]
    [Tooltip("Fill origion from the center.")]
    [SerializeField] bool fillFromCenter = false;
    [Tooltip("Flip the fill direction.")]
    [ConditionalHide("fillFromCenter", true)] [SerializeField] bool flipFillDirection = false;
    [Tooltip("Have a custom radial UI amount")]
    [ConditionalEnumHide("fillType", (int)FillMethod.Radial360)] [SerializeField] bool customRadialUI = false;
    [Tooltip("Set the custom radial amount for the UI.")]
    [ConditionalHide("customRadialUI", true)] [SerializeField] [Range(0, 360)] int customRadial = 360;

    [Space(5)]
    [Tooltip("Vertical fill bar or horizontal fill bar.")]
    [SerializeField] FillMethod fillType = FillMethod.Horizontal;
    [Tooltip("Fill direction of the UI image.")]
    [ShowIf("fillType", (int)FillMethod.Vertical)] [SerializeField] V_FillOrigin verticalFillOrigin = V_FillOrigin.Bottom;
    [Tooltip("Fill direction of the UI image.")]
    [ShowIf("fillType", (int)FillMethod.Horizontal)] [SerializeField] H_FillOrigin horizontalFillOrigin = H_FillOrigin.Left;
    [Tooltip("Fill direction of the UI image.")]
    [ShowIf("fillType", (int)FillMethod.Radial360)] [SerializeField] R_FillOrigin radialFillOrigin = R_FillOrigin.Bottom;

    [Space(10)]
    [Tooltip("List of images for UI bar.")]
    [SerializeField] List<Image> UI_Images = new List<Image>(2);

    [Space(10)]
    [SerializeField] bool canOverFill = false;
    [ShowIf("canOverFill", true)] [SerializeField] Color overFillColor = Color.blue;

    [Space(5)]
    [Tooltip("Range for positive color on UI bar.")]
    [SerializeField] [MinMaxRange(0, 1)] Vector2 positiveRange = new Vector2(0.75f, 1f);
    [Tooltip("Color of UI bar when high.")]
    [SerializeField] Color positiveColor = Color.green;

    [Space(5)]
    [Tooltip("Range for negative color on UI bar.")]
    [SerializeField] [MinMaxRange(0, 1)] Vector2 negativeRange = new Vector2(0f, 0.25f);
    [Tooltip("Color of UI bar when low.")]
    [SerializeField] Color negativeColor = Color.red;

    Gradient UIBarGradient;
    GradientColorKey[] ColorKeys;
    GradientAlphaKey[] AlphaKeys;

    float RadialLimit = 0;
    float CurrentFillPercent = 0;
    #endregion

    #region UI Bar Fill Methods
    [ExecuteInEditMode]
    private void OnValidate() {
        // Clamp values
        if (fillSpeed < 0) {
            fillSpeed = 0;
        }

        // Set color gradient for UI bar
        UIBarGradient = new Gradient();

        // Set gradient colors
        ColorKeys = new GradientColorKey[4];
        ColorKeys[0].color = negativeColor;
        ColorKeys[0].time = negativeRange.x;
        ColorKeys[1].color = negativeColor;
        ColorKeys[1].time = negativeRange.y;
        ColorKeys[2].color = positiveColor;
        ColorKeys[2].time = positiveRange.x;
        ColorKeys[3].color = positiveColor;
        ColorKeys[3].time = positiveRange.y;

        // Set gradient alphas
        AlphaKeys = new GradientAlphaKey[4];
        AlphaKeys[0].alpha = 1.0f;
        AlphaKeys[0].time = negativeRange.x;
        AlphaKeys[1].alpha = 5.0f;
        AlphaKeys[1].time = negativeRange.y;
        AlphaKeys[2].alpha = 5.0f;
        AlphaKeys[2].time = positiveRange.x;
        AlphaKeys[3].alpha = 1.0f;
        AlphaKeys[3].time = positiveRange.y;

        UIBarGradient.SetKeys(ColorKeys, AlphaKeys);

        // Setup UI bar for every UI image element
        for (int i = 0; i < UI_Images.Count; i++) {
            UI_Images[i].sprite = imageSprite;
            UI_Images[i].type = Image.Type.Filled;
            if (fillType == FillMethod.Radial360 && customRadialUI) {
                float RadialFill = (float)customRadial / 360f;
                CurrentFillPercent = (RadialFill * startFillPercent) * (360f / (float)customRadial);
                UI_Images[i].fillAmount = RadialFill * startFillPercent;

                RadialLimit = RadialFill;
            }
            else {
                CurrentFillPercent = startFillPercent;
                UI_Images[i].fillAmount = startFillPercent;
            }

            switch (fillType) {
                case FillMethod.Vertical:
                    UI_Images[i].fillMethod = Image.FillMethod.Vertical;
                    break;
                case FillMethod.Horizontal:
                    UI_Images[i].fillMethod = Image.FillMethod.Horizontal;
                    break;
                case FillMethod.Radial360:
                    UI_Images[i].fillMethod = Image.FillMethod.Radial360;
                    break;
            }

            UI_Images[i].color = UIBarGradient.Evaluate(CurrentFillPercent);

            if (startFillPercent > positiveRange.x) {
                UI_Images[i].color = positiveColor;
            }
            else if (startFillPercent < negativeRange.y) {
                UI_Images[i].color = negativeColor;
            }

            if (fillFromCenter) {
                if (i % 2 == 0) {
                    if (fillType == FillMethod.Radial360) {
                        RadialFillDirection(i, flipFillDirection);
                    }
                    else {
                        BarFillDirection(i, flipFillDirection);
                    }
                }
                else {
                    if (fillType == FillMethod.Radial360) {
                        RadialFillDirection(i, !flipFillDirection);
                    }
                    else {
                        BarFillDirection(i, !flipFillDirection);
                    }
                }
            }
            else {
                if (fillType == FillMethod.Radial360) {
                    RadialFillDirection(i, flipFillDirection);
                }
                else {
                    BarFillDirection(i, flipFillDirection);
                }
            }
        }
    }
    // Determine the fill direction
    void BarFillDirection(int index, bool flip) {
        if (fillFromCenter) {
            switch (fillType) {
                case FillMethod.Vertical:
                    if (flip) {
                        UI_Images[index].fillOrigin = (int)Image.OriginVertical.Top;
                    }
                    else {
                        UI_Images[index].fillOrigin = (int)Image.OriginVertical.Bottom;
                    }
                    break;
                case FillMethod.Horizontal:
                    if (flip) {
                        UI_Images[index].fillOrigin = (int)Image.OriginHorizontal.Left;
                    }
                    else {
                        UI_Images[index].fillOrigin = (int)Image.OriginHorizontal.Right;
                    }
                    break;
            }
        }
        else {
            switch (fillType) {
                case FillMethod.Vertical:
                    UI_Images[index].fillOrigin = (int)verticalFillOrigin;
                    break;
                case FillMethod.Horizontal:
                    UI_Images[index].fillOrigin = (int)horizontalFillOrigin;
                    break;
            }
        }
    }
    void RadialFillDirection(int index, bool flip) {
        UI_Images[index].fillClockwise = flip;
        UI_Images[index].fillOrigin = (int)radialFillOrigin;
    }

    // Getter Setter for current fill
    public float GetCurrentFill() {
        return CurrentFillPercent;
    }
    public void SetCurrentFill(float SetFill) {
        CurrentFillPercent = SetFill;
    }

    // Main method for the UI bar
    public IEnumerator Fill_UI_Bar(float FillAmount) {
        float Counter = 0;

        // Checks to see the fill are within boundries
        CurrentFillPercent += FillAmount;

        

        if (fillType == FillMethod.Radial360) {
            if (CurrentFillPercent >= 1 && !canOverFill) {
                CurrentFillPercent = 1;
            }
        }
        else {
            if (CurrentFillPercent >= 1 && !canOverFill) {
                CurrentFillPercent = 1;
            }
        }
        if (CurrentFillPercent <= 0) {
            CurrentFillPercent = 0;
        }

        if (SmoothFillBar) {
            while (Counter < fillSpeed) {
                Counter += Time.deltaTime;
                foreach (Image i in UI_Images) {
                    float NextFill;
                    if (fillType == FillMethod.Radial360) {
                        NextFill = CurrentFillPercent / (360f / (float)customRadial);
                    }
                    else {
                        NextFill = CurrentFillPercent;
                    }

                    i.fillAmount = Mathf.Lerp(i.fillAmount, NextFill, Time.deltaTime);

                    if (canOverFill && CurrentFillPercent > positiveRange.y) {
                        i.color = overFillColor;
                    }
                    else if (CurrentFillPercent <= positiveRange.y) {
                        i.color = UIBarGradient.Evaluate(CurrentFillPercent);
                    }
                }
                yield return null;
            }
        }
        else {
            foreach (Image i in UI_Images) {
                i.fillAmount = CurrentFillPercent;

                if (CurrentFillPercent > 1 && canOverFill) {
                    i.color = overFillColor;
                }
                else {
                    i.color = UIBarGradient.Evaluate(CurrentFillPercent);
                }
            }
        }
    }
    #endregion
}