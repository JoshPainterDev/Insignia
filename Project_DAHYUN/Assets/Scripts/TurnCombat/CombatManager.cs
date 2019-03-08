using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CombatManager : MonoBehaviour {

    // DEFINES
    public const float LIMIT_BREAK_THRESH = 0.2f;
    public const float VULNERABLE_REDUCTION = 0.2f;
    public const float BLINDED_REDUCTION = 66.6f;
    public const int BLIND_DURATION = 2;
    public const float CRITICAL_THRESHOLD = 0.8f;
    public const int STAT_LIMIT = 50;
    public const int PLAYER_HEALTH_SCALE = 70;

    enum State { MainMenu, Stance, Abilities, Back, Done };
    enum Stance { Aggressive, Defensive, Focused, none };

    public GameObject cameraObj;
    public bool hasTutorial = false;
    public bool chaosPlayerAI = false;
    public Canvas canvas;
    public GameObject playerMannequin;
    public GameObject enemyMannequin;
    private Vector3 initPlayerPos;
    public EnemyEncounter encounter;
    private GameObject enemyPrfb;

    //MAIN BUTTON VARIABLES
    public GameObject middleButton, leftButton, rightButton, backButton;
    public GameObject agressiveStance_B, defensiveStance_B, focusedStance_B;
    public Color strike_C, stance_C, abilities_C, back_C;
    public GameObject blackSq;
    private State currentState = State.MainMenu;
    public GameObject Music_Manager;

    //COMBAT VARIABLES
    [HideInInspector]
    public SpecialCase currSpecialCase = SpecialCase.None;
    public GameObject playerHealthBar;

    private int playerLevel;
    private int playerHealth = 0;
    [HideInInspector]
    private int playerMaxHealth = 100;
    private Stance playerStance = Stance.none;
    [HideInInspector]
    public int playerAttackBoost = 0;
    private int playerAttBoostDur = 0;
    [HideInInspector]
    public int playerDefenseBoost = 0;
    private int playerDefBoostDur = 0;
    [HideInInspector]
    public int playerSpeedBoost = 0;
    private int playerSpdBoostDur = 0;
    [HideInInspector]
    public bool playerStunned = false;
    [HideInInspector]
    public bool playerVulnernable = false;
    [HideInInspector]
    public bool playerBlinded = false;
    [HideInInspector]
    public int ability1CD, ability2CD, ability3CD, ability4CD;
    public Color abilityReadyColor, abilityUsedColor;

    // LIMIT BREAK VARIABLES
    [HideInInspector]
    public bool canLimitBreak = false;
    [HideInInspector]
    public LimitBreak playerLimitBreak;
    public LimitBreak enemyLimitBreak;
    [HideInInspector]
    public bool enemyCanLB;
    [HideInInspector]
    public bool playerLimitBreaking = false;
    [HideInInspector]
    public bool enemyLimitBreaking = false;

    // ENEMY COMBAT VARIABLES
    public EnemyInfo enemyInfo;
    public GameObject enemyHealthBar;
    private int enemyHealth = 0;
    [HideInInspector]
    public int enemyMaxHealth = 100;
    [HideInInspector]
    public int enemyAttackBoost = 0;
    private int enemyAttBoostDur = 0;
    [HideInInspector]
    public int enemyDefenseBoost = 0;
    private int enemyDefBoostDur = 0;
    [HideInInspector]
    public int enemySpeedBoost = 0;
    private int enemySpdBoostDur = 0;
    [HideInInspector]
    public bool enemyStunned = false;
    [HideInInspector]
    public bool enemyVulnernable = false;
    [HideInInspector]
    public bool enemyBlinded = false;
    private Color enemyOrigColor;
    [HideInInspector]
    public int enemyAbility1CD, enemyAbility2CD, enemyAbility3CD, enemyAbility4CD;
    

    //STRIKE VARIABLES
    public float strikeAnimDuration = 3.5f;
    public float strikePosX = 320f;
    public string strikeMod = "none";
    private float strikeExecutePercent = 0.20f;
    private bool wasCriticalHit = false;

    //ABILITY VARIABLES
    public Vector2 ab1_pos, ab2_pos, ab3_pos, ab4_pos;
    public GameObject abilityButtonPrefab;
    public GameObject abilityButton1, abilityButton2, abilityButton3, abilityButton4;
    public Ability ability1, ability2, ability3, ability4;
    public Color abilityTextColor;
    public Color abilitySelectColor;
    public Color physicalColor;
    public Color magicalColor;
    public Color utilityColor;
    public int playerBlindedDur = 0;
    public int enemyBlindedDur = 0;

    //ENCOUNTER VARIABLES
    [HideInInspector]
    public int enemiesRemaining;
    public GameObject enemyCounter;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
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
            playerHealth = (int)(playerMaxHealth * 0.25f);

            float var1 = ((float)origHealth / (float)playerMaxHealth);
            float var2 = ((float)playerHealth / (float)playerMaxHealth);
            playerHealthBar.GetComponent<HealthScript>().Hurt();
            playerHealthBar.GetComponent<HealthScript>().LerpHealth(var1, var2, (2.5f - (var2 - var1)));
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            this.GetComponent<StruggleManager_C>().ForceExecute();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            this.GetComponent<LimitBreakManager_C>().UseLimitBreak(playerLimitBreak);
        }
    }

    // Use this for initialization
    void Start ()
    {
        playerMannequin = GameController.controller.playerObject;
        encounter = GameController.controller.currentEncounter;

        initPlayerPos = playerMannequin.transform.position;
        playerMannequin.GetComponent<AnimationController>().SetCombatState(true);

        // DEFINE A DEFAULT DUMMY TO FIGHT
        //REMOVE THIS LATER
        if (encounter == null)
        {
            encounter = new EnemyEncounter();
            encounter = GameController.controller.GetComponent<EncounterToolsScript>().SpecifyEncounter(1,3);
            GameController.controller.currentEncounter = encounter;
            encounter.enemyNames = new string[3];
            encounter.totalEnemies = 1;
            encounter.enemyNames[0] = "Dummy";
            //encounter.enemyNames[1] = "Skitter";
            //encounter.enemyNames[2] = "Shadow Assassin";
            encounter.encounterNumber = -1;
            encounter.returnOnSuccessScene = "MainMenu_Scene";
        }

        enemiesRemaining = encounter.totalEnemies;

        //REMOVE THIS LATER
        if (GameController.controller.playerName == "")
        {
            GameController.controller.Load(GameController.controller.charNames[1]);
            print(GameController.controller.charNames[1]);
        }

        // 1. Load in player and enemy
        playerLevel = GameController.controller.playerLevel;
        playerHealthBar.transform.GetChild(3).GetComponent<Text>().text = "Lv " + playerLevel.ToString();
        playerHealthBar.transform.GetChild(4).GetComponent<Text>().text = GameController.controller.playerName;
        //GameController.controller.playerLevel = 5;
        //GameController.controller.playerAttack = 17;
        //GameController.controller.playerDefense = 25;
        //GameController.controller.playerProwess = 6;
        //GameController.controller.playerSpeed = 4;

        playerMaxHealth = (PLAYER_HEALTH_SCALE * playerLevel) + (9 * GameController.controller.playerDefense);
        playerHealth = playerMaxHealth;
        currSpecialCase = SpecialCase.None;
        print("Player max HP: " + playerHealth);
        //print("base def: " + GameController.controller.playerBaseDef);
        //print("total defense: " + GameController.controller.playerDefense);
        //print("base spd: " + GameController.controller.playerSpeed);

        if (!hasTutorial)
        {
            setAbilityVariables();
            setAbilityButtons();    

            if (GameController.controller.limitBreakTracker == 0)
                canLimitBreak = true;
        }

        //2. Display buttons: STRIKE, STANCE, ABILITIES
        HideStanceButtons();
        HideAbilityButtons();
        HideBackButton();
        HideMainButtons();

        DisableAbilityButtons();
        DisableStanceButtons();
        DisableMainButtons();

        if (!hasTutorial)
        {
            //3. Load in initial enemy
            LoadNextEnemyInfo(true);
            LoadCharacterLevels(enemyInfo);

            StartCoroutine(InitialStanceSelect());

            //if (GameController.controller.playerSpeed >= enemyInfo.enemySpeed)
            //{
            //    //print("enemy speed: " + enemyInfo.enemySpeed);
            //    StartCoroutine(ShowStartingButtons());
            //}
            //else
            //    StartCoroutine(EnemyStarts());        
        }
        else // Tutorial buttons
        {
            StartCoroutine(ShowStartingButtons());
        }
    }

    void setAbilityVariables()
    {
        //REMOVE THIS LATER
        GameController.controller.playerAbility1 = AbilityToolsScript.tools.LookUpAbility("Guard Break");
        GameController.controller.playerAbility2 = AbilityToolsScript.tools.LookUpAbility("Blade Storm");
        GameController.controller.playerAbility3 = AbilityToolsScript.tools.LookUpAbility("Stranglehold");
        GameController.controller.playerAbility4 = AbilityToolsScript.tools.LookUpAbility("Thunder Charge");
        //GameController.controller.strikeModifier = "Serated Strike";
        //playerLimitBreak = this.GetComponent<LimitBreakManager_C>().LookUpLimitBreak(LimitBreakName.Super_Nova);
        playerLimitBreak = this.GetComponent<LimitBreakManager_C>().LookUpLimitBreak(GameController.controller.limitBreakModifier);

        //strikeMod = GameController.controller.strikeModifier;

        ability1 = GameController.controller.playerAbility1;
        ability2 = GameController.controller.playerAbility2;
        ability3 = GameController.controller.playerAbility3;
        ability4 = GameController.controller.playerAbility4;

        ability1CD = 0;
        ability2CD = 0;
        ability3CD = 0;
        ability4CD = 0;

        abilityButton1.transform.GetChild(0).GetComponent<Text>().text = ability1.Name;
        abilityButton2.transform.GetChild(0).GetComponent<Text>().text = ability2.Name;
        abilityButton3.transform.GetChild(0).GetComponent<Text>().text = ability3.Name;
        abilityButton4.transform.GetChild(0).GetComponent<Text>().text = ability4.Name;
    }

    IEnumerator changeToCombatStance()
    {
        yield return new WaitForSeconds(0.2f);
        playerMannequin.GetComponent<AnimationController>().SetCombatState(true);
    }

    IEnumerator EnemyStarts()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(StartEnemyTurn());
    }

    // This is called when an ability button is selected
    public void AbilitySelected(int selectedOption = 0)
    {
        switch(selectedOption)
        {
            case 1:
                if(ability1CD < 1 && ability1.Name != "-")
                {
                    DisableAbilityButtons();
                    ability1CD = ability1.Cooldown + 1;
                    StartCoroutine(AbilitySelectAnim(abilityButton1, ability1));
                }
                break;
            case 2:
                if (ability2CD < 1 && ability2.Name != "-")
                {
                    DisableAbilityButtons();
                    ability2CD = ability2.Cooldown + 1;
                    abilityButton2.GetComponent<Button>().enabled = false;
                    StartCoroutine(AbilitySelectAnim(abilityButton2, ability2));
                }
                break;
            case 3:
                if (ability3CD < 1 && ability3.Name != "-")
                {
                    DisableAbilityButtons();
                    ability3CD = ability3.Cooldown + 1;
                    StartCoroutine(AbilitySelectAnim(abilityButton3, ability3));
                }
                break;
            case 4:
                if (ability4CD < 1 && ability4.Name != "-")
                {
                    DisableAbilityButtons();
                    ability4CD = ability4.Cooldown + 1;
                    StartCoroutine(AbilitySelectAnim(abilityButton4, ability4));
                }
                break;
        }
    }

    public Color getAbilityTypeColor(Ability ability)
    {
        Color temp = Color.white;

        switch (ability.Type)
        {
            case AbilityType.Physical:
                temp = physicalColor;
                break;
            case AbilityType.Magical:
                temp = magicalColor;
                break;
            case AbilityType.Utility:
                temp = utilityColor;
                break;
        }

        return temp;
    }

    public void EndPlayerTurn(int damageDealt, int originalHP = 0)
    {
        print("PLAYER TURN IS OVER");
        print("Current SC: " + currSpecialCase);
        PlayerAbilityCooldownTick();
        BoostTick(true);
        StartCoroutine(ResolveAttack(damageDealt, false, originalHP, wasCriticalHit));
        wasCriticalHit = false;
    }

    public void EndEnemyTurn(int damageDealt, int originalHP = 0)
    {
        print("ENEMY TURN IS OVER");
        this.GetComponent<EnemyCombatScript>().TickCooldowns();
        BoostTick(false);
        StartCoroutine(ResolveAttack(damageDealt, true, originalHP, wasCriticalHit));
        wasCriticalHit = false;
    }

    IEnumerator ResolveAttack(int damage, bool targetIsPlayer, int origHP, bool critical = false)
    {
        ShowHealthBars();

        if(targetIsPlayer)
            print("CHECKING DAMAGE AGAINST PLAYER");
        else
            print("CHECKING DAMAGE AGAINST ENEMY");

        if (ResolveSpecialCase(!targetIsPlayer))
        {
            if (damage > 0)
            {
                if (targetIsPlayer)//is being attacked
                {
                    float var1 = ((float)origHP / (float)playerMaxHealth);
                    float var2 = ((float)playerHealth / (float)playerMaxHealth);
                    playerHealthBar.GetComponent<HealthScript>().Hurt();
                    yield return new WaitForSeconds(0.25f);
                    playerHealthBar.GetComponent<HealthScript>().LerpHealth(var1, var2, (2.5f - (var2 - var1)));
                    playerHealthBar.GetComponent<DamageVisualizer_C>().SpawnDamage(origHP - playerHealth, critical);

                    if (CheckForDeath(false))
                        StartCoroutine(PlayPlayerDeathAnim());
                    else
                        StartCoroutine(StartPlayerTurn());
                }
                else //if enemy is being attacked
                {
                    float var1 = ((float)origHP / (float)enemyMaxHealth);
                    float var2 = ((float)enemyHealth / (float)enemyMaxHealth);
                    var1 = Mathf.Clamp(var1, 0.0f, 1.0f);
                    var2 = Mathf.Clamp(var2, 0.0f, 1.0f);
                    enemyHealthBar.GetComponent<HealthScript>().Hurt();
                    yield return new WaitForSeconds(0.25f);
                    enemyHealthBar.GetComponent<HealthScript>().LerpHealth(var1, var2, (2.5f - (var2 - var1)));
                    enemyHealthBar.GetComponent<DamageVisualizer_C>().SpawnDamage(damage, critical);

                    if (critical)
                        this.GetComponent<CombatAudio>().playCriticalHit();

                    if (CheckForDeath(true))
                        StartCoroutine(PlayEnemyDeathAnim());
                    else
                        StartCoroutine(StartEnemyTurn());      
                }
            }
            else
            {
                if (targetIsPlayer)//is being attacked
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

    public void EndEnemyStruggle(int damageDealt, int originalHP)
    {
        currentState = State.MainMenu;
        DisableAbilityButtons();
        HideAbilityButtons();
        HideBackButton();
        //enable main buttons?

        StartCoroutine(ResolveAttack(damageDealt, true, originalHP, false));
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

    IEnumerator InitialStanceSelect()
    {
        DisableMainButtons();
        HideMainButtons();
        yield return new WaitForSeconds(2);
        ShowButton("middle");
        yield return new WaitForSeconds(0.1f);
        HideButton("middle");
        yield return new WaitForSeconds(0.1f);
        ShowButton("middle");
        ShowStanceButtons();
        EnableStanceButtons();
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
        DisableMainButtons();
        HideMainButtons();

        DisableAbilityButtons();
        HideAbilityButtons();

        DisableStanceButtons();
        HideStanceButtons();

        HideBackButton();

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

        middleButton.GetComponentInChildren<Text>().text = "STANCE";
        middleButton.GetComponent<Image>().color = stance_C;
        leftButton.GetComponentInChildren<Text>().text = "ABILITIES";
        leftButton.GetComponent<Image>().color = abilities_C;
        rightButton.GetComponentInChildren<Text>().text = "STRIKE";
        rightButton.GetComponent<Image>().color = strike_C;

        yield return new WaitForSeconds(0.5f);

        ShowMainButtons();
        EnableMainButtons();
    }

    IEnumerator AbilitySelectAnim(GameObject button, Ability abilityUsed)
    {
        Vector3 origPos = button.transform.position;
        Color origColor = button.GetComponent<Image>().color;

        DisableAbilityButtons();
        HideButton("back");
        HideHealthBars();

        this.GetComponent<CombatAudio>().playUIAbilitySelect();
        button.GetComponent<Image>().color = abilitySelectColor;
        Vector3 centerPos = Vector3.zero;
        Vector3 ascend = Vector3.zero;

        if (button.name == "TLA1_Button")
        {
            centerPos = button.transform.position + new Vector3(100, -50, 0);
            ascend = centerPos + new Vector3(0, 300, 0);
            abilityButton1.transform.GetChild(1).GetComponent<Text>().color = abilityUsedColor;
        }
        else if (button.name == "TLA2_Button")
        {
            centerPos = button.transform.position - new Vector3(100, 50, 0);
            ascend = centerPos + new Vector3(0, 300, 0);
            abilityButton2.transform.GetChild(1).GetComponent<Text>().color = abilityUsedColor;
        }
        else if (button.name == "TLA3_Button")
        {
            centerPos = button.transform.position + new Vector3(100, 50, 0);
            ascend = centerPos + new Vector3(0, 300, 0);
            abilityButton3.transform.GetChild(1).GetComponent<Text>().color = abilityUsedColor;
        }
        else if (button.name == "TLA4_Button")
        {
            centerPos = button.transform.position - new Vector3(100, -50, 0);
            ascend = centerPos + new Vector3(0, 300, 0);
            abilityButton4.transform.GetChild(1).GetComponent<Text>().color = abilityUsedColor;
        }

        yield return new WaitForSeconds(0.15f);
        HideAbilityButtons();
        ShowButton(button.name);
        Color seethrough = new Color(abilitySelectColor.r, abilitySelectColor.g, abilitySelectColor.b, 0f);

        button.GetComponent<LerpScript>().LerpToPos(button.transform.position, centerPos, 8f);
        yield return new WaitForSeconds(0.75f);
        button.GetComponent<LerpScript>().LerpToPos(centerPos, ascend, 1.2f);
        button.GetComponent<LerpScript>().LerpToColor(abilitySelectColor, seethrough, 5f);
        Transform child = button.transform.GetChild(0);
        child.gameObject.GetComponent<LerpScript>().LerpToColor(abilityTextColor, Color.clear, 5f);
        child = button.transform.GetChild(1);
        child.GetComponent<LerpScript>().LerpToColor(abilityUsedColor, Color.clear, 5f);

        yield return new WaitForSeconds(1f);
        HideButton(button.name);
        button.GetComponentInChildren<Text>().color = abilityTextColor;
        button.transform.GetChild(1).GetComponent<Text>().color = abilityUsedColor;
        button.GetComponent<Image>().color = (Color.grey - new Color(0,0,0,0.15f));
        button.transform.position = origPos;

        // Ability selected and sent to manager
        this.GetComponent<AbilityManager_C>().SetupSelectedAbility(abilityUsed, enemyHealth);
    }

    // Combat Functions
    ////////////////////////////////////////////
    public void PlayerAbilityCooldownTick()
    {
        if(ability1CD > 0)
        {
            --ability1CD;
            abilityButton1.transform.GetChild(1).GetComponent<Text>().text = ability1CD.ToString();

            if (ability1CD == 0)
            {
                abilityButton1.transform.GetChild(1).GetComponent<Text>().text = ability1.Cooldown.ToString();
                abilityButton1.transform.GetChild(1).GetComponent<Text>().color = abilityReadyColor;
                abilityButton1.GetComponent<Image>().color = getAbilityTypeColor(ability1);
            }
        }

        if (ability2CD > 0)
        {
            --ability2CD;
            abilityButton2.transform.GetChild(1).GetComponent<Text>().text = ability2CD.ToString();

            if (ability2CD == 0)
            {
                abilityButton2.transform.GetChild(1).GetComponent<Text>().text = ability1.Cooldown.ToString();
                abilityButton2.transform.GetChild(1).GetComponent<Text>().color = abilityReadyColor;
                abilityButton2.GetComponent<Image>().color = getAbilityTypeColor(ability2);
            }
        }

        if (ability3CD > 0)
        {
            --ability3CD;
            abilityButton3.transform.GetChild(1).GetComponent<Text>().text = ability3CD.ToString();

            if (ability3CD == 0)
            {
                abilityButton3.transform.GetChild(1).GetComponent<Text>().text = ability1.Cooldown.ToString();
                abilityButton3.transform.GetChild(1).GetComponent<Text>().color = abilityReadyColor;
                abilityButton3.GetComponent<Image>().color = getAbilityTypeColor(ability3);
            }
        }
            
        if (ability4CD > 0)
        {
            --ability4CD;
            abilityButton4.transform.GetChild(1).GetComponent<Text>().text = ability4CD.ToString();

            if (ability4CD == 0)
            {
                abilityButton4.transform.GetChild(1).GetComponent<Text>().text = ability1.Cooldown.ToString();
                abilityButton4.transform.GetChild(1).GetComponent<Text>().color = abilityReadyColor;
                abilityButton4.GetComponent<Image>().color = getAbilityTypeColor(ability4);
            }
        }
    }

    public void BoostTick(bool player)
    {
        Transform trans = null;

        if (this.GetComponent<AbilityManager_C>().boostHandle != null)
            trans = this.GetComponent<AbilityManager_C>().boostHandle.transform;
        else
            return;

        if (player)
        {
            if (playerAttBoostDur == 0)
            {
                playerAttackBoost = 0;
                trans.GetChild(0).GetComponent<Image>().enabled = false;
            }
            else
                --playerAttBoostDur;

            if (playerDefBoostDur == 0)
            {
                playerDefenseBoost = 0;
                trans.GetChild(1).GetComponent<Image>().enabled = false;
            }
            else
                --playerDefBoostDur;

            if (playerSpdBoostDur == 0)
            {
                playerSpeedBoost = 0;
                trans.GetChild(2).GetComponent<Image>().enabled = false;
            }
            else
                --playerSpdBoostDur;

            if (playerBlinded)
            {
                --playerBlindedDur;

                if (playerBlindedDur <= 0)
                {
                    playerBlinded = false;
                }
            }
        }
        else
        {
            if (enemyAttBoostDur == 0)
            {
                enemyAttackBoost = 0;
            }
            else
                --enemyAttBoostDur;

            if (enemyDefBoostDur == 0)
            {
                enemyDefenseBoost = 0;
            }
            else
                --enemyDefBoostDur;

            if (enemySpdBoostDur == 0)
            {
                enemySpeedBoost = 0;
            }
            else
                --enemySpdBoostDur;

            if(enemyBlinded)
            {
                --enemyBlindedDur;

                if (enemyBlindedDur <= 0)
                {
                    enemyBlinded = false;
                }
            }

        }
    }

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
                    print("Oh jeez Rick, " + (encounter.enemyNames[encounter.totalEnemies - enemiesRemaining]) + " is dead...");
                    --enemiesRemaining;
                    enemyCounter.GetComponent<enemyCounterScript>().EnemyDied();
                    if(this.GetComponent<ExperienceScript>().isActiveAndEnabled)
                        this.GetComponent<ExperienceScript>().AddEXP(enemyInfo.expReward);
                    return true;
                }
            }
        }
        else
        {
            print("Check Player LB");
            // check if the player has triggered a Limit Break
            if (canLimitBreak && ((float)playerHealth / (float)playerMaxHealth) <= LIMIT_BREAK_THRESH)
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
        float accuracy = 87 + (3 * ((GameController.controller.playerSpeed + playerSpeedBoost) - (enemyInfo.enemySpeed + enemySpeedBoost)));

        DisableMainButtons();
        HideHealthBars();
        HideButton("right");
        yield return new WaitForSeconds(0.1f);
        ShowButton("right");
        yield return new WaitForSeconds(0.1f);
        HideButton("right");
        yield return new WaitForSeconds(0.1f);
        ShowButton("right");
        yield return new WaitForSeconds(0.1f);
        HideMainButtons();
        yield return new WaitForSeconds(0.25f);

        if (playerBlinded)
        {
            playerBlinded = false;
            accuracy -= BLINDED_REDUCTION;
            currSpecialCase = SpecialCase.None;
        }

        if (exectueVar == 0)
        {
            print("ACCURACY: " + accuracy);

            // accuracy check the attack
            if (accuracy >= rand)
            {
                this.GetComponent<StrikeManager_C>().StrikeUsed(strikeMod, enemyHealth);
            }
            else
                this.GetComponent<StrikeManager_C>().PlayerStrikeMiss();
        }
        else
        {
            HideHealthBars();
            this.GetComponent<StruggleManager_C>().BeginStruggle_Player(exectueVar, enemyHealth);
        }
    }

    int EvaluateExecution(int overrideDamage = 0)
    {
        int attack = GameController.controller.playerAttack;
        int prowess = GameController.controller.playerProwess;
        float attBoostMod = 1;
        int vulnerablePenalty = 0;

        //print("REGISTERED AB: " + playerAttackBoost);

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

        if (enemyVulnernable)
        {
            vulnerablePenalty = (int)(VULNERABLE_REDUCTION * enemyHealth);
            print("vulnerable penalty: " + vulnerablePenalty);
            enemyVulnernable = false;
        }

        if (overrideDamage != 0)
        {
            float threshold = 1.5f * overrideDamage;
            threshold -= vulnerablePenalty;

            print("threshold: " + threshold);

            if (threshold >= enemyHealth)
            {
                return (int)(damageDealt + vulnerablePenalty);
            }
        }
        else //Do the normal execution calculation
        {
            int rand = Random.Range(0, 9);
            damageDealt = (((attack * 2) + rand) * attBoostMod) * 1.5f;

            damageDealt -= (enemyInfo.enemyDefense * enemyDefenseBoost);

            float threshold = (damageDealt + (damageDealt * GameController.controller.playerProwess * 0.02f));
            threshold -= vulnerablePenalty;

            print("prowess bonus: " + (GameController.controller.playerProwess * 0.02f));
            print("threshold: " + threshold);
            print("E_health: " + enemyHealth);

            if (threshold >= enemyHealth)
            {
                //return (int)(damageDealt + vulnerablePenalty); 
                return (int)threshold;
            }
        }

        print("damage dealt: " + damageDealt);

        return 0;
    }

    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Player DAMAGE FUNCTIONS

    // DAMAGE AN ENEMY WITH STRIKE ONLY!
    public int DamageEnemy_Strike(float percentOfDamage = 1.0f)
    {
        int damageReturn = 0;
        int randDamageBuffer = Random.Range(0, 9);
        float critRand = Random.Range(0.0f, 1.0f);
        float attBoostMod = 1;
        float damageDealt = 0;
        int attack = GameController.controller.playerAttack;
        int defense = GameController.controller.playerDefense;
        int prowess = GameController.controller.playerProwess;
        float levelMod = 1.0f;
        float vulnerableBonus = 0.0f;

        if (playerLevel > enemyInfo.enemyLevel)
            levelMod = 1.15f;
        else if (playerLevel < enemyInfo.enemyLevel)
            levelMod = 0.85f;

        if(enemyVulnernable)
        {
            vulnerableBonus = VULNERABLE_REDUCTION;
            print("vulnerableBonus active + " + vulnerableBonus);
        }

        //handle attack boost modifier
        switch (playerAttackBoost)
        {
            case 1:
                attBoostMod = 1.35f;
                break;
            case 2:
                attBoostMod = 1.75f;
                break;
            case 3:
                attBoostMod = 2.15f;
                break;
            default:
                attBoostMod = 1;
                break;
        }

        damageDealt = ((attack * 2 * playerLevel) + (randDamageBuffer)) * attBoostMod;

        damageDealt -= (enemyInfo.enemyDefense * enemyDefenseBoost);

        damageDealt *= levelMod;

        // critical hit chance
        float chance = (critRand + ((prowess / STAT_LIMIT) * 0.3f) + vulnerableBonus);

        if (chance >= CRITICAL_THRESHOLD)
        {
            wasCriticalHit = true;
            damageDealt *= 1.5f;
        }

        damageDealt *= percentOfDamage;

        if (damageDealt < 1)
            damageDealt = 1;

        damageReturn = (int)damageDealt;
        enemyHealth -= damageReturn;

        print("PLAYER DAMAGE: " + damageDealt);
        print("enemy is now at: " + enemyHealth);

        return damageReturn;
    }

    public void ExecuteEnemy_Strike(int damageDealt, bool useCrit = false)
    {
        int origHP = enemyHealth;
        //spawn effects or something idk
        enemyHealth -= damageDealt;
        wasCriticalHit = useCrit;
        EndPlayerTurn(damageDealt, origHP);
    }

    // DAMAGE AN ENEMY WITH AN ABILITY ONLY!
    public int DamageEnemy_Ability(Ability abilityUsed)
    {
        int damageReturn = 0;
        int rand = Random.Range(0, 100);
        int randDamageBuffer = Random.Range(0, 9);
        float accuracy = abilityUsed.Accuracy;
        float attBoostMod = 1;
        float damageDealt = 0;
        int attack = GameController.controller.playerAttack;
        int defense = GameController.controller.playerDefense;
        int prowess = GameController.controller.playerProwess;

        if (hasTutorial)
            return damageReturn;

        if (playerBlinded)
        {
            playerBlinded = false;
            accuracy -= BLINDED_REDUCTION;
            currSpecialCase = SpecialCase.None;
        }

        if (abilityUsed.Type == AbilityType.Physical)
        {
            // accuracy check the attack
            if (accuracy >= rand)
            {
                //handle attack boost modifier
                switch (playerAttackBoost)
                {
                    case 1:
                        attBoostMod = 1.2f;
                        break;
                    case 2:
                        attBoostMod = 1.5f;
                        break;
                    case 3:
                        attBoostMod = 1.7f;
                        break;
                    default:
                        attBoostMod = 1;
                        break;
                }
                
                damageDealt = ((attack + playerLevel) + (abilityUsed.BaseDamage + (abilityUsed.BaseDamage * playerLevel / 2))) * attBoostMod;

                damageDealt -= (enemyInfo.enemyDefense * (enemyDefenseBoost * enemyDefenseBoost)) / 1.5f;

                if (damageDealt < 1)
                    damageDealt = 1;

                damageReturn = (int)damageDealt;
                enemyHealth -= damageReturn;

                print("Ability damage: " + damageDealt);
                print("enemy HP: " + enemyHealth);
            }
            else
            {
                this.GetComponent<AbilityManager_C>().PlayerAbilityMiss();
            }
        }
        else if(abilityUsed.Type == AbilityType.Magical)
        {
            // accuracy check the attack
            if (accuracy >= rand)
            {
                //handle attack boost modifier
                switch (playerAttackBoost)
                {
                    case 1:
                        attBoostMod = 1.2f;
                        break;
                    case 2:
                        attBoostMod = 1.5f;
                        break;
                    case 3:
                        attBoostMod = 1.7f;
                        break;
                    default:
                        attBoostMod = 1;
                        break;
                }

                damageDealt = (((float)attack * (float)abilityUsed.BaseDamage) - ((enemyInfo.enemyLevel - playerLevel) * 
                                                                                                                    ((float)abilityUsed.BaseDamage * (float)playerLevel))) * attBoostMod;
                
                print("eDefense: " + enemyInfo.enemyDefense);

                damageDealt -= (enemyInfo.enemyDefense * (enemyDefenseBoost * enemyDefenseBoost)) / 2.0f;

                if (damageDealt < 1)
                    damageDealt = 1;

                damageReturn = (int)damageDealt;
                enemyHealth -= damageReturn;

                print("damage: " + damageDealt);
                print("enemy HP: " + enemyHealth);
            }
            else
            {
                this.GetComponent<AbilityManager_C>().PlayerAbilityMiss();
            }
        }
        else
        {
            // accuracy check the attack
            if (accuracy >= rand)
            {
                //handle attack boost modifier
                switch (playerAttackBoost)
                {
                    case 1:
                        attBoostMod = 1.2f;
                        break;
                    case 2:
                        attBoostMod = 1.5f;
                        break;
                    case 3:
                        attBoostMod = 1.7f;
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

                damageReturn = (int)damageDealt;
                enemyHealth -= damageReturn;

                print("damage: " + damageDealt);
                print("enemy HP: " + enemyHealth);
            }
            else
            {
                this.GetComponent<AbilityManager_C>().PlayerAbilityMiss();
            }
        }

        return damageReturn;
    }

    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// ENEMY DAMAGE FUNCTIONS
    /// 
        // DAMAGE PLAYER WITH STRIKE ONLY!
    public int DamagePlayer_Strike(float percentOfDamage = 1.0f)
    {
        int damageReturn = 0;
        int randDamageBuffer = Random.Range(0, 9);
        float critRand = Random.Range(0.0f, 1.0f);
        float attBoostMod = 1;
        float defBoostMod = 1;
        float damageDealt = 0;
        int attack = enemyInfo.enemyAttack;
        int defense = enemyInfo.enemyDefense;
        int prowess = enemyInfo.enemyProwess;
        float levelMod = 1.0f;
        float vulnerableBonus = 0.0f;

        if (playerLevel > enemyInfo.enemyLevel)
            levelMod = 0.85f;
        else if(playerLevel < enemyInfo.enemyLevel)
            levelMod = 1.15f;

        if (playerVulnernable)
        {
            vulnerableBonus = VULNERABLE_REDUCTION;
            print("vulnerableBonus active + " + vulnerableBonus);
        }

        //handle attack boost modifier
        switch (enemyAttackBoost)
        {
            case 1:
                attBoostMod = 1.35f;
                break;
            case 2:
                attBoostMod = 1.75f;
                break;
            case 3:
                attBoostMod = 2.15f;
                break;
            default:
                attBoostMod = 1;
                break;
        }

        //handle attack boost modifier
        switch (enemyDefenseBoost)
        {
            case 1:
                defBoostMod = 1.35f;
                break;
            case 2:
                defBoostMod = 1.75f;
                break;
            case 3:
                defBoostMod = 2.15f;
                break;
            default:
                defBoostMod = 1;
                break;
        }

        print("attack: " + attack);
        print("enemyInfo.enemyLevel: " + enemyInfo.enemyLevel);
        damageDealt = (((attack * 2) * enemyInfo.enemyLevel)) * attBoostMod;

        damageDealt -= (GameController.controller.playerDefense * defBoostMod);
        print("Player Def: " + GameController.controller.playerDefense);
        print("Damage dealt: " + damageDealt);
        print("AttBoostMod: " + attBoostMod);

        damageDealt += randDamageBuffer;
        damageDealt *= levelMod;

        // critical hit chance
        float chance = (critRand + ((prowess / STAT_LIMIT) * 0.3f) + vulnerableBonus);
        print(chance);
        if ((chance - 0.05f) >= CRITICAL_THRESHOLD)
        {
            print("Critical Hit!");
            wasCriticalHit = true;
            damageDealt *= 1.5f;
        }

        damageDealt *= percentOfDamage;

        if (damageDealt < 1)
            damageDealt = 1;

        damageReturn = (int)damageDealt;
        playerHealth -= damageReturn;

        print("ENEMY DAMAGE: " + damageDealt);
        print("player is now at: " + playerHealth);

        return damageReturn;
    }

    public int DamagePlayer_Ability(Ability abilityUsed)
    {
        int damageReturn = 0;
        int rand = Random.Range(0, 100);
        int randDamageBuffer = Random.Range(0, 9);
        float accuracy = abilityUsed.Accuracy;
        float attBoostMod = 0;
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
                        attBoostMod = 1.2f;
                        break;
                    case 2:
                        attBoostMod = 1.5f;
                        break;
                    case 3:
                        attBoostMod = 1.7f;
                        break;
                    default:
                        attBoostMod = 1;
                        break;
                }

                damageDealt = ((attack + randDamageBuffer) * abilityUsed.BaseDamage) * attBoostMod;

                damageDealt -= (GameController.controller.playerDefense * (playerDefenseBoost * playerDefenseBoost));

                if (damageDealt < 1)
                    damageDealt = 1;

                damageReturn = (int)damageDealt;

                playerHealth -= damageReturn;

                print("damage: " + damageDealt);
                print("player HP: " + playerHealth);
            }
            else
            {
                this.GetComponent<AbilityManager_C>().EnemyAbilityMiss();
            }
        }
        else if (abilityUsed.Type == AbilityType.Magical)
        {
            // accuracy check the attack
            if (accuracy > rand)
            {
                //handle attack boost modifier
                switch (enemyAttackBoost)
                {
                    case 1:
                        attBoostMod = 1.2f;
                        break;
                    case 2:
                        attBoostMod = 1.5f;
                        break;
                    case 3:
                        attBoostMod = 1.7f;
                        break;
                    default:
                        attBoostMod = 1;
                        break;
                }

                damageDealt = ((attack + randDamageBuffer) * abilityUsed.BaseDamage) * attBoostMod;

                damageDealt -= (GameController.controller.playerDefense * (playerDefenseBoost * playerDefenseBoost));

                if (damageDealt < 1)
                    damageDealt = 1;

                damageReturn = (int)damageDealt;

                playerHealth -= damageReturn;
            }
            else
            {
                this.GetComponent<AbilityManager_C>().EnemyAbilityMiss();
            }
        }
        else
        {
            // accuracy check the attack
            if (accuracy > rand)
            {
                //handle attack boost modifier
                switch (enemyAttackBoost)
                {
                    case 1:
                        attBoostMod = 1.2f;
                        break;
                    case 2:
                        attBoostMod = 1.5f;
                        break;
                    case 3:
                        attBoostMod = 1.7f;
                        break;
                    default:
                        attBoostMod = 1;
                        break;
                }

                damageDealt = ((attack * abilityUsed.BaseDamage) + (randDamageBuffer)) * attBoostMod;

                damageDealt -= (GameController.controller.playerDefense * (playerDefenseBoost));

                if (damageDealt < 1)
                    damageDealt = 1;

                damageReturn = (int)damageDealt;

                playerHealth -= damageReturn;
            }
            else
            {
                this.GetComponent<AbilityManager_C>().EnemyAbilityMiss();
            }
        }

        return damageReturn;
    }

    /// END OF DAMAGE FUNCTIONS
    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void StruggleFailed(int damage)
    {
        print("Struggle failed");
        int originalHP = enemyHealth;
        enemyHealth -= damage;
        EndPlayerTurn(damage, originalHP);
    }

    public void HealPlayer(int amount)
    {
        float origHP = ((float)playerHealth / (float)playerMaxHealth);
        playerHealth += amount;

        if (playerHealth > playerMaxHealth)
            playerHealth = playerMaxHealth;


        StartCoroutine(HealAnim(origHP, ((float)playerHealth / (float)playerMaxHealth)));
    }

    IEnumerator HealAnim(float var1, float var2)
    {
        playerHealthBar.GetComponent<Image>().enabled = true;
        playerHealthBar.transform.GetChild(2).GetComponent<Image>().enabled = true;
        playerHealthBar.transform.GetChild(3).GetComponent<Text>().enabled = true;
        playerHealthBar.transform.GetChild(4).GetComponent<Text>().enabled = true;
        foreach (Image img in playerHealthBar.GetComponentsInChildren<Image>())
        {
            img.enabled = true;
        }

        yield return new WaitForSeconds(0.75f);

        playerHealthBar.GetComponent<HealthScript>().LerpHealth(var1, var2, 1f);
        print("orig HP: " + var1);
        print("new health: " + var2);
    }

    IEnumerator StanceChangeOptions()
    {
        DisableMainButtons();
        HideButton("middle");
        yield return new WaitForSeconds(0.1f);
        ShowButton("middle");
        yield return new WaitForSeconds(0.1f);
        HideButton("middle");
        yield return new WaitForSeconds(0.1f);
        ShowButton("middle");
        yield return new WaitForSeconds(0.1f);
        HideMainButtons();
        yield return new WaitForSeconds(0.25f);
        ShowStanceButtons();
        EnableStanceButtons();
    }

    public void UseMultiHit(bool playerTurn, Ability abilityUsed)
    {
        StartCoroutine(MultiHit(playerTurn, abilityUsed.Ticks, abilityUsed.SpecialValue));
    }

    IEnumerator MultiHit(bool playerTurn, int numHits, int baseDamage)
    {
        float var1, var2;
        int chance = 100;
        int random = 0;

        random = Random.Range(0, 100);

        if(chance >= random)
        {
            if (playerTurn && (enemyHealth > 0))
            {
                for (int i = 0; i < numHits; ++i)
                {
                    int damage = (int)(baseDamage + GameController.controller.playerAttack * 0.2f);
                    damage += Random.Range(1, 10);
                    print("multihit damage: " + damage);

                    var1 = (float)(enemyHealth / enemyMaxHealth);
                    var2 = (float)(enemyHealth - damage) / enemyMaxHealth;
                    yield return new WaitForSeconds(0.45f);
                    enemyHealth -= damage;
                    enemyHealthBar.GetComponent<HealthScript>().LerpHealth(var1, var2, 1f);
                    print("orig HP: " + var1);
                    print("new health: " + var2);
                }
            }
            else if(!playerTurn && (playerHealth > 0))
            {
                for (int i = 0; i < numHits; ++i)
                {
                    int damage = (int)(baseDamage * enemyInfo.enemyAttack * 0.2f);
                    damage += Random.Range(1, 10);
                    print("multihit damage: " + damage);

                    var1 = (float)(playerHealth / playerMaxHealth);
                    var2 = (float)(playerHealth - damage) / playerMaxHealth;
                    yield return new WaitForSeconds(0.45f);
                    playerHealth -= damage;
                    playerHealthBar.GetComponent<HealthScript>().LerpHealth(var1, var2, 1f);
                    print("orig HP: " + var1);
                    print("new health: " + var2);
                }
            }

            chance -= 35;
            random = Random.Range(0, 100);
        }
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
                {
                    enemyBlinded = true;
                    enemyBlindedDur = BLIND_DURATION;
                }   
                else
                {
                    playerBlinded = true;
                    playerBlindedDur = BLIND_DURATION;
                }
                break;
            case SpecialCase.Execute://
                currSpecialCase = SpecialCase.None;
                print("pProwess: " + (GameController.controller.playerProwess));
                int executeDamage = ((GameController.controller.playerProwess) * AbilityToolsScript.tools.LookUpAbility("Final Cut").SpecialValue);
                print("executeDamage: " + executeDamage);
                print("enemy hp: " + enemyHealth);
                
                if (enemyHealth > 0)
                {
                    if (playerTurn)
                    {
                        executeDamage = EvaluateExecution(executeDamage);
                        if (executeDamage > 0)
                        {
                            this.GetComponent<StruggleManager_C>().BeginStruggle_Player(executeDamage, enemyHealth);
                            return false;
                        }
                    }
                    else
                    {
                        print("ENEMIES CANT STRUGGLE DUMMY!");
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
        enemyMannequin.transform.GetChild(0).GetComponent<EnemyMannequinController>().PlayDeathSound();
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
    }

    public void CheckForMoreEnemies()
    {
        print("enemies remaining: " + enemiesRemaining);
        if(enemiesRemaining > 0)
        {
            StartCoroutine(LoadNextEnemy());
        }
        else // player won! go back YOU WIN!
        {
            this.GetComponent<LootManager_C>().GenerateLoot();
        }
    }

    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Enemy loading
    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// 
    void LoadNextEnemyInfo(bool initialEnemy = false)
    {
        enemyInfo = EnemyToolsScript.tools.LookUpEnemy(encounter.enemyNames[encounter.totalEnemies - enemiesRemaining]);
        StartCoroutine(LoadNextEnemy(initialEnemy));
    }

    IEnumerator LoadNextEnemy(bool initialEnemy = false)
    {
        if(enemyPrfb)
            Destroy(enemyPrfb);
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

                print("My spd: " + GameController.controller.playerSpeed + " Enemy spd: " + enemyInfo.enemySpeed);
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
        tempInfo.enemySpeed = tempInfo.enemySpeed * tempInfo.enemyLevel;
        enemyInfo = tempInfo;

        enemyCanLB = enemyInfo.canLimitBreak;
        currSpecialCase = SpecialCase.None;
        enemyStunned = false;
        enemyBlinded = false;
        enemyVulnernable = false;
        enemyAttackBoost = 0;
        enemyDefenseBoost = 0;
        enemySpeedBoost = 0;

        enemyMaxHealth = (enemyInfo.enemyMaxHealthBase * enemyInfo.enemyLevel) + (9 * enemyInfo.enemyDefense);
        enemyHealth = enemyMaxHealth;
        enemyCanLB = enemyInfo.canLimitBreak;

        this.GetComponent<EnemyCombatScript>().ResetEnemy(enemyInfo);
    }

    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Enemy loading
    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    IEnumerator PlayPlayerDeathAnim()
    {
        yield return new WaitForSeconds(1.5f);
        this.playerHealthBar.GetComponent<HealthScript>().Death();
        playerMannequin.GetComponent<AnimationController>().PlayDeathAnim();
        yield return new WaitForSeconds(0.5f);
        this.GetComponent<CombatAudio>().playSwordDrop();
        yield return new WaitForSeconds(2.5f);
        blackSq.GetComponent<FadeScript>().FadeIn();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Retry_Scene");
    }

    // --------------------    BUTTON SELECTION CODE --------------------- //

    public void RightSelected()
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

    public void LeftSelected()
    {
        this.GetComponent<CombatAudio>().playUISelect();

        switch (currentState)
        {
            case State.MainMenu:
                currentState = State.Abilities;
                HideMainButtons();
                DisableMainButtons();
                ShowBackButton();
                ShowAbilityButtons();
                EnableAbilityButtons();
                break;
        }
    }

    public void MiddleSelected()
    {
        this.GetComponent<CombatAudio>().playUISelect();

        switch (currentState)
        {
            case State.MainMenu:
                currentState = State.Stance;
                DisableMainButtons();
                ShowBackButton();
                ShowStanceButtons();
                EnableStanceButtons();
                StartCoroutine(StanceChangeOptions());
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
            case State.Stance:
                StartCoroutine(ShowMainMenuOptions());
                break;
        }
    }

    // --------------------    BUTTON SELECTION CODE --------------------- //


    // --------------------    BUTTON VISIBILITY CODE --------------------- //

    public void HideMainButtons()
    {
        middleButton.GetComponent<Image>().enabled = false;
        leftButton.GetComponent<Image>().enabled = false;
        rightButton.GetComponent<Image>().enabled = false;

        middleButton.GetComponentInChildren<Text>().enabled = false;
        leftButton.GetComponentInChildren<Text>().enabled = false;
        rightButton.GetComponentInChildren<Text>().enabled = false;
    }

    public void ShowMainButtons()
    {
        middleButton.GetComponent<Image>().enabled = true;
        leftButton.GetComponent<Image>().enabled = true;
        rightButton.GetComponent<Image>().enabled = true;

        middleButton.GetComponentInChildren<Text>().enabled = true;
        leftButton.GetComponentInChildren<Text>().enabled = true;
        rightButton.GetComponentInChildren<Text>().enabled = true;
    }

    void HideButton(string buttonName)
    {
        ChangeButtonStatus(buttonName, false);
    }

    void ShowButton(string buttonName)
    {
        ChangeButtonStatus(buttonName, true);
    }

    void ChangeButtonStatus(string buttonName, bool status)
    {
        switch (buttonName)
        {
            case "middle":
                middleButton.GetComponent<Image>().enabled = status;
                middleButton.GetComponentInChildren<Text>().enabled = status;
                break;
            case "left":
                leftButton.GetComponent<Image>().enabled = status;
                leftButton.GetComponentInChildren<Text>().enabled = status;
                break;
            case "right":
                rightButton.GetComponent<Image>().enabled = status;
                rightButton.GetComponentInChildren<Text>().enabled = status;
                break;
            case "TLA1_Button":
                abilityButton1.SetActive(status);
                break;
            case "TLA2_Button":
                abilityButton2.SetActive(status);
                break;
            case "TLA3_Button":
                abilityButton3.SetActive(status);
                break;
            case "TLA4_Button":
                abilityButton4.SetActive(status);
                break;
            case "back":
                backButton.SetActive(status);
                break;
            case "agressive":
                agressiveStance_B.SetActive(status);
                break;
            case "defensive":
                defensiveStance_B.SetActive(status);
                break;
            case "focused":
                focusedStance_B.SetActive(status);
                break;
            default:
                break;
        }
    }

    public void ShowStanceButtons()
    {
        agressiveStance_B.SetActive(true);
        defensiveStance_B.SetActive(true);
        focusedStance_B.SetActive(true);
    }

    public void HideStanceButtons()
    {
        agressiveStance_B.SetActive(false);
        defensiveStance_B.SetActive(false);
        focusedStance_B.SetActive(false);
    }

    public void ShowAbilityButtons()
    {
        abilityButton1.SetActive(true);
        abilityButton2.SetActive(true);
        abilityButton3.SetActive(true);
        abilityButton4.SetActive(true);
    }

    public void HideAbilityButtons()
    {
        abilityButton1.SetActive(false);
        abilityButton2.SetActive(false);
        abilityButton3.SetActive(false);
        abilityButton4.SetActive(false);
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

    public void ShowBackButton()
    {
        backButton.SetActive(true);
    }

    public void HideBackButton()
    {
        backButton.SetActive(false);
    }

    // --------------------    BUTTON VISIBILITY CODE --------------------- //


    // --------------------    BUTTON ENABLED CODE --------------------- //
    public void EnableMainButtons()
    {
        middleButton.GetComponentInChildren<Button>().enabled = true;
        leftButton.GetComponentInChildren<Button>().enabled = true;
        rightButton.GetComponentInChildren<Button>().enabled = true;
    }

    public void DisableMainButtons()
    {
        middleButton.GetComponentInChildren<Button>().enabled = false;
        leftButton.GetComponentInChildren<Button>().enabled = false;
        rightButton.GetComponentInChildren<Button>().enabled = false;
    }

    public void EnableStanceButtons()
    {
        agressiveStance_B.GetComponent<Button>().enabled = true;
        defensiveStance_B.GetComponent<Button>().enabled = true;
        focusedStance_B.GetComponent<Button>().enabled = true;
    }

    public void DisableStanceButtons()
    {
        agressiveStance_B.GetComponent<Button>().enabled = false;
        defensiveStance_B.GetComponent<Button>().enabled = false;
        focusedStance_B.GetComponent<Button>().enabled = false;
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

    // --------------------    BUTTON ENABLED CODE --------------------- //

    //void SpawnItemsUI()
    //{
    //    GameController.controller.playerInventory = new string[3];
    //    GameController.controller.playerInventoryQuantity = new int[3];

    //    GameController.controller.playerInventory[0] = "woa";
    //    GameController.controller.playerInventory[1] = "Premium Quality cheese";
    //    GameController.controller.playerInventory[2] = "a shoe";

    //    GameController.controller.playerInventoryQuantity[0] = 1;
    //    GameController.controller.playerInventoryQuantity[1] = 69;
    //    GameController.controller.playerInventoryQuantity[2] = 1;

    //    for (int buttonNum = 0; buttonNum < GameController.controller.playerInventory.Length; ++buttonNum)
    //    {
    //        Vector2 newpos = new Vector2(200 * buttonNum, 0);
    //        GameObject testB = Instantiate(abilityButtonPrefab, newpos, Quaternion.identity) as GameObject;
    //        testB.transform.SetParent(canvas.transform);
    //        testB.name = "ItemButton" + buttonNum + "_" + GameController.controller.playerInventory[buttonNum];
    //        testB.GetComponentInChildren<Text>().text = GameController.controller.playerInventory[buttonNum];
    //    }
    //}

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

    public void setAbilityButtons()
    {
        if (ability1.Name == "-")
        {
            abilityButton1.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0.75f);
            abilityButton1.transform.GetChild(1).GetComponent<Text>().text = "";
        }
        else
        {
            abilityButton1.GetComponent<Image>().color = getAbilityTypeColor(ability1);
            abilityButton1.transform.GetChild(1).GetComponent<Text>().text = ability1.Cooldown.ToString();
        }

        if (ability2.Name == "-")
        {
            abilityButton2.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0.75f);
            abilityButton2.transform.GetChild(1).GetComponent<Text>().text = "";
        }
        else
        {
            abilityButton2.GetComponent<Image>().color = getAbilityTypeColor(ability2);
            abilityButton2.transform.GetChild(1).GetComponent<Text>().text = ability2.Cooldown.ToString();
        }

        if (ability3.Name == "-")
        {
            abilityButton3.transform.GetChild(1).GetComponent<Text>().text = "";
            abilityButton3.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0.75f);
        }
        else
        {
            abilityButton3.GetComponent<Image>().color = getAbilityTypeColor(ability3);
            abilityButton3.transform.GetChild(1).GetComponent<Text>().text = ability3.Cooldown.ToString();
        }

        if (ability4.Name == "-")
        {
            abilityButton4.transform.GetChild(1).GetComponent<Text>().text = "";
            abilityButton4.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0.75f);
        }
        else
        {
            abilityButton4.GetComponent<Image>().color = getAbilityTypeColor(ability4);
            abilityButton4.transform.GetChild(1).GetComponent<Text>().text = ability4.Cooldown.ToString();
        }
    }

    public int getPlayerHealth()
    {
        return playerHealth;
    }

    public int getEnemyHealth()
    {
        return enemyHealth;
    }

    public void setBoostDur(string stat, int duration, bool player)
    {
        switch(stat)
        {
            case "Attack":
                if (player)
                    playerAttBoostDur = duration;
                else
                    enemyAttBoostDur = duration;
                break;
            case "Defense":
                if (player)
                    playerDefBoostDur = duration;
                else
                    enemyDefBoostDur = duration;
                break;
            case "Speed":
                if (player)
                    playerSpdBoostDur = duration;
                else
                    enemySpdBoostDur = duration;
                break;
        }
    }
}
