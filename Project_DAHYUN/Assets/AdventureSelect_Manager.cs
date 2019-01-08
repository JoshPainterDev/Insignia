using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AdventureSelect_Manager : MonoBehaviour
{
    public int MAX_LEVELS = 5;
    public GameObject playerMannequin;

    public GameObject canvas;
    public GameObject camera;
    public GameObject blackSq;
    public Vector3 mmCameraPos;
    public GameObject selectPrefab;
    public GameObject background;
    public GameObject gridThing;

    public GameObject BackButton;
    public Vector3 adventureCameraPos;

    public Color highlight_C;
    public Color unselected_C;
    public GameObject selectButton;
    public GameObject scrollbar;

    private string levelToLoad;
    private int currentLevel;
    private int prevLevel = 0;

    // Use this for initialization
    void Start ()
    {
        playerMannequin = GameController.controller.playerObject;
        background.GetComponent<SpriteRenderer>().color = GameController.controller.getPlayerColorPreference();

        //GameController.controller.stagesCompleted = 5;
        //print("Stages completed: " + GameController.controller.stagesCompleted);
        for(int i = 0; i < gridThing.transform.childCount; ++i)
        {
            print(GameController.controller.stagesCompleted);
            if(i < (GameController.controller.stagesCompleted + 1))
            {
                UnlockLevel(i);
            }
        }

        HighlightLevel(Mathf.Min(GameController.controller.stagesCompleted, MAX_LEVELS));
    }

    //public void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha9))
    //    {
    //        GameController.controller.currentEncounter = EncounterToolsScript.tools.SpecifyEncounter(0, 1);
    //        SceneManager.LoadScene("TurnCombat_Scene");
    //    }
    //}

    public void UnlockLevel(int levelNum)
    {
        switch(levelNum)
        {
            case 0:
                gridThing.transform.GetChild(levelNum).GetChild(0).GetChild(0).GetComponent<Text>().text = "THE BULWARK";
                gridThing.transform.GetChild(levelNum).GetChild(1).gameObject.SetActive(false);
                break;
            case 1:
                gridThing.transform.GetChild(levelNum).GetChild(0).GetChild(0).GetComponent<Text>().text = "KINGDOM OF LIGHT";
                gridThing.transform.GetChild(levelNum).GetChild(1).gameObject.SetActive(false);
                break;
            case 2:
                gridThing.transform.GetChild(levelNum).GetChild(0).GetChild(0).GetComponent<Text>().text = "THE DARK FOREST";
                gridThing.transform.GetChild(levelNum).GetChild(1).gameObject.SetActive(false);
                break;
            case 3:
                gridThing.transform.GetChild(levelNum).GetChild(0).GetChild(0).GetComponent<Text>().text = "RAVEN'S CRYPT";
                gridThing.transform.GetChild(levelNum).GetChild(1).gameObject.SetActive(false);
                break;
            case 4:
                gridThing.transform.GetChild(levelNum).GetChild(0).GetChild(0).GetComponent<Text>().text = "SANCTUM SOLORAI";
                gridThing.transform.GetChild(levelNum).GetChild(1).gameObject.SetActive(false);
                break;
        }
    }

    public void HighlightLevel(int levelNum)
    {
        if (levelNum == prevLevel)
            return;

        currentLevel = levelNum;

        gridThing.transform.GetChild(currentLevel - 1).GetChild(0).GetComponent<Image>().color = highlight_C;

        float scrollVal = currentLevel < 3 ? 1 : (1.0f - (float)(currentLevel) / (float)MAX_LEVELS);
        scrollbar.GetComponent<Scrollbar>().value = scrollVal;

        if (prevLevel > 0)
        {
            gridThing.transform.GetChild(prevLevel - 1).GetChild(0).GetComponent<Image>().color = unselected_C;
        }

        prevLevel = currentLevel;

        print("Current Level: " + currentLevel);
        print("completed Levels: " + GameController.controller.stagesCompleted);
        if (currentLevel > (GameController.controller.stagesCompleted + 1))
        {
            // play a "you havnt unlocked this level" sound
            GameController.controller.GetComponent<MenuUIAudio>().playNope();
            selectButton.SetActive(false);
            return;
        }

        selectButton.SetActive(true);
    }

    public void SelectAdventure()
    {
        selectButton.GetComponent<Button>().enabled = false;

        switch (currentLevel)
        {
            case 1:
                levelToLoad = "Exposition_Scene01";
                StartCoroutine(LoadEncounter(currentLevel, GameController.controller.levelsCompleted));
                break;
            case 2:
                levelToLoad = "Exposition_Scene09";
                StartCoroutine(LoadEncounter(currentLevel, GameController.controller.levelsCompleted));
                break;
            case 3:
                levelToLoad = "Exposition_Scene16";
                StartCoroutine(LoadEncounter(currentLevel, GameController.controller.levelsCompleted));
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
        selectButton.SetActive(false);
        //DO SOME CUTE ANIMATION WITH THE MANNEQUIN!
        Destroy(selectPrefab);
        Destroy(BackButton);
        yield return new WaitForSeconds(0.25f);
        playerMannequin.GetComponent<AnimationController>().PlayCheerAnim();
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
