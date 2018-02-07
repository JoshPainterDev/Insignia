using System.Collections;
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
            case 1: // 5 total stages, last stage is a small boss fight
                if (levelsCompleted == 0)
                {
                    encounter.encounterNumber = 1;
                    //load first stage
                    encounter.environment = Environment.Castel_Hall;
                    encounter.totalEnemies = 2;
                    encounter.enemyNames[0] = "Skitter";
                    encounter.bossFight[0] = false;

                    encounter.enemyNames[1] = "Skitter";
                    encounter.bossFight[1] = false;

                    encounter.returnOnSuccessScene = "Exposition_Scene05";

                    Reward newReward = new Reward();
                    newReward.experience = 22;
                    encounter.reward = newReward;
                }
                else if (levelsCompleted == 1)
                {
                    encounter.encounterNumber = 2;
                    //load first stage
                    encounter.environment = Environment.none;
                    encounter.totalEnemies = 2;
                    encounter.enemyNames[0] = "Solaris Knight";
                    encounter.bossFight[0] = false;
                    encounter.enemyNames[1] = "Solaris Knight";
                    encounter.bossFight[1] = false;
                    encounter.returnOnSuccessScene = "Exposition_Scene14";

                    Reward newReward = new Reward();
                    newReward.experience = 47;
                    encounter.reward = newReward;
                }
                else if (levelsCompleted == 2)
                {
                    encounter.encounterNumber = 3;
                    encounter.environment = Environment.none;
                    encounter.totalEnemies = 1;
                    encounter.enemyNames[0] = "The Seamstress";
                    encounter.bossFight[0] = true;
                    encounter.returnOnSuccessScene = "Exposition_Scene10";

                    Reward newReward = new Reward();
                    newReward.experience = 150;
                    encounter.reward = newReward;
                }
                else if (levelsCompleted == 3)
                {
                    encounter.encounterNumber = 4;
                    encounter.totalEnemies = 1;
                    encounter.enemyNames[0] = "Shadow Assassin";
                    encounter.bossFight[0] = false;

                    Reward newReward = new Reward();
                    newReward.experience = 2500;
                    encounter.reward = newReward;
                }
                else if (levelsCompleted == 4)
                {
                    encounter.encounterNumber = 5;
                    // boss fight
                    encounter.totalEnemies = 1;
                    encounter.enemyNames[0] = "Shadow Assassin";
                    encounter.bossFight[0] = true;

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
