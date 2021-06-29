using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This is a simple script where the core logic of loot prize / reward 
    are randomly selected. The presentation of the loot is left empty for
    the various styles of showcasing the loot to the player.
*/

public class GachaLoot : MonoBehaviour {
    [Header("Loot Properties")]
    [SerializeField] GachaLootProperties gachaLootProperties = null;

    // This method can be called from another script to generate the loot drop
    public GameObject[] GenerateLoot(int NumberOfRewards) {
        int MaxLootPool = 1;
        GameObject[] GeneratedLoot = null;

        // Creating a poll of numbers for the total chance of winning any loot
        for (int i = 0; i < gachaLootProperties.LootTable.Length; i++) {
            MaxLootPool += gachaLootProperties.LootTable[i].LootChance;
        }

        // Choosing the loot reward
        for (int x = 0; x < NumberOfRewards; x++) {
            int TierNumber = Random.Range(0, MaxLootPool);
            int SelectedCatagory = 0;
            int LastMinNumber = 0;
            int LastMaxNumber = 0;

            for (int y = 0; y < gachaLootProperties.LootTable.Length; y++) {
                LastMinNumber = LastMaxNumber;
                LastMaxNumber += gachaLootProperties.LootTable[y].LootChance;

                if (TierNumber > LastMinNumber && TierNumber <= LastMaxNumber) {
                    SelectedCatagory = y;
                    break;
                }
            }

            // Picks and save the selected loot reward
            int SelectedReward = Random.Range(0, gachaLootProperties.LootTable[SelectedCatagory].LootRewards.Length);
            Debug.Log("The selected reward is: " + gachaLootProperties.LootTable[SelectedCatagory].LootRewards[SelectedReward] + ", which is a " + gachaLootProperties.LootTable[SelectedCatagory].LootCatagory + " reward catagory");
            GeneratedLoot[x] = gachaLootProperties.LootTable[SelectedCatagory].LootRewards[SelectedReward];
        }

        // The saved loot reward
        return GeneratedLoot;
    }
}