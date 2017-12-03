using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Exposition_Manager : MonoBehaviour
{
    public int sceneNumber;
    public GameObject playerMannequin;
    [HideInInspector]
    public bool ready4Input = false;

    public GameObject canvas;
    public GameObject camera;
    public GameObject blackSq;
    public GameObject background;
    public GameObject dialoguePanel;
    public GameObject sfxManager;
    public GameObject MusicManager;
    public GameObject touch2Continue;

    public AudioClip CombatStartup;

    public GameObject speaker01;
    public GameObject speaker02;
    public GameObject speaker03;
    public GameObject speaker04;
    public GameObject speaker05;

    private Vector3 panelUpPos;
    private Vector3 panelDownPos;
    private Color panelOrigColor;
    private string nextLevel;
    private Dialogue_Manager_C dialogueManager;

    private bool actionsCompleted = false;
    private int actionCounter = 0;

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
        dialogueManager = this.GetComponent<Dialogue_Manager_C>();
        ready4Input = false;

        if (encounter == null)
        {
            encounter = new EnemyEncounter();
            encounter.encounterNumber = 1;  
        }

        BeginCutscene(encounter.encounterNumber);
    }

    //public void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha9))
    //    {
    //        SceneManager.LoadScene("TurnCombat_Scene");
    //    }
    //}

    public void InputDetected()
    {
        if(ready4Input)
        {
            ready4Input = false;
            StartCoroutine(handleInput());
        }
    }

    IEnumerator handleInput()
    {
        if (dialogueManager.dDialogueCompleted)
            dialogueManager.StopDialogue();
        else
        {
            dialogueManager.DialogueHandler();

            yield return new WaitForSeconds(1.15f);
            ready4Input = true;
        }
    }

    public void NextAction()
    {
        ++actionCounter;
        BeginCutscene(encounter.encounterNumber);
    }

    public void BeginCutscene(int encounterNum)
    {
        switch(sceneNumber)
        {
            case 1:
                StartCoroutine(Cutscene1(actionCounter));
                break;
            case 2:
                StartCoroutine(Cutscene2(actionCounter));
                break;
            case 3:
                StartCoroutine(Cutscene3(actionCounter));
                break;
            case 4:
                StartCoroutine(Cutscene4(actionCounter));
                break;
            case 5:
                StartCoroutine(Cutscene5(actionCounter));
                break;
            case 6:
                StartCoroutine(Cutscene6(actionCounter));
                break;
            case 7:
                StartCoroutine(Cutscene7(actionCounter));
                break;
            case 8:
                StartCoroutine(Cutscene8(actionCounter));
                break;
            case 9:
                StartCoroutine(Cutscene9(actionCounter));
                break;
            case 10:
                StartCoroutine(Cutscene10(actionCounter));
                break;
        }
    }

    public IEnumerator LoadCombatScene(int level, int encounterNum, string returnScene)
    {
        GameController.controller.currentEncounter = EncounterToolsScript.tools.SpecifyEncounter(level, encounterNum);
        GameController.controller.currentEncounter.returnOnSuccessScene = returnScene;
        
        yield return new WaitForSeconds(1f);
        GameController.controller.GetComponent<MenuUIAudio>().playSoundClip(CombatStartup);
        yield return new WaitForSeconds(0.25f);
        blackSq.GetComponent<FadeScript>().FadeIn(10f);
        yield return new WaitForSeconds(0.15f);
        blackSq.GetComponent<FadeScript>().FadeOut(10f);
        yield return new WaitForSeconds(0.15f);
        blackSq.GetComponent<FadeScript>().FadeIn(10f);
        yield return new WaitForSeconds(0.15f);
        blackSq.GetComponent<FadeScript>().FadeOut(10f);
        yield return new WaitForSeconds(0.15f);
        blackSq.GetComponent<FadeScript>().FadeIn(10f);
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene("TurnCombat_Scene");
    }

    public void EndDialogue()
    {
        ready4Input = false;
        touch2Continue.SetActive(false);
        dialoguePanel.GetComponent<LerpScript>().LerpToPos(panelUpPos, panelDownPos, 2f);
        dialoguePanel.GetComponent<LerpScript>().LerpToColor(panelOrigColor, Color.clear, 2f);
    }

    IEnumerator LoadNextLv()
    {
        Time.timeScale = 1.0f;
        MusicManager.GetComponent<Music_Controller>().FadeVolume(MusicManager.GetComponent<AudioSource>().volume, 0);

        yield return new WaitForSeconds(1.75f);
        SceneManager.LoadScene(nextLevel);
    }

    IEnumerator NewDialogue(int cutscene, int instance)
    {
        int totalLines = 0;
        bool usesPlayer = false;
        string[] speaker = new string[20];
        bool[] leftspeaker = new bool[20];
        string[] script = new string[20];

        actionCounter = 0;

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

                        speaker[4] = "Not Steve";
                        leftspeaker[4] = false;
                        script[4] = "As he says then!";
                        speaker[5] = "Not Steve";
                        leftspeaker[5] = false;
                        script[5] = "Let's see what you can do, Nova...";

                        totalLines = 6;
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
            case 7:
                switch (instance)
                {
                    case 1:
                        speaker[0] = "Not Steve";
                        leftspeaker[0] = false;
                        script[0] = "Listen you brat! I've had enough of th-";

                        speaker[1] = "Seamstress";
                        leftspeaker[1] = true;
                        script[1] = "Watch it Slade!";

                        speaker[2] = "Seamstress";
                        leftspeaker[2] = true;
                        script[2] = " Clearly the Nova can handle a blade.";

                        speaker[3] = "Slade";
                        leftspeaker[3] = false;
                        script[3] = "I guess it's your lucky day then!";

                        speaker[4] = "Slade";
                        leftspeaker[4] = false;
                        script[4] = "I look forward to seeing you again, Nova...";

                        totalLines = 5;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 8:
                switch (instance)
                {
                    case 1:
                        speaker[0] = "Seamstress";
                        leftspeaker[0] = false;
                        script[0] = "Sorry about that. Slade can be a bit rough with newcomers.";

                        speaker[1] = playerName;
                        leftspeaker[1] = true;
                        script[1] = "Rough?! He was trying to kill me!";

                        speaker[2] = "Seamstress";
                        leftspeaker[2] = false;
                        script[2] = "I'm sure he'll warm up to you.";

                        speaker[3] = "Seamstress";
                        leftspeaker[3] = false;
                        script[3] = "Anyway, I'm The Seamstress, but you can call me Hyun. What should I call you?";

                        speaker[4] = playerName;
                        leftspeaker[4] = true;
                        script[4] = "I'm " + playerName + ".";

                        speaker[5] = "Seamstress";
                        leftspeaker[5] = false;
                        script[5] = "Nice to meet you, " + playerName + ".";

                        speaker[6] = playerName;
                        leftspeaker[6] = true;
                        script[6] = "Why have I been brought here? And who is the spooky-cloak-guy?";

                        speaker[7] = "Seamstress";
                        leftspeaker[7] = false;
                        script[7] = "Let me show you around. There's a lot to explain.";

                        totalLines = 8;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 9:
                switch (instance)
                {
                    case 1:
                        speaker[0] = "Seamstress";
                        leftspeaker[0] = false;
                        script[0] = "And here is the combat yard -";

                        speaker[1] = "Seamstress";
                        leftspeaker[1] = true;
                        script[1] = "You've shown knowledge of your weapon, but...";

                        speaker[2] = "Seamstress";
                        leftspeaker[2] = true;
                        script[2] = "Something tells me you could use a lesson or two in the Arcane arts!";

                        speaker[3] = playerName;
                        leftspeaker[3] = true;
                        script[3] = "*Gulp*";

                        totalLines = 4;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 10:
                switch (instance)
                {
                    case 1:
                        speaker[0] = "Seamstress";
                        leftspeaker[0] = false;
                        script[0] = "You're not too bad, " + playerName;

                        speaker[1] = "Seamstress";
                        leftspeaker[1] = false;
                        script[1] =  "With some proper training, you could really benefit our cause.";

                        speaker[2] = playerName;
                        leftspeaker[2] = true;
                        script[2] = "What cause? I'm still not sure what you want with me.";

                        speaker[3] = "Seamstress";
                        leftspeaker[3] = false;
                        script[3] = "Our primary goal is to take control of the Kingdom of Light...";

                        speaker[4] = playerName;
                        leftspeaker[4] = true;
                        script[4] = "Wait what? Why?!";

                        speaker[5] = "Seamstress";
                        leftspeaker[5] = false;
                        script[5] = "Our reasons are our own right now. I've probably said too much already.";

                        speaker[6] = "Seamstress";
                        leftspeaker[6] = false;
                        script[6] = "But since you are Nova, I do trust you enough to get to know you better.";

                        speaker[7] = playerName;
                        leftspeaker[7] = true;
                        script[7] = "Nova? Why is everone calling me that?! What does this even have to do with me?!";

                        speaker[8] = "???";
                        leftspeaker[8] = false;
                        script[8] = "She is referring to your true name, " + playerName + ". The title that was taken from you...";

                        speaker[9] = "???";
                        leftspeaker[9] = false;
                        script[9] = "Nova, if you do not join us, I fear the worst for you and the people of Light.";

                        speaker[10] = playerName;
                        leftspeaker[10] = true;
                        script[10] = "I don't even know who you are! You want me to drop everything and join your shady cult?!";

                        speaker[11] = "???";
                        leftspeaker[11] = false;
                        script[11] = "Who I am is not important. And there isn't much for you to 'drop' now, is there?";

                        speaker[12] = "???";
                        leftspeaker[12] = false;
                        script[12] = "You've forgotten everything that you were. And now the fog of war is upon us...";

                        speaker[13] = "???";
                        leftspeaker[13] = false;
                        script[13] = "Those corrupted walls will crumble, with or without you. Even I cannot stop it.";

                        speaker[14] = "???";
                        leftspeaker[14] = false;
                        script[14] = "But you, Nova, have the power to rebuild something even greater from the ashes.";

                        speaker[15] = "???";
                        leftspeaker[15] = false;
                        script[15] = "So I must ask you," + playerName + " will you join us? Or be burried in the sands of time?";

                        totalLines = 16;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
        }
    }

    IEnumerator Cutscene10(int action)
    {
        switch (action)
        {
            case 0:
                nextLevel = "MainMenu_Tutorial_Scene";
                // Set next Level //
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 0.5f);
                speaker01.transform.GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 6);
                yield return new WaitForSeconds(0.75f);
                speaker01.transform.GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 1);
                StartCoroutine(NewDialogue(10, 1));
                yield return new WaitForSeconds(1f);
                break;
            case 9:
                yield return new WaitForSeconds(1);
                //actionsCompleted = true; //actions are completed
                speaker02.GetComponent<LerpScript>().LerpToPos(speaker02.transform.position, speaker02.transform.position + new Vector3(70, 0, 0), 1f);
                yield return new WaitForSeconds(0.5f);
                playerMannequin.GetComponent<AnimationController>().FlipFlop();
                yield return new WaitForSeconds(0.5f);
                playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, playerMannequin.transform.position + new Vector3(70, 0, 0), 2f);
                break;
            case 1:
                yield return new WaitForSeconds(0.75f);
                blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, new Color(0.75f, 0.75f, 0.75f, 0.75f), 0.85f);
                yield return new WaitForSeconds(1.5f);

                speaker04.SetActive(true);
                speaker05.SetActive(true);

                Color temp = speaker04.GetComponent<Image>().color;
                speaker04.GetComponent<LerpScript>().LerpToColor(temp, temp + new Color(0, 0, 0, 1), 0.75f);
                temp = speaker04.transform.GetChild(0).GetComponent<Text>().color;
                speaker04.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(temp, temp + new Color(0, 0, 0, 1), 0.75f);
                temp = speaker04.transform.GetChild(1).GetComponent<Text>().color;
                speaker04.transform.GetChild(1).GetComponent<LerpScript>().LerpToColor(temp, temp + new Color(0, 0, 0, 1), 0.75f);

                yield return new WaitForSeconds(1.5f);

                temp = speaker05.GetComponent<Image>().color;
                speaker05.GetComponent<LerpScript>().LerpToColor(temp, temp + new Color(0, 0, 0, 1), 0.75f);
                temp = speaker05.transform.GetChild(0).GetComponent<Text>().color;
                speaker05.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(temp, temp + new Color(0, 0, 0, 1), 0.75f);
                temp = speaker05.transform.GetChild(1).GetComponent<Text>().color;
                speaker05.transform.GetChild(1).GetComponent<LerpScript>().LerpToColor(temp, temp + new Color(0, 0, 0, 1), 0.75f);

                yield return new WaitForSeconds(2.5f);

                speaker04.GetComponent<Button>().enabled = true;
                speaker05.GetComponent<Button>().enabled = true;
                break;
        }
        //////////////////
    }

    IEnumerator Cutscene9(int action)
    {
        switch (action)
        {
            case 0:
                nextLevel = "TurnCombat_Scene";
                // Set next Level //
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 0.5f);
                yield return new WaitForSeconds(1f);
                speaker01.GetComponent<LerpScript>().LerpToPos(speaker01.transform.position, speaker01.transform.position + new Vector3(250, 0, 0), 1f);
                yield return new WaitForSeconds(1f);
                speaker01.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
                yield return new WaitForSeconds(0.65f);
                StartCoroutine(NewDialogue(9, 1));
                break;
            case 4:
                yield return new WaitForSeconds(2);
                actionsCompleted = true; //actions are completed
                StartCoroutine(LoadCombatScene(1, 1, "Exposition_Scene10"));
                break;
        }
        //////////////////
    }

    IEnumerator Cutscene8(int action)
    {
        switch (action)
        {
            case 0:
                // Set next Level //
                nextLevel = "Exposition_Scene09";
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 0.5f);
                yield return new WaitForSeconds(1f);
                StartCoroutine(NewDialogue(8, 1));
                break;
            case 9:
                yield return new WaitForSeconds(2f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), 0.75f);
                actionsCompleted = true; //actions are completed
                StartCoroutine(LoadNextLv());
                break;
        }
        //////////////////
    }

    IEnumerator Cutscene7(int action)
    {
        switch(action)
        {
            case 0:
                // Set next Level //
                nextLevel = "Exposition_Scene08";
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 0.5f);
                yield return new WaitForSeconds(1f);
                StartCoroutine(NewDialogue(7, 1));
                break;
            case 1:
                yield return new WaitForSeconds(1f);
                speaker02.SetActive(true);
                speaker03.SetActive(true);
                speaker02.GetComponent<LerpScript>().LerpToPos(speaker02.transform.position, speaker02.transform.position + new Vector3(90, 0, 0), 2.5f);
                speaker03.GetComponent<LerpScript>().LerpToPos(speaker03.transform.position, speaker03.transform.position - new Vector3(90, 0, 0), 2.5f);
                break;
            case 2:
                yield return new WaitForSeconds(1f);
                speaker05.GetComponent<LerpScript>().LerpToPos(speaker05.transform.position, speaker05.transform.position + new Vector3(90, 0, 0), 1.25f);
                break;
            case 5:
                yield return new WaitForSeconds(1.75f);
                speaker01.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.white, Color.grey, 2.5f);
                yield return new WaitForSeconds(0.75f);
                speaker01.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.grey, Color.clear, 2.5f);
                speaker02.SetActive(false);
                speaker03.SetActive(false);
                speaker04.GetComponent<ParticleSystem>().Play();
                break;
            case 6:
                yield return new WaitForSeconds(1f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), 0.75f);
                actionsCompleted = true; //actions are completed
                StartCoroutine(LoadNextLv());
                break;
        }
        //////////////////
    }

    IEnumerator Cutscene6(int action)
    {
        switch(action)
        {
            case 0:
                // Set next Level //
                nextLevel = "TurnCombat_Scene";
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 0.5f);
                yield return new WaitForSeconds(1f);
                StartCoroutine(NewDialogue(6, 1));
                break;
            case 2:
                speaker02.GetComponent<LerpScript>().LerpToPos(speaker02.transform.position, speaker02.transform.position + new Vector3(50, 0, 0), 2);
                yield return new WaitForSeconds(0.5f);
                speaker02.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                StartCoroutine(LoadCombatScene(1, 0, "Exposition_Scene07"));
                actionsCompleted = true; //actions are completed
                break;
        }
    }

    IEnumerator Cutscene5(int action)
    {
        switch (action)
        {
            case 0:
                // Set next Level //
                nextLevel = "Tutorial_Scene02";
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0.75f), 0.25f);
                yield return new WaitForSeconds(3f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 0.75f), new Color(0, 0, 0, 1), 0.5f);
                yield return new WaitForSeconds(3.5f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 0.25f);   
                yield return new WaitForSeconds(5f);
                StartCoroutine(NewDialogue(5, 1));
                break;
            case 4:
                yield return new WaitForSeconds(1f);
                speaker02.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                yield return new WaitForSeconds(0.5f);
                speaker02.GetComponent<LerpScript>().LerpToPos(speaker02.transform.position, speaker02.transform.position + new Vector3(100, 0, 0));
                break;
            case 5:
                yield return new WaitForSeconds(2f);
                speaker03.GetComponent<LerpScript>().LerpToPos(speaker03.transform.position, speaker03.transform.position - new Vector3(300, 0, 0));
                yield return new WaitForSeconds(1f);
                sfxManager.GetComponent<SoundFXManager_C>().playSkitterScreech();
                break;
            case 6:
                yield return new WaitForSeconds(1f);
                MusicManager.GetComponent<Music_Controller>().stopAllMusic();
                GameController.controller.GetComponent<MenuUIAudio>().playSoundClip(CombatStartup, 0.1f);
                yield return new WaitForSeconds(0.25f);
                blackSq.GetComponent<FadeScript>().FadeIn(10f);
                yield return new WaitForSeconds(0.15f);
                blackSq.GetComponent<FadeScript>().FadeOut(10f);
                yield return new WaitForSeconds(0.15f);
                blackSq.GetComponent<FadeScript>().FadeIn(10f);
                yield return new WaitForSeconds(0.15f);
                blackSq.GetComponent<FadeScript>().FadeOut(10f);
                yield return new WaitForSeconds(0.15f);
                blackSq.GetComponent<FadeScript>().FadeIn(10f);
                actionsCompleted = true; //actions are completed
                StartCoroutine(LoadNextLv());
                break;
        }
    }

    IEnumerator Cutscene4(int action)
    {
        // Set next Level //
        nextLevel = "Tutorial_Scene01";
        
        //////////////////
        switch(action)
        {
            case 0:
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 0.5f);
                yield return new WaitForSeconds(1f);
                StartCoroutine(NewDialogue(4, 1));
                break;
            case 5:
                yield return new WaitForSeconds(1f);
                MusicManager.GetComponent<Music_Controller>().stopAllMusic();
                GameController.controller.GetComponent<MenuUIAudio>().playSoundClip(CombatStartup, 0.1f);
                yield return new WaitForSeconds(0.25f);
                blackSq.GetComponent<FadeScript>().FadeIn(10f);
                yield return new WaitForSeconds(0.15f);
                blackSq.GetComponent<FadeScript>().FadeOut(10f);
                yield return new WaitForSeconds(0.15f);
                blackSq.GetComponent<FadeScript>().FadeIn(10f);
                yield return new WaitForSeconds(0.15f);
                blackSq.GetComponent<FadeScript>().FadeOut(10f);
                yield return new WaitForSeconds(0.15f);
                blackSq.GetComponent<FadeScript>().FadeIn(10f);
                actionsCompleted = true; //actions are completed
                StartCoroutine(LoadNextLv());
                break;
        }
    }

    IEnumerator Cutscene3(int action)
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
        StartCoroutine(LoadNextLv());
    }

    IEnumerator Cutscene2(int action)
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
        StartCoroutine(LoadNextLv());
    }

    IEnumerator Cutscene1(int action)
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
        StartCoroutine(LoadNextLv());
    }
}
