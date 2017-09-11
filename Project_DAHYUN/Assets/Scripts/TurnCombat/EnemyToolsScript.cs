using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyToolsScript : MonoBehaviour {
    public static EnemyToolsScript tools;

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
                enemyInfo.enemyLevel = 10;
                enemyInfo.ability_1 = "Solar Flare";
                enemyInfo.ability_2 = "Shadow Clone";
                enemyInfo.ability_3 = "";
                enemyInfo.ability_4 = "";

                enemyInfo.enemyAttack = 16;
                enemyInfo.enemyDefense = 16;
                enemyInfo.enemySpeed = 2;
                enemyInfo.enemyMaxHealthBase = 80;
                break;
            default:
                break;
        }

        return enemyInfo;
    }
}
