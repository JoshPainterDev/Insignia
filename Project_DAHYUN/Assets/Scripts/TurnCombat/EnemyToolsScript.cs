using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyToolsScript : MonoBehaviour {
    public static EnemyToolsScript tools;

    public int TRASH_EXP = 15;
    public int MODERATE_EXP = 50;
    public int THICC_EXP = 100;


    public GameObject Steve_Prefab;
    public GameObject Seamstress_Prefab;
    public GameObject Ayo_Prefab;
    public GameObject Slade_Prefab;
    public GameObject Dummy_Prefab;
    public GameObject ShadowAssassin_Prefab;
    public GameObject Skitter_Prefab;
    public GameObject DragonLord_Prefab;

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
            case "Steve":
                enemyInfo.enemyPrefab = ShadowAssassin_Prefab;
                enemyInfo.enemyLevel = 1;
                enemyInfo.expReward = MODERATE_EXP;
                enemyInfo.ability_1 = "Outrage";
                enemyInfo.ability_2 = "Shadow Clone";
                enemyInfo.ability_3 = "Reap";
                enemyInfo.ability_4 = "Final Cut";

                enemyInfo.enemyName = name;
                enemyInfo.enemyAttack = 5;
                enemyInfo.enemyDefense = 2;
                enemyInfo.enemySpeed = 1;
                enemyInfo.enemyMaxHealthBase = 60;
                break;
            case "Shadow Assassin":
                enemyInfo.enemyImageSource = "Animations\\NPCs\\zed_idle01";
                enemyInfo.enemyPrefab = ShadowAssassin_Prefab;
                enemyInfo.enemyLevel = 1;
                enemyInfo.expReward = MODERATE_EXP;
                enemyInfo.ability_1 = "Outrage";
                enemyInfo.ability_2 = "Shadow Clone";
                enemyInfo.ability_3 = "Reap";
                enemyInfo.ability_4 = "Final Cut";

                enemyInfo.enemyName = name;
                enemyInfo.enemyAttack = 5;
                enemyInfo.enemyDefense = 2;
                enemyInfo.enemySpeed = 1;
                enemyInfo.enemyMaxHealthBase = 60;
                break;
            case "Skitter":
                enemyInfo.enemyImageSource = "Animations\\NPCs\\Skitter_Image";
                enemyInfo.enemyPrefab = Skitter_Prefab;
                enemyInfo.enemyLevel = 2;
                enemyInfo.expReward = TRASH_EXP;
                enemyInfo.ability_1 = "Reap";
                enemyInfo.ability_2 = "";
                enemyInfo.ability_3 = "";
                enemyInfo.ability_4 = "";

                enemyInfo.enemyName = name;
                enemyInfo.enemyAttack = 6;
                enemyInfo.enemyDefense = 4;
                enemyInfo.enemySpeed = 1;
                enemyInfo.enemyMaxHealthBase = 80;
                break;
            case "Dragon Lord":
                enemyInfo.enemyImageSource = "Animations\\NPCs\\dragonLord_0004_dragonlord-3";
                enemyInfo.enemyPrefab = DragonLord_Prefab;
                enemyInfo.enemyLevel = 5;
                enemyInfo.expReward = THICC_EXP;
                enemyInfo.ability_1 = "Solar Flare";
                enemyInfo.ability_2 = "Shadow Clone";
                enemyInfo.ability_3 = "Final Cut";
                enemyInfo.ability_4 = "Reap";

                enemyInfo.enemyName = name;
                enemyInfo.enemyAttack = 3;
                enemyInfo.enemyDefense = 3;
                enemyInfo.enemySpeed = 1;
                enemyInfo.enemyMaxHealthBase = 110;
                break;
            case "The Seamstress":
                enemyInfo.enemyImageSource = "Animations\\NPCs\\Skitter_Image";
                enemyInfo.enemyPrefab = Seamstress_Prefab;
                enemyInfo.enemyLevel = 3;
                enemyInfo.expReward = THICC_EXP;
                enemyInfo.ability_1 = "Solar Flare";
                enemyInfo.ability_2 = "Shadow Clone";
                enemyInfo.ability_3 = "Final Cut";
                enemyInfo.ability_4 = "Reap";

                enemyInfo.enemyName = name;
                enemyInfo.enemyAttack = 1;
                enemyInfo.enemyDefense = 1;
                enemyInfo.enemySpeed = 1;
                enemyInfo.enemyMaxHealthBase = 110;
                break;
            case "Dummy":
                enemyInfo.enemyPrefab = Dummy_Prefab;
                enemyInfo.enemyLevel = 1;
                enemyInfo.expReward = MODERATE_EXP;
                enemyInfo.ability_1 = "Outrage";
                enemyInfo.ability_2 = "Shadow Clone";
                enemyInfo.ability_3 = "Reap";
                enemyInfo.ability_4 = "Final Cut";

                enemyInfo.enemyName = "Dummy";
                enemyInfo.enemyAttack = 5;
                enemyInfo.enemyDefense = 5;
                enemyInfo.enemySpeed = 1;
                enemyInfo.enemyMaxHealthBase = 60;
                break;
            default:
                break;
        }

        return enemyInfo;
    }
}
