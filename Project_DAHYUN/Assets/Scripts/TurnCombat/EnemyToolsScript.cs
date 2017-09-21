using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyToolsScript : MonoBehaviour {
    public static EnemyToolsScript tools;

    public int TRASH_EXP = 15;
    public int MODERATE_EXP = 50;
    public int THICC_EXP = 100;

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

    public EnemyInfo LookUpEnemy(string name)
    {
        EnemyInfo enemyInfo = new EnemyInfo();

        switch (name)
        {
            case "Shadow Assassin":
                enemyInfo.enemyLevel = 1;
                enemyInfo.expReward = MODERATE_EXP;
                enemyInfo.ability_1 = "Outrage";
                enemyInfo.ability_2 = "Shadow Clone";
                enemyInfo.ability_3 = "Reap";
                enemyInfo.ability_4 = "Final Cut";

                enemyInfo.enemyName = "Shadow Assassin";
                enemyInfo.enemyAttack = 5;
                enemyInfo.enemyDefense = 2;
                enemyInfo.enemySpeed = 2;
                enemyInfo.enemyMaxHealthBase = 60;
                break;
            case "bubber duck":
                enemyInfo.enemyLevel = 1;
                enemyInfo.expReward = TRASH_EXP;
                enemyInfo.ability_1 = "Solar Flare";
                enemyInfo.ability_2 = "Reap";
                enemyInfo.ability_3 = "Final Cut";
                enemyInfo.ability_4 = "";

                enemyInfo.enemyName = "bubber duck";
                enemyInfo.enemyAttack = 3;
                enemyInfo.enemyDefense = 4;
                enemyInfo.enemySpeed = 1;
                enemyInfo.enemyMaxHealthBase = 80;
                break;
            case "that weird guy in the corner of the room":
                enemyInfo.enemyLevel = 1;
                enemyInfo.expReward = THICC_EXP;
                enemyInfo.ability_1 = "Solar Flare";
                enemyInfo.ability_2 = "Shadow Clone";
                enemyInfo.ability_3 = "Final Cut";
                enemyInfo.ability_4 = "Reap";

                enemyInfo.enemyName = "that weird guy in the corner of the room";
                enemyInfo.enemyAttack = 3;
                enemyInfo.enemyDefense = 3;
                enemyInfo.enemySpeed = 1;
                enemyInfo.enemyMaxHealthBase = 110;
                break;
            default:
                break;
        }

        return enemyInfo;
    }
}
