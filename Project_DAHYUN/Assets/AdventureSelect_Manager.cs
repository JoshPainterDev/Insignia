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

    public GameObject BackButton;

    public Vector3 adventureCameraPos;

    // Use this for initialization
    void Start ()
    {
        GameController.controller.levelsCompleted = 0;
        GameController.controller.stagesCompleted = 0;
        LoadCharacter();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SpecifyEncounter(0,0);
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
                StartCoroutine(LoadEncounter(1, GameController.controller.levelsCompleted));
                break;
            case 2:
                StartCoroutine(LoadEncounter(2, GameController.controller.levelsCompleted));
                break;
            case 3:
                StartCoroutine(LoadEncounter(3, GameController.controller.levelsCompleted));
                break;
            default:
                StartCoroutine(LoadCombatScreen());
                break;
        }
    }

    IEnumerator LoadEncounter(int stageToLoad, int levelsCompleted)
    {
        SpecifyEncounter(stageToLoad, levelsCompleted);

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
        SceneManager.LoadScene("Exposition_Scene");
    }

    void SpecifyEncounter(int stageToLoad, int levelsCompleted)
    {
        EnemyEncounter encounter = new EnemyEncounter();
        encounter.enemyNames = new string[10]; //max number of enemies
        encounter.bossFight = new bool[10]; //max number of enemies in one encounter

        switch (stageToLoad)
        {
            case 1: // 5 total stages, last stage is a small boss fight
                if (levelsCompleted == 0)
                {
                    encounter.encounterNumber = 1;
                    //load first stage
                    encounter.backgroundName = "\\Environments\\Forest02";
                    encounter.totalEnemies = 1;
                    encounter.enemyNames[0] = "Shadow Assassin";;
                    encounter.bossFight[0] = false;

                    Reward newReward = new Reward();
                    newReward.experience = 2500;
                    encounter.reward = newReward;
                }
                else if (levelsCompleted == 1)
                {
                    encounter.encounterNumber = 2;
                    encounter.backgroundName = "\\Environments\\Forest01";
                    encounter.totalEnemies = 1;
                    encounter.enemyNames[0] = "Shadow Assassin";
                    encounter.bossFight[0] = false;

                    Reward newReward = new Reward();
                    newReward.experience = 2500;
                    encounter.reward = newReward;
                }
                else if (levelsCompleted == 2)
                {
                    encounter.encounterNumber = 3;
                    encounter.totalEnemies = 1;
                    encounter.enemyNames[0] = "Shadow Assassin";
                    encounter.bossFight[0] = false;

                    Reward newReward = new Reward();
                    newReward.experience = 2500;
                    encounter.reward = newReward;
                }
                else if (levelsCompleted == 3)
                {
                    encounter.encounterNumber = 4;
                    encounter.totalEnemies = 1;
                    encounter.enemyNames[0] = "Shadow Assassin";
                    encounter.bossFight[0] = false;

                    Reward newReward = new Reward();
                    newReward.experience = 2500;
                    encounter.reward = newReward;
                }
                else if (levelsCompleted == 4)
                {
                    encounter.encounterNumber = 5;
                    // boss fight
                    encounter.totalEnemies = 1;
                    encounter.enemyNames[0] = "Shadow Assassin";
                    encounter.bossFight[0] = true;

                    Reward newReward = new Reward();
                    newReward.experience = 2500;
                    encounter.reward = newReward;
                }
                break;
            default:
                break;
        }

        GameController.controller.currentEncounter = encounter;
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

    public void LoadCharacter()
    {
        //head
        EquipmentInfo info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[0], GameController.controller.playerEquippedIDs[1]);
        playerMannequin.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        if (!info.hideUnderLayer)
            playerMannequin.transform.GetChild(8).GetComponent<SpriteRenderer>().enabled = true;

        //torso
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[2], GameController.controller.playerEquippedIDs[3]);
        playerMannequin.transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        if (!info.hideUnderLayer)
        {
            playerMannequin.transform.GetChild(9).GetComponent<SpriteRenderer>().enabled = true;
            playerMannequin.transform.GetChild(11).GetComponent<SpriteRenderer>().enabled = true;
        }

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
        if (!info.hideUnderLayer)
            playerMannequin.transform.GetChild(10).GetComponent<SpriteRenderer>().enabled = true;
        //shoes
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[10], GameController.controller.playerEquippedIDs[11]);
        playerMannequin.transform.GetChild(5).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        //weapon
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[12], GameController.controller.playerEquippedIDs[13]);
        playerMannequin.transform.GetChild(6).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        //Aura
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[14], GameController.controller.playerEquippedIDs[15]);
        playerMannequin.transform.GetChild(12).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
    }
}
