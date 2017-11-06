﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CombatManager : MonoBehaviour {

    // DEFINES
    public float LIMIT_BREAK_THRESH = 0.2f;
    public float VULNERABLE_REDUCTION = 0.2f;
    public float BLINDED_REDUCTION = 66.6f;

    enum State { MainMenu, Retreat, Abilities, Back, Done };

    public bool hasTutorial = false;
    public Canvas canvas;
    public GameObject playerMannequin;
    public GameObject enemyMannequin;
    private Vector3 initPlayerPos;
    public EnemyEncounter encounter;
    private GameObject enemyPrfb;

    //MAIN BUTTON VARIABLES
    public GameObject topButton, leftButton, rightButton, backButton;
    public Color strike_C, retreat_C, abilities_C, back_C;
    public GameObject blackSq;
    private State currentState = State.MainMenu;
    public GameObject Music_Manager;

    //COMBAT VARIABLES
    public SpecialCase currSpecialCase = SpecialCase.None;
    public GameObject playerHealthBar;

    private int playerLevel;
    private int playerHealth = 0;
    [HideInInspector]
    private int playerMaxHealth = 100;
    [HideInInspector]
    public int playerAttackBoost = 0;
    [HideInInspector]
    public int playerDefenseBoost = 0;
    [HideInInspector]
    public int playerSpeedBoost = 0;
    [HideInInspector]
    public bool playerStunned = false;
    [HideInInspector]
    public bool playerVulernable = false;
    [HideInInspector]
    public bool playerBlinded = false;

    // LIMIT BREAK VARIABLES
    [HideInInspector]
    public bool canLimitBreak = false;
    [HideInInspector]
    public LimitBreak playerLimitBreak;
    public LimitBreak enemyLimitBreak;
    [HideInInspector]
    public bool enemyCanLB;

    // ENEMY COMBAT VARIABLES
    public EnemyInfo enemyInfo;
    public GameObject enemyHealthBar;
    private int enemyHealth = 0;
    [HideInInspector]
    public int enemyMaxHealth = 100;
    [HideInInspector]
    public int enemyAttackBoost = 0;
    [HideInInspector]
    public int enemyDefenseBoost = 0;
    [HideInInspector]
    public int enemySpeedBoost = 0;
    [HideInInspector]
    public bool enemyStunned = false;
    [HideInInspector]
    public bool enemyVulernable = false;
    [HideInInspector]
    public bool enemyBlinded = false;
    private Color enemyOrigColor;

    //STRIKE VARIABLES
    public float strikeAnimDuration = 3.5f;
    public float strikePosX = 320f;
    public string strikeMod = "none";
    private float strikeExecutePercent = 0.20f;

    //ABILITY VARIABLES
    public Vector2 ab1_pos, ab2_pos, ab3_pos, ab4_pos;
    public GameObject abilityButtonPrefab;
    public GameObject abilityButton1, abilityButton2, abilityButton3, abilityButton4;
    public Ability ability1, ability2, ability3, ability4;
    public Color abilityTextColor;
    public Color abilitySelectColor;

    //ENCOUNTER VARIABLES
    [HideInInspector]
    public int enemiesRemaining;
    public GameObject enemyCounter;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            int origHealth = enemyHealth;
            enemyHealth = (int)(enemyMaxHealth * 0.25f);

            float var1 = ((float)origHealth / (float)enemyMaxHealth);
            float var2 = ((float)enemyHealth / (float)enemyMaxHealth);
            enemyHealthBar.GetComponent<HealthScript>().Hurt();
            enemyHealthBar.GetComponent<HealthScript>().LerpHealth(var1, var2, (2.5f - (var2 - var1)));
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            int origHealth = playerHealth;
            playerHealth = (int)(playerMaxHealth * 0.01f);

            float var1 = ((float)origHealth / (float)playerMaxHealth);
            float var2 = ((float)playerHealth / (float)playerMaxHealth);
            playerHealthBar.GetComponent<HealthScript>().Hurt();
            playerHealthBar.GetComponent<HealthScript>().LerpHealth(var1, var2, (2.5f - (var2 - var1)));
        }
    }

    // Use this for initialization
    void Start ()
    {
        encounter = GameController.controller.currentEncounter;

        if(encounter == null)
        {
            encounter = new EnemyEncounter();
            encounter.enemyNames = new string[3];
            encounter.totalEnemies = 3;
            encounter.enemyNames[0] = "Dummy";
            encounter.enemyNames[1] = "Skitter";
            encounter.enemyNames[2] = "Shadow Assassin";
            encounter.encounterNumber = -1;
        }
            

        enemiesRemaining = encounter.totalEnemies;
        encounter.returnOnSuccessScene = "AdventureSelect_Scene"; // remove this later 

        //REMOVE THIS LATER
        GameController.controller.Load(GameController.controller.charNames[1]);

        //0. pretend the player has save data for ability sake
        GameController.controller.playerLevel = 1;
        playerHealthBar.transform.GetChild(4).GetComponent<Text>().text = GameController.controller.playerName;

        //1. Load in player and enemy
        playerLevel = GameController.controller.playerLevel;
        //print("player def: " + GameController.controller.playerDefense);
        //Max Health = ((base max hp) * player lv) + (9 * player def)
        playerMaxHealth = (playerMaxHealth * playerLevel) + (9 * GameController.controller.playerDefense);
        playerHealth = playerMaxHealth;
        //print("Player max HP: " + playerHealth);

        if(!hasTutorial)
        {
            GameController.controller.playerAbility1 = AbilityToolsScript.tools.LookUpAbility("Thunder Charge");
            GameController.controller.playerAbility2 = AbilityToolsScript.tools.LookUpAbility("Guard Break");
            GameController.controller.playerAbility3 = AbilityToolsScript.tools.LookUpAbility("Outrage");
            GameController.controller.playerAbility4 = AbilityToolsScript.tools.LookUpAbility("Final Cut");
            GameController.controller.strikeModifier = "Serated Strike";

            ResetEnemyValues();
            //strikeMod = GameController.controller.strikeModifier;

            ability1 = GameController.controller.playerAbility1;
            ability2 = GameController.controller.playerAbility2;
            ability3 = GameController.controller.playerAbility3;
            ability4 = GameController.controller.playerAbility4;

            abilityButton1.GetComponentInChildren<Text>().text = ability1.Name;
            abilityButton2.GetComponentInChildren<Text>().text = ability2.Name;
            abilityButton3.GetComponentInChildren<Text>().text = ability3.Name;
            abilityButton4.GetComponentInChildren<Text>().text = ability4.Name;

            if (GameController.controller.limitBreakTracker == 0)
                canLimitBreak = true;
        }

        initPlayerPos = playerMannequin.transform.position;

        //2. Display buttons: STRIKE, ITEMS, ABILITIES
        DisableAbilityButtons();
        DisableBackButton();
        HideAbilityButtons();
        DisableBackButton();

        if (!hasTutorial)
        {
            //3. Load in initial enemy
            //enemyInfo = EnemyToolsScript.tools.LookUpEnemy(encounter.enemyNames[0]);
            StartCoroutine(LoadNextEnemy(true));
            LoadCharacterLevels(enemyInfo);

            if (GameController.controller.playerSpeed >= enemyInfo.enemySpeed )
                StartCoroutine(ShowStartingButtons());    
        }
    }

    public void AbilitySelected(int selectedOption = 0)
    {
        this.GetComponent<CombatAudio>().playUIAbilitySelect();

        switch(selectedOption)
        {
            case 1:
                StartCoroutine(AbilitySelectAnim(abilityButton1, ability1));
                break;
            case 2:
                StartCoroutine(AbilitySelectAnim(abilityButton2, ability2));
                break;
            case 3:
                StartCoroutine(AbilitySelectAnim(abilityButton3, ability3));
                break;
            case 4:
                StartCoroutine(AbilitySelectAnim(abilityButton4, ability4));
                break;
        }
    }

    public void EndPlayerTurn(bool damageDealt, int originalHP = 0)
    {
        print("PLAYER TURN IS OVER");
        StartCoroutine(CheckForDamage(damageDealt, false, originalHP));
    }

    public void EndEnemyTurn(bool damageDealt, int originalHP = 0)
    {
        print("ENEMY TURN IS OVER");
        StartCoroutine(CheckForDamage(damageDealt, true, originalHP));
    }

    IEnumerator CheckForDamage(bool damage, bool player, int origHP)
    {
        ShowHealthBars();

        if(player)
            print("CHECKING DAMAGE AGAINST PLAYER");
        else
            print("CHECKING DAMAGE AGAINST ENEMY");

        if (ResolveSpecialCase(!player))
        {
            yield return new WaitForSeconds(0.5f);

            if (damage)
            {
                if (player)//is being attacked
                {
                    float var1 = ((float)origHP / (float)playerMaxHealth);
                    float var2 = ((float)playerHealth / (float)playerMaxHealth);
                    playerHealthBar.GetComponent<HealthScript>().Hurt();
                    yield return new WaitForSeconds(0.25f);
                    playerHealthBar.GetComponent<HealthScript>().LerpHealth(var1, var2, (2.5f - (var2 - var1)));
                    playerHealthBar.GetComponent<DamageVisualizer_C>().SpawnDamage(origHP - playerHealth);

                    if (CheckForDeath(false))
                        StartCoroutine(PlayPlayerDeathAnim());
                    else
                        StartCoroutine(StartPlayerTurn());
                }
                else //if enemy is being attacked
                {
                    float var1 = ((float)origHP / (float)enemyMaxHealth);
                    float var2 = ((float)enemyHealth / (float)enemyMaxHealth);
                    enemyHealthBar.GetComponent<HealthScript>().Hurt();
                    yield return new WaitForSeconds(0.25f);
                    enemyHealthBar.GetComponent<HealthScript>().LerpHealth(var1, var2, (2.5f - (var2 - var1)));
                    enemyHealthBar.GetComponent<DamageVisualizer_C>().SpawnDamage(origHP - enemyHealth);

                    if (CheckForDeath(true))
                        StartCoroutine(PlayEnemyDeathAnim());
                    else
                        StartCoroutine(StartEnemyTurn());
                }
            }
            else
            {
                if (player)//is being attacked
                    StartCoroutine(StartPlayerTurn());
                else//enemy is being attacked
                    StartCoroutine(StartEnemyTurn());
            }
        }
        else
        {
            print("got here");
        }
    }

    public void EndEnemyStruggle(bool damageDealt, int originalHP)
    {
        currentState = State.MainMenu;
        DisableAbilityButtons();
        HideAbilityButtons();
        DisableBackButton();
        //enable main buttons?

        StartCoroutine(CheckForDamage(damageDealt, true, originalHP));
    }

    IEnumerator StartPlayerTurn()
    {
        if (playerStunned)
        {
            currentState = State.Done;
            playerStunned = false;
            this.GetComponent<AbilityManager_C>().PlayStunAnim(true);
            currSpecialCase = SpecialCase.None;
            yield return new WaitForSeconds(2);
            StartCoroutine(StartEnemyTurn());
        }
        else
        {
            currentState = State.MainMenu;
            StartCoroutine(ShowMainMenuOptions());
        }
    }

    IEnumerator StartEnemyTurn()
    {
        if (enemyStunned)
        {
            enemyStunned = false;
            this.GetComponent<AbilityManager_C>().PlayStunAnim(false);
            currSpecialCase = SpecialCase.None;
            yield return new WaitForSeconds(2);
            StartCoroutine(StartPlayerTurn());
        }
        else
        {
            HideMainButtons();
            DisableMainButtons();
            yield return new WaitForSeconds(2);
            this.GetComponent<EnemyCombatScript>().BeginEnemyTurn();
        }
    }

    IEnumerator ShowStartingButtons()
    {
        DisableMainButtons();
        HideMainButtons();
        yield return new WaitForSeconds(2);
        ShowMainButtons();
        yield return new WaitForSeconds(0.1f);
        HideMainButtons();
        yield return new WaitForSeconds(0.1f);
        ShowMainButtons();
        EnableMainButtons();
    }

    IEnumerator ShowMainMenuOptions()
    {
        currentState = State.MainMenu;
        DisableAbilityButtons();
        DisableBackButton();
        DisableMainButtons();
        HideAbilityButtons();
        HideMainButtons();

        yield return new WaitForSeconds(0.15f);

        ShowMainButtons();
        EnableMainButtons();
    }

    IEnumerator ShowBackOptions()
    {
        DisableMainButtons();
        HideButton("left");
        yield return new WaitForSeconds(0.1f);
        ShowButton("left");
        yield return new WaitForSeconds(0.1f);
        HideButton("left");
        yield return new WaitForSeconds(0.1f);
        ShowButton("left");
        yield return new WaitForSeconds(0.1f);
        HideMainButtons();

        topButton.GetComponentInChildren<Text>().text = "STRIKE";
        topButton.GetComponent<Image>().color = strike_C;
        leftButton.GetComponentInChildren<Text>().text = "ITEMS";
        leftButton.GetComponent<Image>().color = back_C;
        rightButton.GetComponentInChildren<Text>().text = "ABILITIES";
        rightButton.GetComponent<Image>().color = abilities_C;

        yield return new WaitForSeconds(0.5f);

        ShowMainButtons();
        EnableMainButtons();
    }

    IEnumerator AbilitySelectAnim(GameObject button, Ability abilityUsed)
    {
        Vector3 origPos = button.transform.position;
        Color origColor = button.GetComponent<Image>().color;

        DisableAbilityButtons();
        DisableBackButton();
        HideButton("Back_Button");
        HideHealthBars();

        button.GetComponent<Image>().color = abilitySelectColor;
        Vector3 centerPos = Vector3.zero;
        if (button.name == "TLA1_Button")
        {
            centerPos = button.transform.position + new Vector3(300, 0, 0);
        }
        else if (button.name == "TLA2_Button")
        {
            centerPos = button.transform.position - new Vector3(300, 0, 0);
        }
        else if (button.name == "TLA3_Button")
        {
            centerPos = button.transform.position + new Vector3(300, 0, 0);
        }
        else if (button.name == "TLA4_Button")
        {
            centerPos = button.transform.position - new Vector3(300, 0, 0);
        }

        Vector3 ascend = centerPos + new Vector3(0, 600, 0);

        yield return new WaitForSeconds(0.15f);
        HideAbilityButtons();
        ShowButton(button.name);
        Color seethrough = new Color(abilitySelectColor.r, abilitySelectColor.g, abilitySelectColor.b, 0f);

        button.GetComponent<LerpScript>().LerpToPos(button.transform.position, centerPos, 8f);
        yield return new WaitForSeconds(0.75f);
        button.GetComponent<LerpScript>().LerpToPos(centerPos, ascend, 1.5f);
        button.GetComponent<LerpScript>().LerpToColor(abilitySelectColor, seethrough, 5f);
        Transform child = button.transform.GetChild(0);
        child.gameObject.GetComponent<LerpScript>().LerpToColor(abilityTextColor, Color.clear, 5f);

        yield return new WaitForSeconds(1f);
        HideButton(button.name);
        button.GetComponentInChildren<Text>().color = abilityTextColor;
        button.GetComponent<Image>().color = origColor;
        button.transform.position = origPos;

        this.GetComponent<AbilityManager_C>().AbilityUsed(abilityUsed, enemyHealth);
    }

    // Combat Functions
    ////////////////////////////////////////////
    public bool CheckForDeath(bool EnemyCheck)
    {
        canLimitBreak = false;

        if(EnemyCheck)
        {
            // check if the enemy was defeated
            if(enemyHealth <= 0)
            {
                if(enemyCanLB)
                {
                    //limit break voodoo
                    enemyHealth = (int)(enemyMaxHealth * 0.25f);
                }
                else //enemy is now dead
                {
                    enemyHealth = 0;
                    RewardToolsScript.tools.AddEXP(encounter.enemyNames[encounter.totalEnemies - enemiesRemaining]);
                    print("Oh jeez Rick, " + (encounter.enemyNames[encounter.totalEnemies - enemiesRemaining]) + " is dead...");
                    --enemiesRemaining;
                    enemyCounter.GetComponent<enemyCounterScript>().EnemyDied();
                    return true;
                }
            }
        }
        else
        {
            // check if the player has triggered a Limit Break
            if(canLimitBreak && ((float)playerHealth / (float)playerMaxHealth) <= LIMIT_BREAK_THRESH)
            {
                print("THIS IS IMPOSSIBLE?!?!?!");
                canLimitBreak = false;
                playerLimitBreak = this.GetComponent<LimitBreakManager_C>().LookUpLimitBreak(GameController.controller.limitBreakModifier);
                GameController.controller.limitBreakTracker = playerLimitBreak.coolDown;
                this.GetComponent<LimitBreakManager_C>().UseLimitBreak(playerLimitBreak);
            }
            else 
            {
                if(playerHealth <= 0) //player died
                {
                    playerHealth = 0;
                    StartCoroutine(PlayPlayerDeathAnim());
                    return true;
                }
            }
        }

        return false;
    }

    IEnumerator UseStrike()
    {
        int exectueVar = EvaluateExecution();
        int rand = Random.Range(0, 99);
        float accuracy = 70 + (5 * ((GameController.controller.playerSpeed + playerSpeedBoost) - (enemyInfo.enemySpeed + enemySpeedBoost)));

        DisableMainButtons();
        HideHealthBars();
        HideButton("top");
        yield return new WaitForSeconds(0.1f);
        ShowButton("top");
        yield return new WaitForSeconds(0.1f);
        HideButton("top");
        yield return new WaitForSeconds(0.1f);
        ShowButton("top");
        yield return new WaitForSeconds(0.1f);
        HideMainButtons();
        yield return new WaitForSeconds(0.25f);

        if (playerBlinded)
        {
            playerBlinded = false;
            accuracy -= BLINDED_REDUCTION;
            currSpecialCase = SpecialCase.None;
            this.GetComponent<StrikeManager_C>().PlayerStrikeMiss();
        }

        if (exectueVar != 1)
        {
            print("ACCURACY: " + accuracy);

            // accuracy check the attack
            if (accuracy > rand)
            {
                if (exectueVar == 0)
                {
                    this.GetComponent<StrikeManager_C>().StrikeUsed(strikeMod, enemyHealth);
                }
                else
                {
                    print("percentage of strike used");
                    //do only a percentage of your full strike damage 
                    this.GetComponent<StrikeManager_C>().StrikeUsed(strikeMod, enemyHealth, 0.33f);
                }
            }
            else
            {
                print("whoops we missed...");
                this.GetComponent<StrikeManager_C>().PlayerStrikeMiss();
            }
        }
        else
        {
            HideHealthBars();
            this.GetComponent<StruggleManager_C>().BeginStruggle_Player();
        }
    }

    int EvaluateExecution(int overrideDamage = 0)
    {
        int attack = GameController.controller.playerAttack;
        int prowess = GameController.controller.playerProwess;
        float attBoostMod = 1;

        switch(playerAttackBoost)
        {
            case 1:
                attBoostMod = 1.5f;
                    break;
            case 2:
                attBoostMod = 2f;
                break;
            case 3:
                attBoostMod = 2.5f;
                break;
            default:
                attBoostMod = 1;
                break;
        }

        float damageDealt = 0;
        float percentRemaining = 0;
        float percentDealt = 0;

        if (overrideDamage != 0)
        {
            strikeExecutePercent = .15f + (((GameController.controller.playerProwess - (enemyInfo.enemyDefense / 2)) + 0.01f) / GameController.controller.playerProwess);
            if (strikeExecutePercent < 0.15f)
                strikeExecutePercent = .15f;
            percentRemaining = ((float)enemyHealth / (float)enemyMaxHealth);
            percentDealt = overrideDamage / (float)enemyMaxHealth;

            print("strikeExecutePercent: " + strikeExecutePercent);
            print("percentRemaining: " + percentRemaining);

            if(percentDealt > percentRemaining)
            {
                if (percentRemaining > strikeExecutePercent)
                    return 2;
            }
        }
        else //Do the normal execution calculation
        {
            strikeExecutePercent = .15f + (((GameController.controller.playerProwess - (enemyInfo.enemyDefense / 2)) + 0.01f) / GameController.controller.playerProwess);
            if (strikeExecutePercent < 0.15f)
                strikeExecutePercent = .15f;

            damageDealt = (attack * attack) * attBoostMod;
            percentRemaining = ((float)enemyHealth / (float)enemyMaxHealth);
            percentDealt = damageDealt / (float)enemyMaxHealth;

            print("strikeExecutePercent: " + strikeExecutePercent);
            print("percent Dealt: " + percentDealt);
            print("percentRemaining: " + percentRemaining);
            if (percentDealt > percentRemaining)
            {
                if (percentRemaining > strikeExecutePercent)
                {
                    print("returning 2");
                    return 2;
                }  
            }
        }

        print("percent damage: " + percentDealt);

        if(enemyVulernable)
        {
            percentRemaining -= VULNERABLE_REDUCTION;
            print("vulnerable execution percent: " + percentRemaining);
            enemyVulernable = false;
        }

        // check if the player can press the enemy
        if (percentDealt >= percentRemaining)
            return 1;

        return 0;
    }

    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Player DAMAGE FUNCTIONS

    // DAMAGE AN ENEMY WITH STRIKE ONLY!
    public void DamageEnemy_Strike(float percentOfDamage = 1.0f)
    {
        int randDamageBuffer = Random.Range(0, 9);
        float attBoostMod = 1;
        float damageDealt = 0;
        int attack = GameController.controller.playerAttack;
        int defense = GameController.controller.playerDefense;
        int prowess = GameController.controller.playerProwess;


        //handle attack boost modifier
        switch (playerAttackBoost)
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

        damageDealt = ((attack * attack) + (randDamageBuffer)) * attBoostMod;
            
        damageDealt -= (enemyInfo.enemyDefense * enemyDefenseBoost);

        damageDealt *= percentOfDamage;

        if (damageDealt < 1)
            damageDealt = 1;

        enemyHealth -= (int)damageDealt;

        print("PLAYER DAMAGE: " + damageDealt);
        print("enemy is now at: " + enemyHealth);
    }

    public void ExecuteEnemy_Strike()
    {
        int origHP = enemyHealth;
        //spawn effects or something idk
        enemyHealth = 0;
        EndPlayerTurn(true, origHP);
    }

    // DAMAGE AN ENEMY WITH AN ABILITY ONLY!
    public void DamageEnemy_Ability(Ability abilityUsed)
    {
        int rand = Random.Range(0, 100);
        int randDamageBuffer = Random.Range(0, 9);
        float accuracy = abilityUsed.Accuracy;
        float attBoostMod = 1;
        float damageDealt = 0;
        int attack = GameController.controller.playerAttack;
        int defense = GameController.controller.playerDefense;
        int prowess = GameController.controller.playerProwess;

        if (hasTutorial)
            return;

        if (playerBlinded)
        {
            playerBlinded = false;
            accuracy -= BLINDED_REDUCTION;
            currSpecialCase = SpecialCase.None;
        }

        if (abilityUsed.Type == AbilityType.Physical)
        {
            // accuracy check the attack
            if (accuracy > rand)
            {
                //handle attack boost modifier
                switch (playerAttackBoost)
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

                damageDealt = ((attack + randDamageBuffer) + (abilityUsed.BaseDamage * playerLevel)) * attBoostMod;

                damageDealt -= (enemyInfo.enemyDefense * (enemyDefenseBoost * enemyDefenseBoost)) / 1.5f;

                if (damageDealt < 1)
                    damageDealt = 1;

                enemyHealth -= (int)damageDealt;

                print("Ability damage: " + damageDealt);
                print("enemy HP: " + enemyHealth);
            }
        }
        else
        {
            // accuracy check the attack
            if (accuracy > rand)
            {
                //handle attack boost modifier
                switch (playerAttackBoost)
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

                damageDealt = ((attack + randDamageBuffer) + (abilityUsed.BaseDamage * playerLevel)) * attBoostMod;
                print("pAttack: " + attack);
                print("eDefense: " + enemyInfo.enemyDefense);
                damageDealt -= (enemyInfo.enemyDefense * (enemyDefenseBoost * enemyDefenseBoost)) / 2.0f;

                if (damageDealt < 1)
                    damageDealt = 1;

                enemyHealth -= (int)damageDealt;

                print("damage: " + damageDealt);
                print("enemy HP: " + enemyHealth);
            }
        }
    }

    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// ENEMY DAMAGE FUNCTIONS
    /// 
        // DAMAGE PLAYER WITH STRIKE ONLY!
    public void DamagePlayer_Strike(float percentOfDamage = 1.0f)
    {
        int randDamageBuffer = Random.Range(0, 9);
        float attBoostMod = 1;
        float damageDealt = 0;
        int attack = enemyInfo.enemyAttack;
        int defense = enemyInfo.enemyDefense;
        int prowess = enemyInfo.enemyProwess;

        //handle attack boost modifier
        switch (playerAttackBoost)
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

        damageDealt = ((attack * attack) + (randDamageBuffer)) * attBoostMod;

        damageDealt -= ((GameController.controller.playerDefense) * (GameController.controller.playerDefense * playerDefenseBoost)) / 2f;

        damageDealt *= percentOfDamage;

        if (damageDealt < 1)
            damageDealt = 1;

        playerHealth -= (int)damageDealt;

        print("ENEMY DAMAGE: " + damageDealt);
        print("player is now at: " + playerHealth);
    }

    public void DamagePlayer_Ability(Ability abilityUsed)
    {
        int rand = Random.Range(0, 100);
        int randDamageBuffer = Random.Range(0, 9);
        float accuracy = abilityUsed.Accuracy;
        float attBoostMod = 1;
        float damageDealt = 0;
        int attack = enemyInfo.enemyAttack;
        int defense = enemyInfo.enemyDefense;
        int prowess = enemyInfo.enemyProwess;

        if (enemyBlinded)
        {
            enemyBlinded = false;
            accuracy -= BLINDED_REDUCTION;
            currSpecialCase = SpecialCase.None;
        }

        if (abilityUsed.Type == AbilityType.Physical)
        {
            // accuracy check the attack
            if (accuracy > rand)
            {
                //handle attack boost modifier
                switch (enemyAttackBoost)
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

                damageDealt = ((attack + randDamageBuffer) * abilityUsed.BaseDamage) * attBoostMod;

                damageDealt -= (GameController.controller.playerDefense * (playerDefenseBoost * playerDefenseBoost));

                if (damageDealt < 1)
                    damageDealt = 1;

                playerHealth -= (int)damageDealt;

                print("damage: " + damageDealt);
                print("player HP: " + playerHealth);
            }
        }
        else
        {
            // accuracy check the attack
            if (accuracy > rand)
            {
                //handle attack boost modifier
                switch (playerAttackBoost)
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

                damageDealt = ((attack * abilityUsed.BaseDamage) + (randDamageBuffer)) * attBoostMod;

                damageDealt -= (GameController.controller.playerDefense * (playerDefenseBoost));

                if (damageDealt < 1)
                    damageDealt = 1;

                playerHealth -= (int)damageDealt;
            }
        }
    }

    /// END OF DAMAGE FUNCTIONS
    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void StruggleFailed(bool playerFailed)
    {
        int originalHP;

        if (playerFailed)
        {
            originalHP = enemyHealth;
            enemyHealth -= (int)((float)enemyHealth * 0.35f);
            EndPlayerTurn(true, originalHP);
        }
        else
        {
            originalHP = playerHealth;
            playerHealth -= (int)((float)playerHealth * 0.35f);
            EndEnemyStruggle(true, originalHP);
        }
    }

    public void HealPlayer()
    {

    }

    bool ResolveSpecialCase(bool playerTurn)
    {
        print("Current SC: " + currSpecialCase + ", " + playerTurn);

        switch(currSpecialCase)
        {
            case SpecialCase.None:
                break;
            case SpecialCase.Ablaze:
                break;
            case SpecialCase.Blind://done (not tested)
                if (playerTurn)
                    enemyBlinded = true;
                else
                    playerBlinded = true;
                break;
            case SpecialCase.Execute://
                currSpecialCase = SpecialCase.None;
                print("pProwess: " + (GameController.controller.playerProwess));
                int executeDamage = ((GameController.controller.playerProwess) * AbilityToolsScript.tools.LookUpAbility("Final Cut").SpecialValue);
                print("executeDamage: " + executeDamage);
                print("enemy hp: " + enemyHealth);
                
                if (enemyHealth > 0)
                {
                    if (EvaluateExecution(executeDamage) == 1)
                    {
                        if (playerTurn)
                        {
                            this.GetComponent<StruggleManager_C>().BeginStruggle_Player();
                            return false;
                        }
                    }
                }
                break;
            case SpecialCase.Deceive: //
                break;
            case SpecialCase.Outrage://done
                int outrageDmg;
                float boost = 0;
                int baseDamage = AbilityToolsScript.tools.LookUpAbility("Outrage").SpecialValue;

                if (playerTurn)
                {
                    baseDamage *= playerAttackBoost;
                    switch (playerAttackBoost)
                    {
                        case 0:
                            boost = 1.0f;
                            break;
                        case 1:
                            boost = 1.25f;
                            break;
                        case 2:
                            boost = 1.75f;
                            break;
                        case 3:
                            boost = 2.15f;
                            break;
                    }

                    print("playerAttackBoost: " + playerAttackBoost);
                    outrageDmg =  (int)Mathf.Pow((playerLevel), boost) + baseDamage;
                    print("Outrage Damage: " + outrageDmg);
                    enemyHealth -= outrageDmg;
                }
                else
                {
                    baseDamage *= enemyAttackBoost;
                    switch (enemyAttackBoost)
                    {
                        case 0:
                            boost = 1.0f;
                            break;
                        case 1:
                            boost = 1.25f;
                            break;
                        case 2:
                            boost = 1.75f;
                            break;
                        case 3:
                            boost = 2.15f;
                            break;
                    }
                    outrageDmg = (int)Mathf.Pow((enemyInfo.enemyLevel), boost) + baseDamage;
                    playerHealth -= outrageDmg;
                }
                break;
            case SpecialCase.ShadowClone:
                break;
            case SpecialCase.StunFoe://done

                if (playerTurn)
                {
                    enemyStunned = true;
                }
                else
                {
                    playerStunned = true;
                }
                    
                break;
            case SpecialCase.StunSelf://done
                if (playerTurn)
                    playerStunned = true;
                else
                    enemyStunned = true;
                break;
        }

        return true;
    }

    /// Helper Functions
    /// ////////////////////////////////////////

    IEnumerator PlayEnemyDeathAnim()
    {
        yield return new WaitForSeconds(1.5f);
        this.enemyHealthBar.GetComponent<HealthScript>().Death();
        this.GetComponent<EnemyCombatScript>().PlayDeathAnim();
        yield return new WaitForSeconds(0.5f);
        foreach (SpriteRenderer sprite in enemyMannequin.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.color = Color.clear;
        }

        yield return new WaitForSeconds(0.1f);

        foreach (SpriteRenderer sprite in enemyMannequin.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.color = enemyOrigColor;
        }

        yield return new WaitForSeconds(0.1f);

        foreach (SpriteRenderer sprite in enemyMannequin.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.color = Color.clear;
        }

        yield return new WaitForSeconds(0.1f);

        foreach (SpriteRenderer sprite in enemyMannequin.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.color = enemyOrigColor;
        }

        yield return new WaitForSeconds(0.1f);

        foreach (LerpScript lerp in enemyMannequin.GetComponentsInChildren<LerpScript>())
        {
            lerp.LerpToColor(enemyOrigColor, Color.clear, 1.5f);
        }

        yield return new WaitForSeconds(2f);

        CheckForMoreEnemies();
    }

    private void CheckForMoreEnemies()
    {
        print("enemies remaining: " + enemiesRemaining);
        if(enemiesRemaining > 0)
        {
            StartCoroutine(LoadNextEnemy());
        }
        else // player won! go back YOU WIN!
        {
            RewardToolsScript.tools.StoreReward(encounter.reward);
            StartCoroutine(LoadSuccessScene());
        }
    }

    IEnumerator LoadSuccessScene()
    {
        // this could get complicated depending on where I'm supposed to return to
        // store the return level in the game controller
        blackSq.GetComponent<FadeScript>().FadeIn();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("CombatReward_Scene");
    }

    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Enemy loading
    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    IEnumerator LoadNextEnemy(bool initialEnemy = false)
    {
        if(enemyPrfb)
            Destroy(enemyPrfb);

        enemyInfo = EnemyToolsScript.tools.LookUpEnemy(encounter.enemyNames[encounter.totalEnemies - enemiesRemaining]);
        enemyPrfb = Instantiate(enemyInfo.enemyPrefab, enemyMannequin.transform.position, Quaternion.identity) as GameObject;
        enemyPrfb.transform.SetParent(enemyMannequin.transform);
        enemyPrfb.transform.localScale = Vector3.one;
        enemyOrigColor = enemyPrfb.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        enemyHealthBar.GetComponent<HealthScript>().setColors(enemyOrigColor);

        ResetEnemyValues();

        if (enemiesRemaining != encounter.totalEnemies)
        {
            if(!initialEnemy)
            {
                foreach (SpriteRenderer sprite in enemyPrfb.GetComponentsInChildren<SpriteRenderer>())
                {
                    sprite.enabled = false;
                }

                yield return new WaitForSeconds(1f);

                foreach (SpriteRenderer sprite in enemyPrfb.GetComponentsInChildren<SpriteRenderer>())
                {
                    sprite.enabled = true;
                }

                yield return new WaitForSeconds(1.15f);

                //print("My spd: " + GameController.controller.playerSpeed + " Enemy spd: " + enemyInfo.enemySpeed);
                if ((GameController.controller.playerSpeed + playerSpeedBoost) >= enemyInfo.enemySpeed)
                    StartCoroutine(StartPlayerTurn());
                else
                    StartCoroutine(StartEnemyTurn());
            }

            enemyHealthBar.GetComponent<HealthScript>().StartAnim();
            LoadCharacterLevels(enemyInfo);
        }
        else
        {
            //if(!initialEnemy)
            //{
            //    foreach (SpriteRenderer sprite in enemyPrfb.GetComponentsInChildren<SpriteRenderer>())
            //        sprite.enabled = false;

            //    yield return new WaitForSeconds(0.25f);

            //    foreach (SpriteRenderer sprite in enemyPrfb.GetComponentsInChildren<SpriteRenderer>())
            //        sprite.enabled = true;

            //    yield return new WaitForSeconds(1.15f);

            //    if ((GameController.controller.playerSpeed + playerSpeedBoost) >= enemyInfo.enemySpeed)
            //        StartCoroutine(StartPlayerTurn());
            //    else
            //        StartCoroutine(StartEnemyTurn());
            //}

            enemyHealthBar.GetComponent<HealthScript>().StartAnim();
            LoadCharacterLevels(enemyInfo);
        }
    }

    public void ResetEnemyValues()
    {
        if (enemyInfo == null)
            enemyInfo = EnemyToolsScript.tools.LookUpEnemy(encounter.enemyNames[0]); //start with the initial enemy lookup
        else
            enemyInfo = EnemyToolsScript.tools.LookUpEnemy(encounter.enemyNames[encounter.totalEnemies - enemiesRemaining]);

        EnemyInfo tempInfo = enemyInfo;
        tempInfo.enemyAttack = tempInfo.enemyAttack * tempInfo.enemyLevel;
        tempInfo.enemyDefense = tempInfo.enemyDefense * tempInfo.enemyLevel;
        tempInfo.enemyProwess = tempInfo.enemyProwess * tempInfo.enemyLevel;
        tempInfo.enemySpeed = (tempInfo.enemySpeed * tempInfo.enemyLevel) / 2;
        enemyInfo = tempInfo;

        enemyCanLB = enemyInfo.canLimitBreak;
        currSpecialCase = SpecialCase.None;
        enemyStunned = false;
        enemyBlinded = false;
        enemyVulernable = false;
        enemyAttackBoost = 0;
        enemyDefenseBoost = 0;
        enemySpeedBoost = 0;

        enemyMaxHealth = (enemyInfo.enemyMaxHealthBase * enemyInfo.enemyLevel) + (9 * enemyInfo.enemyDefense);
        enemyHealth = enemyMaxHealth;
        enemyCanLB = enemyInfo.canLimitBreak;
    }

    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Enemy loading
    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    IEnumerator PlayPlayerDeathAnim()
    {
        yield return new WaitForSeconds(1.5f);
        this.playerHealthBar.GetComponent<HealthScript>().Death();
        yield return new WaitForSeconds(3.5f);
        blackSq.GetComponent<FadeScript>().FadeIn();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("AdventureSelect_Scene");
    }

        public void TopSelected()
    {
        this.GetComponent<CombatAudio>().playUISelect();

        switch (currentState)
        {
            case State.MainMenu:
                currentState = State.Done;
                StartCoroutine(UseStrike());
                break;
        }
    }

    public void RightSelected()
    {
        this.GetComponent<CombatAudio>().playUISelect();

        switch (currentState)
        {
            case State.MainMenu:
                currentState = State.Abilities;
                HideMainButtons();
                DisableMainButtons();
                EnableBackButton();
                ShowAbilityButtons();
                EnableAbilityButtons();
                break;
        }
    }

    public void RetreatSelected()
    {
        this.GetComponent<CombatAudio>().playUISelect();

        switch (currentState)
        {
            case State.MainMenu:
                currentState = State.Retreat;
                DisableMainButtons();
                EnableBackButton();
                SpawnItemsUI();
                break;
        }
    }

    public void BackSelected()
    {
        this.GetComponent<CombatAudio>().playUIBack();

        switch (currentState)
        {
            case State.Abilities:
                StartCoroutine(ShowMainMenuOptions());
                break;
            case State.Retreat:
                StartCoroutine(ShowMainMenuOptions());
                break;
        }
    }

    public void HideMainButtons()
    {
        topButton.GetComponent<Image>().enabled = false;
        leftButton.GetComponent<Image>().enabled = false;
        rightButton.GetComponent<Image>().enabled = false;

        topButton.GetComponentInChildren<Text>().enabled = false;
        leftButton.GetComponentInChildren<Text>().enabled = false;
        rightButton.GetComponentInChildren<Text>().enabled = false;
    }

    public void ShowMainButtons()
    {
        topButton.GetComponent<Image>().enabled = true;
        leftButton.GetComponent<Image>().enabled = true;
        rightButton.GetComponent<Image>().enabled = true;

        topButton.GetComponentInChildren<Text>().enabled = true;
        leftButton.GetComponentInChildren<Text>().enabled = true;
        rightButton.GetComponentInChildren<Text>().enabled = true;
    }

    void HideButton(string buttonName)
    {
        switch (buttonName)
        {
            case "top":
                topButton.GetComponent<Image>().enabled = false;
                topButton.GetComponentInChildren<Text>().enabled = false;
                break;
            case "left":
                leftButton.GetComponent<Image>().enabled = false;
                leftButton.GetComponentInChildren<Text>().enabled = false;
                break;
            case "right":
                rightButton.GetComponent<Image>().enabled = false;
                rightButton.GetComponentInChildren<Text>().enabled = true;
                break;
            case "TLA1_Button":
                abilityButton1.GetComponent<Image>().enabled = false;
                abilityButton1.GetComponentInChildren<Text>().enabled = false;
                break;
            case "TLA2_Button":
                abilityButton2.GetComponent<Image>().enabled = false;
                abilityButton2.GetComponentInChildren<Text>().enabled = false;
                break;
            case "TLA3_Button":
                abilityButton3.GetComponent<Image>().enabled = false;
                abilityButton3.GetComponentInChildren<Text>().enabled = false;
                break;
            case "TLA4_Button":
                abilityButton4.GetComponent<Image>().enabled = false;
                abilityButton4.GetComponentInChildren<Text>().enabled = false;
                break;
            default:
                break;
        }
    }

    void ShowButton(string buttonName)
    {
        switch (buttonName)
        {
            case "top":
                topButton.GetComponent<Image>().enabled = true;
                topButton.GetComponentInChildren<Text>().enabled = true;
                break;
            case "left":
                leftButton.GetComponent<Image>().enabled = true;
                leftButton.GetComponentInChildren<Text>().enabled = true;
                break;
            case "right":
                rightButton.GetComponent<Image>().enabled = true;
                rightButton.GetComponentInChildren<Text>().enabled = true;
                break;
            case "TLA1_Button":
                abilityButton1.GetComponent<Image>().enabled = true;
                abilityButton1.GetComponentInChildren<Text>().enabled = true;
                break;
            case "TLA2_Button":
                abilityButton2.GetComponent<Image>().enabled = true;
                abilityButton2.GetComponentInChildren<Text>().enabled = true;
                break;
            case "TLA3_Button":
                abilityButton3.GetComponent<Image>().enabled = true;
                abilityButton3.GetComponentInChildren<Text>().enabled = true;
                break;
            case "TLA4_Button":
                abilityButton4.GetComponent<Image>().enabled = true;
                abilityButton4.GetComponentInChildren<Text>().enabled = true;
                break;
            default:
                break;
        }
    }

    public void EnableMainButtons()
    {
        topButton.GetComponentInChildren<Button>().enabled = true;
        leftButton.GetComponentInChildren<Button>().enabled = true;
        rightButton.GetComponentInChildren<Button>().enabled = true;
    }

    public void DisableMainButtons()
    {
        topButton.GetComponentInChildren<Button>().enabled = false;
        leftButton.GetComponentInChildren<Button>().enabled = false;
        rightButton.GetComponentInChildren<Button>().enabled = false;
    }

    public void EnableBackButton()
    {
        backButton.GetComponent<Image>().enabled = true;
        backButton.GetComponent<Button>().enabled = true;
        backButton.GetComponentInChildren<Text>().enabled = true;
    }

    public void DisableBackButton()
    {
        backButton.GetComponent<Image>().enabled = false;
        backButton.GetComponent<Button>().enabled = false;
        backButton.GetComponentInChildren<Text>().enabled = false;
    }

    public void EnableAbilityButtons()
    {
        abilityButton1.GetComponent<Button>().enabled = true;
        abilityButton2.GetComponent<Button>().enabled = true;
        abilityButton3.GetComponent<Button>().enabled = true;
        abilityButton4.GetComponent<Button>().enabled = true;
    }

    public void DisableAbilityButtons()
    {
        abilityButton1.GetComponent<Button>().enabled = false;
        abilityButton2.GetComponent<Button>().enabled = false;
        abilityButton3.GetComponent<Button>().enabled = false;
        abilityButton4.GetComponent<Button>().enabled = false;
    }

    public void ShowAbilityButtons()
    {
        abilityButton1.GetComponent<Image>().enabled = true;
        abilityButton2.GetComponent<Image>().enabled = true;
        abilityButton3.GetComponent<Image>().enabled = true;
        abilityButton4.GetComponent<Image>().enabled = true;

        abilityButton1.GetComponentInChildren<Text>().enabled = true;
        abilityButton2.GetComponentInChildren<Text>().enabled = true;
        abilityButton3.GetComponentInChildren<Text>().enabled = true;
        abilityButton4.GetComponentInChildren<Text>().enabled = true;
    }

    public void HideAbilityButtons()
    {
        abilityButton1.GetComponent<Image>().enabled = false;
        abilityButton2.GetComponent<Image>().enabled = false;
        abilityButton3.GetComponent<Image>().enabled = false;
        abilityButton4.GetComponent<Image>().enabled = false;

        abilityButton1.GetComponentInChildren<Text>().enabled = false;
        abilityButton2.GetComponentInChildren<Text>().enabled = false;
        abilityButton3.GetComponentInChildren<Text>().enabled = false;
        abilityButton4.GetComponentInChildren<Text>().enabled = false;
    }

    public void HideHealthBars()
    {
        playerHealthBar.GetComponent<Image>().enabled = false;
        playerHealthBar.transform.GetChild(3).GetComponent<Text>().enabled = false;
        playerHealthBar.transform.GetChild(4).GetComponent<Text>().enabled = false;
        foreach (Image img in playerHealthBar.GetComponentsInChildren<Image>())
        {
            img.enabled = false;
        }

        enemyHealthBar.GetComponent<Image>().enabled = false;
        foreach (Image img in enemyHealthBar.GetComponentsInChildren<Image>())
        {
            img.enabled = false;
        }

        foreach (Text text in enemyHealthBar.GetComponentsInChildren<Text>())
        {
            text.enabled = false;
        }
    }

    public void ShowHealthBars()
    {
        playerHealthBar.GetComponent<Image>().enabled = true;
        playerHealthBar.transform.GetChild(2).GetComponent<Image>().enabled = true;
        playerHealthBar.transform.GetChild(3).GetComponent<Text>().enabled = true;
        playerHealthBar.transform.GetChild(4).GetComponent<Text>().enabled = true;
        foreach (Image img in playerHealthBar.GetComponentsInChildren<Image>())
        {
            img.enabled = true;
        }

        enemyHealthBar.GetComponent<Image>().enabled = true;
        foreach (Image img in enemyHealthBar.GetComponentsInChildren<Image>())
        {
            img.enabled = true;
        }

        foreach (Text text in enemyHealthBar.GetComponentsInChildren<Text>())
        {
            text.enabled = true;
        }
    }

    void SpawnItemsUI()
    {
        GameController.controller.playerInventory = new string[3];
        GameController.controller.playerInventoryQuantity = new int[3];

        GameController.controller.playerInventory[0] = "woa";
        GameController.controller.playerInventory[1] = "Premium Quality cheese";
        GameController.controller.playerInventory[2] = "a shoe";

        GameController.controller.playerInventoryQuantity[0] = 1;
        GameController.controller.playerInventoryQuantity[1] = 69;
        GameController.controller.playerInventoryQuantity[2] = 1;

        for (int buttonNum = 0; buttonNum < GameController.controller.playerInventory.Length; ++buttonNum)
        {
            Vector2 newpos = new Vector2(200 * buttonNum, 0);
            GameObject testB = Instantiate(abilityButtonPrefab, newpos, Quaternion.identity) as GameObject;
            testB.transform.SetParent(canvas.transform);
            testB.name = "ItemButton" + buttonNum + "_" + GameController.controller.playerInventory[buttonNum];
            testB.GetComponentInChildren<Text>().text = GameController.controller.playerInventory[buttonNum];
        }
    }

    public void LoadCharacterLevels(EnemyInfo enemyInfo)
    {
        playerHealthBar.transform.GetChild(3).GetComponent<Text>().text = "Lv " + GameController.controller.playerLevel;
        enemyHealthBar.transform.GetChild(3).GetComponent<Text>().text = "Lv " + enemyInfo.enemyLevel;
        enemyHealthBar.transform.GetChild(3).GetComponent<Text>().enabled = true;
        enemyHealthBar.transform.GetChild(3).GetComponent<Text>().color = Color.white;

        enemyHealthBar.transform.GetChild(4).GetComponent<Text>().text = enemyInfo.enemyName;
        enemyHealthBar.transform.GetChild(4).GetComponent<Text>().enabled = true;
        enemyHealthBar.transform.GetChild(4).GetComponent<Text>().color = Color.white;
    }

    public int getPlayerHealth()
    {
        return playerHealth;
    }

    public int getEnemyHealth()
    {
        return enemyHealth;
    }
}
