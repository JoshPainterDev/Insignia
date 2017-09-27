using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour {

    public static GameController controller;
    public string characterName = "Steve Lee";
    public Ability playerAbility1;
    public Ability playerAbility2;
    public Ability playerAbility3;
    public Ability playerAbility4;
    public string strikeModifier = "none";
    public LimitBreakName limitBreakModifier = LimitBreakName.none;
    public int limitBreakTracker;
    public string[] playerInventory;
    public int[] playerInventoryQuantity;
    public bool [,] playerEquipmentList;
    public int[] playerEquippedIDs;
    public int difficultyScale;
    public int playerLevel;
    public int playerEXP;
    public int playerAttack;
    public int playerDefense;
    public int playerProwess;
    public int playerSpeed;
    public int playerBaseAtk;
    public int playerBaseDef;
    public int playerBasePrw;
    public int playerBaseSpd;
    public float[] playerColorPreference;
    public int levelsCompleted;
    public int stagesCompleted;
    public EnemyEncounter currentEncounter;
    public Reward rewardEarned;

    private int atk = 0;
    private int def = 0;
    private int spd = 0;
    private int prw = 0;

    // Use this for initialization
    void Awake () {

        if(controller == null)
        {
            DontDestroyOnLoad(gameObject);
            controller = this;
            playerEquippedIDs = new int[16];
        }
        else if(controller != this)
        {
            Destroy(gameObject);
        }
	}

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream playerInfoFile = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

        PlayerData data = new PlayerData();

        data.charName = characterName;
        data.Level = playerLevel;
        data.PlayerExperience = playerEXP;
        data.difficulty = difficultyScale;
        data.ability1 = playerAbility1;
        data.ability2 = playerAbility2;
        data.ability3 = playerAbility3;
        data.ability4 = playerAbility4;
        data.StrikeMod = strikeModifier;
        data.limitBreakMod = limitBreakModifier;
        data.limitBreakTrack = limitBreakTracker;
        data.attack = playerAttack;
        data.defense = playerDefense;
        data.prowess = playerProwess;
        data.speed = playerSpeed;
        data.PlayerColor = playerColorPreference;

        data.EquippedIDs = playerEquippedIDs;
        data.InventoryList = playerInventory;
        data.LevelsCompleted = levelsCompleted;
        data.StagesCompleted = stagesCompleted;

        bf.Serialize(playerInfoFile, data);
        playerInfoFile.Close();
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            // set variables here
            playerLevel = data.Level;
            playerEXP = data.PlayerExperience;
            difficultyScale = data.difficulty;
            playerAbility1 = data.ability1;
            playerAbility2 = data.ability2;
            playerAbility3 = data.ability3;
            playerAbility4 = data.ability4;
            strikeModifier = data.StrikeMod;
            limitBreakModifier = data.limitBreakMod;
            limitBreakTracker = data.limitBreakTrack;
            playerInventory = data.InventoryList;
            playerColorPreference = data.PlayerColor;
            playerEquipmentList = data.EquipmentList;
            playerEquippedIDs = data.EquippedIDs;
            levelsCompleted = data.LevelsCompleted;
            stagesCompleted = data.StagesCompleted;
        }
    }

    public void Delete()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            File.Delete(Application.persistentDataPath + "/playerInfo.dat");
        }
    }

    public void UpdateEquipmentStats(string stat)
    {
        
        def = GameController.controller.playerDefense;
        prw = GameController.controller.playerProwess;
        spd = GameController.controller.playerSpeed;

        switch (stat)
        {
            case "atk":
                atk = GameController.controller.playerAttack;

                break;
        }
    }

    public void AddItemToInventory(string itemName, int quantity = 1)
    {
        int index = InventoryContainsItem(itemName);

        if (index != -1)
        {
            playerInventoryQuantity[index] += quantity;
        }
        else
        {
            // increase inventory size
            string[] temp = new string[playerInventory.Length + 1];
            playerInventory.CopyTo(temp, 0);
            playerInventory = temp;
            playerInventoryQuantity[index] += quantity;

            // increase inventory quantity size
            int[] temp2 = new int[playerInventoryQuantity.Length + 1];
            playerInventoryQuantity.CopyTo(temp2, 0);
            playerInventoryQuantity = temp2;
            playerInventoryQuantity[playerInventoryQuantity.Length - 1] = quantity;
        }
    }

    public void RemoveItemFromInventory(string itemName, int quantity = 1, bool removeAll = false)
    {
        int index = InventoryContainsItem(itemName);

        if (index != -1)
        {
            if(removeAll)
            {
                playerInventoryQuantity[index] = 0;
            }
            else
            {
                playerInventoryQuantity[index] -= quantity;

                if(playerInventoryQuantity[index] <= 0)
                {
                    // decrease inventory size
                    string[] tempArr1 = new string[playerInventory.Length - 1];
                    // copy initial portion of array
                    for(int j = 0; j < index; ++j)
                        tempArr1[j] = playerInventory[j];
                    // copy over remaining portion of the array -1
                    for (int i = index; i < tempArr1.Length; ++i)
                        tempArr1[i] = playerInventory[i + 1];

                    playerInventory = tempArr1;

                    // decrease inventory quantity size
                    int[] tempArr2 = new int[playerInventoryQuantity.Length - 1];
                    // copy first portion of the inventory
                    for (int j = 0; j < index; ++j)
                        tempArr2[j] = playerInventoryQuantity[j];
                    // copy over the remaining portion
                    for (int i = index; i < tempArr2.Length; ++i)
                        tempArr2[i] = playerInventoryQuantity[i + 1];

                    playerInventoryQuantity = tempArr2;
                }
            }
        }
        else
            return;
    }

    // Returns the index of the item in the invetory where it was located
    // returns -1 is it failed to find the item
    public int InventoryContainsItem(string itemName)
    {
        int j = 0;

        foreach(string i in playerInventory)
        {
            if(i == itemName)
                return j;
            ++j;
        }

        return -1;
    }
}

[Serializable]
class PlayerData
{
    public string charName;
    public Ability ability1, ability2, ability3, ability4;
    public string StrikeMod;
    public string PlayerClass;
    public int Level;
    public int PlayerExperience;
    public int difficulty;
    public LimitBreakName limitBreakMod;
    public int limitBreakTrack;
    public string [] InventoryList;
    public int [] InventoryQuantities;
    public bool [,] EquipmentList;
    public int attack, defense, prowess, speed;
    public int [] EquippedIDs; // [0,1] = head, [2,3] = torso, [4,5] = legs, [6,7] = back, [8,9] = gloves, [10,11] = shoes, [12,13] = weapon, [14,15] = aura
    public float[] PlayerColor;
    public int LevelsCompleted;
    public int StagesCompleted;
}
