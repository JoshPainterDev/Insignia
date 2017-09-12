using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CombatManager : MonoBehaviour {

    // DEFINES
    public float LIMIT_BREAK_THRESH = 0.2f;

    enum State { MainMenu, Retreat, Abilities, Back, Done };

    public Canvas canvas;
    public GameObject playerMannequin;
    public GameObject enemyMannequin;
    private Vector3 initPlayerPos;
    public EnemyEncounter encounter;

    //MAIN BUTTON VARIABLES
    public GameObject topButton, leftButton, rightButton, backButton;
    public Color strike_C, items_C, abilities_C, back_C;

    private State currentState = State.MainMenu;

    //COMBAT VARIABLES
    public SpecialCase currSpecialCase = SpecialCase.None;
    public GameObject playerHealthBar;

    private int playerHealth = 0;
    [HideInInspector]
    public int playerMaxHealth = 600;
    [HideInInspector]
    public int playerAttackBoost = 0;
    [HideInInspector]
    public int playerDefenseBoost = 0;
    [HideInInspector]
    public int playerSpeedBoost = 0;

    // LIMIT BREAK VARIABLES
    [HideInInspector]
    public bool canLimitBreak = false;
    [HideInInspector]
    public LimitBreak playerLimitBreak;
    public LimitBreak enemyLimitBreak;

    // ENEMY COMBAT VARIABLES
    public EnemyInfo enemyInfo;
    public GameObject enemyHealthBar;
    private int enemyHealth = 0;
    [HideInInspector]
    public int enemyMaxHealth = 1000;
    [HideInInspector]
    public int enemyAttackBoost = 0;
    [HideInInspector]
    public int enemyDefenseBoost = 0;
    [HideInInspector]
    public int enemySpeedBoost = 0;

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


    // Use this for initialization
    void Start ()
    {
        encounter = GameController.controller.currentEncounter;

        if(encounter == null)
        {
            encounter = new EnemyEncounter();
        }

        //0. pretend the player has save data for ability sake
        GameController.controller.playerLevel = 10;
        GameController.controller.playerAbility1 = AbilityToolsScript.tools.LookUpAbility("Final Cut");
        GameController.controller.playerAbility2 = AbilityToolsScript.tools.LookUpAbility("Solar Flare");
        GameController.controller.playerAbility3 = AbilityToolsScript.tools.LookUpAbility("Outrage");
        GameController.controller.playerAbility4 = AbilityToolsScript.tools.LookUpAbility("Illusion");
        GameController.controller.strikeModifier = "Shadow Strike";
        GameController.controller.playerAttack = 26;
        GameController.controller.playerDefense = 16;
        GameController.controller.playerProwess = 1;
        GameController.controller.playerSpeed = 1;

        enemyInfo = EnemyToolsScript.tools.LookUpEnemy(encounter.enemyNames[0]); //start with the initial enemy lookup
        enemyInfo.enemyLevel = 10;
        enemyInfo.ability_1 = "Solar Flare";
        enemyInfo.ability_2 = "Outrage";
        enemyInfo.ability_3 = "Reap";
        enemyInfo.ability_4 = "Final Cut";

        enemyInfo.enemyAttack = 16;
        enemyInfo.enemyDefense = 16;
        enemyInfo.enemySpeed = 2;
        /// 

        //1. Load in player and enemy
        playerMaxHealth = 600;
        enemyMaxHealth = 1000;
        playerHealth = playerMaxHealth;
        enemyHealth = enemyMaxHealth;

        if (GameController.controller.limitBreakTracker == 0)
            canLimitBreak = true;

        initPlayerPos = playerMannequin.transform.position;
        strikeMod = GameController.controller.strikeModifier;
        strikeExecutePercent = .15f + (((GameController.controller.playerProwess - enemyInfo.enemyDefense) + 0.01f) / GameController.controller.playerProwess);
        ability1 = GameController.controller.playerAbility1;
        ability2 = GameController.controller.playerAbility2;
        ability3 = GameController.controller.playerAbility3;
        ability4 = GameController.controller.playerAbility4;

        abilityButton1.GetComponentInChildren<Text>().text = ability1.Name;
        abilityButton2.GetComponentInChildren<Text>().text = ability2.Name;
        abilityButton3.GetComponentInChildren<Text>().text = ability3.Name;
        abilityButton4.GetComponentInChildren<Text>().text = ability4.Name;

        LoadCharacterLevels();

        //2. Display buttons: STRIKE, ITEMS, ABILITIES
        DisableAbilityButtons();
        HideAbilityButtons();
        StartCoroutine(ShowStartingButtons());
        DisableBackButton();

        LoadCharacter();
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
        currentState = State.MainMenu;
        DisableAbilityButtons();
        HideAbilityButtons();
        DisableBackButton();
        HideMainButtons();
        //EnableMainButtons();  // only here for testing
        StartCoroutine(CheckForDamage(damageDealt, false, originalHP));
    }

    public void EndEnemyTurn(bool damageDealt, int originalHP = 0)
    {
        currentState = State.MainMenu;
        DisableAbilityButtons();
        HideAbilityButtons();
        DisableBackButton();
        StartCoroutine(CheckForDamage(damageDealt, true, originalHP));
    }

    IEnumerator CheckForDamage(bool damage, bool player, int origHP)
    {
        ShowHealthBars();
        yield return new WaitForSeconds(0.5f);

        if (damage)
        {
            if(player)
            {
                print("player was hit");
                float var1 = ((float)origHP / (float)playerMaxHealth);
                float var2 = ((float)playerHealth / (float)playerMaxHealth);
                playerHealthBar.GetComponent<HealthScript>().Hurt();
                yield return new WaitForSeconds(0.25f);
                playerHealthBar.GetComponent<HealthScript>().LerpHealth(var1, var2, (2.5f - (var2 - var1)));
                currentState = State.MainMenu;
                DisableAbilityButtons();
                HideAbilityButtons();
                DisableBackButton();
                ShowMainButtons();
                EnableMainButtons();
            }
            else
            {
                print("enemy was hit");
                float var1 = ((float)origHP / (float)enemyMaxHealth);
                float var2 = ((float)enemyHealth / (float)enemyMaxHealth);
                enemyHealthBar.GetComponent<HealthScript>().Hurt();
                yield return new WaitForSeconds(0.25f);
                enemyHealthBar.GetComponent<HealthScript>().LerpHealth(var1, var2, (2.5f - (var2 - var1)));
                StartCoroutine(StartEnemyTurn());
            }
        }
        else
        {
            if(player)
            {
                currentState = State.MainMenu;
                DisableAbilityButtons();
                HideAbilityButtons();
                DisableBackButton();
                ShowMainButtons();
                EnableMainButtons();
            }
            else
            {
                StartCoroutine(StartEnemyTurn());
            }
        }
    }

    public void EndEnemyStruggle(bool damageDealt, int originalHP)
    {
        currentState = State.MainMenu;
        DisableAbilityButtons();
        HideAbilityButtons();
        DisableBackButton();
        EnableMainButtons();

        StartCoroutine(CheckForDamage(damageDealt, true, originalHP));
    }

    //EndEnemyTurn

    IEnumerator StartEnemyTurn()
    {
        HideMainButtons();
        DisableMainButtons();
        yield return new WaitForSeconds(2);
        this.GetComponent<EnemyCombatScript>().BeginEnemyTurn();
    }

    IEnumerator ShowStartingButtons()
    {
        HideMainButtons();
        yield return new WaitForSeconds(2);
        ShowMainButtons();
        yield return new WaitForSeconds(0.1f);
        HideMainButtons();
        yield return new WaitForSeconds(0.1f);
        ShowMainButtons();
    }

    IEnumerator ShowMainMenuOptions()
    {
        DisableBackButton();
        DisableMainButtons();
        yield return new WaitForSeconds(0.1f);
        HideMainButtons();

        topButton.GetComponentInChildren<Text>().text = "STRIKE";
        topButton.GetComponent<Image>().color = strike_C;
        leftButton.GetComponentInChildren<Text>().text = "ITEMS";
        leftButton.GetComponent<Image>().color = items_C;
        rightButton.GetComponentInChildren<Text>().text = "ABILITIES";
        rightButton.GetComponent<Image>().color = abilities_C;

        yield return new WaitForSeconds(0.5f);

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
        HideHealthBars();

        button.GetComponent<Image>().color = abilitySelectColor;

        yield return new WaitForSeconds(0.15f);
        HideAbilityButtons();
        ShowButton(button.name);
        Vector3 centerPos = new Vector3(640, 250, 1);
        Vector3 ascend = new Vector3(640, 800, 1);
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
    public void CharacterDamaged(int damageVal, bool EnemyDamaged)
    {
        if(EnemyDamaged)
        {
            enemyHealth -= damageVal;

            // check if the enemy was defeated
            if(enemyHealth <= 0)
            {
                enemyHealth = 0;

                //enemy defeated
                if(GameController.controller.limitBreakTracker > 0)
                {
                    --GameController.controller.limitBreakTracker;
                }
            }
        }
        else
        {
            playerHealth -= damageVal;

            // check if the player has triggered a Limit Break
            if(canLimitBreak && ((float)playerHealth / (float)playerMaxHealth) <= LIMIT_BREAK_THRESH)
            {
                canLimitBreak = false;
                playerLimitBreak = this.GetComponent<LimitBreakManager_C>().LookUpLimitBreak(GameController.controller.limitBreakModifier);
                GameController.controller.limitBreakTracker = playerLimitBreak.coolDown;
                this.GetComponent<LimitBreakManager_C>().UseLimitBreak(playerLimitBreak);
            }
        }
    }

    IEnumerator UseStrike()
    {
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

        if(!EvaluateExecution())
            this.GetComponent<StrikeManager_C>().StrikeUsed(strikeMod, enemyHealth);
        else
        {
            HideHealthBars();
            this.GetComponent<StruggleManager_C>().BeginStruggle_Player();
        }
            
    }

    bool EvaluateExecution()
    {
        print("enemy percent health: " + ((float)enemyHealth / (float)enemyMaxHealth));
        print("execute percent: " + strikeExecutePercent);

        // check if the player can press the enemy
        if (((float)enemyHealth / (float)enemyMaxHealth) <= strikeExecutePercent)
        {
            return true;
        }

        return false;
    }

    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Player DAMAGE FUNCTIONS

    // DAMAGE AN ENEMY WITH STRIKE ONLY!
    public void DamageEnemy_Strike()
    {
        int rand = Random.Range(0, 100);
        int randDamageBuffer = Random.Range(0, 9);
        int accuracy = (10 * ((GameController.controller.playerSpeed + playerSpeedBoost) - (enemyInfo.enemySpeed + enemySpeedBoost))) + 60;
        float attBoostMod = 1;
        float damageDealt = 0;
        int attack = GameController.controller.playerAttack;
        int defense = GameController.controller.playerDefense;
        int prowess = GameController.controller.playerProwess;

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

            damageDealt = ((attack * attack) + (randDamageBuffer)) * attBoostMod;
            
            damageDealt -= (enemyInfo.enemyDefense) * (enemyInfo.enemyDefense * enemyDefenseBoost);

            CharacterDamaged((int)damageDealt, true);

            print("PLAYER DAMAGE: " + damageDealt);
            print("enemy is now at: " + enemyHealth);

            // check for special attack modifier
            if (currSpecialCase == SpecialCase.None)
            {

            }
            else
            {
                ResolveSpecialCase();
            }
        }
        else
            print("fuck we missed...");
    }

    // DAMAGE AN ENEMY WITH AN ABILITY ONLY!
    public void DamageEnemy_Ability(Ability abilityUsed)
    {
        int rand = Random.Range(0, 100);
        int randDamageBuffer = Random.Range(0, 9);
        int accuracy = abilityUsed.Accuracy;
        float attBoostMod = 1;
        float damageDealt = 0;
        int attack = GameController.controller.playerAttack;
        int defense = GameController.controller.playerDefense;
        int prowess = GameController.controller.playerProwess;

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

                damageDealt = ((attack + randDamageBuffer) * abilityUsed.BaseDamage) * attBoostMod;

                damageDealt -= (enemyInfo.enemyDefense * (enemyDefenseBoost * enemyDefenseBoost));

                enemyHealth -= (int)damageDealt;
                print("damage: " + damageDealt);
                print("enemy HP: " + enemyHealth);

                // check for special attack modifier
                if (currSpecialCase == SpecialCase.None)
                {

                }
                else
                {
                    ResolveSpecialCase();
                }
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

                damageDealt -= (enemyInfo.enemyDefense * (enemyDefenseBoost));

                enemyHealth -= (int)damageDealt;


                // check for special attack modifier
                if (currSpecialCase == SpecialCase.None)
                {

                }
                else
                {
                    ResolveSpecialCase();
                }
            }
        }
    }

    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// ENEMY DAMAGE FUNCTIONS
    /// 
        // DAMAGE AN ENEMY WITH STRIKE ONLY!
    public void DamagePlayer_Strike()
    {
        int rand = Random.Range(0, 100);
        int randDamageBuffer = Random.Range(0, 9);
        int accuracy = (5 * ((enemyInfo.enemySpeed + enemySpeedBoost) - (GameController.controller.playerSpeed + playerSpeedBoost))) + 60;
        float attBoostMod = 1;
        float damageDealt = 0;
        int attack = enemyInfo.enemyAttack;
        int defense = enemyInfo.enemyDefense;
        int prowess = enemyInfo.enemyProwess;

        print("ENEMY STRIKE!");
        print("ENEMY DEFENSE: " + enemyInfo.enemyDefense);

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

            damageDealt = ((attack * attack) + (randDamageBuffer)) * attBoostMod;

            damageDealt -= (GameController.controller.playerDefense) * (GameController.controller.playerDefense * playerDefenseBoost);

            CharacterDamaged((int)damageDealt, false);

            print("ENEMY DAMAGE: " + damageDealt);
            print("player is now at: " + playerHealth);

            // check for special attack modifier
            if (currSpecialCase == SpecialCase.None)
            {

            }
            else
            {
                ResolveSpecialCase();
            }
        }
        else
            print("fuck we missed...");
    }

    public void DamagePlayer_Ability(Ability abilityUsed)
    {
        int rand = Random.Range(0, 100);
        int randDamageBuffer = Random.Range(0, 9);
        int accuracy = abilityUsed.Accuracy;
        float attBoostMod = 1;
        float damageDealt = 0;
        int attack = enemyInfo.enemyAttack;
        int defense = enemyInfo.enemyDefense;
        int prowess = enemyInfo.enemyProwess;

        print("ENEMY DEFENSE: " + enemyInfo.enemyDefense);

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

                playerHealth -= (int)damageDealt;
                print("damage: " + damageDealt);
                print("player HP: " + playerHealth);

                // check for special attack modifier
                if (currSpecialCase == SpecialCase.None)
                {

                }
                else
                {
                    ResolveSpecialCase();
                }
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

                playerHealth -= (int)damageDealt;


                // check for special attack modifier
                if (currSpecialCase == SpecialCase.None)
                {

                }
                else
                {
                    ResolveSpecialCase();
                }
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

    void ResolveSpecialCase()
    {

    }

    /// Helper Functions
    /// ////////////////////////////////////////

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
                currentState = State.MainMenu;
                DisableAbilityButtons();
                HideAbilityButtons();
                StartCoroutine(ShowMainMenuOptions());
                DisableBackButton();
                break;
            case State.Retreat:
                currentState = State.MainMenu;
                StartCoroutine(ShowMainMenuOptions());
                break;
        }
    }

    void HideMainButtons()
    {
        topButton.GetComponent<Image>().enabled = false;
        leftButton.GetComponent<Image>().enabled = false;
        rightButton.GetComponent<Image>().enabled = false;

        topButton.GetComponentInChildren<Text>().enabled = false;
        leftButton.GetComponentInChildren<Text>().enabled = false;
        rightButton.GetComponentInChildren<Text>().enabled = false;
    }

    void ShowMainButtons()
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

    void EnableMainButtons()
    {
        topButton.GetComponentInChildren<Button>().enabled = true;
        leftButton.GetComponentInChildren<Button>().enabled = true;
        rightButton.GetComponentInChildren<Button>().enabled = true;
    }

    void DisableMainButtons()
    {
        topButton.GetComponentInChildren<Button>().enabled = false;
        leftButton.GetComponentInChildren<Button>().enabled = false;
        rightButton.GetComponentInChildren<Button>().enabled = false;
    }

    void EnableBackButton()
    {
        backButton.GetComponent<Image>().enabled = true;
        backButton.GetComponent<Button>().enabled = true;
        backButton.GetComponentInChildren<Text>().enabled = true;
    }

    void DisableBackButton()
    {
        backButton.GetComponent<Image>().enabled = false;
        backButton.GetComponent<Button>().enabled = false;
        backButton.GetComponentInChildren<Text>().enabled = false;
    }

    void EnableAbilityButtons()
    {
        abilityButton1.GetComponent<Button>().enabled = true;
        abilityButton2.GetComponent<Button>().enabled = true;
        abilityButton3.GetComponent<Button>().enabled = true;
        abilityButton4.GetComponent<Button>().enabled = true;
    }

    void DisableAbilityButtons()
    {
        abilityButton1.GetComponent<Button>().enabled = false;
        abilityButton2.GetComponent<Button>().enabled = false;
        abilityButton3.GetComponent<Button>().enabled = false;
        abilityButton4.GetComponent<Button>().enabled = false;
    }

    void ShowAbilityButtons()
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

    void HideAbilityButtons()
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

    void HideHealthBars()
    {
        playerHealthBar.GetComponent<Image>().enabled = false;

        foreach(Image img in playerHealthBar.GetComponentsInChildren<Image>())
        {
            img.enabled = false;
        }

        enemyHealthBar.GetComponent<Image>().enabled = false;
        foreach (Image img in enemyHealthBar.GetComponentsInChildren<Image>())
        {
            img.enabled = false;
        }
    }

    void ShowHealthBars()
    {
        playerHealthBar.GetComponent<Image>().enabled = true;
        foreach (Image img in playerHealthBar.GetComponentsInChildren<Image>())
        {
            img.enabled = true;
        }

        enemyHealthBar.GetComponent<Image>().enabled = true;
        foreach (Image img in enemyHealthBar.GetComponentsInChildren<Image>())
        {
            img.enabled = true;
        }
    }

    void SpawnItemsUI()
    {
        GameController.controller.playerInventory = new string[3];
        GameController.controller.playerInventoryQuantity = new int[3];

        GameController.controller.playerInventory[0] = "fuckFace";
        GameController.controller.playerInventory[1] = "Premium Quality Dildo";
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

    public void LoadCharacterLevels()
    {
        playerHealthBar.transform.GetChild(2).GetComponent<Text>().text = "Lv " + GameController.controller.playerLevel;
        enemyHealthBar.transform.GetChild(2).GetComponent<Text>().text = "Lv " + enemyInfo.enemyLevel;
    }

    public void LoadCharacter()
    {
        //head
        EquipmentInfo info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[0], GameController.controller.playerEquippedIDs[1]);
        playerMannequin.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        //torso
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[2], GameController.controller.playerEquippedIDs[3]);
        playerMannequin.transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

        string newStr = info.imgSourceName;
        string match = "Torso";
        string replace = "Arms";
        int mSize = 0;
        int tracker = 0;
        //Alters the form of the string to include the Arms animator with the Torso
        foreach (char c in info.imgSourceName)
        {
            if (c == match[mSize])
            {
                ++mSize;

                if (mSize == 5)
                {
                    newStr = newStr.Remove(tracker - 4, mSize);
                    newStr = newStr.Insert(tracker - 4, replace);
                    mSize = 0;
                    --tracker;
                }
            }
            else
                mSize = 0;

            ++tracker;
        }

        //sleeve
        playerMannequin.transform.GetChild(7).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(newStr, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        //legs
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[4], GameController.controller.playerEquippedIDs[5]);
        playerMannequin.transform.GetChild(2).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        //back
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[6], GameController.controller.playerEquippedIDs[7]);
        playerMannequin.transform.GetChild(3).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        //gloves
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[8], GameController.controller.playerEquippedIDs[9]);
        playerMannequin.transform.GetChild(4).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        //shoes
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[10], GameController.controller.playerEquippedIDs[11]);
        playerMannequin.transform.GetChild(5).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        //weapon
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[12], GameController.controller.playerEquippedIDs[13]);
        playerMannequin.transform.GetChild(6).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
    }

    public int getPlayerHealth()
    {
        return playerHealth;
    }
}
