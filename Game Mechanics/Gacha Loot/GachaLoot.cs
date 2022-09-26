using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a simple script where the core logic of loot prize / reward 
/// are randomly selected. The presentation of the loot is left empty for
/// the various styles of showcasing the loot to the player.
/// </summary>

public class GachaLoot : MonoBehaviour {
    [Header("Gatcha Loot Properties")]
    [Tooltip("Gatcha loot scriptable object.")]
    [SerializeField] private GatchaLootProperties lootProperty;

    // This method can be called from another script to generate the loot drop
    protected GameObject[] GenerateLoot(int NumberOfRewards) {
        int MaxLootPool = 1;
        GameObject[] GeneratedLoot = null;

        // Creating a poll of numbers for the total chance of winning any loot
        for (int i = 0; i < lootProperty.LootTable.Length; i++) {
            MaxLootPool += lootProperty.LootTable[i].LootChance;
        }

        // Choosing the loot reward
        for (int x = 0; x < NumberOfRewards; x++) {
            int TierNumber = Random.Range(0, MaxLootPool);
            int SelectedCatagory = 0;
            int LastMinNumber = 0;
            int LastMaxNumber = 0;

            for (int y = 0; y < lootProperty.LootTable.Length; y++) {
                LastMinNumber = LastMaxNumber;
                LastMaxNumber += lootProperty.LootTable[y].LootChance;

                if (TierNumber > LastMinNumber && TierNumber <= LastMaxNumber) {
                    SelectedCatagory = y;
                    break;
                }
            }

            // Picks and save the selected loot reward
            int SelectedReward = Random.Range(0, lootProperty.LootTable[SelectedCatagory].LootRewards.Length);
            Debug.Log("The selected reward is: " + lootProperty.LootTable[SelectedCatagory].LootRewards[SelectedReward] + ", which is a " + lootProperty.LootTable[SelectedCatagory].LootCatagory + " reward catagory");
            GeneratedLoot[x] = lootProperty.LootTable[SelectedCatagory].LootRewards[SelectedReward];
        }

        // The saved loot reward
        return GeneratedLoot;
    }
}