using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_CombatManager_C : MonoBehaviour
{
    // DEFINES
    [HideInInspector]
    public float LIMIT_BREAK_THRESH = 0.2f;
    [HideInInspector]
    public float VULNERABLE_REDUCTION = 0.2f;
    [HideInInspector]
    public float BLINDED_REDUCTION = 66.6f;

    public int iterations = 1;

    private bool running = false;
    private float elapsedTime = 0.0f;
    private int AI1_Wins = 0;
    private int AI2_Wins = 0;

    [HideInInspector]
    public SpecialCase currSpecialCase = SpecialCase.None;

    private int AI1_Level;
    private int AI1_Health;
    private int AI1_MaxHealth;
    private int AI1_Attack, AI1_Defense, AI1_Prowess, AI1_Speed;
    private int AI1_AttackBoost = 0;
    private int AI1_DefenseBoost = 0;
    private int AI1_SpeedBoost = 0;
    private bool AI1_Stunned = false;
    private bool AI1_Vulnerable = false;
    private bool AI1_Blinded = false;
    private Ability AI1_ability1, AI1_ability2, AI1_ability3, AI1_ability4;

    ////////////////////////////////////////////////////////////////
    private int AI2_Level;
    private int AI2_Health;
    private int AI2_MaxHealth;
    private int AI2_Attack, AI2_Defense, AI2_Prowess, AI2_Speed;
    private int AI2_AttackBoost = 0;
    private int AI2_DefenseBoost = 0;
    private int AI2_SpeedBoost = 0;
    private bool AI2_Stunned = false;
    private bool AI2_Vulnerable = false;
    private bool AI2_Blinded = false;
    private Ability AI2_ability1, AI2_ability2, AI2_ability3, AI2_ability4;

    // Use this for initialization
    void Start()
    {
        // AI1 Base Stats
        AI1_Level = 5;
        AI1_MaxHealth = 1000;
        AI1_Attack = 75;
        AI1_Defense = 75;
        AI1_Prowess = 30;
        AI1_Speed = 5;
        // AI1 Boosts
        AI1_AttackBoost = 0;
        AI1_DefenseBoost = 0;
        AI1_SpeedBoost = 0;
        // AI1 Abilities
        AI1_ability1 = AbilityToolsScript.tools.LookUpAbility("Solar Flare");
        AI1_ability2 = AbilityToolsScript.tools.LookUpAbility("Outrage");
        AI1_ability3 = AbilityToolsScript.tools.LookUpAbility("Thunder Charge");
        AI1_ability4 = AbilityToolsScript.tools.LookUpAbility("Guard Break");

        ////////////////////////////////////////////////////////////////

        // AI2 Base Stats
        AI2_Level = 5;
        AI2_MaxHealth = 1000;
        AI2_Attack = 75;
        AI2_Defense = 75;
        AI2_Prowess = 30;
        AI2_Speed = 5;
        // AI2 Boosts
        AI2_AttackBoost = 0;
        AI2_DefenseBoost = 0;
        AI2_SpeedBoost = 5;
        // AI2 Abilities
        AI2_ability1 = AbilityToolsScript.tools.LookUpAbility("Solar Flare");
        AI2_ability2 = AbilityToolsScript.tools.LookUpAbility("Outrage");
        AI2_ability3 = AbilityToolsScript.tools.LookUpAbility("Thunder Charge");
        AI2_ability4 = AbilityToolsScript.tools.LookUpAbility("Guard Break");

        ////////////////////////////////////////////////////////////////
    }

    private void Update()
    {
        //if (running)
        //{
        //    elapsedTime += Time.deltaTime * 100;
        //    print(elapsedTime);
        //}  

        if (Input.GetKeyDown(KeyCode.Space) == true && !running)
        {
            running = true;
            RunSimulation();
        }
    }

    public void RunSimulation()
    {
        for (int i = 0; i < iterations; ++i)
        {
            BeginCombatSimulation();
        }

        EndSimulation();
    }

    public void EndSimulation()
    {
        running = false;
        print("---------------------------- REPORT ----------------------------");
        //print("Elapsed Time: " + (elapsedTime * 100));
        print("AI1 Wins: " + AI1_Wins);
        print("AI1 Wins: " + AI2_Wins);

        SaveStats();

        AI1_Wins = 0;
        AI2_Wins = 0;
    }

    void SaveStats()
    {
        AnalyticsController.controller.AI1_Wins = AI1_Wins;
        AnalyticsController.controller.AI2_Wins = AI2_Wins;
        print(AnalyticsController.controller.AI1_ability4Uses);

        AnalyticsController.controller.SaveData();
        AnalyticsController.controller.LoadCurrentSheet();
    }

    public void BeginCombatSimulation()
    {
        AI1_Health = AI1_MaxHealth;
        AI2_Health = AI2_MaxHealth;

        AI1_AttackBoost = 0;
        AI1_DefenseBoost = 0;
        AI1_SpeedBoost = 0;

        AI2_AttackBoost = 0;
        AI2_DefenseBoost = 0;
        AI2_SpeedBoost = 0;

        // compare speeds
        if (AI1_Speed >= AI2_Speed)
        {
            BeginAI1Turn();
        }
        else
        {
            BeginAI2Turn();
        }
    }

    public void EndBattle(bool AI1_Win)
    {
        if (AI1_Win)
            ++AI1_Wins;
        else
            ++AI2_Wins;
    }

    public void BeginAI1Turn()
    {
        //select a random action
        int rand = Random.Range(1, 5);
        int damageDealt = 0;

        if (AI1_Stunned)
        {
            AI1_Stunned = false;
            BeginAI2Turn();
        }

        switch (rand)
        {
            case 0:
                damageDealt = AI1_UseStrike();
                break;
            case 1:
                if (AI1_ability1.Name != "-")
                {
                    damageDealt = AI1_UseAbility(AI1_ability1);
                    AnalyticsController.controller.AI1_ability1Uses++;
                }
                else
                    damageDealt = AI1_UseStrike();
                break;
            case 2:
                if (AI1_ability2.Name != "-")
                {
                    damageDealt = AI1_UseAbility(AI1_ability2);
                    AnalyticsController.controller.AI1_ability2Uses++;
                }
                else
                    damageDealt = AI1_UseStrike();
                break;
            case 3:
                if (AI1_ability3.Name != "-")
                {
                    damageDealt = AI1_UseAbility(AI1_ability3);
                    AnalyticsController.controller.AI1_ability3Uses++;
                }
                else
                    damageDealt = AI1_UseStrike();
                break;
            case 4:
                if (AI1_ability4.Name != "-")
                {
                    damageDealt = AI1_UseAbility(AI1_ability4);
                    AnalyticsController.controller.AI1_ability4Uses++;
                }
                else
                    damageDealt = AI1_UseStrike();
                break;
        }

        AI2_Health -= damageDealt;

        print("AI1 Damage Dealt: " + damageDealt);
        print("AI2 is now at: " + AI2_Health);

        // is the match over?
        if (AI2_Health <= 0)
        {
            EndBattle(false);
        }
        else
        {
            BeginAI2Turn();
        }
    }

    public void BeginAI2Turn()
    {
        //select a random action
        int rand = Random.Range(1, 5);
        int damageDealt = 0;

        if (AI2_Stunned)
        {
            AI2_Stunned = false;
            BeginAI1Turn();
        }

        switch (rand)
        {
            case 0:
                damageDealt = AI2_UseStrike();
                break;
            case 1:
                if(AI2_ability1.Name != "-")
                {
                    damageDealt = AI2_UseAbility(AI2_ability1);
                    AnalyticsController.controller.AI2_ability1Uses++;
                }                    
                else
                    damageDealt = AI2_UseStrike();
                break;
            case 2:
                if (AI2_ability2.Name != "-")
                {
                    damageDealt = AI2_UseAbility(AI2_ability2);
                    AnalyticsController.controller.AI2_ability2Uses++;
                }
                else
                    damageDealt = AI2_UseStrike();
                break;
            case 3:
                if (AI2_ability3.Name != "-")
                {
                    damageDealt = AI2_UseAbility(AI2_ability3);
                    AnalyticsController.controller.AI2_ability3Uses++;
                }
                else
                    damageDealt = AI2_UseStrike();
                break;
            case 4:
                if (AI2_ability4.Name != "-")
                {
                    damageDealt = AI2_UseAbility(AI2_ability4);
                    AnalyticsController.controller.AI2_ability4Uses++;
                }
                else
                    damageDealt = AI2_UseStrike();
                break;
        }

        AI1_Health -= damageDealt;

        print("AI2 Damage Dealt: " + damageDealt);
        print("AI1 is now at: " + AI1_Health);

        // is the match over?
        if(AI1_Health <= 0)
        {
            EndBattle(true);
        }
        else
        {
            BeginAI1Turn();
        }
    }

    public int AI1_UseStrike()
    {
        int randDamageBuffer = Random.Range(0, 9);
        float attBoostMod = 1;
        float damageDealt = 0;
        int rand = Random.Range(0, 99);
        float accuracy = 70 + (5 * ((AI1_Speed + AI1_SpeedBoost) - (AI2_Speed + AI2_SpeedBoost)));

        if (AI1_Blinded)
        {
            AI1_Blinded = false;
            accuracy -= BLINDED_REDUCTION;
            currSpecialCase = SpecialCase.None;
        }

        if (accuracy >= rand)
        {
            //handle attack boost modifier
            switch (AI1_AttackBoost)
            {
                case 1:
                    attBoostMod = 1.5f;
                    break;
                case 2:
                    attBoostMod = 2;
                    break;
                case 3:
                    attBoostMod = 2.5f;
                    break;
                default:
                    attBoostMod = 1;
                    break;
            }

            damageDealt = ((AI1_Attack * AI1_Attack) + (randDamageBuffer)) * attBoostMod;

            damageDealt -= (AI2_Defense * AI2_DefenseBoost);

            if (damageDealt < 1)
                damageDealt = 1;

            return (int)damageDealt;
        }
        else
            return 0; // we missed

    }

    public int AI1_UseAbility(Ability abilityInfo)
    {
        int rand = Random.Range(0, 100);
        int randDamageBuffer = Random.Range(0, 9);
        float accuracy = abilityInfo.Accuracy;
        float attBoostMod = 1;
        float damageDealt = 0;

        if (AI1_Blinded)
        {
            AI1_Blinded = false;
            accuracy -= BLINDED_REDUCTION;
            currSpecialCase = SpecialCase.None;
        }

        if (abilityInfo.Type == AbilityType.Physical)
        {
            // accuracy check the attack
            if (accuracy >= rand)
            {
                //handle attack boost modifier
                switch (AI1_AttackBoost)
                {
                    case 1:
                        attBoostMod = 1.5f;
                        break;
                    case 2:
                        attBoostMod = 2;
                        break;
                    case 3:
                        attBoostMod = 2.5f;
                        break;
                    default:
                        attBoostMod = 1;
                        break;
                }

                damageDealt = ((AI1_Attack + randDamageBuffer) + (abilityInfo.BaseDamage * AI1_Level)) * attBoostMod;

                damageDealt -= (AI2_Defense * (AI2_DefenseBoost * AI2_DefenseBoost)) / 1.5f;

                if (damageDealt < 1)
                    damageDealt = 1;

                currSpecialCase = abilityInfo.specialCase;

                return (int)damageDealt;
            }
            else
                return 0; // we missed
        }
        else if (abilityInfo.Type == AbilityType.Magical)
        {
            // accuracy check the attack
            if (accuracy >= rand)
            {
                //handle attack boost modifier
                switch (AI1_AttackBoost)
                {
                    case 1:
                        attBoostMod = 1.5f;
                        break;
                    case 2:
                        attBoostMod = 2;
                        break;
                    case 3:
                        attBoostMod = 2.5f;
                        break;
                    default:
                        attBoostMod = 1;
                        break;
                }

                damageDealt = ((AI1_Attack + randDamageBuffer) + (abilityInfo.BaseDamage * AI1_Level)) * attBoostMod;

                damageDealt -= (AI2_Defense * (AI2_DefenseBoost * AI2_DefenseBoost)) / 2.0f;

                if (damageDealt < 1)
                    damageDealt = 1;

                currSpecialCase = abilityInfo.specialCase;

                return (int)damageDealt;
            }
            else
                return 0;
        }
        else
        {
            return 0;
        }
    }

    // AI2 //
    public int AI2_UseStrike()
    {
        int randDamageBuffer = Random.Range(0, 9);
        float attBoostMod = 1;
        float damageDealt = 0;
        int rand = Random.Range(0, 99);
        float accuracy = 70 + (5 * ((AI2_Speed + AI2_SpeedBoost) - (AI1_Speed + AI1_SpeedBoost)));

        if (AI2_Blinded)
        {
            AI2_Blinded = false;
            accuracy -= BLINDED_REDUCTION;
            currSpecialCase = SpecialCase.None;
        }

        if (accuracy >= rand)
        {
            //handle attack boost modifier
            switch (AI2_AttackBoost)
            {
                case 1:
                    attBoostMod = 1.5f;
                    break;
                case 2:
                    attBoostMod = 2;
                    break;
                case 3:
                    attBoostMod = 2.5f;
                    break;
                default:
                    attBoostMod = 1;
                    break;
            }

            damageDealt = ((AI2_Attack * AI2_Attack) + (randDamageBuffer)) * attBoostMod;

            damageDealt -= (AI1_Defense * AI1_DefenseBoost);

            if (damageDealt < 1)
                damageDealt = 1;

            return (int)damageDealt;
        }
        else
            return 0; // we missed

    }

    public int AI2_UseAbility(Ability abilityInfo)
    {
        int rand = Random.Range(0, 100);
        int randDamageBuffer = Random.Range(0, 9);
        float accuracy = abilityInfo.Accuracy;
        float attBoostMod = 1;
        float damageDealt = 0;

        if (AI2_Blinded)
        {
            AI2_Blinded = false;
            accuracy -= BLINDED_REDUCTION;
            currSpecialCase = SpecialCase.None;
        }

        if (abilityInfo.Type == AbilityType.Physical)
        {
            // accuracy check the attack
            if (accuracy >= rand)
            {
                //handle attack boost modifier
                switch (AI2_AttackBoost)
                {
                    case 1:
                        attBoostMod = 1.5f;
                        break;
                    case 2:
                        attBoostMod = 2;
                        break;
                    case 3:
                        attBoostMod = 2.5f;
                        break;
                    default:
                        attBoostMod = 1;
                        break;
                }

                damageDealt = ((AI2_Attack + randDamageBuffer) + (abilityInfo.BaseDamage * AI2_Level)) * attBoostMod;

                damageDealt -= (AI1_Defense * (AI1_DefenseBoost * AI1_DefenseBoost)) / 1.5f;

                if (damageDealt < 1)
                    damageDealt = 1;

                currSpecialCase = abilityInfo.specialCase;

                return (int)damageDealt;
            }
            else
                return 0; // we missed
        }
        else if (abilityInfo.Type == AbilityType.Magical)
        {
            // accuracy check the attack
            if (accuracy >= rand)
            {
                //handle attack boost modifier
                switch (AI2_AttackBoost)
                {
                    case 1:
                        attBoostMod = 1.5f;
                        break;
                    case 2:
                        attBoostMod = 2;
                        break;
                    case 3:
                        attBoostMod = 2.5f;
                        break;
                    default:
                        attBoostMod = 1;
                        break;
                }

                damageDealt = ((AI2_Attack + randDamageBuffer) + (abilityInfo.BaseDamage * AI2_Level)) * attBoostMod;

                damageDealt -= (AI1_Defense * (AI1_DefenseBoost * AI1_DefenseBoost)) / 2.0f;

                if (damageDealt < 1)
                    damageDealt = 1;

                currSpecialCase = abilityInfo.specialCase;

                return (int)damageDealt;
            }
            else
                return 0;
        }
        else
        {
            return 0;
        }
    }
}
