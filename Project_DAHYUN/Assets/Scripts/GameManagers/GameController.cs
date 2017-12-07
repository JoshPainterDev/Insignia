using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public enum PlayerClass {Knight, Guardian, Occultist, Cutthroat, none};

public class GameController : MonoBehaviour {

    public static GameController controller;
    public int playerNumber = 0;
    public string playerName = "";
    public float[] playerSkinColor;
    public bool[] unlockedAbilities;
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
    public int[] playerDecisions;

    public string[] charNames;
    public int numChars;
    public PlayerClass[] charClasses = new PlayerClass[6];

    // Use this for initialization
    void Awake () {

        if(controller == null)
        {
            DontDestroyOnLoad(gameObject);
            controller = this;
            unlockedAbilities = new bool[15];
            playerSkinColor = new float[4];
            playerColorPreference = new float[4];
            playerEquippedIDs = new int[16];
            playerEquipmentList = new bool[30, 4];
            playerDecisions = new int[8];
            charNames = new string[6];
            charClasses = new PlayerClass[6];
            for(int i = 0; i < 6; ++i)
                charClasses[i] = PlayerClass.none;

            if (File.Exists(Application.persistentDataPath + "/accountInfo.dat"))
            {
                LoadCharacters();
            }
            else
            {
                print("no file existed");
                var sr = File.CreateText(Application.persistentDataPath + "/accountInfo.dat");
                charNames[0] = "Skip";
                numChars = 0;
                charClasses[0] = PlayerClass.none;
                SaveCharacters();
            }
        }
        else if(controller != this)
        {
            Destroy(gameObject);
        }
	}

    /*ONLY FOR SAVING ACCOUNT INFORMATION*/
    public void SaveCharacters()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream accountInfoFile = File.Create(Application.persistentDataPath + "/accountInfo.dat");

        AccountData data = new AccountData();

        data.characterNames = charNames;
        data.numberOfCharacters = numChars;
        data.characterClasses = charClasses;

        bf.Serialize(accountInfoFile, data);
        accountInfoFile.Close();
    }

    public bool LoadCharacters()
    {
        if (File.Exists(Application.persistentDataPath + "/accountInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/accountInfo.dat", FileMode.Open);
            AccountData data = (AccountData)bf.Deserialize(file);
            charNames = data.characterNames;
            numChars = data.numberOfCharacters;
            charClasses = data.characterClasses;
            file.Close();
            return true;
        }

        return false;
    }

    /*FOR LOADING PLAYER SPECIFIC DATA*/

    public void Save(string saveName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream playerInfoFile = File.Create(Application.persistentDataPath + "/playerInfo_" + saveName + ".dat");

        PlayerData data = new PlayerData();

        data.PlayerNumber = playerNumber;
        data.PlayerName = playerName;
        data.PlayerSkinColor = playerSkinColor;
        data.Level = playerLevel;
        data.PlayerExperience = playerEXP;
        data.difficulty = difficultyScale;
        data.UnlockedAbilities = unlockedAbilities;
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
        data.PlayerDecisions = playerDecisions;

        data.EquipmentList = playerEquipmentList;
        data.EquippedIDs = playerEquippedIDs;
        data.InventoryList = playerInventory;
        data.LevelsCompleted = levelsCompleted;
        data.StagesCompleted = stagesCompleted;

        bf.Serialize(playerInfoFile, data);
        playerInfoFile.Close();
    }

    public void Load(string saveName)
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo_" + saveName + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo_" + saveName + ".dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            // set variables here
            playerNumber = data.PlayerNumber;
            playerName = data.PlayerName;
            playerLevel = data.Level;
            playerEXP = data.PlayerExperience;
            difficultyScale = data.difficulty;
            unlockedAbilities = data.UnlockedAbilities;
            playerAbility1 = data.ability1;
            playerAbility2 = data.ability2;
            playerAbility3 = data.ability3;
            playerAbility4 = data.ability4;
            strikeModifier = data.StrikeMod;
            limitBreakModifier = data.limitBreakMod;
            limitBreakTracker = data.limitBreakTrack;
            playerSkinColor = data.PlayerSkinColor;
            playerColorPreference = data.PlayerColor;
            playerAttack = data.attack;
            playerDefense = data.defense;
            playerProwess = data.prowess;
            playerSpeed = data.speed;
            playerDecisions = data.PlayerDecisions;

            playerInventory = data.InventoryList;
            playerEquipmentList = data.EquipmentList;
            playerEquippedIDs = data.EquippedIDs;
            levelsCompleted = data.LevelsCompleted;
            stagesCompleted = data.StagesCompleted;
        }
    }

    public void Delete(string saveName)
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo_" + saveName + ".dat"))
        {
            File.Delete(Application.persistentDataPath + "/playerInfo_" + saveName + ".dat");
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

    public void setPlayerSkinColor(Color newColor)
    {
        playerSkinColor[0] = newColor.r;
        playerSkinColor[1] = newColor.g;
        playerSkinColor[2] = newColor.b;
        playerSkinColor[3] = newColor.a;
    }

    public Color getPlayerSkinColor()
    {
        Color player_C = new Color(playerSkinColor[0], playerSkinColor[1], playerSkinColor[2], playerSkinColor[3]);
        return player_C;
    }

    public void setPlayerColorPreference(Color newColor)
    {
        playerColorPreference[0] = newColor.r;
        playerColorPreference[1] = newColor.g;
        playerColorPreference[2] = newColor.b;
        playerColorPreference[3] = newColor.a;
    }

    public Color getPlayerColorPreference()
    {
        Color player_C = new Color(playerColorPreference[0], playerColorPreference[1], playerColorPreference[2], playerColorPreference[3]);
        return player_C;
    }
}

[Serializable]
class PlayerData
{
    public int PlayerNumber;
    public string PlayerName;
    public float[] PlayerSkinColor;
    public bool[] UnlockedAbilities;
    public Ability ability1, ability2, ability3, ability4;
    public string StrikeMod;
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
    public int[] PlayerDecisions;
}

[Serializable]
class AccountData
{
    public string[] characterNames;
    public PlayerClass[] characterClasses;
    public int numberOfCharacters = 0;
}
