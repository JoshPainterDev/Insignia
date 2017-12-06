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

    public GameObject camera;
    public GameObject blackSq;
    public Vector3 characterCameraPos;
    public Vector3 adventureCameraPos;
    public Vector3 craftingCameraPos;

    public void Start()
    {
        playerMannequin.GetComponent<AnimationController>().LoadCharacter();
        background.GetComponent<SpriteRenderer>().color = GameController.controller.getPlayerColorPreference();
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
        camera.GetComponent<LerpScript>().LerpToPos(camera.transform.position, characterCameraPos, 1f);
        yield return new WaitForSeconds(0.25f);
        blackSq.GetComponent<FadeScript>().FadeIn(1.5f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Character_Scene");
    }

    IEnumerator LoadAdventureScreen()
    {
        camera.GetComponent<LerpScript>().LerpToPos(camera.transform.position, adventureCameraPos, 1f);
        yield return new WaitForSeconds(0.25f);
        blackSq.GetComponent<FadeScript>().FadeIn(1.5f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("AdventureSelect_Scene");
    }

    IEnumerator LoadCraftingScreen()
    {
        camera.GetComponent<LerpScript>().LerpToPos(camera.transform.position, craftingCameraPos, 1f);
        yield return new WaitForSeconds(0.25f);
        blackSq.GetComponent<FadeScript>().FadeIn(1.5f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Crafting_Scene");
    }

    IEnumerator LoadSettingsScreen()
    {
        yield return new WaitForSeconds(0.1f);
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
}
