﻿using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour {

    public static GameController controller;
    public Ability playerAbility1;
    public Ability playerAbility2;
    public Ability playerAbility3;
    public Ability playerAbility4;
    public string strikeModifier = "none";
    public string[] playerInventory;
    public int[] playerInventoryQuantity;
    public int playerAttack;
    public int playerDefense;
    public int playerProwess;

    // Use this for initialization
    void Awake () {

        if(controller == null)
        {
            DontDestroyOnLoad(gameObject);
            controller = this;
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

        data.ability1 = playerAbility1;
        data.ability2 = playerAbility2;
        data.ability3 = playerAbility3;
        data.ability4 = playerAbility4;
        data.StrikeMod = strikeModifier;
        data.attack = playerAttack;
        data.defense = playerDefense;
        data.prowess = playerProwess;

        data.InventoryList = playerInventory;

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
            playerAbility1 = data.ability1;
            playerAbility2 = data.ability2;
            playerAbility3 = data.ability3;
            playerAbility4 = data.ability4;
            strikeModifier = data.StrikeMod;
            playerInventory = data.InventoryList;
        }
    }

    public void Delete()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            File.Delete(Application.persistentDataPath + "/playerInfo.dat");
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
    public Ability ability1;
    public Ability ability2;
    public Ability ability3;
    public Ability ability4;
    public string StrikeMod;
    public string PlayerClass;
    public string[] InventoryList;
    public int[] InventoryQuantities;
    public int attack;
    public int defense;
    public int prowess;
}
