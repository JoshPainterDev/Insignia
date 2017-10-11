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
    private string nextLevel;

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
        playerName = GameController.controller.playerName;
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
        SceneManager.LoadScene(nextLevel);
    }

    IEnumerator Cutscene1()
    {
        // Set next Level //
        nextLevel = "TurnCombat_Scene";
        //////////////////
        blackSq.GetComponent<FadeScript>().FadeColored(Color.white, Color.clear, 0.15f);
        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerInitPos, playerInitPos + new Vector3(500, 0, 0), 0.25f);
        playerMannequin.GetComponent<AnimationController>().PlayWalkAnim();
        playerMannequin.GetComponent<AnimationController>().SetPlaySpeed(0.5f);
        yield return new WaitForSeconds(3f);
        playerMannequin.GetComponent<AnimationController>().PlayIdleAnim();
        yield return new WaitForSeconds(1f);
        //play death anim here?
        blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.white, 1f);
        yield return new WaitForSeconds(1f);
        blackSq.GetComponent<FadeScript>().FadeColored(Color.white, Color.clear, 1f);
        yield return new WaitForSeconds(1f);
        blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.white, 1f);
        yield return new WaitForSeconds(3f);
        blackSq.GetComponent<FadeScript>().FadeColored(Color.white, Color.clear, 0.25f);
        yield return new WaitForSeconds(3f);
        StartCoroutine(NewDialogue(1, 1));
        yield return new WaitForSeconds(7f);
        blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.white, 0.5f);
        LoadNextLv();
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
                switch (instance)
                {
                    case 1:
                        usesPlayer = false;

                        speaker[0] = "???";
                        leftspeaker[0] = false;
                        script[0] = "...";
                        speaker[1] = "???";
                        leftspeaker[1] = false;
                        script[1] = "Over here...";

                        totalLines = 2;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, script, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
        }
    }
}
