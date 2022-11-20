using UnityEngine;
using ByteAttributes;

/// <summary>
/// A scriptable object for the StatModifier script.
/// Will create the stat modifier(s) properties for the script.
/// </summary>

[CreateAssetMenu(fileName = "Stat Modifier Properties", menuName = "Scriptable Objects/Stat Modifier")]
public class StatModifierProperties : ScriptableObject {
    #region Editable Values
    public enum ModifyType { Additive, Percentage };
    public enum StatType { Buff, Debuff };

    [Header("Stat Modifier Properties")]
    [Tooltip("Name of the stat modifier.")]
    [SerializeField] string modifierName = null;
    [Tooltip("Limited levels or number of upgrades for the modifier")]
    [SerializeField] bool limitedModifierLevels = true;
    [Tooltip("The max level or number of upgrades for the modifier, limit is set to 20.")]
    [SerializeField][ConditionalHide("limitedModifierLevels", true)] int maxModifierLevel = 1;
    [Space(5)]
    [LineDivider(4, color: LineColors.Black)]
    [Tooltip("Number of stats to be modified and its properties.")]
    [SerializeField] StatProperties[] statProperties = new StatProperties[1];

    [System.Serializable]
    public class StatProperties {
        [Tooltip("Stat buff or debuff.")]
        public StatType statType = StatType.Buff;
        [Tooltip("Type of stat modifier.")]
        [SerializeField] public ModifyType modifierType = ModifyType.Additive;
        [Tooltip("Stat value at level 0.")]
        [AbsoluteValue] public float baseStatValue = 0;
        [Tooltip("How much the stat will increase by.")]
        [AbsoluteValue] public float increaseStatBy = 0;
        [Tooltip("Purpose of stat.")]
        public string statDescription = null;
    }

    [ExecuteInEditMode]
    private void OnValidate() {
        if (maxModifierLevel < 1) {
            maxModifierLevel = 1;
        }
        else if (maxModifierLevel > 20 && limitedModifierLevels) {
            maxModifierLevel = 20;
        }

        if (statProperties.Length != 0) {
            foreach (StatProperties stats in statProperties) {
                if (stats.baseStatValue < 0) {
                    stats.baseStatValue = 0;
                }
                if (stats.increaseStatBy < 0) {
                    stats.increaseStatBy = 0;
                }
            }
        }
    }
    #endregion

    #region Final Script Values
    public string ModifierName => modifierName;
    public bool LimitedModifierLevels => limitedModifierLevels;
    public int MaxModifierLevel => maxModifierLevel;
    public StatProperties[] Stats => statProperties;
    #endregion
}