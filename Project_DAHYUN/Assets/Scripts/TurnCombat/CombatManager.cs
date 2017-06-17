using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CombatManager : MonoBehaviour {

    enum State { MainMenu, Fight, Items, Abilities, Stances, Back };

    //MAIN BUTTON VARIABLES
    public GameObject topButton, leftButton, rightButton, backButton;
    public Color fight_C, retreat_C, items_C, stance_C, abilities_C, back_C;

    private State currentState = State.MainMenu;

    //ABILITY VARIABLES
    public Vector2 ab1_pos, ab2_pos, ab3_pos, ab4_pos;
    public GameObject abilityButtonPrefab;
    GameObject abilityButton1, abilityButton2, abilityButton3, abilityButton4;
    public Ability ability1, ability2, ability3, ability4;


    // Use this for initialization
    void Start () {
        //1. Load in player and enemy
        //2. Display buttons: FIGHT, ITEMS, RETREAT
        StartCoroutine(ShowStartingButtons());
        DisableBackButton();
    }
	
	// Update is called once per frame
	void Update () {
	    
	}


    IEnumerator ShowStartingButtons()
    {
        HideMainButtons();
        //yield return new WaitForSeconds(2);
        ShowMainButtons();
        yield return new WaitForSeconds(0.1f);
        HideMainButtons();
        yield return new WaitForSeconds(0.1f);
        ShowMainButtons();
    }

    IEnumerator ShowFightOptions()
    {
        DisableMainButtons();
        DisableBackButton();
        HideButton("top");
        yield return new WaitForSeconds(0.1f);
        ShowButton("top");
        yield return new WaitForSeconds(0.1f);
        HideButton("top");
        yield return new WaitForSeconds(0.1f);
        ShowButton("top");
        yield return new WaitForSeconds(0.1f);
        HideMainButtons();

        topButton.GetComponentInChildren<Text>().text = "STANCE";
        topButton.GetComponent<Image>().color = stance_C;
        leftButton.GetComponentInChildren<Text>().text = "BACK";
        leftButton.GetComponent<Image>().color = back_C;
        rightButton.GetComponentInChildren<Text>().text = "ABILITIES";
        rightButton.GetComponent<Image>().color = abilities_C;

        yield return new WaitForSeconds(0.5f);

        ShowMainButtons();
        EnableMainButtons();
        EnableBackButton();
    }

    IEnumerator ShowMainMenuOptions()
    {
        DisableBackButton();
        DisableMainButtons();
        yield return new WaitForSeconds(0.25f);
        HideMainButtons();

        topButton.GetComponentInChildren<Text>().text = "FIGHT";
        topButton.GetComponent<Image>().color = fight_C;
        leftButton.GetComponentInChildren<Text>().text = "RETREAT";
        leftButton.GetComponent<Image>().color = retreat_C;
        rightButton.GetComponentInChildren<Text>().text = "ITEMS";
        rightButton.GetComponent<Image>().color = items_C;

        yield return new WaitForSeconds(0.5f);

        ShowMainButtons();
        EnableMainButtons();
    }

    IEnumerator ShowStancesOptions()
    {
        DisableMainButtons();
        DisableBackButton();
        HideButton("top");
        yield return new WaitForSeconds(0.1f);
        ShowButton("top");
        yield return new WaitForSeconds(0.1f);
        HideButton("top");
        yield return new WaitForSeconds(0.1f);
        ShowButton("top");
        yield return new WaitForSeconds(0.1f);
        HideMainButtons();

        topButton.GetComponentInChildren<Text>().text = "ATTACK";
        topButton.GetComponent<Image>().color = stance_C;
        leftButton.GetComponentInChildren<Text>().text = "REACT";
        leftButton.GetComponent<Image>().color = back_C;
        rightButton.GetComponentInChildren<Text>().text = "DEFEND";
        rightButton.GetComponent<Image>().color = abilities_C;

        yield return new WaitForSeconds(0.5f);

        ShowMainButtons();
        EnableMainButtons();
        EnableBackButton();
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

        topButton.GetComponentInChildren<Text>().text = "FIGHT";
        topButton.GetComponent<Image>().color = stance_C;
        leftButton.GetComponentInChildren<Text>().text = "RETREAT";
        leftButton.GetComponent<Image>().color = back_C;
        rightButton.GetComponentInChildren<Text>().text = "ITEMS";
        rightButton.GetComponent<Image>().color = abilities_C;

        yield return new WaitForSeconds(0.5f);

        ShowMainButtons();
        EnableMainButtons();
    }

    /// Helper Functions
    /// ////////////////////////////////////////

    public void TopSelected()
    {
        switch(currentState)
        {
            case State.MainMenu:
                currentState = State.Fight;
                StartCoroutine(ShowFightOptions());
                break;
            case State.Fight:
                currentState = State.Stances;
                StartCoroutine(ShowStancesOptions());
                break;
            case State.Stances:
                SpawnAbilityButtons();
                break;
        }
    }

    public void BackSelected()
    {
        switch (currentState)
        {
            case State.Fight:
                currentState = State.MainMenu;
                StartCoroutine(ShowMainMenuOptions());
                DisableBackButton();
                break;
            case State.Stances:
                currentState = State.Fight;
                StartCoroutine(ShowFightOptions());
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

        //ability1 = GameController.controller.playersa
    }
}
