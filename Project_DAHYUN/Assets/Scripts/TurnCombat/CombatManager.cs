using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CombatManager : MonoBehaviour {

    enum State { MainMenu, Items, Abilities, Back, Done };

    public GameObject playerMannequin;
    private Vector3 initPlayerPos;

    //MAIN BUTTON VARIABLES
    public GameObject topButton, leftButton, rightButton, backButton;
    public Color strike_C, items_C, abilities_C, back_C;

    private State currentState = State.MainMenu;

    //COMBAT VARIABLES
    SpecialCase currSpecialCase = SpecialCase.None;

    //STRIKE VARIABLES
    public float strikeAnimDuration = 3.5f;
    public float strikePosX = 320f;
    public string strikeMod;

    //ABILITY VARIABLES
    public Vector2 ab1_pos, ab2_pos, ab3_pos, ab4_pos;
    public GameObject abilityButtonPrefab;
    GameObject abilityButton1, abilityButton2, abilityButton3, abilityButton4;
    public Ability ability1, ability2, ability3, ability4;


    // Use this for initialization
    void Start () {
        //1. Load in player and enemy
        initPlayerPos = playerMannequin.transform.position;
        strikeMod = GameController.controller.strikeModifier;
        ability1 = GameController.controller.playerAbility1;
        ability2 = GameController.controller.playerAbility2;
        ability3 = GameController.controller.playerAbility3;
        ability4 = GameController.controller.playerAbility4;

        //2. Display buttons: STRIKE, ITEMS, ABILITIES
        StartCoroutine(ShowStartingButtons());
        DisableBackButton();
    }

    public void StrikeSelected(int selectedOption = 0)
    {
            this.GetComponent<StrikeManager_C>().StrikeUsed(strikeMod);
    }

    public void AbilitySelected(int selectedOption = 0)
    {
        if (selectedOption == 1)
            this.GetComponent<AbilityManager_C>().AbilityUsed(ability1);
        if (selectedOption == 2)
            this.GetComponent<AbilityManager_C>().AbilityUsed(ability2);
        if (selectedOption == 3)
            this.GetComponent<AbilityManager_C>().AbilityUsed(ability3);
        if (selectedOption == 4)
            this.GetComponent<AbilityManager_C>().AbilityUsed(ability4);
    }

    public void ItemSelected(int itemNum = 0)
    {
        this.GetComponent<ItemsManager_C>().ItemUsed(itemNum);
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

    // Combat Functions
    ////////////////////////////////////////////
    IEnumerator UseStrike()
    {
        DisableMainButtons();
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
        AnimatePlayerStrike();
        yield return new WaitForSeconds(0.25f);
    }

    void DealDamage()
    {
        int rand = Random.Range(0, 100);
        int accuracy = 75;

        // accuracy check the attack
        if(accuracy > rand)
        {
            // check for special attack modifier
            if(currSpecialCase == SpecialCase.None)
            {

            }
            else
            {
                ResolveSpecialCase();
            }
        }
    }

    void AnimatePlayerStrike()
    {
        Vector3 newPos = new Vector3(strikePosX, initPlayerPos.y, 0);
        playerMannequin.GetComponent<LerpScript>().LerpToPos(initPlayerPos, newPos, strikeAnimDuration * .5f);
        
    }

    void ResolveSpecialCase()
    {

    }

    /// Helper Functions
    /// ////////////////////////////////////////

    public void TopSelected()
    {
        switch(currentState)
        {
            case State.MainMenu:
                currentState = State.Done;
                StartCoroutine(UseStrike());
                break;
        }
    }

    public void RightSelected()
    {
        switch (currentState)
        {
            case State.MainMenu:
                currentState = State.Abilities;
                SpawnAbilityButtons();
                break;
        }
    }

    public void LeftSelected()
    {
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
        switch (currentState)
        {
            case State.Abilities:
                currentState = State.MainMenu;
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

    void SpawnAbilityButtons()
    {
        abilityButton1 = Instantiate(abilityButtonPrefab, ab1_pos, Quaternion.identity) as GameObject;
        abilityButton2 = Instantiate(abilityButtonPrefab, ab2_pos, Quaternion.identity) as GameObject;
        abilityButton3 = Instantiate(abilityButtonPrefab, ab3_pos, Quaternion.identity) as GameObject;
        abilityButton4 = Instantiate(abilityButtonPrefab, ab4_pos, Quaternion.identity) as GameObject;
         
        abilityButton1.GetComponentInChildren<Text>().text = ability1.Name;
        abilityButton2.GetComponentInChildren<Text>().text = ability2.Name;
        abilityButton3.GetComponentInChildren<Text>().text = ability3.Name;
        abilityButton4.GetComponentInChildren<Text>().text = ability4.Name;
    }

    void SpawnItemsUI()
    {
        
    }
}
