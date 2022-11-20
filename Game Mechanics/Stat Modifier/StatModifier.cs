using UnityEngine;
using ByteAttributes;

/// <summary>
/// This script allows for the manipulation of player or game stats,
/// based on the level of the modifier. The modifier allows for a buff
/// or de-buff like affect on any given numerical stats. 
/// 
/// The script is similar to game mechancis like mods, stat based 
/// abilites, and upgrades.
/// </summary>

public class StatModifier : MonoBehaviour {
    #region Stat Modifier Upgrade Properties
    [Header("Stat Modifier Properties")]
    [Tooltip("Stat modifier scriptable object.")]
    [NotNullable] [SerializeField] private StatModifierProperties statModifierProperty;

    // Private variables
    int CurrentLevel = 0;
    float[] CurrentStatValues;

    // String variable names used for saving and loading stat data
    string StatLevel_ID;
    string[] BaseStat_ID;
    string[] IncreaseStat_ID;
    [HideInInspector] protected string[] FinalStatDescription;
    #endregion

    // Start is called before the first frame update
    void Start() {
        StatLevel_ID = statModifierProperty.ModifierName + "_CurrentLevel";

        BaseStat_ID = new string[statModifierProperty.Stats.Length];
        for (int i = 0; i < statModifierProperty.Stats.Length; i++) {
            BaseStat_ID[i] = statModifierProperty.ModifierName + "_Stat_" + i;
        }
        IncreaseStat_ID = new string[statModifierProperty.Stats.Length];
        for (int i = 0; i < statModifierProperty.Stats.Length; i++) {
            IncreaseStat_ID[i] = statModifierProperty.ModifierName + "_IncreaseStat_" + i;
        }
    }

    // Level up modifier by the amount indicated by the user
    public void UpgradeModifier(int NumOfUpgrades) {
        CurrentLevel += NumOfUpgrades;

        // Check to see if upgrade went over the limit or under zero
        if (CurrentLevel > statModifierProperty.MaxModifierLevel && statModifierProperty.LimitedModifierLevels) {
            CurrentLevel = statModifierProperty.MaxModifierLevel;
        }
        else if (CurrentLevel < 0) {
            CurrentLevel = 0;
        }

        // Update stats based on the current level of modifier
        CurrentStatValues = new float[statModifierProperty.Stats.Length];
        for (int i = 0; i < statModifierProperty.Stats.Length; i++) {
            CurrentStatValues[i] = statModifierProperty.Stats[i].baseStatValue + (CurrentLevel * statModifierProperty.Stats[i].increaseStatBy);
        }
    }

    // Returns the final stat values
    public float[] StatResults() {
        // Container for all stats output value
        float[] FinalStatResults = new float[statModifierProperty.Stats.Length];
        FinalStatDescription = new string[statModifierProperty.Stats.Length];

        // Determine final stat value as a buff or debuff
        for (int i = 0; i < statModifierProperty.Stats.Length; i++) {
            switch (statModifierProperty.Stats[i].statType) {
                case StatModifierProperties.StatType.Buff:
                    FinalStatResults[i] = Mathf.Abs(CurrentStatValues[i]);
                    FinalStatDescription[i] = statModifierProperty.Stats[i].statDescription + " +" + FinalStatResults[i];
                    if (statModifierProperty.Stats[i].modifierType == StatModifierProperties.ModifyType.Percentage) {
                        FinalStatResults[i] /= 100;
                        FinalStatDescription[i] = FinalStatDescription[i] + "%";
                    }
                    break;
                case StatModifierProperties.StatType.Debuff:
                    FinalStatResults[i] = (Mathf.Abs(CurrentStatValues[i]) * -1f);
                    FinalStatDescription[i] = statModifierProperty.Stats[i].statDescription + " " + FinalStatResults[i];
                    if (statModifierProperty.Stats[i].modifierType == StatModifierProperties.ModifyType.Percentage) {
                        FinalStatResults[i] /= 100;
                        FinalStatDescription[i] = FinalStatDescription[i] + "%";
                    }
                    break;
            }
        }

        return FinalStatResults;
    }

    // Saves any data relating to the stat modifier
    public void SaveStats() {
        PlayerPrefs.SetInt(StatLevel_ID, CurrentLevel);

        // Only store the base stat and incrementation
        for (int i = 0; i < statModifierProperty.Stats.Length; i++) {
            PlayerPrefs.SetFloat(BaseStat_ID[i], statModifierProperty.Stats[i].baseStatValue);
        }
        for (int i = 0; i < statModifierProperty.Stats.Length; i++) {
            PlayerPrefs.SetFloat(IncreaseStat_ID[i], statModifierProperty.Stats[i].baseStatValue);
        }
    }

    // Loads any data relating to the stat modifier
    public void LoadStats() {
        // Check if there is something to load
        if (!PlayerPrefs.HasKey(StatLevel_ID)) {
            return;
        }

        else {
            CurrentLevel = PlayerPrefs.GetInt(StatLevel_ID);

            // Only get the base stat and incrementation
            for (int i = 0; i < statModifierProperty.Stats.Length; i++) {
                statModifierProperty.Stats[i].baseStatValue = PlayerPrefs.GetFloat(BaseStat_ID[i]);
            }
            for (int i = 0; i < statModifierProperty.Stats.Length; i++) {
                statModifierProperty.Stats[i].baseStatValue = PlayerPrefs.GetFloat(IncreaseStat_ID[i]);
            }
        }
    }

    // Clear all data relating to the stat modifier
    public void ClearStats() {
        // Check if there is something to clear
        if (!PlayerPrefs.HasKey(StatLevel_ID)) {
            return;
        }

        // Delete the player prefs
        PlayerPrefs.DeleteKey(StatLevel_ID);
        for (int i = 0; i < statModifierProperty.Stats.Length; i++) {
            PlayerPrefs.DeleteKey(BaseStat_ID[i]);
        }
        for (int i = 0; i < statModifierProperty.Stats.Length; i++) {
            PlayerPrefs.DeleteKey(IncreaseStat_ID[i]);
        }
    }
}