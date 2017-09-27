using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Exposition_Manager : MonoBehaviour
{
    public GameObject playerMannequin;

    public GameObject canvas;
    public GameObject camera;
    public GameObject blackSq;
    public GameObject background;
    public GameObject dialoguePanel;

    private Vector3 panelUpPos;
    private Vector3 panelDownPos;
    private Color panelOrigColor;

    string playerName;
    EnemyEncounter encounter;
    Vector3 playerInitPos;
    Vector3 origCameraPos;

    // Use this for initialization
    void Start ()
    {
        encounter = GameController.controller.currentEncounter;
        origCameraPos = camera.transform.position;
        playerInitPos = playerMannequin.transform.position;
        playerName = GameController.controller.characterName;
        panelDownPos = dialoguePanel.transform.position;
        panelUpPos = panelDownPos + new Vector3(0, 100, 0);
        panelOrigColor = dialoguePanel.GetComponent<Image>().color;
        dialoguePanel.GetComponent<Image>().color = Color.clear;

        if (encounter == null)
        {
            encounter = new EnemyEncounter();
            encounter.encounterNumber = 1;  
        }

        BeginCutscene(encounter.encounterNumber);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SceneManager.LoadScene("TurnCombat_Scene");
        }
    }

    public void BeginCutscene(int encounterNum)
    {
        switch(encounterNum)
        {
            case 1:
                StartCoroutine(Cutscene1());
                break;
        }
    }

    IEnumerator Cutscene1()
    {
        blackSq.GetComponent<FadeScript>().FadeOut();
        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerInitPos, playerInitPos + new Vector3(300,0,0), 1);
        playerMannequin.GetComponent<AnimationController>().PlayWalkAnim();
        yield return new WaitForSeconds(1.25f);
        playerMannequin.GetComponent<AnimationController>().PlayIdleAnim();
        yield return new WaitForSeconds(1f);
        playerMannequin.GetComponent<AnimationController>().FlipFlop();
        yield return new WaitForSeconds(1f);
        playerMannequin.GetComponent<AnimationController>().FlipFlop();
        yield return new WaitForSeconds(0.75f);
        StartCoroutine(NewDialogue(1,1));
        yield return new WaitForSeconds(5f);
        StartCoroutine(LoadCombatScene());
    }

    public IEnumerator LoadCombatScene()
    {
        yield return new WaitForSeconds(1.15f);
        blackSq.GetComponent<FadeScript>().FadeIn(10f);
        yield return new WaitForSeconds(0.15f);
        blackSq.GetComponent<FadeScript>().FadeOut(10f);
        yield return new WaitForSeconds(0.15f);
        blackSq.GetComponent<FadeScript>().FadeIn(10f);
        yield return new WaitForSeconds(0.15f);
        blackSq.GetComponent<FadeScript>().FadeOut(10f);
        yield return new WaitForSeconds(0.15f);
        blackSq.GetComponent<FadeScript>().FadeIn(10f);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("TurnCombat_Scene");
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

    IEnumerator NewDialogue(int cutscene, int instance)
    {
        int totalLines = 0;
        bool usesPlayer = false;
        string[] speaker = new string[20];
        bool[] leftspeaker = new bool[20];
        string[] script = new string[20];

        dialoguePanel.GetComponent<LerpScript>().LerpToPos(panelDownPos, panelUpPos, 2f);
        dialoguePanel.GetComponent<LerpScript>().LerpToColor(Color.clear, panelOrigColor, 2f);

        yield return new WaitForSeconds(0.5f);

        switch (cutscene)
        {
            case 1:
                switch(instance)
                {
                    case 1:
                        usesPlayer = true;

                        speaker[0] = playerName;
                        leftspeaker[0] = true;
                        script[0] = "(Huh?)";

                        totalLines = 1;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, script, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
        }
    }

    public void EndDialogue()
    {
        dialoguePanel.GetComponent<LerpScript>().LerpToPos(panelUpPos, panelDownPos, 2f);
        dialoguePanel.GetComponent<LerpScript>().LerpToColor(panelOrigColor, Color.clear, 2f);
    }

    public void SkipCutscene()
    {
        GameController.controller.GetComponent<TimeController>().LerpTimeScale(1, 0.1f, 3);
        dialoguePanel.GetComponent<Image>().enabled = false;
        dialoguePanel.GetComponentInChildren<Text>().enabled = false;
        blackSq.GetComponent<FadeScript>().FadeIn(10f);
        Invoke("LoadNextLv", 0.25f);
    }

    public void LoadNextLv()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("TurnCombat_Scene");
    }
}
