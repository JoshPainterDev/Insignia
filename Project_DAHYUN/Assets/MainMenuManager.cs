using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {

    public GameObject playerMannequin;
    public GameObject characterButton;
    public GameObject adventureButton;
    public GameObject craftingButton;
    public GameObject settingsButton;
    public GameObject charSelectButton;
    public GameObject background;
    public GameObject settingsPopup;
    public GameObject settingsManager;
    public GameObject battlePopUp;

    public GameObject camera;
    public GameObject blackSq;
    public Vector3 characterCameraPos;
    public Vector3 adventureCameraPos;
    public Vector3 arenaCameraPos;

    public void Start()
    {
        playerMannequin = GameController.controller.playerObject;
        background.GetComponent<SpriteRenderer>().color = GameController.controller.getPlayerColorPreference();
        playerMannequin.GetComponent<AnimationController>().SetCombatState(false);
        playerMannequin.GetComponent<AnimationController>().PlayIdleAnim();
    }

    public void ButtonPressed(int buttonNumber)
    {
        switch(buttonNumber)
        {
            case 1: // Character
                StartCoroutine(LoadCharacterScreen());
                break;
            case 2: // Adventure
                StartCoroutine(LoadAdventureScreen());
                break;
            case 3: // Battle
                StartCoroutine(LoadBattlePopUp());
                return;
            case 4: // Settings
                StartCoroutine(LoadSettingsScreen());
                break;
            case 5: // Character Select 
                StartCoroutine(LoadCharSelectScene());
                break;
        }

        DisableButtons();
    }


    IEnumerator LoadCharacterScreen()
    {
        camera.GetComponent<LerpScript>().LerpToPos(camera.transform.position, characterCameraPos, 1.5f);
        yield return new WaitForSeconds(0.25f);
        blackSq.GetComponent<FadeScript>().FadeIn(2.5f);
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene("Character_Scene");
    }

    IEnumerator LoadAdventureScreen()
    {
        camera.GetComponent<LerpScript>().LerpToPos(camera.transform.position, adventureCameraPos, 1.5f);
        yield return new WaitForSeconds(0.25f);
        blackSq.GetComponent<FadeScript>().FadeIn(2.5f);
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene("AdventureSelect_Scene");
    }

    IEnumerator LoadBattlePopUp()
    {
        yield return new WaitForSeconds(0.15f);
        battlePopUp.SetActive(true);
        //camera.GetComponent<LerpScript>().LerpToPos(camera.transform.position, arenaCameraPos, 1.5f);
        //yield return new WaitForSeconds(0.25f);
        //blackSq.GetComponent<FadeScript>().FadeIn(2.5f);
        //yield return new WaitForSeconds(0.75f);
    }

    IEnumerator LoadSettingsScreen()
    {
        yield return new WaitForSeconds(0.15f);
        settingsPopup.SetActive(true);
        settingsManager.GetComponent<SettingsManager>().ResetPosition();
    }

    IEnumerator LoadCharSelectScene()
    {
        yield return new WaitForSeconds(0.25f);
        blackSq.GetComponent<FadeScript>().FadeIn(1.5f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("CharacterSelect_Scene");
    }

    public void DisableButtons()
    {
        characterButton.GetComponent<Button>().enabled = false;
        adventureButton.GetComponent<Button>().enabled = false;
        craftingButton.GetComponent<Button>().enabled = false;
        settingsButton.GetComponent<Button>().enabled = false;
        charSelectButton.GetComponent<Button>().enabled = false;
    }

    public void EnableButtons()
    {
        characterButton.GetComponent<Button>().enabled = true;
        adventureButton.GetComponent<Button>().enabled = true;
        craftingButton.GetComponent<Button>().enabled = true;
        settingsButton.GetComponent<Button>().enabled = true;
        charSelectButton.GetComponent<Button>().enabled = true;
    }
}
