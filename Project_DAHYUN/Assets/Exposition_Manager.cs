using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Exposition_Manager : MonoBehaviour
{
    public int sceneNumber;
    public GameObject playerMannequin;

    public GameObject canvas;
    public GameObject camera;
    public GameObject blackSq;
    public GameObject background;
    public GameObject dialoguePanel;
    public GameObject sfxManager;

    public GameObject speaker01;
    public GameObject speaker02;
    public GameObject speaker03;
    public GameObject speaker04;

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
        switch(sceneNumber)
        {
            case 1:
                StartCoroutine(Cutscene1());
                break;
            case 2:
                StartCoroutine(Cutscene2());
                break;
            case 3:
                StartCoroutine(Cutscene3());
                break;
            case 4:
                StartCoroutine(Cutscene4());
                break;
            case 5:
                StartCoroutine(Cutscene5());
                break;
            case 6:
                StartCoroutine(Cutscene6());
                break;
        }
    }

    public IEnumerator LoadCombatScene(int level, int encounterNum)
    {
        GameController.controller.currentEncounter = EncounterToolsScript.tools.SpecifyEncounter(level, encounterNum);
        print(GameController.controller.currentEncounter.backgroundName);
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
        if (nextLevel == "TurnCombat_Scene")
            return;

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

    IEnumerator NewDialogue(int cutscene, int instance)
    {
        int totalLines = 0;
        bool usesPlayer = false;
        string[] speaker = new string[20];
        bool[] leftspeaker = new bool[20];
        string[] script = new string[20];

        dialoguePanel.GetComponent<LerpScript>().LerpToPos(panelDownPos, panelUpPos, 2f);
        dialoguePanel.GetComponent<LerpScript>().LerpToColor(Color.clear, panelOrigColor, 2f);
        dialoguePanel.transform.GetChild(0).GetComponent<Text>().text = "";

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
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 2:
                switch (instance)
                {
                    case 1:
                        speaker[0] = "Jess";
                        leftspeaker[0] = false;
                        script[0] = "Signs of hypothermia, stab wounds, 2 broken ribs, and yet...";
                        speaker[1] = "Jess";
                        leftspeaker[1] = false;
                        script[1] = "Even the commander didn't want to waste-";

                        totalLines = 2;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                    case 2:
                        speaker[0] = "Jess";
                        leftspeaker[0] = false;
                        script[0] = "Hmm...";
                        speaker[1] = "Jess";
                        leftspeaker[1] = false;
                        script[1] = "Well isn't this peculiar...";

                        totalLines = 2;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 3:
                switch(instance)
                {
                    case 1:
                        speaker[0] = "Tesdin";
                        leftspeaker[0] = false;
                        script[0] = "Can you hear me?";
                        speaker[1] = "Tesdin";
                        leftspeaker[1] = false;
                        script[1] = "I am Commander Tesdin. I'm in charge here at Fort Marr.";
                        speaker[2] = "Tesdin";
                        leftspeaker[2] = false;
                        script[2] = "Jes here tells me that you've made a full recovery.";

                        totalLines = 3;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                    case 2:
                        speaker[0] = "Tesdin";
                        leftspeaker[0] = false;
                        script[0] = "Once you feel up to it, Jes will test your physical condition.";

                        speaker[1] = "Jess";
                        leftspeaker[1] = true;
                        script[1] = "Actually, Steve should be here any moment to assist in the evaluation.";

                        speaker[2] = "Tesdin";
                        leftspeaker[2] = false;
                        script[2] = "Grand! I'll want a report on your findings of what we discussed.";

                        speaker[3] = "Jess";
                        leftspeaker[3] = true;
                        script[3] = "Yes sir!";

                        totalLines = 4;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 4:
                switch (instance)
                {
                    case 1:
                        speaker[0] = "Steve";
                        leftspeaker[0] = false;
                        script[0] = "So you claim to remember very little of your past, but...";
                        speaker[1] = "Steve";
                        leftspeaker[1] = false;
                        script[1] = "Sorceress Jess beleives you to have been a practiced warrior.";
                        speaker[2] = "Steve";
                        leftspeaker[2] = false;
                        script[2] = "I shall remind you your skills with a friendly duel.";
                        speaker[3] = "Steve";
                        leftspeaker[3] = false;
                        script[3] = "Ready yourself... En garde!";

                        totalLines = 4;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 5:
                switch (instance)
                {
                    case 1:
                        speaker[0] = "Not Steve";
                        leftspeaker[0] = false;
                        script[0] = "Our forces are ready, my Lord.";
                        speaker[1] = "Not Steve";
                        leftspeaker[1] = false;
                        script[1] = "Why is our time wasted on this lackie?";
                        speaker[2] = "???";
                        leftspeaker[2] = false;
                        script[2] = "Do you question my judgement?";
                        speaker[3] = "???";
                        leftspeaker[3] = false;
                        script[3] = "All will be made clear to you soon enough... ";

                        totalLines = 4;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                    case 2:
                        speaker[0] = "Not Steve";
                        leftspeaker[0] = false;
                        script[0] = "As he says then!";
                        speaker[1] = "Not Steve";
                        leftspeaker[1] = false;
                        script[1] = "Let's see what you can do, Nova...";

                        totalLines = 2;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 6:
                switch (instance)
                {
                    case 1:
                        speaker[0] = "Not Steve";
                        leftspeaker[0] = false;
                        script[0] = "So there's a bit of fight left in you.";
                        speaker[1] = "Not Steve";
                        leftspeaker[1] = false;
                        script[1] = "No matter. Seize this vermin!";

                        totalLines = 2;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
        }
    }

    IEnumerator Cutscene6()
    {
        // Set next Level //
        nextLevel = "TurnCombat_Scene";

        //////////////////
        blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 0.5f);
        yield return new WaitForSeconds(1f);
        StartCoroutine(NewDialogue(6, 1));
        yield return new WaitForSeconds(10f);
        speaker02.GetComponent<LerpScript>().LerpToPos(speaker02.transform.position, speaker02.transform.position + new Vector3(50, 0, 0), 2);
        yield return new WaitForSeconds(0.5f);
        speaker02.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(LoadCombatScene(1,0));
    }

    IEnumerator Cutscene5()
    {
        // Set next Level //
        nextLevel = "Tutorial_Scene02";

        //////////////////
        blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 0.5f);
        yield return new WaitForSeconds(1f);
        StartCoroutine(NewDialogue(5, 1));
        yield return new WaitForSeconds(20f);
        speaker02.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
        speaker02.GetComponent<LerpScript>().LerpToPos(speaker02.transform.position, speaker02.transform.position + new Vector3(100,0,0));
        yield return new WaitForSeconds(3f);
        StartCoroutine(NewDialogue(5, 2));
        yield return new WaitForSeconds(4f);
        speaker03.GetComponent<LerpScript>().LerpToPos(speaker03.transform.position, speaker03.transform.position - new Vector3(300, 0, 0));
        yield return new WaitForSeconds(1f);
        sfxManager.GetComponent<SoundFXManager_C>().playSkitterScreech();
        yield return new WaitForSeconds(3f);
        LoadNextLv();
    }

    IEnumerator Cutscene4()
    {
        // Set next Level //
        nextLevel = "Tutorial_Scene01";
        
        //////////////////
        blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 0.5f);
        yield return new WaitForSeconds(1f);
        StartCoroutine(NewDialogue(4, 1));
        yield return new WaitForSeconds(25f);
        LoadNextLv();
    }

    IEnumerator Cutscene3()
    {
        // Set next Level //
        nextLevel = "Exposition_Scene04";
        //////////////////
        blackSq.GetComponent<FadeScript>().FadeColored(new Color(0,0,0,1), new Color(0,0,0, 0.8f));
        yield return new WaitForSeconds(1f);
        blackSq.GetComponent<FadeScript>().FadeColored(new Color(0,0,0, 0.8f), new Color(0,0,0, 1));
        yield return new WaitForSeconds(2f);
        blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0.8f));
        yield return new WaitForSeconds(1f);
        blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 0.8f), new Color(0, 0, 0, 1));
        yield return new WaitForSeconds(3f);
        blackSq.GetComponent<FadeScript>().FadeOut(0.15f);
        yield return new WaitForSeconds(1f);
        StartCoroutine(NewDialogue(3, 1));
        yield return new WaitForSeconds(20f);
        StartCoroutine(NewDialogue(3, 2));
        yield return new WaitForSeconds(30f);
        blackSq.GetComponent<FadeScript>().FadeIn();
        sfxManager.GetComponent<SoundFXManager_C>().FadeVolume(1, 0, 0.7f, true);
        yield return new WaitForSeconds(3.25f);
        LoadNextLv();
    }

    IEnumerator Cutscene2()
    {
        // Set next Level //
        nextLevel = "Exposition_Scene03";
        //////////////////
        yield return new WaitForSeconds(1f);
        blackSq.GetComponent<FadeScript>().FadeOut(0.25f);
        yield return new WaitForSeconds(2f);
        StartCoroutine(NewDialogue(2, 1));
        yield return new WaitForSeconds(18f);
        StartCoroutine(NewDialogue(2, 2));
        yield return new WaitForSeconds(10f);
        blackSq.GetComponent<FadeScript>().FadeIn();
        sfxManager.GetComponent<SoundFXManager_C>().FadeVolume(1, 0, 0.7f, true);
        yield return new WaitForSeconds(3.25f);
        LoadNextLv();
    }

    IEnumerator Cutscene1()
    {
        // Set next Level //
        nextLevel = "Exposition_Scene02";
        //////////////////
        blackSq.GetComponent<FadeScript>().FadeColored(Color.white, Color.clear, 0.15f);
        sfxManager.GetComponent<SoundFXManager_C>().playSnowSteps();
        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerInitPos, playerInitPos + new Vector3(400, 0, 0), 0.25f);
        playerMannequin.GetComponent<AnimationController>().PlayWalkAnim();
        playerMannequin.GetComponent<AnimationController>().SetPlaySpeed(0.5f);
        yield return new WaitForSeconds(3f);
        sfxManager.GetComponent<SoundFXManager_C>().stopAllMusic();
        playerMannequin.GetComponent<AnimationController>().PlayIdleAnim();
        yield return new WaitForSeconds(1f);
        //play death anim here?
        blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.white, 1f);
        yield return new WaitForSeconds(0.25f);
        sfxManager.GetComponent<SoundFXManager_C>().playSnowCollapse();
        yield return new WaitForSeconds(2f);
        blackSq.GetComponent<FadeScript>().FadeColored(Color.white, Color.clear, 1f);
        yield return new WaitForSeconds(2f);
        blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.white, 1f);
        yield return new WaitForSeconds(3f);
        blackSq.GetComponent<FadeScript>().FadeColored(Color.white, Color.clear, 0.25f);
        yield return new WaitForSeconds(0.5f);
        sfxManager.GetComponent<SoundFXManager_C>().playSnowSteps();
        yield return new WaitForSeconds(2f);
        speaker01.GetComponent<LerpScript>().LerpToPos(speaker01.transform.position, speaker01.transform.position - new Vector3(45, 0, 0), 1f);
        yield return new WaitForSeconds(1f);
        sfxManager.GetComponent<SoundFXManager_C>().stopAllMusic();
        StartCoroutine(NewDialogue(1, 1));
        yield return new WaitForSeconds(7f);
        blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.white, 0.5f);
        sfxManager.GetComponent<SoundFXManager_C>().FadeVolume(1, 0, 0.7f, true);
        yield return new WaitForSeconds(3.25f);
        LoadNextLv();
    }
}
