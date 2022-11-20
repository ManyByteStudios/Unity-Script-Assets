using UnityEngine;
using ByteAttributes;

/// <summary>
/// A scriptable object for the GatchaLoot script.
/// Will create a loot table for the script.
/// </summary>

[CreateAssetMenu(fileName = "Loot Table", menuName = "Scriptable Objects/Loot Table")]
public class GatchaLootProperties : ScriptableObject {
    #region Editable Values
    [Header("Loot Properties")]
    [Tooltip("Possible loot rewards with chances.")]
    [SerializeField] LootProperties[] lootTable = null;

    [System.Serializable]
    public class LootProperties {
        public string LootCatagory = null;
        [AbsoluteValue()] public int LootChance = 0;
        public GameObject[] LootRewards = null;
    }
    #endregion

    #region Gatcha Loot Script Values
    public LootProperties[] LootTable => lootTable;
    #endregion
}