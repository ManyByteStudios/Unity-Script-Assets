using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This script is a scriptable object for the Gatcha Loot script.
    It contains all of the variables needed for the script.
*/

[CreateAssetMenu(fileName = "Gacha Loot Properties", menuName = "Scriptable Objects/GachaLoot")]
public class GachaLootProperties : ScriptableObject {
    [SerializeField] LootProperties[] lootTable = null;

    [System.Serializable]
    public class LootProperties {
        public string LootCatagory = null;
        [AbsoluteValue()] public int LootChance = 0;
        public GameObject[] LootRewards = null;
    }

    public LootProperties[] LootTable => lootTable;
}