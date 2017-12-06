using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AdventureSelect_Manager : MonoBehaviour
{
    public GameObject playerMannequin;

    public GameObject canvas;
    public GameObject camera;
    public GameObject blackSq;
    public Vector3 mmCameraPos;
    public GameObject selectPrefab;
    public GameObject background;

    public GameObject BackButton;
    public Vector3 adventureCameraPos;

    private string levelToLoad;

    // Use this for initialization
    void Start ()
    {
        GameController.controller.levelsCompleted = 0;
        GameController.controller.stagesCompleted = 0;
        playerMannequin.GetComponent<AnimationController>().LoadCharacter();
        background.GetComponent<Image>().color = GameController.controller.getPlayerColorPreference();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            GameController.controller.currentEncounter = EncounterToolsScript.tools.SpecifyEncounter(0, 1);
            SceneManager.LoadScene("TurnCombat_Scene");
        }
    }

    public void SelectAdventure(int adventureNum)
    {
        if (adventureNum > (GameController.controller.stagesCompleted + 1))
        {
            // play a "you havnt unlocked this level" sound
            GameController.controller.GetComponent<MenuUIAudio>().playNope();
            return;
        }

        switch(adventureNum)
        {
            case 1:
                levelToLoad = "Exposition_Scene05";
                StartCoroutine(LoadEncounter(adventureNum, GameController.controller.levelsCompleted));
                break;
            case 2:
                levelToLoad = "Exposition_Scene01";
                StartCoroutine(LoadEncounter(adventureNum, GameController.controller.levelsCompleted));
                break;
            case 3:
                levelToLoad = "Exposition_Scene01";
                StartCoroutine(LoadEncounter(adventureNum, GameController.controller.levelsCompleted));
                break;
            default:
                StartCoroutine(LoadCombatScreen());
                break;
        }
    }

    IEnumerator LoadEncounter(int stageToLoad, int levelsCompleted)
    {
        GameController.controller.currentEncounter =  EncounterToolsScript.tools.SpecifyEncounter(stageToLoad, levelsCompleted);

        yield return new WaitForSeconds(0.35f);
        //DO SOME CUTE ANIMATION WITH THE MANNEQUIN!
        Destroy(selectPrefab);
        Destroy(BackButton);
        yield return new WaitForSeconds(0.25f);
        playerMannequin.GetComponent<AnimationController>().PlayAttackAnim();
        yield return new WaitForSeconds(1.5f);
        playerMannequin.GetComponent<AnimationController>().PlayWalkAnim();
        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, playerMannequin.transform.position + new Vector3(500, 0, 0), 0.5f);
        camera.GetComponent<LerpScript>().LerpToPos(camera.transform.position, adventureCameraPos, 0.5f);
        yield return new WaitForSeconds(1f);
        blackSq.GetComponent<FadeScript>().FadeIn(1.5f);
        yield return new WaitForSeconds(0.25f);
        blackSq.GetComponent<FadeScript>().FadeIn(1.5f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(levelToLoad);
    }

    public void GoBack()
    {
        StartCoroutine(GoToMainMenu());
    }

    public IEnumerator GoToMainMenu()
    {
        DisableButtons();
        camera.GetComponent<LerpScript>().LerpToPos(camera.transform.position, mmCameraPos, 1f);
        yield return new WaitForSeconds(0.25f);
        blackSq.GetComponent<FadeScript>().FadeIn(1.5f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("MainMenu_Scene");
    }

    IEnumerator LoadCombatScreen()
    {
        Destroy(BackButton);
        Destroy(selectPrefab);
        yield return new WaitForSeconds(0.25f);
        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, new Vector3(100,-63,0),2);
        camera.GetComponent<LerpScript>().LerpToPos(camera.transform.position, adventureCameraPos, 1f);
        yield return new WaitForSeconds(3.5f);
        blackSq.GetComponent<FadeScript>().FadeIn(1.5f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("TurnCombat_Scene");
    }

    public void DisableButtons()
    {
        BackButton.GetComponent<Button>().enabled = false;
    }
}
