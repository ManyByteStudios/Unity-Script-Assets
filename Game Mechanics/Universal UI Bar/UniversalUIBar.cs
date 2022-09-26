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
    #region Universal UI Bar Properties
    [Header("Stat Modifier Properties")]
    [Tooltip("Stat modifier scriptable object.")]
    [SerializeField] private UI_BarProperties UI_BarProperty;
    [Space(10)]
    [Tooltip("List of images for UI bar.")]
    [SerializeField] private List<Image> UI_Images = new List<Image>(2);

    Gradient UIBarGradient;
    GradientColorKey[] ColorKeys;
    GradientAlphaKey[] AlphaKeys;

    float RadialLimit = 0;
    float CurrentFillPercent = 0;
    #endregion

    #region UI Bar Fill Methods
    [ExecuteInEditMode]
    private void OnValidate() {
        // Set color gradient for UI bar
        UIBarGradient = new Gradient();

        // Set gradient colors
        ColorKeys = new GradientColorKey[4];
        ColorKeys[0].color = UI_BarProperty.NegativeColor;
        ColorKeys[0].time = UI_BarProperty.NegativeRange.x;
        ColorKeys[1].color = UI_BarProperty.NegativeColor;
        ColorKeys[1].time = UI_BarProperty.NegativeRange.y;
        ColorKeys[2].color = UI_BarProperty.PositiveColor;
        ColorKeys[2].time = UI_BarProperty.PositiveRange.x;
        ColorKeys[3].color = UI_BarProperty.PositiveColor;
        ColorKeys[3].time = UI_BarProperty.PositiveRange.y;

        // Set gradient alphas
        AlphaKeys = new GradientAlphaKey[4];
        AlphaKeys[0].alpha = 1.0f;
        AlphaKeys[0].time = UI_BarProperty.NegativeRange.x;
        AlphaKeys[1].alpha = 5.0f;
        AlphaKeys[1].time = UI_BarProperty.NegativeRange.y;
        AlphaKeys[2].alpha = 5.0f;
        AlphaKeys[2].time = UI_BarProperty.PositiveRange.x;
        AlphaKeys[3].alpha = 1.0f;
        AlphaKeys[3].time = UI_BarProperty.PositiveRange.y;

        UIBarGradient.SetKeys(ColorKeys, AlphaKeys);

        // Setup UI bar for every UI image element
        for (int i = 0; i < UI_Images.Count; i++) {
            UI_Images[i].sprite = UI_BarProperty.ImageSprite;
            UI_Images[i].type = Image.Type.Filled;
            if (UI_BarProperty.FillType == UI_BarProperties.FillMethod.Radial360 && UI_BarProperty.CustomRadialUI) {
                float RadialFill = (float)UI_BarProperty.CustomRadial / 360f;
                CurrentFillPercent = (RadialFill * UI_BarProperty.StartFillPercent) * (360f / (float)UI_BarProperty.CustomRadial);
                UI_Images[i].fillAmount = RadialFill * UI_BarProperty.StartFillPercent;

                RadialLimit = RadialFill;
            }
            else {
                CurrentFillPercent = UI_BarProperty.StartFillPercent;
                UI_Images[i].fillAmount = UI_BarProperty.StartFillPercent;
            }

            switch (UI_BarProperty.FillType) {
                case UI_BarProperties.FillMethod.Vertical:
                    UI_Images[i].fillMethod = Image.FillMethod.Vertical;
                    break;
                case UI_BarProperties.FillMethod.Horizontal:
                    UI_Images[i].fillMethod = Image.FillMethod.Horizontal;
                    break;
                case UI_BarProperties.FillMethod.Radial360:
                    UI_Images[i].fillMethod = Image.FillMethod.Radial360;
                    break;
            }

            UI_Images[i].color = UIBarGradient.Evaluate(CurrentFillPercent);

            if (UI_BarProperty.StartFillPercent > UI_BarProperty.PositiveRange.x) {
                UI_Images[i].color = UI_BarProperty.PositiveColor;
            }
            else if (UI_BarProperty.StartFillPercent < UI_BarProperty.NegativeRange.y) {
                UI_Images[i].color = UI_BarProperty.NegativeColor;
            }

            if (UI_BarProperty.FillFromCenter) {
                if (i % 2 == 0) {
                    if (UI_BarProperty.FillType == UI_BarProperties.FillMethod.Radial360) {
                        RadialFillDirection(i, UI_BarProperty.FlipFillDirection);
                    }
                    else {
                        BarFillDirection(i, UI_BarProperty.FlipFillDirection);
                    }
                }
                else {
                    if (UI_BarProperty.FillType == UI_BarProperties.FillMethod.Radial360) {
                        RadialFillDirection(i, !UI_BarProperty.FlipFillDirection);
                    }
                    else {
                        BarFillDirection(i, !UI_BarProperty.FlipFillDirection);
                    }
                }
            }
            else {
                if (UI_BarProperty.FillType == UI_BarProperties.FillMethod.Radial360) {
                    RadialFillDirection(i, UI_BarProperty.FlipFillDirection);
                }
                else {
                    BarFillDirection(i, UI_BarProperty.FlipFillDirection);
                }
            }
        }
    }

    // Determine the fill direction
    void BarFillDirection(int index, bool flip) {
        if (UI_BarProperty.FillFromCenter) {
            switch (UI_BarProperty.FillType) {
                case UI_BarProperties.FillMethod.Vertical:
                    if (flip) {
                        UI_Images[index].fillOrigin = (int)Image.OriginVertical.Top;
                    }
                    else {
                        UI_Images[index].fillOrigin = (int)Image.OriginVertical.Bottom;
                    }
                    break;
                case UI_BarProperties.FillMethod.Horizontal:
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
            switch (UI_BarProperty.FillType) {
                case UI_BarProperties.FillMethod.Vertical:
                    UI_Images[index].fillOrigin = (int)UI_BarProperty.VerticalFillOrigin;
                    break;
                case UI_BarProperties.FillMethod.Horizontal:
                    UI_Images[index].fillOrigin = (int)UI_BarProperty.HorizontalFillOrigin;
                    break;
            }
        }
    }
    void RadialFillDirection(int index, bool flip) {
        UI_Images[index].fillClockwise = flip;
        UI_Images[index].fillOrigin = (int)UI_BarProperty.RadialFillOrigin;
    }

    // Getter Setter for current fill
    protected float GetCurrentFill() {
        return CurrentFillPercent;
    }
    protected void SetCurrentFill(float SetFill) {
        CurrentFillPercent = SetFill;
    }

    // Main method for the UI bar
    protected IEnumerator Fill_UI_Bar(float FillAmount) {
        float Counter = 0;

        // Checks to see the fill are within boundries
        CurrentFillPercent += FillAmount;

        

        if (UI_BarProperty.FillType == UI_BarProperties.FillMethod.Radial360) {
            if (CurrentFillPercent >= 1 && !UI_BarProperty.CanOverFill) {
                CurrentFillPercent = 1;
            }
        }
        else {
            if (CurrentFillPercent >= 1 && !UI_BarProperty.CanOverFill) {
                CurrentFillPercent = 1;
            }
        }
        if (CurrentFillPercent <= 0) {
            CurrentFillPercent = 0;
        }

        if (UI_BarProperty.SmoothFillBar) {
            while (Counter < UI_BarProperty.FillSpeed) {
                Counter += Time.deltaTime;
                foreach (Image i in UI_Images) {
                    float NextFill;
                    if (UI_BarProperty.FillType == UI_BarProperties.FillMethod.Radial360) {
                        NextFill = CurrentFillPercent / (360f / (float)UI_BarProperty.CustomRadial);
                    }
                    else {
                        NextFill = CurrentFillPercent;
                    }

                    i.fillAmount = Mathf.Lerp(i.fillAmount, NextFill, Time.deltaTime);

                    if (UI_BarProperty.CanOverFill && CurrentFillPercent > UI_BarProperty.PositiveRange.y) {
                        i.color = UI_BarProperty.OverFillColor;
                    }
                    else if (CurrentFillPercent <= UI_BarProperty.PositiveRange.y) {
                        i.color = UIBarGradient.Evaluate(CurrentFillPercent);
                    }
                }
                yield return null;
            }
        }
        else {
            foreach (Image i in UI_Images) {
                i.fillAmount = CurrentFillPercent;

                if (CurrentFillPercent > 1 && UI_BarProperty.CanOverFill) {
                    i.color = UI_BarProperty.OverFillColor;
                }
                else {
                    i.color = UIBarGradient.Evaluate(CurrentFillPercent);
                }
            }
        }
    }
    #endregion
}