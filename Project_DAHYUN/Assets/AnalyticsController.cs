using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class AnalyticsController : MonoBehaviour {
    public static AnalyticsController controller;

    public GameObject currentSheetText;
    public GameObject StatsSheet;

    public int AI1_Wins, AI2_Wins;
    public int AI1_StrikesUsed, AI2_StrikesUsed;
    public int AI1_ability1Uses, AI1_ability2Uses, AI1_ability3Uses, AI1_ability4Uses;
    public int AI2_ability1Uses, AI2_ability2Uses, AI2_ability3Uses, AI2_ability4Uses;
    public int AI1_StartingHP, AI2_StartingHP;
    public int AI1_Level, AI2_Level;
    public int AI1_Attack, AI1_Defense, AI1_Prowess, AI1_Speed;
    public int AI2_Attack, AI2_Defense, AI2_Prowess, AI2_Speed;
    public int AvgNumberOfTurns, TotalBattles, AvgDamagePerTurn;

    public int dataSheetNumber = 0;

    void Awake()
    {

        if (controller == null)
        {
            DontDestroyOnLoad(gameObject);
            controller = this;

            if(File.Exists(Application.persistentDataPath + "/analysisInfo" + dataSheetNumber + ".dat"))
            {
                LoadData(dataSheetNumber);
                print("Current Sheet: " + dataSheetNumber);
                currentSheetText.GetComponent<Text>().text = dataSheetNumber.ToString();
            }
            else
            {
                //var sr = File.CreateText(Application.persistentDataPath + "/analysisInfo" + dataSheetNumber + ".dat");
                SaveData();
                LoadCurrentSheet();
            }
        }
        else if (controller != this)
        {
            Destroy(gameObject);
        }
    }

    public void NextSheet()
    {
        if(File.Exists(Application.persistentDataPath + "/analysisInfo" + (dataSheetNumber + 1) + ".dat"))
        {
            ++dataSheetNumber;
            currentSheetText.GetComponent<Text>().text = dataSheetNumber.ToString();
        }
    }

    public void PrevSheet()
    {
        if(dataSheetNumber != 0)
        {
            if (File.Exists(Application.persistentDataPath + "/analysisInfo" + (dataSheetNumber - 1) + ".dat"))
            {
                --dataSheetNumber;
                currentSheetText.GetComponent<Text>().text = dataSheetNumber.ToString();
            }
        }
    }

    public void LoadCurrentSheet()
    {
        LoadData(dataSheetNumber);

        //update UI
        //AI1 wins
        StatsSheet.transform.GetChild(0).GetComponent<Text>().text = "AI1 Wins: " + AI1_Wins;
        //AI2 wins
        StatsSheet.transform.GetChild(1).GetComponent<Text>().text = "AI2 Wins: " + AI2_Wins;
    }

    public void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();

        dataSheetNumber = 0;

        while (File.Exists(Application.persistentDataPath + "/analysisInfo" + dataSheetNumber + ".dat"))
            dataSheetNumber++;

        print("saving data sheet: " + dataSheetNumber);

        FileStream dataFile = File.Create(Application.persistentDataPath + "/analysisInfo" + dataSheetNumber + ".dat");

        AnalysisData data = new AnalysisData();

        data.sAI1_ability1Uses = AI1_ability1Uses;
        data.sAI1_ability2Uses = AI1_ability2Uses;
        data.sAI1_ability3Uses = AI1_ability3Uses;
        data.sAI1_ability4Uses = AI1_ability4Uses;
        data.sAI1_Attack = AI1_Attack;
        data.sAI1_Defense = AI1_Defense;
        data.sAI1_Level = AI1_Level;
        data.sAI1_Prowess = AI1_Prowess;
        data.sAI1_Speed = AI1_Speed;
        data.sAI1_StartingHP = AI1_StartingHP;
        data.sAI1_StrikesUsed = AI1_StrikesUsed;
        data.sAI1_Wins = AI1_Wins;
        data.sAI2_ability1Uses = AI2_ability1Uses;
        data.sAI2_ability2Uses = AI2_ability2Uses;
        data.sAI2_ability3Uses = AI2_ability3Uses;
        data.sAI2_ability4Uses = AI2_ability4Uses;
        data.sAI2_Attack = AI2_Attack;
        data.sAI2_Defense = AI2_Defense;
        data.sAI2_Level = AI2_Level;
        data.sAI2_Prowess = AI2_Prowess;
        data.sAI2_Speed = AI2_Speed;
        data.sAI2_StartingHP = AI2_StartingHP;
        data.sAI2_StrikesUsed = AI2_StrikesUsed;
        data.sAI2_Wins = AI2_Wins;
        data.sAvgDamagePerTurn = AvgDamagePerTurn;
        data.sAvgNumberOfTurns = AvgNumberOfTurns;
        data.sTotalBattles = TotalBattles;

        bf.Serialize(dataFile, data);
        dataFile.Close();

        currentSheetText.GetComponent<Text>().text = dataSheetNumber.ToString();
        print("dsn: " + dataSheetNumber);
    }


    public bool LoadData(int dataSheetNum)
    {
        if (File.Exists(Application.persistentDataPath + "/analysisInfo" + dataSheetNum + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/analysisInfo" + dataSheetNum + ".dat", FileMode.Open);
            AnalysisData data = (AnalysisData)bf.Deserialize(file);

            AI1_ability1Uses = data.sAI1_ability1Uses;
            AI1_ability2Uses = data.sAI1_ability2Uses;
            AI1_ability3Uses = data.sAI1_ability3Uses;
            AI1_ability4Uses = data.sAI1_ability4Uses;
            AI1_Attack = data.sAI1_Attack;
            AI1_Defense = data.sAI1_Defense;
            AI1_Level = data.sAI1_Level;
            AI1_Prowess = data.sAI1_Prowess;
            AI1_Speed = data.sAI1_Speed;
            AI1_StartingHP = data.sAI1_StartingHP;
            AI1_StrikesUsed = data.sAI1_StrikesUsed;
            AI1_Wins = data.sAI1_Wins;
            AI2_ability1Uses = data.sAI2_ability1Uses;
            AI2_ability2Uses = data.sAI2_ability2Uses;
            AI2_ability3Uses = data.sAI2_ability3Uses;
            AI2_ability4Uses = data.sAI2_ability4Uses;
            AI2_Attack = data.sAI2_Attack;
            AI2_Defense = data.sAI2_Defense;
            AI2_Level = data.sAI2_Level;
            AI2_Prowess = data.sAI2_Prowess;
            AI2_Speed = data.sAI2_Speed;
            AI2_StartingHP = data.sAI2_StartingHP;
            AI2_StrikesUsed = data.sAI2_StrikesUsed;
            AI2_Wins = data.sAI2_Wins;
            AvgDamagePerTurn = data.sAvgDamagePerTurn;
            AvgNumberOfTurns = data.sAvgNumberOfTurns;
            TotalBattles = data.sTotalBattles;

            file.Close();
            return true;
        }

        return false;
    }

    public void DeleteData(int dataSheetNum)
    {
        if (File.Exists(Application.persistentDataPath + "/analysisInfo" + dataSheetNum + ".dat"))
        {
            File.Delete(Application.persistentDataPath + "/analysisInfo" + dataSheetNum + ".dat");
        }
    }
}

[Serializable]
class AnalysisData
{
    public int sAI1_Wins, sAI2_Wins;
    public int sAI1_StrikesUsed, sAI2_StrikesUsed;
    public int sAI1_ability1Uses, sAI1_ability2Uses, sAI1_ability3Uses, sAI1_ability4Uses;
    public int sAI2_ability1Uses, sAI2_ability2Uses, sAI2_ability3Uses, sAI2_ability4Uses;
    public int sAI1_StartingHP, sAI2_StartingHP;
    public int sAI1_Level, sAI2_Level;
    public int sAI1_Attack, sAI1_Defense, sAI1_Prowess, sAI1_Speed;
    public int sAI2_Attack, sAI2_Defense, sAI2_Prowess, sAI2_Speed;
    public int sAvgNumberOfTurns, sTotalBattles, sAvgDamagePerTurn;
}
