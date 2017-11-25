using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardToolsScript : MonoBehaviour {
    public static RewardToolsScript tools;

    private int totalExpGained = 0;

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

    public void AddEXP(string enemyName)
    {
        totalExpGained += EnemyToolsScript.tools.LookUpEnemy(enemyName).expReward;
    }

    public void ForceEXP()
    {
        GameController.controller.playerEXP += totalExpGained;
    }

    public void SaveReward(Reward reward)
    {
        GameController.controller.rewardEarned = reward;
    }

    public void ClearReward()
    {
        GameController.controller.rewardEarned = null;
    }
}

public class Reward
{
    public bool hasAbility = false;
    public bool hasEquipment = false;
    public string[] ability;
    public string item = "";
    public string[] equipment;
    public int experience = 0;
}
