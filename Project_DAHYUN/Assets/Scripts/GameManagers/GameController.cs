using UnityEngine;
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
}
