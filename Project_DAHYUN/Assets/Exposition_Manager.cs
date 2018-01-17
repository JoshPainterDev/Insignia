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
    public GameObject cameraObj;
    public GameObject blackSq;
    public GameObject background;
    public GameObject dialoguePanel;
    public GameObject sfxManager;
    public GameObject MusicManager;

    public GameObject ExclamationPoint;

    public AudioClip CombatStartup;

    public GameObject speaker01;
    public GameObject speaker02;
    public GameObject speaker03;
    public GameObject speaker04;
    public GameObject speaker05;
    public GameObject speaker06;

    private Vector3 panelUpPos;
    private Vector3 panelDownPos;
    private Color panelOrigColor;
    private string nextLevel;
    private Dialogue_Manager_C dialogueManager;
    private int eCurrentCutscene = 0;
    private int eCurrentInstance = 1;
    private int eMaxInstances = 1;
    private float eInstanceDelay = 2.0f;

    [HideInInspector]
    public bool actionsCompleted = false;
    [HideInInspector]
    public int actionCounter = 0;

    string playerName;
    EnemyEncounter encounter;
    Vector3 playerInitPos;
    Vector3 origCameraPos;

    // Use this for initialization
    void Start ()
    {
        encounter = GameController.controller.currentEncounter;
        origCameraPos = cameraObj.transform.position;
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
        BeginCutscene(eCurrentCutscene, eCurrentInstance);
    }

    public void BeginCutscene(int encounterNum, int instance = 1)
    {
        eMaxInstances = 0;
        eCurrentCutscene = sceneNumber;
        eCurrentInstance = instance;

        switch (sceneNumber)
        {
            case 1:
                StartCoroutine(Cutscene1(actionCounter, instance));
                break;
            case 2:
                StartCoroutine(Cutscene2(actionCounter, instance));
                break;
            case 3:
                eMaxInstances = 2;
                StartCoroutine(Cutscene3(actionCounter, instance));
                break;
            case 4:
                StartCoroutine(Cutscene4(actionCounter, instance));
                break;
            case 5:
                StartCoroutine(Cutscene5(actionCounter, instance));
                break;
            case 6:
                StartCoroutine(Cutscene6(actionCounter, instance));
                break;
            case 7:
                eMaxInstances = 2;
                StartCoroutine(Cutscene7(actionCounter, instance));
                break;
            case 8:
                StartCoroutine(Cutscene8(actionCounter, instance));
                break;
            case 9:
                StartCoroutine(Cutscene9(actionCounter, instance));
                break;
            case 10:
                StartCoroutine(Cutscene10(actionCounter, instance));
                break;
        }
    }

    public IEnumerator LoadCombatScene(int level, int encounterNum)
    {
        GameController.controller.currentEncounter = new EnemyEncounter();
        GameController.controller.currentEncounter = EncounterToolsScript.tools.SpecifyEncounter(level, encounterNum);
        
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
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene("TurnCombat_Scene");
    }

    public void EndDialogue()
    {
        ready4Input = false;
        actionsCompleted = false;
        actionCounter = 0;
        eInstanceDelay = 2.0f;
        dialoguePanel.GetComponent<LerpScript>().LerpToPos(panelUpPos, panelDownPos, 2f);
        dialoguePanel.GetComponent<LerpScript>().LerpToColor(panelOrigColor, Color.clear, 2f);

        ++eCurrentInstance;

        if (eCurrentInstance <= eMaxInstances)
        {
            StartCoroutine(ContinueToInstance(eInstanceDelay));
        }
    }

    IEnumerator ContinueToInstance(float delay)
    {
        yield return new WaitForSeconds(delay);
        BeginCutscene(eCurrentCutscene, eCurrentInstance);
    }

    IEnumerator LoadNextLv()
    {
        Time.timeScale = 1.0f;
        MusicManager.GetComponent<Music_Controller>().FadeVolume(MusicManager.GetComponent<AudioSource>().volume, 0);

        yield return new WaitForSeconds(1.75f);
        SceneManager.LoadScene(nextLevel);
    }

    IEnumerator ExitScene(GameObject character)
    {
        character.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        sfxManager.GetComponent<SoundFXManager_C>().playExitScene();
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

                        speaker[0] = "General Vixon";
                        leftspeaker[0] = true;
                        script[0] = "Welcome back soldier, Any word from Solaris?";
                        speaker[1] = "Officer";
                        leftspeaker[1] = false;
                        script[1] = "No sir! Still no sign of Tesdin.";
                        speaker[2] = "General Vixon";
                        leftspeaker[2] = true;
                        script[2] = "It's only been a week. I'm sure he'll turn up!";
                        speaker[3] = "Officer";
                        leftspeaker[3] = false;
                        script[3] = "Sir, he only packed enough food for three days in the mountains...";
                        speaker[4] = "General Vixon";
                        leftspeaker[4] = true;
                        script[4] = "Excellent point, Officer! He's probably frozen derph-fodder by now.";
                        speaker[5] = "General Vixon";
                        leftspeaker[5] = true;
                        script[5] = "Rally the others! We have to be ready for anything!";
                        speaker[6] = "Officer";
                        leftspeaker[6] = false;
                        script[6] = "Right away sir!";

                        totalLines = 7;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 2:
                switch (instance)
                {
                    case 1:
                        speaker[0] = "General Vixon";
                        leftspeaker[0] = true;
                        script[0] = "Weapons, Ready!";

                        speaker[1] = "General Vixon";
                        leftspeaker[1] = true;
                        script[1] = "Steady now! On my mark!";

                        speaker[2] = "PVT Mark";
                        leftspeaker[2] = false;
                        script[2] = "Ready and willing, sir!";

                        speaker[3] = "General Vixon";
                        leftspeaker[3] = true;
                        script[3] = "Shut the hell up, Mark!";

                        speaker[4] = "General Vixon";
                        leftspeaker[4] = true;
                        script[4] = "...";

                        speaker[5] = "General Vixon";
                        leftspeaker[5] = true;
                        script[5] = "Alright!!";

                        speaker[6] = "General Vixon";
                        leftspeaker[6] = true;
                        script[6] = "Give 'em hell boys!";

                        totalLines = 7;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 3:
                switch (instance)
                {
                    case 1:
                        speaker[0] = "General Vixon";
                        leftspeaker[0] = true;
                        script[0] = "Soldier!";

                        speaker[1] = "General Vixon";
                        leftspeaker[1] = true;
                        script[1] = "Listen, I know you're new to the Bulwark,";

                        speaker[2] = "General Vixon";
                        leftspeaker[2] = true;
                        script[2] = "but we've got an apocalypse on our hands!";

                        speaker[3] = "General Vixon";
                        leftspeaker[3] = true;
                        script[3] = "I'm counting on you-";

                        speaker[4] = playerName;
                        leftspeaker[4] = false;
                        script[4] = "Me?!";

                        speaker[5] = "General Vixon";
                        leftspeaker[5] = true;
                        script[5] = "to hold this choke with your life!";

                        speaker[6] = "General Vixon";
                        leftspeaker[6] = true;
                        script[6] = "Nothing gets past that bridge! Got it?!";

                        speaker[7] = playerName;
                        leftspeaker[7] = false;
                        script[7] = "Sir, yes, sir!";

                        speaker[8] = "General Vixon";
                        leftspeaker[8] = true;
                        script[8] = "I knew I could count on you-";

                        speaker[9] = "General Vixon";
                        leftspeaker[9] = true;
                        script[9] = "Uhh-";

                        speaker[10] = playerName;
                        leftspeaker[10] = false;
                        script[10] = "Private " + playerName + ", sir...";

                        speaker[11] = "General Vixon";
                        leftspeaker[11] = true;
                        script[11] = "Right! Ummm...";

                        speaker[12] = "General Vixon";
                        leftspeaker[12] = true;
                        script[12] = "I'm sure you'll do fine out there, kid!";

                        speaker[13] = "General Vixon";
                        leftspeaker[13] = true;
                        script[13] = "Now get your dainty look'n underside out of my sight!";

                        speaker[14] = playerName;
                        leftspeaker[14] = false;
                        script[14] = "Sir, yes, sir!";

                        eInstanceDelay = 3.0f;
                        totalLines = 15;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                    case 2:
                        speaker[0] = "Officer";
                        leftspeaker[0] = true;
                        script[0] = "General!!";

                        speaker[1] = "Officer";
                        leftspeaker[1] = true;
                        script[1] = "We're losing bodies too qui-";

                        speaker[2] = "General Vixon";
                        leftspeaker[2] = true;
                        script[2] = "Rally your men back to the inner wall and ready the Flogger!";

                        speaker[3] = "General Vixon";
                        leftspeaker[3] = true;
                        script[3] = "I've got a buckethead cover'n our retreat!";

                        speaker[4] = "Officer";
                        leftspeaker[4] = true;
                        script[4] = "Of course, sir! Right away, sir!";

                        totalLines = 5;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 4:
                switch (instance)
                {
                    case 1:
                        speaker[0] = playerName;
                        leftspeaker[0] = true;
                        script[0] = "*Defending a tunnel, huh?*";

                        speaker[1] = playerName;
                        leftspeaker[1] = true;
                        script[1] = "Saint's stars!";

                        speaker[2] = playerName;
                        leftspeaker[2] = true;
                        script[2] = "You are one big rat!";

                        speaker[3] = "Skritter";
                        leftspeaker[3] = false;
                        script[3] = "Hhhhhhssssssss...";

                        speaker[4] = playerName;
                        leftspeaker[4] = true;
                        script[4] = "Easy now friend...";

                        speaker[5] = "Skritter";
                        leftspeaker[5] = false;
                        script[5] = "Skhrrreeeeeeh!!";

                        totalLines = 5;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 5:
                switch (instance)
                {
                    case 1:
                        speaker[0] = "Skritter";
                        leftspeaker[0] = true;
                        script[0] = "Hhhhhhssssssss!!";

                        totalLines = 1;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 6:
                switch (instance)
                {
                    case 1:
                        speaker[0] = playerName;
                        leftspeaker[0] = true;
                        script[0] = "*I'm pinned from both sides,*";

                        speaker[1] = playerName;
                        leftspeaker[1] = true;
                        script[1] = "*and there seems to be no end...*";

                        speaker[2] = playerName;
                        leftspeaker[2] = true;
                        script[2] = "Ah! I'm too young to be eaten alive!";

                        totalLines = 3;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 7:
                switch (instance)
                {
                    case 1:
                        speaker[0] = playerName;
                        leftspeaker[0] = false;
                        script[0] = "Not-";

                        speaker[1] = playerName;
                        leftspeaker[1] = true;
                        script[1] = "Not like this...";

                        totalLines = 2;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                    case 2:
                        speaker[0] = "???";
                        leftspeaker[0] = false;
                        script[0] = "...";

                        speaker[1] = "???";
                        leftspeaker[1] = false;
                        script[1] = "Nova.";

                        speaker[2] = "???";
                        leftspeaker[2] = false;
                        script[2] = "After a century,";

                        speaker[3] = "???";
                        leftspeaker[3] = false;
                        script[3] = "destiny finally calls.";

                        totalLines = 4;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 8:
                switch (instance)
                {
                    case 1:
                        speaker[0] = "???";
                        leftspeaker[0] = false;
                        script[0] = "Nova!";

                        speaker[1] = "???";
                        leftspeaker[1] = false;
                        script[1] = "Spawn of Sun and Void.";

                        speaker[2] = "???";
                        leftspeaker[2] = false;
                        script[2] = "I am Theron.";

                        speaker[3] = "???";
                        leftspeaker[3] = false;
                        script[3] = "But that is not important.";

                        speaker[4] = "???";
                        leftspeaker[4] = false;
                        script[4] = "War stretches across our planet.";

                        speaker[5] = "???";
                        leftspeaker[5] = false;
                        script[5] = "If sentient kind is to survive this incursion,";

                        speaker[6] = "???";
                        leftspeaker[6] = false;
                        script[6] = "our forces must be unified...";

                        speaker[7] = "???";
                        leftspeaker[7] = false;
                        script[7] = "The Kingdom of Light must be taken by force.";

                        speaker[8] = playerName;
                        leftspeaker[8] = true;
                        script[8] = "What's my place in all of this?";

                        speaker[9] = "???";
                        leftspeaker[9] = false;
                        script[9] = "You are the key to it all, my small friend.";

                        speaker[10] = "???";
                        leftspeaker[10] = false;
                        script[10] = "I have saved you from the icy whisper of death.";

                        speaker[11] = "???";
                        leftspeaker[11] = false;
                        script[11] = "All I ask of your life debt, is that you aid me in taking Solaris.";

                        speaker[12] = playerName;
                        leftspeaker[12] = true;
                        script[12] = "And if I refuse?";

                        speaker[13] = "???";
                        leftspeaker[13] = false;
                        script[13] = "Test me, child,";

                        speaker[14] = "???";
                        leftspeaker[14] = false;
                        script[14] = "and I will snuf out your life like a fleeting candle flame...";

                        speaker[15] = playerName;
                        leftspeaker[15] = true;
                        script[15] = "Good to know...";

                        speaker[16] = "???";
                        leftspeaker[16] = false;
                        script[16] = "You will come to understand the severity of the situation, Nova.";

                        speaker[17] = "???";
                        leftspeaker[17] = false;
                        script[17] = "In time...";

                        totalLines = 18;
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
                        script[0] = "This here is the combat yard -";

                        speaker[1] = "Seamstress";
                        leftspeaker[1] = true;
                        script[1] = "It seems you can handle youself, but...";

                        speaker[2] = "Seamstress";
                        leftspeaker[2] = true;
                        script[2] = "Something tells me you could use a lesson or two in the Arcane arts.";

                        speaker[3] = "Seamstress";
                        leftspeaker[3] = true;
                        script[3] = "Don't worry, I'll go easy on you this time.";

                        totalLines = 4;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 10:
                switch (instance)
                {
                    case 1:
                        speaker[0] = "Theron";
                        leftspeaker[0] = false;
                        script[0] = "Are preparations proceeding as planned?";

                        speaker[1] = "Slade";
                        leftspeaker[1] = true;
                        script[1] = "Of course, my lord.";

                        speaker[2] = "Slade";
                        leftspeaker[2] = true;
                        script[2] = "The disguises are all in order and my men are preparing as we speak.";

                        speaker[3] = playerName;
                        leftspeaker[3] = true;
                        script[3] = "Wait, you want me to train here? With your cult?";

                        speaker[4] = "Seamstress";
                        leftspeaker[4] = false;
                        script[4] = "The Order of Shadow is a school specializing in ancient powerful magics.";

                        speaker[5] = "Seamstress";
                        leftspeaker[5] = false;
                        script[5] = "As the Nova, you have a lot of natural talent. You just need a teacher.";

                        speaker[6] = playerName;
                        leftspeaker[6] = true;
                        script[6] = "Nova? Why is everone calling me that?";

                        speaker[7] = "???";
                        leftspeaker[7] = false;
                        script[7] = "She is referring to your true name, " + playerName + ". A title you have so easily forgotten...";

                        speaker[8] = playerName;
                        leftspeaker[8] = true;
                        script[8] = "!";

                        speaker[9] = "???";
                        leftspeaker[9] = false;
                        script[9] = "I've been watching you. Pointlessly pacing about in your quarters.";

                        speaker[10] = "???";
                        leftspeaker[10] = false;
                        script[10] = "You lack purpose.";

                        speaker[11] = "???";
                        leftspeaker[11] = false;
                        script[11] = "Your circumstance has... rebirthed you. You're now free from your old life.";

                        speaker[12] = "???";
                        leftspeaker[12] = false;
                        script[12] = "Or maybe you would you like to know who you once were?";

                        speaker[13] = "???";
                        leftspeaker[13] = false;
                        script[13] = "You need not stay forever. Only until you've learned all you need to know.";

                        speaker[14] = "???";
                        leftspeaker[14] = false;
                        script[14] = "So tell me child, will you join us? Or return to your nest of comfort?";

                        totalLines = 15;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
        }
    }

    IEnumerator Cutscene10(int action, int act = 0)
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
            case 8:
                yield return new WaitForSeconds(1);
                //actionsCompleted = true; //actions are completed
                speaker02.GetComponent<LerpScript>().LerpToPos(speaker02.transform.position, speaker02.transform.position + new Vector3(70, 0, 0), 1f);
                yield return new WaitForSeconds(0.5f);
                playerMannequin.GetComponent<AnimationController>().FlipFlop();
                yield return new WaitForSeconds(0.5f);
                playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, playerMannequin.transform.position + new Vector3(70, 0, 0), 2f);
                break;
            case 17:
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

    IEnumerator Cutscene9(int action, int act = 0)
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
            case 3:
                yield return new WaitForSeconds(2);
                actionsCompleted = true; //actions are completed
                StartCoroutine(LoadCombatScene(1, 2));
                break;
        }
        //////////////////
    }

    IEnumerator Cutscene8(int action, int act = 0)
    {
        switch (action)
        {
            case 0:
                // Set next Level //
                nextLevel = "MainMenu_Tutorial_Scene";
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 0.25f);
                yield return new WaitForSeconds(3f);
                speaker01.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.clear, new Color(0.5f, 0.5f, 0.5f, 1.0f), 0.25f);
                yield return new WaitForSeconds(3f);
                StartCoroutine(NewDialogue(8, 1));
                break;
            case 19:
                yield return new WaitForSeconds(2f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), 0.75f);
                actionsCompleted = true; //actions are completed
                StartCoroutine(LoadNextLv());
                break;
        }
        //////////////////
    }

    IEnumerator Cutscene7(int action, int instance = 0)
    {
        switch(instance)
        {
            case 1:
                switch (action)
                {
                    case 0:
                        // Set next Level //
                        nextLevel = "Exposition_Scene08";
                        speaker02.GetComponent<LerpScript>().LerpToColor(Color.clear, Color.white, 1f);
                        yield return new WaitForSeconds(3.5f);
                        speaker02.GetComponent<LerpScript>().LerpToColor(Color.white, Color.clear, 1f);
                        yield return new WaitForSeconds(1f);
                        blackSq.GetComponent<FadeScript>().FadeColored(Color.black, Color.white, 0.6f);
                        yield return new WaitForSeconds(1.5f);
                        blackSq.GetComponent<FadeScript>().FadeColored(Color.white, new Color(1, 1, 1, 0.5f), 0.6f);
                        playerMannequin.GetComponent<AnimationController>().PlayWalkAnim();
                        playerMannequin.GetComponent<AnimationController>().SetPlaySpeed(0.5f);
                        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerInitPos, playerInitPos + new Vector3(100, 0, 0), 0.25f);
                        yield return new WaitForSeconds(1.5f);
                        StartCoroutine(NewDialogue(7, 1));
                        break;
                    case 2:
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0.5f), Color.white, 1f);
                        yield return new WaitForSeconds(1f);
                        blackSq.GetComponent<FadeScript>().FadeColored(Color.white, new Color(1, 1, 1, 0.5f), 0.7f);
                        yield return new WaitForSeconds(0.5f);
                        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerInitPos + new Vector3(100, 0, 0), playerInitPos + new Vector3(140, 0, 0), 0.25f);
                        yield return new WaitForSeconds(3f);
                        playerMannequin.GetComponent<AnimationController>().SetPlaySpeed(0.75f);
                        playerMannequin.GetComponent<AnimationController>().PlayDeathAnim();
                        yield return new WaitForSeconds(0.5f);
                        sfxManager.GetComponent<SoundFXManager_C>().playSnowCollapse();
                        break;
                    case 3:
                        yield return new WaitForSeconds(1f);
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0.5f), Color.white, 0.6f);
                        break;
                }
                break;
            case 2:
                switch(action)
                {
                    case 0:
                        yield return new WaitForSeconds(4f);
                        blackSq.GetComponent<FadeScript>().FadeColored(Color.white, new Color(1, 1, 1, 0.5f), 0.6f);
                        yield return new WaitForSeconds(2.5f);
                        
                        speaker01.GetComponent<LerpScript>().LerpToPos(speaker01.transform.position, speaker01.transform.position - new Vector3(140, 0, 0), 0.25f);
                        sfxManager.GetComponent<SoundFXManager_C>().playSnowSteps();
                        yield return new WaitForSeconds(2.5f);
                        StartCoroutine(NewDialogue(eCurrentCutscene, instance));
                        break;
                    case 5:
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0.5f), Color.white, 0.6f);
                        sfxManager.GetComponent<SoundFXManager_C>().FadeVolume(1, 0, 0.7f, true);
                        yield return new WaitForSeconds(2f);
                        StartCoroutine(LoadNextLv());
                        break;
                }
                break;
        }


        //////////////////
    }

    IEnumerator Cutscene6(int action, int instance = 0)
    {
        switch(action)
        {
            case 0:
                // Set next Level //
                nextLevel = "Exposition_Scene04";
                playerMannequin.GetComponent<AnimationController>().FlipFlop();
                playerMannequin.GetComponent<AnimationController>().PlayHoldAttackAnim();
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 5f);
                yield return new WaitForSeconds(0.15f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 5f);
                yield return new WaitForSeconds(0.15f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 5f);
                yield return new WaitForSeconds(0.15f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 5f);
                yield return new WaitForSeconds(0.15f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 5f);
                yield return new WaitForSeconds(1f);
                StartCoroutine(NewDialogue(6, 1));
                break;
            case 4:
                speaker02.GetComponent<LerpScript>().LerpToPos(speaker02.transform.position, speaker02.transform.position + new Vector3(50, 0, 0), 2);
                sfxManager.GetComponent<SoundFXManager_C>().playSkitterScreech();
                yield return new WaitForSeconds(0.25f);
                speaker04.GetComponent<LerpScript>().LerpToPos(speaker04.transform.position, speaker04.transform.position + new Vector3(50, 0, 0), 2);
                yield return new WaitForSeconds(0.15f);
                speaker03.GetComponent<LerpScript>().LerpToPos(speaker03.transform.position, speaker03.transform.position + new Vector3(75, 0, 0), 2);
                yield return new WaitForSeconds(0.15f);
                playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, playerMannequin.transform.position + new Vector3(50, 0, 0), 1f);
                yield return new WaitForSeconds(0.5f);
                playerMannequin.GetComponent<AnimationController>().FlipFlop();
                yield return new WaitForSeconds(1f);
                playerMannequin.GetComponent<AnimationController>().FlipFlop();
                yield return new WaitForSeconds(1.5f);
                playerMannequin.GetComponent<AnimationController>().FlipFlop();
                playerMannequin.GetComponent<AnimationController>().PlayWalkAnim();
                playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, playerMannequin.transform.position + new Vector3(250,0,0), 1.2f);
                yield return new WaitForSeconds(1f);
                speaker03.GetComponent<LerpScript>().LerpToPos(speaker03.transform.position, speaker03.transform.position + new Vector3(475, 0, 0), 1.2f);
                sfxManager.GetComponent<SoundFXManager_C>().playSkitterScreech();
                yield return new WaitForSeconds(0.25f);
                speaker02.GetComponent<LerpScript>().LerpToPos(speaker02.transform.position, speaker02.transform.position + new Vector3(450, 0, 0), 1.2f);
                yield return new WaitForSeconds(0.1f);
                speaker04.GetComponent<LerpScript>().LerpToPos(speaker04.transform.position, speaker04.transform.position + new Vector3(450, 0, 0), 1.2f);
                sfxManager.GetComponent<SoundFXManager_C>().playSkitterScreech();
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), 1.2f);
                yield return new WaitForSeconds(0.75f);
                StartCoroutine(LoadNextLv());
                actionsCompleted = true; //actions are completed
                break;
        }
    }

    IEnumerator Cutscene5(int action, int instance = 0)
    {
        switch (action)
        {
            case 0:
                // Set next Level //
                nextLevel = "Tutorial_Scene02";
                Vector3 start = speaker01.transform.position;
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 0.5f);
                yield return new WaitForSeconds(0.5f);
                playerMannequin.GetComponent<AnimationController>().PlayCheerAnim();
                yield return new WaitForSeconds(0.75f);
                speaker01.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.white, Color.clear, 0.85f);
                sfxManager.GetComponent<SoundFXManager_C>().playSkitterScreech();
                speaker01.GetComponent<LerpScript>().LerpToPos(start, start + new Vector3(15,0,0), 8.0f);
                yield return new WaitForSeconds(0.175f);
                speaker01.GetComponent<LerpScript>().LerpToPos(start, start - new Vector3(15, 0, 0), 8.0f);
                yield return new WaitForSeconds(0.175f);
                speaker01.GetComponent<LerpScript>().LerpToPos(start - new Vector3(15, 0, 0), start, 8.0f);
                yield return new WaitForSeconds(3f);
                Vector3 spawnPos = new Vector3(playerMannequin.transform.position.x + 10, playerMannequin.transform.position.y + 90, 0);
                GameObject effectClone = (GameObject)Instantiate(ExclamationPoint, spawnPos, transform.rotation);
                yield return new WaitForSeconds(1.5f);
                playerMannequin.GetComponent<AnimationController>().FlipFlop();
                speaker02.GetComponent<LerpScript>().LerpToPos(speaker02.transform.position, speaker02.transform.position + new Vector3(60, 0, 0), 1.0f);
                yield return new WaitForSeconds(1f);
                StartCoroutine(NewDialogue(5, 1));
                speaker03.GetComponent<LerpScript>().LerpToPos(speaker03.transform.position, speaker03.transform.position - new Vector3(100, 0, 0), 1.0f);
                yield return new WaitForSeconds(1f);
                speaker02.GetComponent<LerpScript>().LerpToPos(speaker02.transform.position, speaker02.transform.position + new Vector3(50, 0, 0));
                yield return new WaitForSeconds(0.75f);
                playerMannequin.GetComponent<AnimationController>().FlipFlop();
                break;
            case 2:
                yield return new WaitForSeconds(1f);
                actionsCompleted = true; //actions are completed
                StartCoroutine(LoadCombatScene(1, 0));
                break;
        }
    }

    IEnumerator CutsceneTutorial(int action, int instance = 0)
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

    IEnumerator Cutscene4(int action, int instance = 0)
    {
        // Set next Level //
        nextLevel = "Tutorial_Scene02";
        //////////////////
        switch (action)
        {
            case 0:
                blackSq.GetComponent<FadeScript>().FadeColored(Color.black, Color.clear, 1.2f);
                playerMannequin.GetComponent<AnimationController>().PlayWalkAnim();
                yield return new WaitForSeconds(2f);
                playerMannequin.GetComponent<LerpScript>().LerpToPos(playerInitPos, playerInitPos + new Vector3(220, 0, 0), 1f);
                yield return new WaitForSeconds(2f);
                playerMannequin.GetComponent<AnimationController>().PlayIdleAnim();
                StartCoroutine(NewDialogue(4, 1));
                break;
            case 1:
                yield return new WaitForSeconds(1.5f);
                speaker01.GetComponent<LerpScript>().LerpToPos(speaker01.transform.position, speaker01.transform.position + new Vector3(-115, 0, 0), 0.5f);
                break;
            case 2:
                Vector3 spawnPos = new Vector3(playerMannequin.transform.position.x + 5, playerMannequin.transform.position.y + 90, 0);
                GameObject effectClone = (GameObject)Instantiate(ExclamationPoint, spawnPos, transform.rotation);
                break;
            case 5:
                sfxManager.GetComponent<SoundFXManager_C>().playSkitterScreech();
                yield return new WaitForSeconds(0.7f);
                break;
            case 6:
                playerMannequin.GetComponent<AnimationController>().PlayHoldAttackAnim();
                speaker01.GetComponent<LerpScript>().LerpToPos(speaker01.transform.position, speaker01.transform.position + new Vector3(-130, 0, 0), 2f);
                MusicManager.GetComponent<Music_Controller>().stopAllMusic();
                yield return new WaitForSeconds(0.1f);
                GameController.controller.GetComponent<MenuUIAudio>().playSoundClip(CombatStartup);
                yield return new WaitForSeconds(0.5f);
                blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.white, 2f);
                sfxManager.GetComponent<SoundFXManager_C>().FadeVolume(1, 0, 0.7f, true);
                StartCoroutine(LoadNextLv());
                break;
        }
    }

    IEnumerator Cutscene3(int action, int instance = 0)
    {
        // Set next Level //
        nextLevel = "Exposition_Scene04";
        //////////////////
        switch(instance)
        {
            case 1:
                switch (action)
                {
                    case 0:
                        blackSq.GetComponent<FadeScript>().FadeColored(Color.black, new Color(1, 1, 1, 0.5f), 0.6f);
                        yield return new WaitForSeconds(0.75f);
                        speaker01.GetComponent<LerpScript>().LerpToPos(speaker01.transform.position, speaker01.transform.position + new Vector3(200, 0, 0), 1f);
                        yield return new WaitForSeconds(2f);
                        StartCoroutine(NewDialogue(3, 1));
                        yield return new WaitForSeconds(1f);
                        Vector3 spawnPos = new Vector3(playerMannequin.transform.position.x + 3, playerMannequin.transform.position.y + 90, 0);
                        GameObject effectClone = (GameObject)Instantiate(ExclamationPoint, spawnPos, transform.rotation);
                        yield return new WaitForSeconds(0.75f);
                        playerMannequin.GetComponent<AnimationController>().FlipFlop();
                        break;
                    case 15:
                        yield return new WaitForSeconds(1.5f);
                        dialoguePanel.GetComponent<LerpScript>().LerpToPos(panelUpPos, panelDownPos, 3f);
                        dialoguePanel.GetComponent<LerpScript>().LerpToColor(panelOrigColor, Color.clear, 3f);
                        playerMannequin.GetComponent<AnimationController>().FlipFlop();
                        yield return new WaitForSeconds(0.2f);
                        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerInitPos, playerInitPos + new Vector3(60, 0, 0), 2f);
                        yield return new WaitForSeconds(0.75f);
                        StartCoroutine(ExitScene(playerMannequin));
                        break;
                }
                break;
            case 2:
                switch (action)
                {
                    case 0:
                        yield return new WaitForSeconds(0.5f);
                        speaker02.GetComponent<LerpScript>().LerpToPos(speaker02.transform.position, speaker02.transform.position + new Vector3(100, 0, 0), 1.0f);
                        yield return new WaitForSeconds(1f);
                        StartCoroutine(NewDialogue(eCurrentCutscene, instance));
                        break;
                    case 1:
                        yield return new WaitForSeconds(1f);
                        speaker01.GetComponentInChildren<SpriteRenderer>().flipX = true;
                        break;
                    case 6:
                        yield return new WaitForSeconds(1f);
                        speaker02.GetComponentInChildren<SpriteRenderer>().flipX = true;
                        yield return new WaitForSeconds(0.2f);
                        speaker02.GetComponent<LerpScript>().LerpToPos(speaker02.transform.position, speaker02.transform.position - new Vector3(200, 0, 0), 1.5f);
                        yield return new WaitForSeconds(0.5f);
                        speaker01.GetComponent<LerpScript>().LerpToPos(speaker01.transform.position, speaker02.transform.position - new Vector3(200, 0, 0), 1.5f);
                        yield return new WaitForSeconds(3f);
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0.5f), Color.white, 0.6f);
                        sfxManager.GetComponent<SoundFXManager_C>().FadeVolume(1, 0, 0.7f, true);
                        yield return new WaitForSeconds(2f);
                        StartCoroutine(LoadNextLv());
                        break;
                }
                break;
        }
    }

    IEnumerator Cutscene2(int action, int instance = 0)
    {
        // Set next Level //
        nextLevel = "Exposition_Scene03";
        //////////////////
        switch (action)
        {
            case 0:
                blackSq.GetComponent<FadeScript>().FadeColored(Color.white, Color.clear, 0.15f);
                yield return new WaitForSeconds(1f);
                speaker05.GetComponent<SpriteRenderer>().enabled = true;
                speaker05.GetComponent<Animator>().enabled = true;
                cameraObj.GetComponent<CameraController>().ShakeCamera(1, true, 0.75f);
                yield return new WaitForSeconds(1.5f);
                speaker05.GetComponent<Animator>().speed = 2.5f;
                cameraObj.GetComponent<CameraController>().ShakeCamera(1, true, 0.75f);
                yield return new WaitForSeconds(0.95f);
                speaker05.GetComponent<Animator>().speed = 0.46f;
                cameraObj.GetComponent<CameraController>().ShakeCamera(1, true, 0.75f);
                yield return new WaitForSeconds(2.5f);
                speaker05.GetComponent<Animator>().speed = 0.6f;
                cameraObj.GetComponent<CameraController>().ShakeCamera(1, true, 0.75f);
                yield return new WaitForSeconds(2.5f);
                cameraObj.GetComponent<CameraController>().ShakeCamera(1, true, 2f);
                blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.white, 1f);
                yield return new WaitForSeconds(2f);
                speaker02.GetComponent<SpriteRenderer>().enabled = true;
                speaker02.GetComponent<Animator>().enabled = true;
                blackSq.GetComponent<FadeScript>().FadeColored(Color.white, Color.clear, 1f);
                yield return new WaitForSeconds(1.5f);
                blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.white, 1f);
                yield return new WaitForSeconds(1.5f);
                blackSq.GetComponent<FadeScript>().FadeColored(Color.white, Color.clear, 1f);
                yield return new WaitForSeconds(0.75f);
                blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.white, 1f);
                yield return new WaitForSeconds(1.5f);
                blackSq.GetComponent<FadeScript>().FadeColored(Color.white, Color.clear, 1f);
                yield return new WaitForSeconds(2f);
                StartCoroutine(NewDialogue(2,1));
                break;
            case 4:
                yield return new WaitForSeconds(1f);
                sfxManager.GetComponent<SoundFXManager_C>().playLaserBombardment(true);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), Color.white, 4f);
                yield return new WaitForSeconds(0.2f);
                speaker03.GetComponent<ParticleSystem>().Play();
                speaker04.GetComponent<ParticleSystem>().Play();
                speaker06.GetComponent<ParticleSystem>().Play();
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), Color.white, 4f);
                yield return new WaitForSeconds(0.15f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), Color.white, 4f);
                yield return new WaitForSeconds(0.1f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), Color.white, 4f);
                yield return new WaitForSeconds(0.3f);
                blackSq.GetComponent<FadeScript>().FadeColored(Color.white, new Color(1, 1, 1, 0), 1f);
                yield return new WaitForSeconds(1.75f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), Color.white, 4f);
                yield return new WaitForSeconds(0.2f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), Color.white, 4f);
                yield return new WaitForSeconds(0.15f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), Color.white, 4f);
                yield return new WaitForSeconds(0.1f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), Color.white, 4f);
                yield return new WaitForSeconds(0.3f);
                blackSq.GetComponent<FadeScript>().FadeColored(Color.white, new Color(1, 1, 1, 0), 1f);
                break;
            case 5:
                yield return new WaitForSeconds(0.1f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), Color.white, 4f);
                yield return new WaitForSeconds(0.3f);
                blackSq.GetComponent<FadeScript>().FadeColored(Color.white, new Color(1, 1, 1, 0), 1f);
                break;
            case 6:
                yield return new WaitForSeconds(0.2f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), Color.white, 4f);
                yield return new WaitForSeconds(0.4f);
                blackSq.GetComponent<FadeScript>().FadeColored(Color.white, new Color(1, 1, 1, 0), 1f);
                yield return new WaitForSeconds(0.1f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), Color.white, 4f);
                yield return new WaitForSeconds(0.3f);
                blackSq.GetComponent<FadeScript>().FadeColored(Color.white, new Color(1, 1, 1, 0), 1f);
                break;
            case 7:
                yield return new WaitForSeconds(0.25f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), Color.white, 4f);
                yield return new WaitForSeconds(0.2f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), Color.white, 4f);
                yield return new WaitForSeconds(0.15f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), Color.white, 4f);
                yield return new WaitForSeconds(0.1f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), Color.white, 4f);
                yield return new WaitForSeconds(0.3f);
                blackSq.GetComponent<FadeScript>().FadeColored(Color.white, new Color(1, 1, 1, 0), 1f);
                break;
            case 8:
                
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), Color.white, 0.35f);
                yield return new WaitForSeconds(1.5f);
                sfxManager.GetComponent<SoundFXManager_C>().FadeVolume(1, 0, 0.7f, false);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1,1,1,0.8f), Color.red, 0.75f);
                yield return new WaitForSeconds(1f);
                blackSq.GetComponent<FadeScript>().FadeColored(Color.red, Color.black, 0.75f);
                sfxManager.GetComponent<SoundFXManager_C>().FadeVolume(1, 0, 0.7f, true);
                yield return new WaitForSeconds(2.25f);
                StartCoroutine(LoadNextLv());
                break;
        }
    }

    IEnumerator Cutscene1(int action, int act = 0)
    {
        // Set next Level //
        nextLevel = "Exposition_Scene02";
        //////////////////
        switch (action)
        {
            case 0:
                blackSq.GetComponent<FadeScript>().FadeColored(Color.white, Color.clear, 0.15f);
                yield return new WaitForSeconds(2f);
                speaker02.GetComponent<LerpScript>().LerpToPos(speaker02.transform.position, speaker02.transform.position - new Vector3(320, 0, 0), 0.5f);
                yield return new WaitForSeconds(2f);
                speaker03.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                yield return new WaitForSeconds(0.75f);
                StartCoroutine(NewDialogue(1, 1));
                break;
            case 8:
                blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.white, 0.5f);
                sfxManager.GetComponent<SoundFXManager_C>().FadeVolume(1, 0, 0.7f, true);
                yield return new WaitForSeconds(3.25f);
                StartCoroutine(LoadNextLv());
                break;
        }
    }
}
