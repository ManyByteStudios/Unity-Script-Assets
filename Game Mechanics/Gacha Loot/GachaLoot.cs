using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This is a simple script where the core logic of loot prize / reward 
    are randomly selected. The presentation of the loot is left empty for
    the various styles of showcasing the loot to the player.
*/

public class GachaLoot : MonoBehaviour {
    #region Gacha Loot Properties
    [Header("Loot Properties")]
    [Tooltip("Possible loot rewards with chances.")] [SerializeField] protected LootProperties[] lootTable = null;

    [System.Serializable]
    protected class LootProperties {
        public string LootCatagory = null;
        [AbsoluteValue()] public int LootChance = 0;
        public GameObject[] LootRewards = null;
    }
    #endregion

    // This method can be called from another script to generate the loot drop
    public GameObject[] GenerateLoot(int NumberOfRewards) {
        int MaxLootPool = 1;
        GameObject[] GeneratedLoot = null;

        // Creating a poll of numbers for the total chance of winning any loot
        for (int i = 0; i < lootTable.Length; i++) {
            MaxLootPool += lootTable[i].LootChance;
        }

        // Choosing the loot reward
        for (int x = 0; x < NumberOfRewards; x++) {
            int TierNumber = Random.Range(0, MaxLootPool);
            int SelectedCatagory = 0;
            int LastMinNumber = 0;
            int LastMaxNumber = 0;

            for (int y = 0; y < lootTable.Length; y++) {
                LastMinNumber = LastMaxNumber;
                LastMaxNumber += lootTable[y].LootChance;

                if (TierNumber > LastMinNumber && TierNumber <= LastMaxNumber) {
                    SelectedCatagory = y;
                    break;
                }
            }

            // Picks and save the selected loot reward
            int SelectedReward = Random.Range(0, lootTable[SelectedCatagory].LootRewards.Length);
            Debug.Log("The selected reward is: " + lootTable[SelectedCatagory].LootRewards[SelectedReward] + ", which is a " + lootTable[SelectedCatagory].LootCatagory + " reward catagory");
            GeneratedLoot[x] = lootTable[SelectedCatagory].LootRewards[SelectedReward];
        }

        // The saved loot reward
        return GeneratedLoot;
    }
}