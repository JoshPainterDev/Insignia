using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour {

    public static GameController controller;

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
}
