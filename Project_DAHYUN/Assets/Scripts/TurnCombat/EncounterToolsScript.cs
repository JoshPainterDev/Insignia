﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterToolsScript : MonoBehaviour {
    public static EncounterToolsScript tools;

    // Use this for initialization
    void Awake()
    {
        if (tools == null)
        {
            tools = this;
        }
        else if (tools != this)
        {
            Destroy(gameObject);
        }
    }

    public EnemyEncounter SpecifyEncounter(int stageToLoad, int levelsCompleted)
    {
        EnemyEncounter encounter = new EnemyEncounter();
        encounter.enemyNames = new string[10]; //max number of enemies
        encounter.bossFight = new bool[10]; //max number of enemies in one encounter

        switch (stageToLoad)
        {
            case 1: // BULWARK
                if (levelsCompleted == 0)
                {
                    encounter.encounterNumber = 1;
                    //load first stage
                    encounter.environment = Environment.Castle_Hall;
                    encounter.totalEnemies = 2;
                    encounter.enemyNames[0] = "Skitter";
                    encounter.bossFight[0] = false;

                    encounter.enemyNames[1] = "Skitter";
                    encounter.bossFight[1] = false;

                    encounter.returnOnSuccessScene = "Exposition_Scene06";

                    Reward newReward = new Reward();
                    newReward.experience = 22;
                    encounter.reward = newReward;
                }
                break;
            case 2:
                if (levelsCompleted == 0)
                {
                    encounter.encounterNumber = 2;
                    //load first stage
                    encounter.environment = Environment.Throne_Room;
                    encounter.totalEnemies = 2;
                    encounter.enemyNames[0] = "Solaris Knight";
                    encounter.bossFight[0] = false;
                    encounter.enemyNames[1] = "Solaris Knight";
                    encounter.bossFight[1] = false;
                    encounter.returnOnSuccessScene = "Exposition_Scene13";

                    Reward newReward = new Reward();
                    newReward.experience = 47;
                    encounter.reward = newReward;
                }
                else if (levelsCompleted == 1)
                {
                    encounter.encounterNumber = 3;
                    encounter.environment = Environment.Throne_Room;
                    encounter.totalEnemies = 3;
                    encounter.enemyNames[0] = "Solaris Knight";
                    encounter.bossFight[0] = false;
                    encounter.enemyNames[1] = "Solaris Knight";
                    encounter.bossFight[1] = false;
                    encounter.enemyNames[2] = "Solaris Knight";
                    encounter.bossFight[2] = false;
                    encounter.returnOnSuccessScene = "Exposition_Scene14";

                    Reward newReward = new Reward();
                    newReward.experience = 150;
                    encounter.reward = newReward;
                }
                else if (levelsCompleted == 2)
                {
                    encounter.encounterNumber = 4;
                    encounter.environment = Environment.Throne_Room;
                    encounter.totalEnemies = 1;
                    encounter.enemyNames[0] = "Solaris Officer";
                    encounter.bossFight[0] = true;
                    encounter.returnOnSuccessScene = "Exposition_Scene15";

                    Reward newReward = new Reward();
                    newReward.experience = 2500;
                    encounter.reward = newReward;
                }
                break;
            case 3:
                if (levelsCompleted == 0)
                {
                    encounter.encounterNumber = 5;
                    //load first stage
                    encounter.environment = Environment.Forest_Light;
                    encounter.totalEnemies = 1;
                    encounter.enemyNames[0] = "Shino-Bot";
                    encounter.bossFight[0] = false;
                    encounter.returnOnSuccessScene = "Exposition_Scene18";
                }
                else if (levelsCompleted == 1)
                {
                    encounter.encounterNumber = 6;
                    encounter.environment = Environment.Forest_Light;
                    encounter.totalEnemies = 2;
                    encounter.enemyNames[0] = "Shino-Bot v2";
                    encounter.bossFight[0] = false;
                    encounter.enemyNames[1] = "Shino-Bot v2";
                    encounter.bossFight[1] = false;
                    encounter.returnOnSuccessScene = "Exposition_Scene20";
                }
                else if (levelsCompleted == 2)
                {
                    encounter.encounterNumber = 4;
                    encounter.totalEnemies = 1;
                    encounter.enemyNames[0] = "Solaris Officer";
                    encounter.bossFight[0] = true;
                    encounter.returnOnSuccessScene = "Exposition_Scene15";

                    Reward newReward = new Reward();
                    newReward.experience = 2500;
                    encounter.reward = newReward;
                }
                break;
            default:
                break;
        }

        return encounter;
    }
}
