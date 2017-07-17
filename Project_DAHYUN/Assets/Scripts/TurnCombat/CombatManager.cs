using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CombatManager : MonoBehaviour {

    enum State { MainMenu, Items, Abilities, Back, Done };

    public Canvas canvas;
    public GameObject playerMannequin;
    private Vector3 initPlayerPos;

    //MAIN BUTTON VARIABLES
    public GameObject topButton, leftButton, rightButton, backButton;
    public Color strike_C, items_C, abilities_C, back_C;

    private State currentState = State.MainMenu;

    //COMBAT VARIABLES
    public SpecialCase currSpecialCase = SpecialCase.None;
    public GameObject playerHealthBar;
    public GameObject playerHealthCase;

    private int playerHealth;
    private int playerMaxHealth = 100;
    private int playerAttackBoost = 0;
    private int playerDefenseBoost = 0;
    private int playerSpeedBoost = 0;

    // ENEMY COMBAT VARIABLES
    public GameObject enemyHealthBar;
    private int enemyHealth;
    private int enemyMaxHealth = 100;
    private int enemyAttackBoost = 0;
    private int enemyDefenseBoost = 0;
    private int enemySpeedBoost = 0;

    //STRIKE VARIABLES
    public float strikeAnimDuration = 3.5f;
    public float strikePosX = 320f;
    public string strikeMod = "none";

    //ABILITY VARIABLES
    public Vector2 ab1_pos, ab2_pos, ab3_pos, ab4_pos;
    public GameObject abilityButtonPrefab;
    public GameObject abilityButton1, abilityButton2, abilityButton3, abilityButton4;
    public Ability ability1, ability2, ability3, ability4;
    public Color abilityTextColor;
    public Color abilitySelectColor;


    // Use this for initialization
    void Start () {
        //0. pretend the player has save data for ability sake
        GameController.controller.playerAbility1 = AbilityToolsScript.tools.LookUpAbility("Shadow Strike");
        GameController.controller.playerAbility2 = AbilityToolsScript.tools.LookUpAbility("Solar Flare");
        GameController.controller.playerAbility3 = AbilityToolsScript.tools.LookUpAbility("Outrage");
        GameController.controller.playerAbility4 = AbilityToolsScript.tools.LookUpAbility("Illusion");
        GameController.controller.strikeModifier = "Shadow Strike";
        GameController.controller.playerAttack = 50;

        //1. Load in player and enemy
        playerHealth = playerMaxHealth;

        initPlayerPos = playerMannequin.transform.position;
        strikeMod = GameController.controller.strikeModifier;
        ability1 = GameController.controller.playerAbility1;
        ability2 = GameController.controller.playerAbility2;
        ability3 = GameController.controller.playerAbility3;
        ability4 = GameController.controller.playerAbility4;

        abilityButton1.GetComponentInChildren<Text>().text = ability1.Name;
        abilityButton2.GetComponentInChildren<Text>().text = ability2.Name;
        abilityButton3.GetComponentInChildren<Text>().text = ability3.Name;
        abilityButton4.GetComponentInChildren<Text>().text = ability4.Name;

        //2. Display buttons: STRIKE, ITEMS, ABILITIES
        DisableAbilityButtons();
        HideAbilityButtons();
        StartCoroutine(ShowStartingButtons());
        DisableBackButton();
    }

    public void StrikeSelected(int selectedOption = 0)
    {
        this.GetComponent<StrikeManager_C>().StrikeUsed(strikeMod);
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

    public void ItemSelected(int itemNum = 0)
    {
        this.GetComponent<ItemsManager_C>().ItemUsed(itemNum);
    }

    public void EndPlayerTurn()
    {
        currentState = State.MainMenu;
        DisableAbilityButtons();
        HideAbilityButtons();
        DisableBackButton();
        EnableMainButtons();
        ShowHealthBars();
        StartCoroutine(ShowStartingButtons());
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
        yield return new WaitForSeconds(0.25f);
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
        Vector3 centerPos = new Vector3(225, -55, 1);
        Vector3 ascend = new Vector3(225, 100, 1);
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

        this.GetComponent<AbilityManager_C>().AbilityUsed(abilityUsed);
    }

    // Combat Functions
    ////////////////////////////////////////////
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
        this.GetComponent<StrikeManager_C>().StrikeUsed(strikeMod);
    }

    // DAMAGE AN ENEMY WITH STRIKE ONLY!
    public void DamageEnemy_Strike()
    {
        int rand = Random.Range(0, 100);
        int randDamageBuffer = Random.Range(0, 9);
        int accuracy = 95;
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
                    attBoostMod = 2f;
                    break;
                case 3:
                    attBoostMod = 2.5f;
                    break;
                default:
                    break;
            }

            // check for special attack modifier
            if (currSpecialCase == SpecialCase.None)
            {
                damageDealt = (attack * attack) + (randDamageBuffer);
                playerHealth -= (int)damageDealt;
            }
            else
            {
                ResolveSpecialCase();
            }
        }
    }

    // DAMAGE AN ENEMY WITH AN ABILITY ONLY!
    public void DamageEnemy_Ability(Ability abilityUsed)
    {
        if(abilityUsed.Type == AbilityType.Physical)
        {
            int rand = Random.Range(0, 100);
            int accuracy = 75;

            // accuracy check the attack
            if (accuracy > rand)
            {
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
            //kms
        }
    }

    public void DamagePlayer()
    {
        //idk yet lol
    }

    // DAMAGE ENEMY WITH AN ITEM ONLY!
    public void DamageEnemy_Item(string itemUsed)
    {
        int rand = Random.Range(0, 100);
        int accuracy = 75;



        // accuracy check the attack
        if (accuracy > rand)
        {
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
                HideHealthBars();
                ShowAbilityButtons();
                EnableAbilityButtons();
                break;
        }
    }

    public void LeftSelected()
    {
        this.GetComponent<CombatAudio>().playUISelect();

        switch (currentState)
        {
            case State.MainMenu:
                currentState = State.Items;
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
            case State.Items:
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
}
