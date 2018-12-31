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

    public GameObject decisionManager;

    public Vector3 panelOffset = new Vector3(0, 100, 0);
    private Color panelOrigColor;
    private string nextLevel;
    private Dialogue_Manager_C dialogueManager;
    private int eCurrentCutscene = 0;
    private int eCurrentInstance = 1;
    [HideInInspector]
    public int decisionNumber = 0;
    private int eMaxInstances = 1;
    private float eInstanceDelay = 2.0f;
    private Color clear = new Color(1, 1, 1, 0);

    [HideInInspector]
    public bool actionsCompleted = false;
    [HideInInspector]
    public int actionCounter = 0;

    string playerName;
    EnemyEncounter encounter;
    Vector3 playerInitPos;
    Vector3 origCameraPos;

    Coroutine enableInputHandler;

    // Use this for initialization
    void Start ()
    {
        playerMannequin = GameController.controller.playerObject;
        encounter = GameController.controller.currentEncounter;
        origCameraPos = cameraObj.transform.position;
        playerInitPos = playerMannequin.transform.position;
        playerName = GameController.controller.playerName;
        panelOrigColor = dialoguePanel.GetComponent<Image>().color;
        dialoguePanel.GetComponent<Image>().color = Color.clear;
        dialogueManager = this.GetComponent<Dialogue_Manager_C>();
        ready4Input = false;

        if (encounter == null)
        {
            encounter = new EnemyEncounter();
            encounter.encounterNumber = 1;  
        }

        decisionNumber = 0;
        //BeginCutscene(15, 1);
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
            enableInputHandler = StartCoroutine(handleInput());
        }
    }

    IEnumerator handleInput()
    {
        if (dialogueManager.dDialogueCompleted)
            dialogueManager.StopDialogue();
        else
        {
            dialogueManager.DialogueHandler();
            yield return new WaitForSeconds(0.6f);
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
                if(GameController.controller.stagesCompleted == 1)
                    GameController.controller.stagesCompleted++;
                break;
            case 9:
                eMaxInstances = 2;
                StartCoroutine(Cutscene9(actionCounter, instance));
                break;
            case 10:
                StartCoroutine(Cutscene10(actionCounter, instance));
                break;
            case 11:
                StartCoroutine(Cutscene11(actionCounter, instance));
                break;
            case 12:
                eMaxInstances = 3;
                StartCoroutine(Cutscene12(actionCounter, instance));
                break;
            case 13:
                StartCoroutine(Cutscene13(actionCounter, instance));
                break;
            case 14:
                eMaxInstances = 3;
                StartCoroutine(Cutscene14(actionCounter, instance));
                break;
            case 15:
                eMaxInstances = 2;

                if (GameController.controller.stagesCompleted == 2)
                    GameController.controller.stagesCompleted++;

                switch (decisionNumber)
                {
                    case 0:
                        StartCoroutine(Cutscene15(actionCounter, instance));
                        break;
                    case 1:
                        StartCoroutine(Cutscene15e(actionCounter));
                        break;
                    case 2:
                        StartCoroutine(Cutscene15g(actionCounter));
                        break;
                }
                break;
            case 16:
                StartCoroutine(Cutscene16(actionCounter, instance));
                break;
            case 17:
                eMaxInstances = 2;
                StartCoroutine(Cutscene17(actionCounter, instance));
                break;
            case 18:
                eMaxInstances = 4;
                StartCoroutine(Cutscene18(actionCounter, instance));
                break;
            case 19:
                StartCoroutine(Cutscene19(actionCounter, instance));
                break;
            case 20:
                eMaxInstances = 2;
                if (GameController.controller.stagesCompleted == 3)
                    GameController.controller.stagesCompleted++;
                StartCoroutine(Cutscene20(actionCounter, instance));
                break;
            case 21:
                eMaxInstances = 5;
                StartCoroutine(Cutscene21(actionCounter, instance));
                break;
        }

        GameController.controller.Save(GameController.controller.playerName);
        ready4Input = true;
    }

    public IEnumerator LoadCombatScene(int level, int encounterNum, bool useBlink = true)
    {
        GameController.controller.currentEncounter = new EnemyEncounter();
        GameController.controller.currentEncounter = EncounterToolsScript.tools.SpecifyEncounter(level, encounterNum);
        
        yield return new WaitForSeconds(1f);
        MusicManager.GetComponent<Music_Controller>().stopAllMusic();
        GameController.controller.GetComponent<MenuUIAudio>().playSoundClip(CombatStartup, 0.1f);
        yield return new WaitForSeconds(0.25f);

        if(useBlink)
        {
            blackSq.GetComponent<Image>().color = Color.white;
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
        }

        playerMannequin.SetActive(true);
        SceneManager.LoadScene("TurnCombat_Scene");
    }

    public void EndDialogue()
    {
        if(enableInputHandler != null)
            StopCoroutine(enableInputHandler);

        ready4Input = false;
        actionsCompleted = false;
        actionCounter = 0;
        eInstanceDelay = 2.0f;
        dialoguePanel.GetComponent<LerpScript>().LerpToPos(dialoguePanel.transform.position, dialoguePanel.transform.position - panelOffset, 2f);
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
        playerMannequin.SetActive(true);
        SceneManager.LoadScene(nextLevel);
    }

    IEnumerator EnterScene(GameObject character)
    {
        sfxManager.GetComponent<SoundFXManager_C>().playExitScene();
        yield return new WaitForSeconds(0.5f);
        character.SetActive(true);
    }

    IEnumerator ExitScene(GameObject character)
    {
        character.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        sfxManager.GetComponent<SoundFXManager_C>().playExitScene();
    }

    IEnumerator NewDialogue(int cutscene, int instance, int decisionNum = 0)
    {
        int totalLines = 0;
        bool usesPlayer = false;
        string[] speaker = new string[30];
        bool[] leftspeaker = new bool[30];
        string[] script = new string[30];

        actionCounter = 0;
       
        dialoguePanel.GetComponent<LerpScript>().LerpToPos(dialoguePanel.transform.position, dialoguePanel.transform.position + panelOffset, 2f);
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

                        speaker[0] = "Cmd. Vixon";
                        leftspeaker[0] = true;
                        script[0] = "Welcome back soldier, any word from Solaris?";

                        speaker[1] = "H. Officer";
                        leftspeaker[1] = false;
                        script[1] = "No sir! Still no sign of Tesdin.";

                        speaker[2] = "Cmd. Vixon";
                        leftspeaker[2] = true;
                        script[2] = "It's only been a week. I'm sure he'll turn up!";

                        speaker[3] = "H. Officer";
                        leftspeaker[3] = false;
                        script[3] = "Sir, he only packed enough food for three days in the mountains...";

                        speaker[4] = "Cmd. Vixon";
                        leftspeaker[4] = true;
                        script[4] = "Excellent point, Officer! He's probably frozen derph-fodder by now.";

                        speaker[5] = "Cmd. Vixon";
                        leftspeaker[5] = true;
                        script[5] = "Rally the others! We have to be ready for anything!";

                        speaker[6] = "H. Officer";
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
                        speaker[0] = "Cmd. Vixon";
                        leftspeaker[0] = true;
                        script[0] = "Weapons ready!";

                        speaker[1] = "Cmd. Vixon";
                        leftspeaker[1] = true;
                        script[1] = "Steady now!";

                        speaker[2] = "Cmd. Vixon";
                        leftspeaker[2] = true;
                        script[2] = "...";

                        speaker[3] = "Cmd. Vixon";
                        leftspeaker[3] = true;
                        script[3] = "Give 'em hell boys!";

                        totalLines = 4;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 3:
                switch (instance)
                {
                    case 1:
                        speaker[0] = "Cmd. Vixon";
                        leftspeaker[0] = true;
                        script[0] = "Listen up soldier!";

                        speaker[1] = "Cmd. Vixon";
                        leftspeaker[1] = true;
                        script[1] = "I know you're new to the Bulwark,";

                        speaker[2] = "Cmd. Vixon";
                        leftspeaker[2] = true;
                        script[2] = "but we've got an apocalypse on our hands!";

                        speaker[3] = "Cmd. Vixon";
                        leftspeaker[3] = true;
                        script[3] = "I'm counting on you-";

                        speaker[4] = playerName;
                        leftspeaker[4] = false;
                        script[4] = "Me?!";

                        speaker[5] = "Cmd. Vixon";
                        leftspeaker[5] = true;
                        script[5] = "to hold this bridge with everything you've got!";

                        speaker[6] = "Cmd. Vixon";
                        leftspeaker[6] = true;
                        script[6] = "Nothing gets past that choke! Got it?!";

                        speaker[7] = playerName;
                        leftspeaker[7] = false;
                        script[7] = "Sir, yes, sir!";

                        speaker[8] = "Cmd. Vixon";
                        leftspeaker[8] = true;
                        script[8] = "I knew I could count on you-";

                        speaker[9] = "Cmd. Vixon";
                        leftspeaker[9] = true;
                        script[9] = "Uhh-";

                        speaker[10] = playerName;
                        leftspeaker[10] = false;
                        script[10] = "Private " + playerName + ", sir...";

                        speaker[11] = "Cmd. Vixon";
                        leftspeaker[11] = true;
                        script[11] = "That's great kid, listen here...";

                        speaker[12] = "Cmd. Vixon";
                        leftspeaker[12] = true;
                        script[12] = "That bridge is more important than your life!";

                        speaker[13] = "Cmd. Vixon";
                        leftspeaker[13] = true;
                        script[13] = "Now I need you to correct your effeminate look'n posture and defend it!";

                        speaker[14] = playerName;
                        leftspeaker[14] = false;
                        script[14] = "Sir, yes, sir!";

                        eInstanceDelay = 3.0f;
                        totalLines = 15;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                    case 2:
                        speaker[0] = "H. Officer";
                        leftspeaker[0] = true;
                        script[0] = "Commander!!";

                        speaker[1] = "H. Officer";
                        leftspeaker[1] = true;
                        script[1] = "We're losing bodies too qui-";

                        speaker[2] = "Cmd. Vixon";
                        leftspeaker[2] = true;
                        script[2] = "Rally your men back to the inner wall and barricade the door!";

                        speaker[3] = "Cmd. Vixon";
                        leftspeaker[3] = true;
                        script[3] = "I've got a buckethead cover'n our retreat!";

                        speaker[4] = "H. Officer";
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

                        speaker[3] = "Skitter";
                        leftspeaker[3] = false;
                        script[3] = "Hhhhhhssssssss...";

                        speaker[4] = playerName;
                        leftspeaker[4] = true;
                        script[4] = "Easy now friend...";

                        speaker[5] = "Skitter";
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
                        speaker[0] = "Skitter";
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
                        speaker[0] = "H. Officer";
                        leftspeaker[0] = true;
                        script[0] = "Over here soldier! Quick!";

                        speaker[1] = "H. Officer";
                        leftspeaker[1] = true;
                        script[1] = "*They're gonna seal the-*";

                        speaker[2] = playerName;
                        leftspeaker[2] = false;
                        script[2] = "Oh! No no no no n-";

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
                        leftspeaker[0] = true;
                        script[0] = "It was! No, that's n-n-not right...";

                        speaker[1] = playerName;
                        leftspeaker[1] = true;
                        script[1] = "Or w-w-was it? No!";

                        speaker[2] = playerName;
                        leftspeaker[2] = true;
                        script[2] = "It was d-definitely P-p-porker's";

                        speaker[3] = playerName;
                        leftspeaker[3] = true;
                        script[3] = "I-I had.. l-latrine-";

                        speaker[4] = playerName;
                        leftspeaker[4] = true;
                        script[4] = "Latrine duty w-was l-l-last...";

                        speaker[5] = playerName;
                        leftspeaker[5] = true;
                        script[5] = "week...";

                        totalLines = 6;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                    case 2:
                        speaker[0] = "???";
                        leftspeaker[0] = false;
                        script[0] = "Well then...";

                        speaker[1] = "???";
                        leftspeaker[1] = false;
                        script[1] = "Not the least bit of what I was expecting.";

                        speaker[2] = "???";
                        leftspeaker[2] = false;
                        script[2] = "And yet-";

                        speaker[3] = "???";
                        leftspeaker[3] = false;
                        script[3] = "...";

                        speaker[4] = "???";
                        leftspeaker[4] = false;
                        script[4] = "It burns nonetheless...";

                        totalLines = 5;
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
                        script[0] = "Nova,";

                        speaker[1] = "???";
                        leftspeaker[1] = false;
                        script[1] = "Spawn of Star and Void.";

                        speaker[2] = "???";
                        leftspeaker[2] = false;
                        script[2] = "You may call me Theron.";

                        speaker[3] = "Theron";
                        leftspeaker[3] = false;
                        script[3] = "Things are not so clear right now, yes.";

                        speaker[4] = "Theron";
                        leftspeaker[4] = false;
                        script[4] = "But war is on our doorstep, youngling.";

                        speaker[5] = "Theron";
                        leftspeaker[5] = false;
                        script[5] = "If you are to survive this incursion,";

                        speaker[6] = "Theron";
                        leftspeaker[6] = false;
                        script[6] = "You will join me in unifying Sentianity...";

                        speaker[7] = "Theron";
                        leftspeaker[7] = false;
                        script[7] = "The Kingdom of Light is a beacon for all living beings of this post-world.";

                        speaker[8] = playerName;
                        leftspeaker[8] = true;
                        script[8] = "'Sentianity'? How exactly does this involve me?";

                        speaker[9] = "Theron";
                        leftspeaker[9] = false;
                        script[9] = "Your Aura is a shimmer of hope in dark times.";

                        speaker[10] = "Theron";
                        leftspeaker[10] = false;
                        script[10] = "I have dispelled the icy whisper of death that ensnares you.";

                        speaker[11] = "Theron";
                        leftspeaker[11] = false;
                        script[11] = "And all I ask, is that you promise me your sword in taking the kingdom.";

                        speaker[12] = playerName;
                        leftspeaker[12] = true;
                        script[12] = "I swore oath to the guard. I can't turn my back on my brothers and sisters...";

                        speaker[13] = "Theron";
                        leftspeaker[13] = false;
                        script[13] = "Your 'kin', would sooner toss you to the wargs than honor an oath.";

                        speaker[14] = "Theron";
                        leftspeaker[14] = false;
                        script[14] = "Besides that...";

                        speaker[15] = "Theron";
                        leftspeaker[15] = false;
                        script[15] = "Lady Fate led you here to die in the snow, broken and forgotten.";

                        speaker[16] = "Theron";
                        leftspeaker[16] = false;
                        script[16] = "Should you refuse my offer, I will return you to her cold embrace...";

                        speaker[17] = playerName;
                        leftspeaker[17] = true;
                        script[17] = "I don't have much of a choice it seems...";

                        speaker[18] = "Theron";
                        leftspeaker[18] = false;
                        script[18] = "There is always a choice, " + playerName + ".";

                        speaker[19] = "Theron";
                        leftspeaker[19] = false;
                        script[19] = "But some choices are easier to make than others...";

                        totalLines = 20;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 9:
                switch (instance)
                {
                    case 1:
                        speaker[0] = "Theron";
                        leftspeaker[0] = false;
                        script[0] = "Are your preparations proceeding smoothly?";

                        speaker[1] = "Slade";
                        leftspeaker[1] = true;
                        script[1] = "Of course, my lord. I must say though,";

                        speaker[2] = "Slade";
                        leftspeaker[2] = true;
                        script[2] = "keeping the disguises clean was quite the challenge.";

                        speaker[3] = "Theron";
                        leftspeaker[3] = false;
                        script[3] = "Excellent.";

                        speaker[4] = "Theron";
                        leftspeaker[4] = false;
                        script[4] = "It's times like these I forget my own age.";

                        speaker[5] = "Slade";
                        leftspeaker[5] = true;
                        script[5] = "I would argue that it's the ages, that have forgotten you.";

                        speaker[6] = "Theron";
                        leftspeaker[6] = false;
                        script[6] = "Then we must remind them we still stand, brother.";

                        speaker[7] = "Theron";
                        leftspeaker[7] = false;
                        script[7] = "Ready your men. We march on the kingdom at dusk...";

                        totalLines = 8;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                    case 2:
                        speaker[0] = "Slade";
                        leftspeaker[0] = false;
                        script[0] = "I hope you're ready, lackey.";

                        speaker[1] = "Hyun";
                        leftspeaker[1] = true;
                        script[1] = "Lose the attitude, Slade.";

                        speaker[2] = "Hyun";
                        leftspeaker[2] = true;
                        script[2] = playerName + " knows better than you just how important this mission is.";

                        speaker[3] = "Slade";
                        leftspeaker[3] = false;
                        script[3] = "We don't all share the same mission, princess.";

                        speaker[4] = "Slade";
                        leftspeaker[4] = false;
                        script[4] = "So keep your threads to yourself, and they won't get tangled...";

                        totalLines = 5;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 10:
                switch (instance)
                {
                    case 1:
                        break;
                }
                break;
            case 11:
                switch (instance)
                {
                    case 1:
                        speaker[0] = "S. Officer";
                        leftspeaker[0] = true;
                        script[0] = "Hold it!";

                        speaker[1] = "S. Officer";
                        leftspeaker[1] = true;
                        script[1] = "...";

                        speaker[2] = "S. Officer";
                        leftspeaker[2] = true;
                        script[2] = "I hope you've got a good show planned for us.";

                        speaker[3] = "Theron";
                        leftspeaker[3] = false;
                        script[3] = "Oh, of course!";

                        speaker[4] = "Theron";
                        leftspeaker[4] = false;
                        script[4] = "I assure you-";

                        speaker[5] = "Theron";
                        leftspeaker[5] = false;
                        script[5] = "the experience will leave you,";

                        speaker[6] = "Theron";
                        leftspeaker[6] = false;
                        script[6] = "breathless...";

                        speaker[7] = "S. Officer";
                        leftspeaker[7] = true;
                        script[7] = "By Jourjh!";

                        speaker[8] = "S. Officer";
                        leftspeaker[8] = true;
                        script[8] = "I wish you could see the excitement on my face right now!";

                        speaker[9] = "S. Officer";
                        leftspeaker[9] = true;
                        script[9] = "Carry on then!";

                        totalLines = 10;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 12:
                switch (instance)
                {
                    case 1:
                        speaker[0] = "King Gerard";
                        leftspeaker[0] = true;
                        script[0] = "Wonderful! Wonderful!";

                        speaker[1] = "King Gerard";
                        leftspeaker[1] = true;
                        script[1] = "The Myriads of Beyond have graced us with their presence, yet again!";

                        speaker[2] = "King Gerard";
                        leftspeaker[2] = true;
                        script[2] = "The Lady went on about your previous performance for weeks!";

                        speaker[3] = "Theron";
                        leftspeaker[3] = false;
                        script[3] = "Well isn't that funny...";

                        speaker[4] = "Theron";
                        leftspeaker[4] = false;
                        script[4] = "I don't seem to rememeber it at all.";

                        speaker[5] = "King Gerard";
                        leftspeaker[5] = true;
                        script[5] = "Oh you were all so magnificent!";

                        speaker[6] = "King Gerard";
                        leftspeaker[6] = true;
                        script[6] = "Surely you recall the sword swallowing Blumfark fellow!";

                        speaker[7] = "Theron";
                        leftspeaker[7] = false;
                        script[7] = "Hhhhmmm...";

                        speaker[8] = "Theron";
                        leftspeaker[8] = false;
                        script[8] = "No, I believe you're the only one I've ever met.";

                        speaker[9] = "King Gerard";
                        leftspeaker[9] = true;
                        script[9] = "Excuse me!?";

                        speaker[10] = "Theron";
                        leftspeaker[10] = false;
                        script[10] = "As you wish...";

                        totalLines = 11;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                    case 2:
                        speaker[0] = "King Gerard";
                        leftspeaker[0] = true;
                        script[0] = "And here I thought the show was planned for the evening!";

                        totalLines = 1;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                    case 3:
                        speaker[0] = "King Gerard";
                        leftspeaker[0] = true;
                        script[0] = "Aaauuuggghhh!";

                        speaker[1] = "Theron";
                        leftspeaker[1] = false;
                        script[1] = "Break them!!";

                        totalLines = 2;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 13:
                switch (instance)
                {
                    case 1:
                        //totalLines = 0;
                        //this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 14:
                switch (instance)
                {
                    case 1:
                        speaker[0] = "Theron";
                        leftspeaker[0] = true;
                        script[0] = "CEASE THIS!!!";

                        totalLines = 1;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                    case 2:
                        speaker[0] = "Theron";
                        leftspeaker[0] = true;
                        script[0] = "Your old rule is dead!";

                        speaker[1] = "Theron";
                        leftspeaker[1] = true;
                        script[1] = "Losing your lives here will bring your families no honor!";

                        speaker[2] = "Theron";
                        leftspeaker[2] = true;
                        script[2] = "A far more menacing threat is looming just out of sight.";

                        speaker[3] = "Theron";
                        leftspeaker[3] = true;
                        script[3] = "The Ashen wretches of legend are amassing beneath our feet!";

                        speaker[4] = "Theron";
                        leftspeaker[4] = true;
                        script[4] = "And if sentient kind is to survive the coming apocalypse-";

                        speaker[5] = "Theron";
                        leftspeaker[5] = true;
                        script[5] = "I must have the fighting spirits of each and every one of you!";

                        speaker[6] = "Theron";
                        leftspeaker[6] = true;
                        script[6] = "Forget the oaths made with monarch corpses!";

                        speaker[7] = "Theron";
                        leftspeaker[7] = true;
                        script[7] = "Lend me your blades!";

                        speaker[8] = "Theron";
                        leftspeaker[8] = true;
                        script[8] = "And let us unite in the coming apocalypse!";

                        totalLines = 9;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                    case 3:
                        speaker[0] = "Cmd. Agyrii";
                        leftspeaker[0] = false;
                        script[0] = "You've raided our kingdom,";

                        speaker[1] = "Cmd. Agyrii";
                        leftspeaker[1] = false;
                        script[1] = "in fear of legends?";

                        speaker[2] = "Theron";
                        leftspeaker[2] = true;
                        script[2] = "The warriors at the Bulwark lie in graves of snow!";

                        speaker[3] = "Theron";
                        leftspeaker[3] = true;
                        script[3] = "And they were not the first to succumb to the hordes...";

                        speaker[4] = "S. Officer";
                        leftspeaker[4] = false;
                        script[4] = "My brother wrote to me of demons he had seen while scouting.";

                        speaker[5] = "S. Officer";
                        leftspeaker[5] = false;
                        script[5] = "I thought he had lost his mind...";

                        speaker[6] = "Theron";
                        leftspeaker[6] = true;
                        script[6] = "Your leaders turned their eye to the horrors that befell your brethren.";

                        speaker[7] = "Theron";
                        leftspeaker[7] = true;
                        script[7] = "I urge all of you to not make the same mistake...";

                        speaker[8] = "Cmd. Agyrii";
                        leftspeaker[8] = false;
                        script[8] = "If what you say is true, then we have no choice in the matter.";

                        speaker[9] = "Cmd. Agyrii";
                        leftspeaker[9] = false;
                        script[9] = "Soldiers!";

                        speaker[10] = "Cmd. Agyrii";
                        leftspeaker[10] = false;
                        script[10] = "Stand down...";

                        totalLines = 11;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 15:
                if(decisionNum == 0)
                {
                    switch (instance)
                    {
                        case 1:
                            speaker[0] = "Theron";
                            leftspeaker[0] = true;
                            script[0] = "Ah! This is the rat I presume?";

                            speaker[1] = "Cmd. Agyrii";
                            leftspeaker[1] = false;
                            script[1] = "Don't give him too much credit...";

                            speaker[2] = "Theron";
                            leftspeaker[2] = true;
                            script[2] = "You have impressed me, Commander.";

                            totalLines = 3;
                            this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                            break;
                        case 2:
                            speaker[0] = "Theron";
                            leftspeaker[0] = true;
                            script[0] = "So-";

                            speaker[1] = "Theron";
                            leftspeaker[1] = true;
                            script[1] = "You're the ''messenger'' who deserted Fort Hammerfell?";

                            speaker[2] = "Theron";
                            leftspeaker[2] = true;
                            script[2] = "Corporal Tesdin, was it?";

                            speaker[3] = "Tesdin";
                            leftspeaker[3] = false;
                            script[3] = "I abandoned a sinking ship!";

                            speaker[4] = "Theron";
                            leftspeaker[4] = true;
                            script[4] = "As your own kin drowned before you.";

                            speaker[5] = "Tesdin";
                            leftspeaker[5] = false;
                            script[5] = "Please! I only did what I had-";

                            speaker[6] = "Theron";
                            leftspeaker[6] = true;
                            script[6] = "Silence!";

                            speaker[7] = "Theron";
                            leftspeaker[7] = true;
                            script[7] = playerName + ", you've more than repaid your debt.";

                            speaker[8] = "Theron";
                            leftspeaker[8] = true;
                            script[8] = "As a token of my gratitude,";

                            speaker[9] = "Theron";
                            leftspeaker[9] = true;
                            script[9] = "I will grant you his execution.";

                            speaker[10] = playerName;
                            leftspeaker[10] = false;
                            script[10] = "My Lord! I don't even know this man!";

                            speaker[11] = "Theron";
                            leftspeaker[11] = true;
                            script[11] = "Would you prefer he buy you dinner first?";

                            speaker[12] = "Theron";
                            leftspeaker[12] = true;
                            script[12] = "He abandoned you and hundreds of soldiers to be slaughtered...";

                            speaker[13] = "Theron";
                            leftspeaker[13] = true;
                            script[13] = "Death is a mercy to scum like him.";

                            totalLines = 14;
                            this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                            break;
                    }
                }
                else if(decisionNum == 1) // EVIL OPTION :: DECISION 1
                {
                    switch (instance)
                    {
                        case 1:
                            speaker[0] = "Tesdin";
                            leftspeaker[0] = false;
                            script[0] = "*Incoherent gurgling*";

                            speaker[1] = "Theron";
                            leftspeaker[1] = true;
                            script[1] = "Good riddance.";

                            speaker[2] = "Theron";
                            leftspeaker[2] = true;
                            script[2] = "You're a free spirit now Nova.";

                            speaker[3] = "Theron";
                            leftspeaker[3] = true;
                            script[3] = "May fortune smile upon your travels.";

                            speaker[4] = playerName;
                            leftspeaker[4] = false;
                            script[4] = "My lord...";

                            speaker[5] = playerName;
                            leftspeaker[5] = false;
                            script[5] = "I-";

                            speaker[6] = playerName;
                            leftspeaker[6] = false;
                            script[6] = "I want to stay and fight.";

                            speaker[7] = "Theron";
                            leftspeaker[7] = true;
                            script[7] = "Hhmm.";

                            speaker[8] = "Theron";
                            leftspeaker[8] = true;
                            script[8] = "You are quite the rebel Nova...";

                            speaker[9] = "Theron";
                            leftspeaker[9] = true;
                            script[9] = "Why would you risk your life in a losing battle?";

                            speaker[10] = "Theron";
                            leftspeaker[10] = true;
                            script[10] = "Has the taste of blood stained your thoughts?";

                            speaker[11] = playerName;
                            leftspeaker[11] = false;
                            script[11] = "If I truly am 'Nova',";

                            speaker[12] = playerName;
                            leftspeaker[12] = false;
                            script[12] = "then I must have a place in this fight!";

                            speaker[13] = playerName;
                            leftspeaker[13] = false;
                            script[13] = "I'd gladly risk a fight to end Skorje's conquest!";

                            speaker[14] = playerName;
                            leftspeaker[14] = false;
                            script[14] = "Whatever it takes to stop the planet from being burried in ash!";

                            speaker[15] = "Theron";
                            leftspeaker[15] = true;
                            script[15] = "I would call your words selfless,";

                            speaker[16] = "Theron";
                            leftspeaker[16] = true;
                            script[16] = "but Skorje surely intends to destroy you along with all sentient life...";

                            speaker[17] = "Theron";
                            leftspeaker[17] = true;
                            script[17] = "And the life of purpose you seek is not given;";

                            speaker[18] = "Theron";
                            leftspeaker[18] = true;
                            script[18] = "It is taken.";

                            speaker[19] = "Theron";
                            leftspeaker[19] = true;
                            script[19] = "While there is much to teach you for the struggle ahead,";

                            speaker[20] = "Theron";
                            leftspeaker[20] = true;
                            script[20] = "I can only show you as much as you are willing to give.";

                            speaker[21] = playerName;
                            leftspeaker[21] = false;
                            script[21] = "The blade is all I have left to offer, my lord...";

                            speaker[22] = "Theron";
                            leftspeaker[22] = true;
                            script[22] = "Then I shall take your blade into my service once more.";

                            speaker[23] = playerName;
                            leftspeaker[23] = false;
                            script[23] = "Thank you, my lord!";

                            speaker[24] = playerName;
                            leftspeaker[24] = false;
                            script[24] = "I will not fail you!";

                            speaker[25] = "Theron";
                            leftspeaker[25] = true;
                            script[25] = "Come my apprentice,";

                            speaker[26] = "Theron";
                            leftspeaker[26] = true;
                            script[26] = "We have little time to waste.";

                            totalLines = 27;
                            this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                            break;
                    }
                }
                else // GOOD OPTION :: DECISION 1
                {
                    switch (instance)
                    {
                        case 1:
                            speaker[0] = "Tesdin";
                            leftspeaker[0] = false;
                            script[0] = "Solar Sons above!";

                            speaker[1] = "Tesdin";
                            leftspeaker[1] = false;
                            script[1] = "Spare my wretched soul!";

                            speaker[2] = "Tesdin";
                            leftspeaker[2] = false;
                            script[2] = "I beg of you!";

                            speaker[3] = "Tesdin";
                            leftspeaker[3] = false;
                            script[3] = "Please don't kill me!";

                            speaker[4] = playerName;
                            leftspeaker[4] = true;
                            script[4] = "I won't do it...";

                            speaker[5] = playerName;
                            leftspeaker[5] = true;
                            script[5] = "His life is not mine to judge.";

                            speaker[6] = "Theron";
                            leftspeaker[6] = true;
                            script[6] = "Very well.";

                            speaker[7] = "Tesdin";
                            leftspeaker[7] = false;
                            script[7] = "Oh! bless your soul strang-";

                            speaker[8] = "Tesdin";
                            leftspeaker[8] = false;
                            script[8] = "AAHH!!";

                            speaker[9] = "Theron";
                            leftspeaker[9] = false;
                            script[9] = "His life was a burden that I will not force upon my people.";

                            speaker[10] = playerName;
                            leftspeaker[10] = false;
                            script[10] = "What happened to needing every sword you could muster?!";

                            speaker[11] = "Theron";
                            leftspeaker[11] = false;
                            script[11] = "I did not acheive my power by taking poor bets.";

                            speaker[12] = "Theron";
                            leftspeaker[12] = true;
                            script[12] = "If you want to win, you keep the cards you can work with-";

                            speaker[13] = "Theron";
                            leftspeaker[13] = true;
                            script[13] = "And burn the rest...";

                            speaker[14] = "Theron";
                            leftspeaker[14] = true;
                            script[14] = "You may not see it now, but you are the ace up my sleeve, Nova.";

                            speaker[15] = playerName;
                            leftspeaker[15] = false;
                            script[15] = "What is at stake that was so much more important than his life!";

                            speaker[16] = "Theron";
                            leftspeaker[16] = true;
                            script[16] = "I can only show you what you are willing to see.";

                            speaker[17] = playerName;
                            leftspeaker[17] = false;
                            script[17] = "Then show me everything!";

                            speaker[18] = playerName;
                            leftspeaker[18] = false;
                            script[18] = "I have nothing left...";

                            speaker[19] = playerName;
                            leftspeaker[19] = false;
                            script[19] = "No home. No family. No purpose.";

                            speaker[20] = "Theron";
                            leftspeaker[20] = true;
                            script[20] = "Come, child.";

                            speaker[21] = "Theron";
                            leftspeaker[21] = true;
                            script[21] = "I will share with you my knowledge,";

                            speaker[22] = "Theron";
                            leftspeaker[22] = true;
                            script[22] = "and together we might stand a chance against Skorje.";

                            speaker[23] = playerName;
                            leftspeaker[23] = false;
                            script[23] = "If your power can help me save others from senseless salughter,";

                            speaker[24] = playerName;
                            leftspeaker[24] = false;
                            script[24] = "then I will do as you ask-";

                            speaker[25] = playerName;
                            leftspeaker[25] = false;
                            script[25] = "my lord...";

                            totalLines = 26;
                            this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                            break;
                    }
                }
                break;
            case 16: 
                switch (instance)
                {
                    case 1:
                        speaker[0] = playerName;
                        leftspeaker[0] = true;
                        script[0] = "AAAAYYYYOOO!!!!";

                        speaker[1] = playerName;
                        leftspeaker[1] = false;
                        script[1] = "WAIT UP!";

                        totalLines = 2;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 17:
                switch (instance)
                {
                    case 1:
                        speaker[0] = playerName;
                        leftspeaker[0] = true;
                        script[0] = "Did your battery die or someth-";

                        speaker[1] = "Ayo";
                        leftspeaker[1] = true;
                        script[1] = "...";

                        totalLines = 2;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                    case 2:
                        speaker[0] = playerName;
                        leftspeaker[0] = true;
                        script[0] = "What in the calamitous cosmos is that?!";

                        speaker[1] = "Shino-Bot";
                        leftspeaker[1] = false;
                        script[1] = "*Apprisal* - My brothers and I are but humble servants of our master.";

                        speaker[2] = "Shino-Bot";
                        leftspeaker[2] = false;
                        script[2] = "*Recitation* - He has requested that we bring you to him alive;";

                        speaker[3] = "Shino-Bot";
                        leftspeaker[3] = false;
                        script[3] = "*Intimidation* - This will be less painful should you surrender now.";

                        speaker[4] = "Ayo";
                        leftspeaker[4] = true;
                        script[4] = "Well I would hate to disapoint,";

                        speaker[5] = "Ayo";
                        leftspeaker[5] = true;
                        script[5] = "but I am not familiar with you or your master.";

                        speaker[6] = "Shino-Bot";
                        leftspeaker[6] = false;
                        script[6] = "*Condescension* - And I am not familiar with a Ji-Jak bearing a Noble's blades.";

                        speaker[7] = "Shino-Bot";
                        leftspeaker[7] = false;
                        script[7] = "*Assimilation* - It seems we both have much to learn.";

                        speaker[8] = "Ayo";
                        leftspeaker[8] = true;
                        script[8] = "I will say this once on the occasion that you are mistaken.";

                        speaker[9] = "Ayo";
                        leftspeaker[9] = true;
                        script[9] = "Stand aside and you will not be harmed...";

                        speaker[10] = "Shino-Bot";
                        leftspeaker[10] = false;
                        script[10] = "*Invalid Operation: Insufficient Credentials*";

                        speaker[11] = "Shino-Bot";
                        leftspeaker[11] = false;
                        script[11] = "*Declaration* - Your lack of compliance has been recorded.";

                        totalLines = 12;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 18:
                switch (instance)
                {
                    case 1:
                        speaker[0] = playerName;
                        leftspeaker[0] = true;
                        script[0] = "Were those friends of yours?";

                        speaker[1] = "Ayo";
                        leftspeaker[1] = false;
                        script[1] = "I was going to ask you the same thing.";

                        speaker[2] = playerName;
                        leftspeaker[2] = true;
                        script[2] = "I hope their master won't be too upset about this.";

                        totalLines = 3;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                    case 2:
                        speaker[0] = "Cmd. Agyrii";
                        leftspeaker[0] = false;
                        script[0] = "Oh, it's you.";

                        speaker[1] = "Cmd. Agyrii";
                        leftspeaker[1] = false;
                        script[1] = "I thought something interesting had happened...";

                        speaker[2] = playerName;
                        leftspeaker[2] = true;
                        script[2] = "Oh, no!";

                        speaker[3] = playerName;
                        leftspeaker[3] = true;
                        script[3] = "Not us. Definitely not us.";

                        speaker[4] = "Ayo";
                        leftspeaker[4] = true;
                        script[4] = "Let us make way to your camp.";

                        speaker[5] = "Ayo";
                        leftspeaker[5] = true;
                        script[5] = "I'm sure you have much to tell us.";

                        totalLines = 6;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                    case 3:
                        speaker[0] = playerName;
                        leftspeaker[0] = true;
                        script[0] = "...";

                        speaker[1] = playerName;
                        leftspeaker[1] = true;
                        script[1] = "Is someone there?";

                        totalLines = 2;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                    case 4:
                        speaker[0] = playerName;
                        leftspeaker[0] = true;
                        script[0] = "Hmmm...";

                        totalLines = 1;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 19:
                switch (instance)
                {
                    case 1:
                        speaker[0] = "Cmd. Agyrii";
                        leftspeaker[0] = true;
                        script[0] = "Then they'll realize what kind of threat Skorje really is,";

                        speaker[1] = "Cmd. Agyrii";
                        leftspeaker[1] = true;
                        script[1] = "and we can win over my troops without fighting 'em.";

                        speaker[2] = "Ayo";
                        leftspeaker[2] = false;
                        script[2] = "Your plan is sound... but-";

                        speaker[3] = "Ayo";
                        leftspeaker[3] = false;
                        script[3] = "How do we get into the Raven's crypt?";

                        speaker[4] = playerName;
                        leftspeaker[4] = false;
                        script[4] = "Uhhh, guys?";

                        speaker[5] = "Cmd. Agyrii";
                        leftspeaker[5] = true;
                        script[5] = "There's got to be an entrance somewhere, right?";

                        speaker[6] = "Shino-Bot";
                        leftspeaker[6] = true;
                        script[6] = "*Interjection* - It appears my master underestimated your abilities.";

                        speaker[7] = "Shino-Bot";
                        leftspeaker[7] = true;
                        script[7] = "However, my corrective algorithms have taken this into account.";

                        speaker[8] = "Shino-Bot";
                        leftspeaker[8] = true;
                        script[8] = "*Proclamation* - Now prepare for detainment, flesh-sacks!";

                        totalLines = 9;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 20:
                switch (instance)
                {
                    case 1:
                        speaker[0] = "Ayo";
                        leftspeaker[0] = true;
                        script[0] = "How is this possible?!";

                        speaker[1] = "Cmd. Agyrii";
                        leftspeaker[1] = true;
                        script[1] = "Less talking, more fighting!";

                        speaker[2] = playerName;
                        leftspeaker[2] = false;
                        script[2] = "A little help over here!";

                        totalLines = 3;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                    case 2:
                        speaker[0] = "???";
                        leftspeaker[0] = true;
                        script[0] = "Back into your holes, worms!";

                        speaker[1] = "Cmd. Agyrii";
                        leftspeaker[1] = true;
                        script[1] = "Good gods!";

                        speaker[2] = "Cmd. Agyrii";
                        leftspeaker[2] = true;
                        script[2] = "Where did you learn to fight like that?!";

                        speaker[3] = "Ayo";
                        leftspeaker[3] = false;
                        script[3] = "Who are you, mysterious warrior?";

                        speaker[4] = "???";
                        leftspeaker[4] = true;
                        script[4] = "Name?! Naaame? Yes!";

                        speaker[5] = "???";
                        leftspeaker[5] = true;
                        script[5] = "Our name! It was taken! Thieved it!";

                        speaker[6] = "???";
                        leftspeaker[6] = true;
                        script[6] = "This creature seems to have once called itself 'Ikilik'...";

                        speaker[7] = playerName;
                        leftspeaker[7] = false;
                        script[7] = "Uuhhh, how many of you are in there?";

                        speaker[8] = "???";
                        leftspeaker[8] = true;
                        script[8] = "I can only control speech for so long, but I am Sir Zadrig of the 64th brigade.";

                        speaker[9] = "Cmd. Agyrii";
                        leftspeaker[9] = true;
                        script[9] = "64th? You're no soldier! Besides, they're all dead...";

                        speaker[10] = "Sir Zadrig";
                        leftspeaker[10] = true;
                        script[10] = "Not all of them I'm afraid...";

                        speaker[11] = "Sir Zadrig";
                        leftspeaker[11] = true;
                        script[11] = "Listen here adventurers, I wish to help you enter the crypt.";

                        speaker[12] = "Ayo";
                        leftspeaker[12] = false;
                        script[12] = "We've only just met you. How are we to trust your aid?";

                        speaker[13] = "Sir Zadrig";
                        leftspeaker[13] = true;
                        script[13] = "I am a knight, a peacekeeper, a nobleman! Or at least...";

                        speaker[14] = "Sir Zadrig";
                        leftspeaker[14] = true;
                        script[14] = "I was...";

                        speaker[15] = "Sir Zadrig";
                        leftspeaker[15] = true;
                        script[15] = "I haven't an ounce of indignity in my body!";

                        speaker[16] = "Ikilik";
                        leftspeaker[16] = true;
                        script[16] = "Because body not yours no more!";

                        speaker[17] = "Sir Zadrig";
                        leftspeaker[17] = true;
                        script[17] = "I'll have your tongue, beast!";

                        speaker[18] = "Cmd. Agyrii";
                        leftspeaker[18] = true;
                        script[18] = "There must be something valuable in that crypt.";

                        speaker[19] = "Sir Zadrig";
                        leftspeaker[19] = true;
                        script[19] = "Very valuable. And I know of a hidden passage!";

                        speaker[20] = "Ayo";
                        leftspeaker[20] = false;
                        script[20] = "Well, let us make haste then.";

                        speaker[21] = "Ikilik";
                        leftspeaker[21] = false;
                        script[21] = "Yes, Shiny! Good! Good! Go home! Yes!";

                        totalLines = 22;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
            case 21:
                switch(instance)
                {
                    case 1:
                        speaker[0] = "Sir Zadrig";
                        leftspeaker[0] = false;
                        script[0] = "Hurry now! It's just this way!";

                        totalLines = 1;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                    case 2:
                        speaker[0] = "Ikilik";
                        leftspeaker[0] = false;
                        script[0] = "Yes! Friends! Crypt, here! Yes!";

                        speaker[1] = "Cmd. Agyrii";
                        leftspeaker[1] = true;
                        script[1] = "'Hidden passage' huh?";

                        speaker[2] = playerName;
                        leftspeaker[2] = true;
                        script[2] = "Open sesame!";

                        totalLines = 3;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                    case 3:
                        speaker[0] = "Ikilik";
                        leftspeaker[0] = false;
                        script[0] = "Se- sa- me?";

                        speaker[1] = playerName;
                        leftspeaker[1] = true;
                        script[1] = "Uummm...";

                        speaker[2] = playerName;
                        leftspeaker[2] = true;
                        script[2] = "How are we supposed to open this?";

                        speaker[3] = "Ikilik";
                        leftspeaker[3] = false;
                        script[3] = "We search high in the clouds, and deep in the soil!";

                        speaker[4] = "Ikilik";
                        leftspeaker[4] = false;
                        script[4] = "She reject us all the same...";

                        speaker[5] = "Ayo";
                        leftspeaker[5] = true;
                        script[5] = "We are wasting time!";

                        speaker[6] = "Sir Zadrig";
                        leftspeaker[6] = false;
                        script[6] = "Give me one moment lads...";

                        totalLines = 7;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                    case 4:
                        speaker[0] = "Sir Zadrig";
                        leftspeaker[0] = false;
                        script[0] = "Ket'threi tul'gar!"; //Thy keep is kept!

                        speaker[1] = "Sir Zadrig";
                        leftspeaker[1] = false;
                        script[1] = "Ket'il tul'desh!"; //Thy will is done!

                        speaker[2] = "Ayo";
                        leftspeaker[2] = true;
                        script[2] = "Shadow-tongue! Here?! You fool!";

                        speaker[3] = playerName;
                        leftspeaker[3] = true;
                        script[3] = "Shadow-tongue?";

                        speaker[4] = "Cmd. Agyrii";
                        leftspeaker[4] = true;
                        script[4] = "Let's hope the spirits have grown hard of hearing...";

                        speaker[5] = "Sir Zadrig";
                        leftspeaker[5] = false;
                        script[5] = "Et koshgraw 'dhuul-"; //The restless await-

                        speaker[6] = "Sir Zadrig";
                        leftspeaker[6] = false;
                        script[6] = "et sagas sohl!"; //the eternal sun!

                        speaker[7] = "Ikilik";
                        leftspeaker[7] = true;
                        script[7] = "GYYYYAAAAHHH-HH-AHH-AAAAAAHHHH!!!";

                        totalLines = 8;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                    case 5:
                        speaker[0] = "Sir Zadrig";
                        leftspeaker[0] = false;
                        script[0] = "Right this way, chaps!"; 

                        speaker[1] = "Ayo";
                        leftspeaker[1] = true;
                        script[1] = "I have a very bad feeling about this.";

                        speaker[2] = "???";
                        leftspeaker[2] = false;
                        script[2] = "As you should...";

                        speaker[3] = "Cmd. Agyrii";
                        leftspeaker[3] = true;
                        script[3] = "Oathsworn! Look alive!!";

                        totalLines = 4;
                        this.GetComponent<Dialogue_Manager_C>().NewDialogue(totalLines, script, speaker, leftspeaker, script, usesPlayer);
                        break;
                }
                break;
        }
        // End of exposition dlg
    }

    IEnumerator Cutscene21(int action, int instance = 0)
    {
        switch (instance)
        {
            case 1:
                switch (action)
                {
                    case 0:
                        nextLevel = "Exposition_Scene22";
                        // Set next Level //
                        blackSq.GetComponent<FadeScript>().FadeColored(Color.black, new Color(0, 0, 0, 0), 0.5f);
                        playerMannequin.GetComponent<AnimationController>().SetCombatState(false);
                        playerMannequin.GetComponent<AnimationController>().PlayWalkAnim();
                        yield return new WaitForSeconds(0.75f);
                        StartCoroutine(NewDialogue(21, 1));
                        break;
                    case 2:
                        yield return new WaitForSeconds(0.5f);
                        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, playerMannequin.transform.position + new Vector3(500, 0, 0), 0.50f);
                        yield return new WaitForSeconds(0.70f);
                        speaker01.GetComponent<LerpScript>().LerpToPos(speaker01.transform.position, speaker01.transform.position + new Vector3(420, 0, 0), 0.50f);
                        yield return new WaitForSeconds(0.5f);
                        speaker02.GetComponent<LerpScript>().LerpToPos(speaker02.transform.position, speaker02.transform.position + new Vector3(490, 0, 0), 0.50f);
                        yield return new WaitForSeconds(0.35f);
                        playerMannequin.GetComponent<AnimationController>().PlayIdleAnim();
                        yield return new WaitForSeconds(0.85f);
                        cameraObj.GetComponent<LerpScript>().LerpToPos(cameraObj.transform.position, cameraObj.transform.position + new Vector3(250,0,0), 0.5f);
                        break;
                }
                break;  
            case 2:
                switch (action)
                {
                    case 0:
                        yield return new WaitForSeconds(2f);
                        StartCoroutine(NewDialogue(21, 2));
                        break;
                }
                break;
            case 3:
                switch (action)
                {
                    case 0:
                        speaker03.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                        yield return new WaitForSeconds(1f);
                        speaker03.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
                        yield return new WaitForSeconds(0.2f);
                        speaker03.GetComponent<LerpScript>().LerpToPos(speaker03.transform.position, speaker03.transform.position - new Vector3(30, 0, 0), 1.50f);
                        yield return new WaitForSeconds(1.5f);
                        StartCoroutine(NewDialogue(21, 3));
                        break;
                    case 4:
                        speaker03.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                        break;
                    case 6:
                        speaker03.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
                        break;
                    case 8:
                        yield return new WaitForSeconds(0.5f);
                        speaker03.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                        yield return new WaitForSeconds(1.5f);
                        StartCoroutine(NewDialogue(21, 4));
                        break;
                }
                break;
            case 4:
                switch (action)
                {
                    case 0:
                        yield return new WaitForSeconds(1.0f);
                        StartCoroutine(NewDialogue(21, 5));
                        break;
                    case 2:
                        cameraObj.GetComponent<CameraController>().ShakeCamera(1, true, 2.0f);
                        break;
                    case 3:
                        cameraObj.GetComponent<CameraController>().ShakeCamera(1, true, 2.0f);
                        break;
                    case 6:
                        cameraObj.GetComponent<CameraController>().ShakeCamera(5, true, 4.0f);
                        speaker05.transform.GetChild(0).gameObject.SetActive(false);
                        speaker05.transform.GetChild(1).gameObject.SetActive(false);
                        yield return new WaitForSeconds(1.0f);
                        speaker06.GetComponent<FadeScript>().FadeColored(clear, Color.black, 1.0f);
                        break;
                    case 7:
                        cameraObj.GetComponent<CameraController>().ShakeCamera(5, true, 4.0f);
                        // open the crypt doors!
                        for(int i = 2; i < 5; ++i)
                        {
                            speaker05.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        break;
                    case 9:
                        speaker06.GetComponent<FadeScript>().FadeColored(Color.black, clear, 1.0f);
                        yield return new WaitForSeconds(1.5f);
                        StartCoroutine(NewDialogue(21, 5));
                        break;
                }
                break;
            case 5:
                switch (action)
                {
                    case 3:
                        cameraObj.GetComponent<CameraController>().ShakeCamera(1, true, 3.0f);
                        // Fade in ghostly Oathsworn
                        speaker04.transform.GetChild(3).gameObject.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(clear, new Color(1,1,1, 0.35f));
                        yield return new WaitForSeconds(0.35f);
                        speaker04.transform.GetChild(4).gameObject.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(clear, new Color(1, 1, 1, 0.35f));
                        yield return new WaitForSeconds(1.35f);
                        speaker04.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(clear, new Color(1, 1, 1, 0.35f));
                        yield return new WaitForSeconds(0.75f);
                        speaker04.transform.GetChild(5).gameObject.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(clear, new Color(1, 1, 1, 0.35f));

                        cameraObj.GetComponent<CameraController>().ShakeCamera(2, true, 2.0f);
                        break;
                    case 5:
                        cameraObj.GetComponent<CameraController>().ShakeCamera(5, true, 4.0f);
                        speaker06.GetComponent<FadeScript>().FadeColored(clear, Color.black, 1.0f);
                        break;
                    case 7:
                        cameraObj.GetComponent<CameraController>().ShakeCamera(5, true, 4.0f);
                        break;
                    case 8:
                        yield return new WaitForSeconds(1.0f);
                        //speaker06.GetComponent<FadeScript>().FadeColored(Color.black, clear, 3.0f);
                        yield return new WaitForSeconds(1.0f);

                        break;
                    case 9:
                        //speaker06.GetComponent<FadeScript>().FadeColored(Color.black, Color.clear, 0.8f);
                        //blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.black, 4.0f);
                        yield return new WaitForSeconds(1.5f);
                        //blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.black, 2.8f);
                        actionsCompleted = true; //actions are completed
                        yield return new WaitForSeconds(1.5f);
                        StartCoroutine(LoadNextLv());
                        break;
                }
                break;
        }
        //////////////////
    }

    IEnumerator Cutscene20(int action, int instance = 0)
    {
        switch (instance)
        {
            case 1:
                switch (action)
                {
                    case 0:
                        nextLevel = "Exposition_Scene21";
                        // Set next Level //
                        blackSq.GetComponent<FadeScript>().FadeColored(Color.black, new Color(0, 0, 0, 0), 0.5f);
                        playerMannequin.GetComponent<AnimationController>().SetCombatState(true);
                        playerMannequin.GetComponent<AnimationController>().PlayIdleAnim();
                        speaker01.transform.GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 1);
                        StartCoroutine(NewDialogue(20, 1));
                        break;
                    case 4:
                        speaker05.transform.GetChild(1).gameObject.SetActive(true);
                        yield return new WaitForSeconds(0.3f);
                        speaker05.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.clear, Color.black, 4.0f);
                        yield return new WaitForSeconds(0.3f);
                        speaker05.transform.GetChild(1).gameObject.SetActive(false);
                        yield return new WaitForSeconds(0.5f);
                        speaker05.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.black, Color.clear, 4.0f);
                        yield return new WaitForSeconds(0.2f);
                        speaker06.transform.GetChild(0).gameObject.SetActive(true);
                        speaker04.transform.GetChild(5).gameObject.SetActive(false);
                        speaker05.transform.position -= new Vector3(400, -50, 0);
                        yield return new WaitForSeconds(0.35f);
                        speaker05.transform.GetChild(3).gameObject.SetActive(true);
                        yield return new WaitForSeconds(0.3f);
                        speaker05.transform.GetChild(3).gameObject.SetActive(false);
                        speaker05.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.clear, Color.black, 4.0f);
                        yield return new WaitForSeconds(0.5f);
                        speaker05.transform.GetChild(2).gameObject.SetActive(true);
                        speaker05.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.black, Color.clear, 4.0f);
                        speaker06.transform.GetChild(0).gameObject.SetActive(true);
                        speaker04.transform.GetChild(4).gameObject.SetActive(false);
                        speaker04.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(false);
                        yield return new WaitForSeconds(0.35f);
                        speaker06.transform.GetChild(1).gameObject.SetActive(true);
                        speaker04.transform.GetChild(6).gameObject.SetActive(false);
                        yield return new WaitForSeconds(0.35f);
                        speaker06.transform.GetChild(2).gameObject.SetActive(true);
                        speaker04.transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(false);
                        speaker04.transform.GetChild(3).gameObject.SetActive(false);
                        yield return new WaitForSeconds(0.5f);
                        speaker04.transform.GetChild(0).GetComponent<LerpScript>().LerpToPos(speaker04.transform.GetChild(0).transform.position, speaker04.transform.GetChild(0).transform.position + new Vector3(50, 0, 0), 4.0f);
                        speaker04.transform.GetChild(1).GetComponent<LerpScript>().LerpToPos(speaker04.transform.GetChild(1).transform.position, speaker04.transform.GetChild(1).transform.position - new Vector3(50, 0, 0), 4.0f);
                        speaker04.transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 2);
                        //yield return new WaitForSeconds(0.25f);
                        blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.white, 8f);
                        yield return new WaitForSeconds(0.5f);
                        speaker04.transform.GetChild(2).gameObject.SetActive(false);
                        blackSq.GetComponent<FadeScript>().FadeColored(Color.white, Color.clear, 4f);
                        break;
                }
                break;
            case 2:
                switch (action)
                {
                    case 0:
                        yield return new WaitForSeconds(3f);
                        speaker05.transform.position -= new Vector3(40, 50, 0);
                        speaker05.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.clear, Color.white, 4.0f);
                        speaker04.transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 0);

                        Vector3 spawnPos = new Vector3(speaker04.transform.GetChild(0).GetChild(0).transform.position.x + 20, speaker04.transform.GetChild(0).GetChild(0).transform.position.y + 130, 0);
                        GameObject effectClone = (GameObject)Instantiate(ExclamationPoint, spawnPos, transform.rotation);
                        Vector3 spawnPos2 = new Vector3(speaker04.transform.GetChild(1).GetChild(0).transform.position.x + 20, speaker04.transform.GetChild(1).GetChild(0).transform.position.y + 130, 0);
                        GameObject effectClone2 = (GameObject)Instantiate(ExclamationPoint, spawnPos2, transform.rotation);
                        StartCoroutine(NewDialogue(20, 2));
                        break;
                    case 1:
                        speaker04.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                        yield return new WaitForSeconds(0.15f);
                        speaker04.transform.GetChild(1).transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
                        yield return new WaitForSeconds(0.15f);
                        playerMannequin.GetComponent<AnimationController>().FlipFlop();
                        yield return new WaitForSeconds(0.15f);
                        playerMannequin.GetComponent<AnimationController>().SetCombatState(false);
                        break;
                    case 7:
                        speaker05.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                        break;
                    case 10:
                        speaker05.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
                        break;
                    case 12:
                        speaker05.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                        break;
                    case 17:
                        speaker05.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
                        break;
                    case 18:
                        speaker05.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                        break;
                    case 22:
                        actionsCompleted = true; //actions are completed
                        blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.black, 0.8f);
                        yield return new WaitForSeconds(1.5f);
                        StartCoroutine(LoadNextLv());
                        break;
                }
                break;
        }
        //////////////////
    }


    IEnumerator Cutscene19(int action, int instance = 0)
    {
        switch (instance)
        {
            case 1:
                switch (action)
                {
                    case 0:
                        nextLevel = "TurnCombat_Scene";
                        // Set next Level //
                        blackSq.GetComponent<FadeScript>().FadeColored(Color.black, new Color(0, 0, 0, 0), 0.5f);
                        playerMannequin.GetComponent<AnimationController>().SetCombatState(false);
                        playerMannequin.GetComponent<AnimationController>().PlayIdleAnim();
                        yield return new WaitForSeconds(1.5f);
                        StartCoroutine(NewDialogue(19, 1));
                        break;
                    case 4:
                        speaker03.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.clear, Color.white, 1.0f);
                        yield return new WaitForSeconds(1f);
                        Vector3 spawnPos = new Vector3(playerMannequin.transform.position.x + 20, playerMannequin.transform.position.y + 90, 0);
                        GameObject effectClone = (GameObject)Instantiate(ExclamationPoint, spawnPos, transform.rotation);
                        yield return new WaitForSeconds(0.5f);
                        playerMannequin.GetComponent<AnimationController>().SetCombatState(true);
                        break;
                    case 7:
                        speaker02.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                        Vector3 spawnPos1 = new Vector3(-61.0f, 13.0f, 0);
                        GameObject effectClone1 = (GameObject)Instantiate(ExclamationPoint, Vector3.zero, transform.rotation);
                        effectClone1.transform.SetParent(speaker02.transform);
                        effectClone1.transform.localPosition = spawnPos1;
                        effectClone1.transform.localScale = new Vector3(3,3,1);
                        break;
                    case 8:
                        speaker05.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.clear, Color.white, 1.0f);
                        speaker06.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.clear, Color.white, 1.0f);
                        break;
                    case 9:
                        speaker03.transform.GetChild(1).GetComponent<LerpScript>().LerpToColor(Color.clear, Color.white, 1.0f);
                        speaker03.transform.GetChild(2).GetComponent<LerpScript>().LerpToColor(Color.clear, Color.white, 1.0f);
                        speaker03.transform.GetChild(1).GetComponent<LerpScript>().LerpToPos(speaker03.transform.GetChild(1).transform.position, speaker03.transform.GetChild(1).transform.position + new Vector3(35, 25,0), 1.0f);
                        speaker03.transform.GetChild(2).GetComponent<LerpScript>().LerpToPos(speaker03.transform.GetChild(2).transform.position, speaker03.transform.GetChild(2).transform.position + new Vector3(-35, -25, 0), 1.0f);
                        break;
                    case 10:
                        speaker04.transform.GetChild(5).GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.clear, Color.white, 1.0f);
                        actionsCompleted = true; //actions are completed
                        yield return new WaitForSeconds(1f);
                        playerMannequin.GetComponent<AnimationController>().FlipFlop();
                        yield return new WaitForSeconds(0.75f);
                        StartCoroutine(LoadCombatScene(3, 1, true));
                        break;
                }
                break;
        }
        //////////////////
    }

    IEnumerator Cutscene18(int action, int instance = 0)
    {
        switch (instance)
        {
            case 1:
                switch (action)
                {
                    case 0:
                        nextLevel = "Exposition_Scene19";
                        // Set next Level //
                        blackSq.GetComponent<FadeScript>().FadeColored(Color.black, new Color(0, 0, 0, 0), 0.25f);
                        speaker01.transform.GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 1);
                        playerMannequin.GetComponent<AnimationController>().SetCombatState(true);
                        yield return new WaitForSeconds(1.0f);
                        speaker02.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.white, Color.clear, 3.0f);
                        yield return new WaitForSeconds(0.10f);
                        speaker01.transform.GetChild(1).gameObject.SetActive(true);
                        speaker03.SetActive(true);
                        blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.white, 3f);
                        yield return new WaitForSeconds(1.0f);
                        blackSq.GetComponent<FadeScript>().FadeColored(Color.white, Color.clear, 3f);
                        speaker01.transform.GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 2);
                        speaker01.GetComponent<LerpScript>().LerpToPos(speaker01.transform.position, speaker01.transform.position + new Vector3(400, 0,0), 7.0f);
                        yield return new WaitForSeconds(1.20f);
                        speaker01.transform.GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 3);
                        //ayo slash!
                        speaker04.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
                        speaker04.transform.GetChild(3).GetChild(1).gameObject.SetActive(true);
                        yield return new WaitForSeconds(1.0f);
                        speaker04.transform.GetChild(2).GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.white, Color.clear, 3.0f);
                        speaker04.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
                        speaker04.transform.GetChild(3).GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.white, Color.clear, 3.0f);
                        speaker04.transform.GetChild(3).GetChild(1).gameObject.SetActive(true);
                        yield return new WaitForSeconds(1.0f);
                        playerMannequin.GetComponent<AnimationController>().SetCombatState(false);
                        playerMannequin.GetComponent<AnimationController>().PlayIdleAnim();
                        playerMannequin.GetComponent<AnimationController>().FlipFlop();
                        yield return new WaitForSeconds(0.50f);
                        speaker01.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                        yield return new WaitForSeconds(0.50f);
                        speaker01.GetComponent<LerpScript>().LerpToPos(speaker01.transform.position, speaker01.transform.position + new Vector3(-200, 0, 0), 2.0f);
                        yield return new WaitForSeconds(2.50f);
                        StartCoroutine(NewDialogue(18, 1));
                        break;
                }
                break;
            case 2:
                switch (action)
                {
                    case 0:
                        speaker05.GetComponent<LerpScript>().LerpToPos(speaker05.transform.position, speaker05.transform.position + new Vector3(-100, 0, 0), 4.0f);
                        speaker01.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
                        yield return new WaitForSeconds(0.75f);
                        StartCoroutine(NewDialogue(18, 2));
                        break;
                    case 7:
                        speaker05.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
                        yield return new WaitForSeconds(0.5f);
                        speaker05.GetComponent<LerpScript>().LerpToPos(speaker05.transform.position, speaker05.transform.position + new Vector3(200, 0, 0), 2.0f);
                        yield return new WaitForSeconds(0.35f);
                        speaker01.GetComponent<LerpScript>().LerpToPos(speaker01.transform.position, speaker01.transform.position + new Vector3(320, 0, 0), 2.0f);
                        yield return new WaitForSeconds(0.75f);
                        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, playerMannequin.transform.position + new Vector3(300, 0, 0), 2.0f);
                        playerMannequin.GetComponent<AnimationController>().PlayWalkAnim();
                        yield return new WaitForSeconds(0.5f);
                        playerMannequin.GetComponent<AnimationController>().PlayIdleAnim();
                        break;
                }
                break;
            case 3:
                switch (action)
                {
                    case 0:
                        Vector3 spawnPos = new Vector3(playerMannequin.transform.position.x + 20, playerMannequin.transform.position.y + 90, 0);
                        GameObject effectClone = (GameObject)Instantiate(ExclamationPoint, spawnPos, transform.rotation);
                        playerMannequin.GetComponent<AnimationController>().FlipFlop();
                        yield return new WaitForSeconds(0.75f);
                        StartCoroutine(NewDialogue(18, 3));
                        break;
                }
                break;
            case 4:
                switch (action)
                {
                    case 0:
                        playerMannequin.GetComponent<AnimationController>().FlipFlop();
                        yield return new WaitForSeconds(0.75f);
                        playerMannequin.GetComponent<AnimationController>().FlipFlop();
                        yield return new WaitForSeconds(0.75f);
                        StartCoroutine(NewDialogue(18, 4));
                        break;
                    case 2:
                        playerMannequin.GetComponent<AnimationController>().FlipFlop();
                        yield return new WaitForSeconds(0.75f);
                        playerMannequin.GetComponent<AnimationController>().PlayWalkAnim();
                        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, playerMannequin.transform.position + new Vector3(300, 0, 0), 1.0f);
                        yield return new WaitForSeconds(1f);
                        actionsCompleted = true; //actions are completed
                        blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.black, 0.8f);
                        yield return new WaitForSeconds(1.5f);
                        StartCoroutine(LoadNextLv());
                        break;
                }
                break;
        }
    }

    IEnumerator Cutscene17(int action, int instance = 0)
    {
        switch (instance)
        {
            case 1:
                switch (action)
                {
                    case 0:
                        nextLevel = "TurnCombat_Scene";
                        // Set next Level //
                        blackSq.GetComponent<FadeScript>().FadeColored(Color.black, new Color(0, 0, 0, 0), 0.25f);
                        playerMannequin.GetComponent<AnimationController>().PlayWalkAnim();
                        yield return new WaitForSeconds(1.0f);
                        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, playerMannequin.transform.position + new Vector3(400, 0, 0), 0.75f);
                        yield return new WaitForSeconds(1.5f);
                        playerMannequin.GetComponent<AnimationController>().SetCombatState(false);
                        playerMannequin.GetComponent<AnimationController>().PlayIdleAnim();
                        yield return new WaitForSeconds(0.50f);
                        StartCoroutine(NewDialogue(17, 1));
                        break;
                    case 3:
                        Vector3 spawnPos = new Vector3(playerMannequin.transform.position.x + 20, playerMannequin.transform.position.y + 90, 0);
                        GameObject effectClone = (GameObject)Instantiate(ExclamationPoint, spawnPos, transform.rotation);
                        yield return new WaitForSeconds(1.50f);
                        cameraObj.GetComponent<LerpScript>().LerpToPos(cameraObj.transform.position, cameraObj.transform.position + new Vector3(280, 0, 0), 0.70f);
                        break;
                }
                break;
          case 2:
                switch(action)
                {
                    case 0:
                        
                        dialoguePanel.transform.position = dialoguePanel.transform.position + new Vector3(280, 0, 0);
                        yield return new WaitForSeconds(1.4f);
                        playerMannequin.GetComponent<AnimationController>().SetCombatState(true);
                        playerMannequin.GetComponent<AnimationController>().PlayIdleAnim();
                        StartCoroutine(NewDialogue(17, 2));
                        break;
                    case 9:
                        speaker01.transform.GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 1);
                        break;
                    case 13:
                        actionsCompleted = true; //actions are completed
                        yield return new WaitForSeconds(0.85f);
                        speaker04.transform.GetChild(2).GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.clear, Color.white, 1.0f);
                        speaker04.transform.GetChild(3).GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.clear, Color.white, 1.0f);
                        yield return new WaitForSeconds(1.0f);
                        speaker02.transform.GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 3);
                        yield return new WaitForSeconds(3.5f);
                        cameraObj.GetComponent<CameraController>().LerpCameraSize(225, 200, 0.40f);
                        blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.white, 1f);
                        yield return new WaitForSeconds(2.0f);
                        StartCoroutine(LoadCombatScene(3, 0, false));
                        break;
                }
                break;
        }
        //////////////////
    }

    IEnumerator Cutscene16(int action, int instance = 0)
    {
        switch (instance)
        {
            case 1:
                switch (action)
                {
                    case 0:
                        nextLevel = "Exposition_Scene17";
                        // Set next Level //
                        blackSq.GetComponent<FadeScript>().FadeColored(Color.black, new Color(0, 0, 0, 0), 0.5f);
                        playerMannequin.GetComponent<AnimationController>().PlayWalkAnim();
                        cameraObj.GetComponent<LerpScript>().LerpToPos(cameraObj.transform.position, cameraObj.transform.position - new Vector3(0, 70, 0), 0.40f);
                        yield return new WaitForSeconds(3.0f);
                        speaker01.GetComponent<LerpScript>().LerpToPos(speaker01.transform.position, speaker01.transform.position + new Vector3(1000, 0, 0), 1.0f);
                        yield return new WaitForSeconds(3.4f);
                        StartCoroutine(NewDialogue(16, 1));
                        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, playerMannequin.transform.position + new Vector3(350, 0, 0), 0.75f);
                        yield return new WaitForSeconds(1.4f);
                        playerMannequin.GetComponent<AnimationController>().SetCombatState(true);
                        playerMannequin.GetComponent<AnimationController>().PlayIdleAnim();
                        break;
                    case 3:
                        playerMannequin.GetComponent<AnimationController>().SetCombatState(false);
                        playerMannequin.GetComponent<AnimationController>().PlayWalkAnim();
                        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, playerMannequin.transform.position + new Vector3(600, 0, 0), 0.750f);
                        actionsCompleted = true; //actions are completed
                        yield return new WaitForSeconds(3f);
                        blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.black, 0.8f);
                        StartCoroutine(LoadNextLv());
                        break;
                }
                break;
        }
        //////////////////
    }

    IEnumerator Cutscene15e(int action) // EVIL OUTCOME
    {
        switch (action)
        {
            case 0:
                nextLevel = "MainMenu_Scene";
                //// Set next Level //
                cameraObj.GetComponent<CameraController>().LerpCameraSize(150, 120, 1.0f);
                //yield return new WaitForSeconds(0.50f);
                cameraObj.GetComponent<LerpScript>().LerpToPos(cameraObj.transform.position, cameraObj.transform.position + new Vector3(80,0,0), 1.0f);
                yield return new WaitForSeconds(2.0f);
                playerMannequin.GetComponent<AnimationController>().PlayHoldAttackAnim();
                playerMannequin.GetComponent<AnimationController>().SetCombatState(true);
                playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, playerMannequin.transform.position + new Vector3(60,0,0), 4.0f);
                speaker04.transform.GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 1);
                blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.white, 8.0f);
                yield return new WaitForSeconds(0.2f);
                blackSq.GetComponent<FadeScript>().FadeColored(Color.white, Color.clear, 8.0f);
                yield return new WaitForSeconds(1.6f);
                playerMannequin.GetComponent<AnimationController>().PlayIdleAnim();
                cameraObj.GetComponent<CameraController>().LerpCameraSize(120, 150, 2.0f);
                cameraObj.GetComponent<LerpScript>().LerpToPos(cameraObj.transform.position, cameraObj.transform.position - new Vector3(80, 0, 0), 2.0f);
                yield return new WaitForSeconds(2.0f);
                StartCoroutine(NewDialogue(15, 1, 1));
                break;
            case 4:
                playerMannequin.GetComponent<AnimationController>().FlipFlop();
                playerMannequin.GetComponent<AnimationController>().SetCombatState(false);
                break;
            case 27:
                actionsCompleted = true; //actions are completed
                yield return new WaitForSeconds(3f);
                blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.black, 0.8f);
                StartCoroutine(LoadNextLv());
                break;
        }
    }

    IEnumerator Cutscene15g(int action) //GOOD OUTCOME
    {
        switch (action)
        {
            case 0:
                nextLevel = "MainMenu_Scene";
                //// Set next Level //
                dialoguePanel.transform.position = dialoguePanel.transform.position + new Vector3(80,0,0);
                cameraObj.GetComponent<LerpScript>().LerpToPos(cameraObj.transform.position, cameraObj.transform.position + new Vector3(80, 0, 0), 0.65f);
                yield return new WaitForSeconds(2.0f);
                StartCoroutine(NewDialogue(15, 1, 2));
                break;
            case 9:
                speaker01.transform.GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 3);
                yield return new WaitForSeconds(1f);
                cameraObj.GetComponent<CameraController>().ShakeCamera(1, true, 2.0f);
                speaker04.GetComponent<LerpScript>().LerpToPos(speaker04.transform.position, speaker04.transform.position + new Vector3(0, 70, 0), 0.5f);
                speaker04.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.white, new Color(0.25f, 0.25f, 0.25f, 1.0f), 0.75f);
                yield return new WaitForSeconds(2f);
                speaker04.transform.GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 1);
                yield return new WaitForSeconds(0.2f);
                cameraObj.GetComponent<CameraController>().ShakeCamera(2, true, 0.80f);
                speaker06.GetComponent<ParticleSystem>().Play();
                yield return new WaitForSeconds(0.75f);
                speaker04.GetComponent<LerpScript>().LerpToPos(speaker04.transform.position, speaker04.transform.position - new Vector3(0, 70, 0), 6f);
                yield return new WaitForSeconds(1f);
                speaker01.transform.GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 0);
                yield return new WaitForSeconds(0.45f);
                Vector3 spawnPos = new Vector3(playerMannequin.transform.position.x + 20, playerMannequin.transform.position.y + 90, 0);
                GameObject effectClone = (GameObject)Instantiate(ExclamationPoint, spawnPos, transform.rotation);
                yield return new WaitForSeconds(1f);
                playerMannequin.GetComponent<AnimationController>().FlipFlop();
                break;
            case 26:
                actionsCompleted = true; //actions are completed
                yield return new WaitForSeconds(3f);
                blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.black, 0.8f);
                StartCoroutine(LoadNextLv());
                break;
        }
    }


    IEnumerator Cutscene15(int action, int instance = 0)
    {
        switch (instance)
        {
            case 1:
                switch (action)
                {
                    case 0:
                        nextLevel = "MainMenu_Scene";
                        // Set next Level //
                        playerMannequin.GetComponent<AnimationController>().SetCombatState(false);
                        blackSq.GetComponent<FadeScript>().FadeColored(Color.black, new Color(0, 0, 0, 0), 0.15f);
                        cameraObj.GetComponent<LerpScript>().LerpToPos(cameraObj.transform.position, cameraObj.transform.position + new Vector3(70, 0, 0), 0.40f);
                        yield return new WaitForSeconds(2.0f);
                        speaker04.GetComponent<LerpScript>().LerpToPos(speaker04.transform.position, speaker04.transform.position + new Vector3(-80, 0, 0), 0.50f);
                        speaker05.GetComponent<LerpScript>().LerpToPos(speaker05.transform.position, speaker05.transform.position + new Vector3(-80, 0, 0), 0.50f);
                        yield return new WaitForSeconds(2.4f);
                        
                        speaker05.GetComponent<LerpScript>().LerpToPos(speaker05.transform.position, speaker05.transform.position + new Vector3(-15, 0, 0), 2.50f);
                        yield return new WaitForSeconds(0.4f);
                        speaker04.GetComponent<LerpScript>().LerpToPos(speaker04.transform.position, speaker04.transform.position + new Vector3(-40, 0, 0), 2.50f);
                        
                        StartCoroutine(NewDialogue(15, 1));
                        break;
                }
                break;
            case 2:
                switch (action)
                {
                    case 0:
                        speaker05.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
                        yield return new WaitForSeconds(0.35f);
                        speaker05.GetComponent<LerpScript>().LerpToPos(speaker05.transform.position, speaker05.transform.position + new Vector3(140, 0, 0), 0.80f);
                        StartCoroutine(NewDialogue(15, 2));
                        break;
                    case 4:
                        speaker02.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
                        yield return new WaitForSeconds(0.80f);
                        speaker02.transform.GetChild(0).GetComponent<LerpScript>().LerpToPos(speaker02.transform.GetChild(0).transform.position, speaker02.transform.GetChild(0).transform.position + new Vector3(300,0,0), 0.5f);
                        yield return new WaitForSeconds(0.80f);
                        speaker02.transform.GetChild(1).GetComponent<LerpScript>().LerpToPos(speaker02.transform.GetChild(1).transform.position, speaker02.transform.GetChild(1).transform.position + new Vector3(300, 0, 0), 0.5f);
                        break;
                    case 11:
                        yield return new WaitForSeconds(0.20f);
                        playerMannequin.GetComponent<AnimationController>().FlipFlop();
                        Vector3 spawnPos = new Vector3(playerMannequin.transform.position.x + 20, playerMannequin.transform.position.y + 90, 0);
                        GameObject effectClone = (GameObject)Instantiate(ExclamationPoint, spawnPos, transform.rotation);
                        break;
                    case 15:
                        playerMannequin.GetComponent<AnimationController>().FlipFlop();
                        decisionManager.GetComponent<DecisionManager>().BeginDecision();
                        break;
                }
                break;
        }
        //////////////////
    }

    IEnumerator Cutscene14(int action, int instance = 0)
    {
        switch (instance)
        {
            case 1:
                switch (action)
                {
                    case 0:
                        nextLevel = "Exposition_Scene15";
                        // Set next Level //
                        playerMannequin.GetComponent<AnimationController>().SetCombatState(true);
                        speaker05.transform.GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 2);
                        speaker06.transform.GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 0);
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(0.75f, 0, 0, 1), new Color(0, 0, 0, 0), 0.15f);
                        cameraObj.GetComponent<LerpScript>().LerpToPos(origCameraPos, origCameraPos + new Vector3(-100, 0, 0), 0.35f);
                        yield return new WaitForSeconds(2.80f);
                        //speaker05.GetComponent<LerpScript>().LerpToPos(speaker05.transform.position, speaker05.transform.position + new Vector3(-250, 0, 0), 4.0f);
                        yield return new WaitForSeconds(0.80f);
                        StartCoroutine(NewDialogue(14, 1));
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 0), new Color(0, 0, 0, 0.15f), 0.15f);
                        cameraObj.GetComponent<CameraController>().ShakeCamera(3);
                        yield return new WaitForSeconds(0.80f);
                        speaker05.transform.GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 3);
                        Vector3 spawnPos;
                        foreach (SpriteRenderer child in speaker03.transform.GetComponentsInChildren<SpriteRenderer>())
                        {
                            spawnPos = new Vector3(child.transform.position.x + 20, child.transform.position.y + (5 * child.transform.localScale.y), 0);
                            GameObject effectClone = (GameObject)Instantiate(ExclamationPoint, spawnPos, transform.rotation);
                            effectClone.transform.localScale *= child.transform.localScale.y / 10.0f;
                            child.flipX = !child.flipX;
                        }
                        yield return new WaitForSeconds(0.5f);
                        spawnPos = new Vector3(playerMannequin.transform.position.x + 20, playerMannequin.transform.position.y + 120, 0);
                        GameObject effectClone1 = (GameObject)Instantiate(ExclamationPoint, spawnPos, transform.rotation);
                        playerMannequin.GetComponent<AnimationController>().FlipFlop();
                        yield return new WaitForSeconds(0.35f);
                        foreach (SpriteRenderer child in speaker02.transform.GetComponentsInChildren<SpriteRenderer>())
                        {
                            spawnPos = new Vector3(child.transform.position.x + 8, child.transform.position.y + 10 + (11 * child.transform.localScale.y), 0);
                            GameObject effectClone = (GameObject)Instantiate(ExclamationPoint, spawnPos, transform.rotation);
                            effectClone.transform.localScale *= child.transform.localScale.y / 10.0f;
                            child.flipX = !child.flipX;
                        }
                        break;
                }
                break;
            case 2:
                switch (action)
                {
                    case 0:
                        StartCoroutine(NewDialogue(14, 2));
                        yield return new WaitForSeconds(0.50f);
                        speaker01.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                        break;
                    case 8:
                        speaker04.transform.GetComponent<LerpScript>().LerpToPos(speaker04.transform.position, speaker04.transform.position - new Vector3(160, 0, 0), 1f);
                        yield return new WaitForSeconds(1.50f);
                        speaker04.transform.GetComponent<LerpScript>().LerpToPos(speaker04.transform.position, speaker04.transform.position - new Vector3(120, 0, 0), 0.2f);
                        break;
                }
                break;
            case 3:
                switch (action)
                {
                    case 0:
                        StartCoroutine(NewDialogue(14, 3));
                        break;
                    case 12:
                        yield return new WaitForSeconds(2f);
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 0.15f), Color.black, 0.8f);
                        actionsCompleted = true; //actions are completed
                        StartCoroutine(LoadNextLv());
                        break;
                }
                break;
        }
        //////////////////
    }

    IEnumerator Cutscene13(int action, int instance = 0)
    {
        switch (instance)
        {
            case 1:
                switch (action)
                {
                    case 0:
                        // Set next Level //
                        speaker02.transform.GetChild(0).GetComponent<Animator>().SetBool("InCombat", true);
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 0.15f);
                        yield return new WaitForSeconds(1.50f);
                        speaker01.transform.GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 1);
                        yield return new WaitForSeconds(1.0f);
                        speaker01.GetComponent<LerpScript>().LerpToPos(speaker01.transform.position, speaker01.transform.position + new Vector3(400, 0, 0), 8.0f);
                        speaker01.transform.GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 2);
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 8.0f);
                        yield return new WaitForSeconds(0.50f);
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 0.5f);
                        yield return new WaitForSeconds(0.25f);
                        speaker02.GetComponent<LerpScript>().LerpToPos(speaker02.transform.position, speaker02.transform.position + new Vector3(330, 0, 0), 4.0f);
                        speaker02.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.black, Color.white, 1.0f);
                        yield return new WaitForSeconds(0.70f);
                        speaker02.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.black, Color.clear, 3.0f);
                        yield return new WaitForSeconds(1.0f);
                        speaker02.SetActive(false);
                        speaker04.SetActive(true);
                        yield return new WaitForSeconds(0.30f);
                        speaker05.GetComponent<LerpScript>().LerpToPos(speaker05.transform.position, speaker05.transform.position + new Vector3(-250, 0, 0), 4.0f);
                        yield return new WaitForSeconds(1f);
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 0, 0, 0), Color.white, 1.0f);
                        actionsCompleted = true; //actions are completed
                        StartCoroutine(LoadCombatScene(2,1));
                        break;
                }
                break;
        }
        //////////////////
    }

    IEnumerator Cutscene12(int action, int instance = 0)
    {
        switch (instance)
        {
            case 1:
                switch (action)
                {
                    case 0:
                        nextLevel = "TurnCombat_Scene";
                        // Set next Level //
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 0.37f);
                        yield return new WaitForSeconds(3.0f);
                        speaker06.GetComponent<LerpScript>().LerpToPos(speaker06.transform.position, speaker06.transform.position + new Vector3(80, 0, 0), 1.0f);
                        yield return new WaitForSeconds(2.0f);
                        StartCoroutine(NewDialogue(12, 1));
                        break;
                    case 10:
                        Vector3 spawnPos = new Vector3(speaker06.transform.position.x + 20, speaker06.transform.position.y + 120, 0);
                        GameObject effectClone = (GameObject)Instantiate(ExclamationPoint, spawnPos, transform.rotation);
                        MusicManager.GetComponent<Music_Controller>().stopAllMusic();
                        break;
                    case 12:
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 8.0f);
                        yield return new WaitForSeconds(0.50f);
                        Destroy(speaker05.gameObject);
                        speaker04.SetActive(true);
                        playerMannequin.transform.position = new Vector3(446, -208, 0);
                        playerMannequin.GetComponent<AnimationController>().SetCombatState(true);
                        yield return new WaitForSeconds(2.0f);
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 0.2f);
                        break;
                }
                break;
            case 2:
                switch (action)
                {
                    case 0:
                        speaker04.transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 1);
                        speaker04.transform.GetChild(2).GetChild(0).GetComponent<Animator>().SetInteger("AnimState", -1);
                        sfxManager.GetComponent<SoundFXManager_C>().playSwordIgnite();
                        yield return new WaitForSeconds(1.5f);
                        StartCoroutine(NewDialogue(12, 2));
                        break;
                    case 2:
                        cameraObj.GetComponent<CameraController>().ShakeCamera(1, true, 2.5f);
                        speaker04.transform.GetChild(3).GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 7);
                        speaker04.transform.GetChild(3).GetChild(0).GetComponent<Animator>().speed = 0.1f;
                        sfxManager.GetComponent<SoundFXManager_C>().playDarkRumblingShort();
                        yield return new WaitForSeconds(1f);
                        speaker06.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.white, new Color(0.4f, 0.4f, 0.4f, 1.0f), 1.5f);
                        yield return new WaitForSeconds(1f);
                        speaker06.GetComponent<LerpScript>().LerpToPos(speaker06.transform.position, speaker06.transform.position + new Vector3(0, 30, 0), 0.5f);
                        yield return new WaitForSeconds(0.5f);
                        Vector3 ogPos = speaker06.transform.position;
                        speaker06.GetComponent<LerpScript>().LerpToPos(ogPos, ogPos + new Vector3(300, -36, 0), 8.5f);
                        speaker04.transform.GetChild(3).GetChild(0).GetComponent<Animator>().speed = 0.65f;
                        yield return new WaitForSeconds(0.75f);
                        ogPos = speaker06.transform.position;
                        speaker06.GetComponent<LerpScript>().LerpToPos(ogPos, ogPos + new Vector3(31.5f, 0, 0), 2.5f);
                        yield return new WaitForSeconds(0.75f);
                        sfxManager.GetComponent<SoundFXManager_C>().playSwordIgnite();
                        yield return new WaitForSeconds(2.25f);
                        sfxManager.GetComponent<SoundFXManager_C>().playSwordRetract();
                        speaker04.transform.GetChild(3).GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 8);
                        speaker04.transform.GetChild(3).GetChild(0).GetComponent<Animator>().speed = 1.5f;
                        yield return new WaitForSeconds(0.35f);
                        speaker06.GetComponent<LerpScript>().LerpToPos(speaker06.transform.position, speaker06.transform.position + new Vector3(-80, 0, 0), 5.0f);
                        //yield return new WaitForSeconds(0.15f);
                        speaker06.transform.GetChild(0).GetComponent<Animator>().SetBool("Dead", true);
                        speaker06.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(new Color(0.4f, 0.4f, 0.4f, 1.0f), new Color(0.8f, 0.8f, 0.8f, 1.0f), 1.5f);
                        sfxManager.GetComponent<SoundFXManager_C>().playBodyCollapse();
                        break;
                }
                break;
            case 3:
                switch(action)
                {
                    case 0:
                        yield return new WaitForSeconds(2.0f);
                        StartCoroutine(NewDialogue(12, 3));
                        break;
                    case 2:
                        yield return new WaitForSeconds(2.5f);
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 0.6f);
                        yield return new WaitForSeconds(2.5f);
                        actionsCompleted = true; //actions are completed
                        GameController.controller.levelsCompleted = 1;
                        StartCoroutine(LoadCombatScene(2,0, false));
                        break;
                }
                break;
        }
        //////////////////
    }

    IEnumerator Cutscene11(int action, int instance = 0)
    {
        switch (instance)
        {
            case 1:
                switch (action)
                {
                    case 0:
                        nextLevel = "Exposition_Scene12";
                        // Set next Level //
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 0.35f);
                        speaker01.GetComponent<LerpScript>().LerpToPos(speaker01.transform.position, speaker01.transform.position - new Vector3(70, 0, 0), 0.5f);
                        speaker02.GetComponent<LerpScript>().LerpToPos(speaker02.transform.position, speaker02.transform.position - new Vector3(70, 0, 0), 0.5f);
                        speaker03.GetComponent<LerpScript>().LerpToPos(speaker03.transform.position, speaker03.transform.position - new Vector3(70, 0, 0), 0.5f);
                        speaker04.GetComponent<LerpScript>().LerpToPos(speaker04.transform.position, speaker04.transform.position - new Vector3(70, 0, 0), 0.5f);
                        speaker05.GetComponent<LerpScript>().LerpToPos(speaker05.transform.position, speaker05.transform.position - new Vector3(70, 0, 0), 0.5f);
                        yield return new WaitForSeconds(2.0f);
                        speaker06.GetComponent<LerpScript>().LerpToPos(speaker06.transform.position, speaker06.transform.position + new Vector3(10, 0, 0), 2.0f);
                        StartCoroutine(NewDialogue(11, 1));
                        break;
                    case 11:
                        speaker06.GetComponent<LerpScript>().LerpToPos(speaker06.transform.position, speaker06.transform.position + new Vector3(5, 5, 0), 2.0f);
                        speaker06.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = -1;
                        yield return new WaitForSeconds(0.50f);
                        speaker01.GetComponent<LerpScript>().LerpToPos(speaker01.transform.position, speaker01.transform.position - new Vector3(500, 0, 0), 0.1f);
                        speaker02.GetComponent<LerpScript>().LerpToPos(speaker02.transform.position, speaker02.transform.position - new Vector3(500, 0, 0), 0.1f);
                        speaker03.GetComponent<LerpScript>().LerpToPos(speaker03.transform.position, speaker03.transform.position - new Vector3(500, 0, 0), 0.1f);
                        speaker04.GetComponent<LerpScript>().LerpToPos(speaker04.transform.position, speaker04.transform.position - new Vector3(500, 0, 0), 0.1f);
                        speaker05.GetComponent<LerpScript>().LerpToPos(speaker05.transform.position, speaker05.transform.position - new Vector3(500, 0, 0), 0.1f);
                        yield return new WaitForSeconds(3f);
                        speaker06.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                        yield return new WaitForSeconds(2f);
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), 0.75f);
                        actionsCompleted = true; //actions are completed
                        StartCoroutine(LoadNextLv());
                        break;
                }
                break;
        }
        //////////////////
    }

    IEnumerator Cutscene10(int action, int instance = 0)
    {
        switch (instance)
        {
            case 1:
                switch (action)
                {
                    case 0:
                        nextLevel = "Exposition_Scene11";
                        // Set next Level //
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 0.5f);
                        speaker01.GetComponent<LerpScript>().LerpToPos(speaker01.transform.position, speaker01.transform.position - new Vector3(900, 0, 0), 0.05f);
                        speaker02.GetComponent<LerpScript>().LerpToPos(speaker02.transform.position, speaker02.transform.position - new Vector3(900, 0, 0), 0.05f);
                        speaker03.GetComponent<LerpScript>().LerpToPos(speaker03.transform.position, speaker03.transform.position - new Vector3(900, 0, 0), 0.05f);
                        speaker04.GetComponent<LerpScript>().LerpToPos(speaker04.transform.position, speaker04.transform.position - new Vector3(900, 0, 0), 0.05f);
                        speaker05.GetComponent<LerpScript>().LerpToPos(speaker05.transform.position, speaker05.transform.position - new Vector3(900, 0, 0), 0.05f);
                        yield return new WaitForSeconds(7f);
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), 0.75f);
                        yield return new WaitForSeconds(2f);
                        actionsCompleted = true; //actions are completed
                        StartCoroutine(LoadNextLv());
                        break;
                }
                break;
        }
        //////////////////
    }

    IEnumerator Cutscene9(int action, int instance = 0)
    {
        switch (instance)
        {
            case 1:
                switch (action)
                {
                    case 0:
                        nextLevel = "Exposition_Scene10";
                        // Set next Level //
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 0.5f);
                        yield return new WaitForSeconds(2f);
                        speaker06.GetComponent<SpriteRenderer>().enabled = true;
                        speaker06.GetComponent<Animator>().enabled = true;
                        speaker06.GetComponent<LerpScript>().LerpToPos(speaker06.transform.position, speaker06.transform.position + new Vector3(0, 100, 0), 3.0f);
                        //yield return new WaitForSeconds(0.5f);
                        speaker02.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.clear, Color.white, 2.0f);
                        yield return new WaitForSeconds(2.0f);
                        StartCoroutine(NewDialogue(9, 1));
                        break;
                    case 8:
                        speaker01.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                        yield return new WaitForSeconds(0.85f);
                        speaker01.GetComponent<LerpScript>().LerpToPos(speaker01.transform.position, speaker01.transform.position + new Vector3(220, 0, 0), 1.0f);
                        break;
                }
                break;
            case 2:
                switch (action)
                {
                    case 0:
                        yield return new WaitForSeconds(0.5f);
                        speaker02.GetComponent<LerpScript>().LerpToPos(speaker02.transform.position, speaker02.transform.position + new Vector3(190, 0, 0), 1.0f);
                        yield return new WaitForSeconds(2.5f);
                        speaker02.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                        yield return new WaitForSeconds(0.75f);
                        StartCoroutine(NewDialogue(eCurrentCutscene, instance));
                        break;
                    case 6:
                        yield return new WaitForSeconds(2f);
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), 0.75f);
                        actionsCompleted = true; //actions are completed
                        StartCoroutine(LoadNextLv());
                        break;
                }
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
            case 21:
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
                        yield return new WaitForSeconds(3.5f);
                        playerMannequin.GetComponent<AnimationController>().PlayIdleAnim();
                        StartCoroutine(NewDialogue(7, 1));
                        break;
                    case 2:
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0.5f), Color.white, 1f);
                        yield return new WaitForSeconds(1f);
                        blackSq.GetComponent<FadeScript>().FadeColored(Color.white, new Color(1, 1, 1, 0.5f), 0.7f);
                        yield return new WaitForSeconds(0.5f);
                        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerInitPos + new Vector3(100, 0, 0), playerInitPos + new Vector3(120, 0, 0), 0.5f);
                        yield return new WaitForSeconds(1f);
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
                        yield return new WaitForSeconds(1.4f);
                        sfxManager.GetComponent<SoundFXManager_C>().playSnowSteps();
                        yield return new WaitForSeconds(1.25f);
                        speaker01.GetComponent<LerpScript>().LerpToPos(speaker01.transform.position, speaker01.transform.position - new Vector3(140, 0, 0), 0.25f);
                        yield return new WaitForSeconds(2.5f);
                        StartCoroutine(NewDialogue(eCurrentCutscene, instance));
                        break;
                    case 5:
                        actionsCompleted = true; //actions are completed
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
                nextLevel = "Exposition_Scene07";
                playerMannequin.GetComponent<AnimationController>().FlipFlop();
                playerMannequin.GetComponent<AnimationController>().SetCombatState(true);
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
                yield return new WaitForSeconds(0.1f);
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
                playerMannequin.GetComponent<AnimationController>().SetCombatState(false);
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
                nextLevel = "Tutorial_Scene01";
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
                playerMannequin.GetComponent<AnimationController>().SetCombatState(true);
                playerMannequin.GetComponent<AnimationController>().FlipFlop();
                speaker02.GetComponent<LerpScript>().LerpToPos(speaker02.transform.position, speaker02.transform.position + new Vector3(60, 0, 0), 1.0f);
                yield return new WaitForSeconds(1f);
                StartCoroutine(NewDialogue(5, 1));
                speaker03.GetComponent<LerpScript>().LerpToPos(speaker03.transform.position, speaker03.transform.position - new Vector3(100, 0, 0), 1.0f);
                sfxManager.GetComponent<SoundFXManager_C>().playSkitterScreech();
                yield return new WaitForSeconds(1f);
                speaker02.GetComponent<LerpScript>().LerpToPos(speaker02.transform.position, speaker02.transform.position + new Vector3(50, 0, 0));
                yield return new WaitForSeconds(0.75f);
                sfxManager.GetComponent<SoundFXManager_C>().playSkitterScreech();
                playerMannequin.GetComponent<AnimationController>().FlipFlop();
                break;
            case 2:
                yield return new WaitForSeconds(1f);
                actionsCompleted = true; //actions are completed
                StartCoroutine(LoadCombatScene(1, 0));
                break;
        }
    }

    IEnumerator Cutscene4(int action, int instance = 0)
    {
        // Set next Level //
        nextLevel = "Tutorial_Scene01";
        //////////////////
        switch (action)
        {
            case 0:
                blackSq.GetComponent<FadeScript>().FadeColored(Color.black, Color.clear, 1.2f);
                StartCoroutine(EnterScene(playerMannequin));
                yield return new WaitForSeconds(0.5f);
                playerMannequin.GetComponent<AnimationController>().PlayWalkAnim();
                yield return new WaitForSeconds(1.5f);
                playerMannequin.GetComponent<LerpScript>().LerpToPos(playerInitPos, playerInitPos + new Vector3(220, 0, 0), 1f);
                yield return new WaitForSeconds(1f);
                playerMannequin.GetComponent<AnimationController>().PlayIdleAnim();
                StartCoroutine(NewDialogue(4, 1));
                break;
            case 1:
                yield return new WaitForSeconds(1.5f);
                speaker01.GetComponent<LerpScript>().LerpToPos(speaker01.transform.position, speaker01.transform.position + new Vector3(-115, 0, 0), 0.5f);
                yield return new WaitForSeconds(1.7f);
                Vector3 spawnPos = new Vector3(playerMannequin.transform.position.x + 5, playerMannequin.transform.position.y + 90, 0);
                GameObject effectClone = (GameObject)Instantiate(ExclamationPoint, spawnPos, transform.rotation);
                yield return new WaitForSeconds(0.5f);
                playerMannequin.GetComponent<AnimationController>().SetCombatState(true);
                break;
            case 5:
                sfxManager.GetComponent<SoundFXManager_C>().playSkitterScreech();
                yield return new WaitForSeconds(0.7f);
                break;
            case 6:
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
                        yield return new WaitForSeconds(2f);
                        StartCoroutine(NewDialogue(3, 2));
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
                        yield return new WaitForSeconds(2f);
                        blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0.5f), Color.white, 0.6f);
                        sfxManager.GetComponent<SoundFXManager_C>().FadeVolume(1, 0, 0.7f, true);
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
                yield return new WaitForSeconds(1f);
                speaker02.GetComponent<SpriteRenderer>().enabled = true;
                speaker02.GetComponent<Animator>().enabled = true;
                yield return new WaitForSeconds(1f);
                blackSq.GetComponent<FadeScript>().FadeColored(Color.white, Color.clear, 1f);
                yield return new WaitForSeconds(2f);
                StartCoroutine(NewDialogue(2,1));
                break;
            case 4:
                yield return new WaitForSeconds(1f);
                sfxManager.GetComponent<SoundFXManager_C>().playLaserBombardment(true);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), Color.white, 4f);
                yield return new WaitForSeconds(0.2f);
                foreach(ParticleSystem child in speaker03.GetComponentsInChildren<ParticleSystem>())
                {
                    child.Play();
                }
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
                yield return new WaitForSeconds(2.5f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), Color.white, 0.35f);
                yield return new WaitForSeconds(1.5f);
                blackSq.GetComponent<FadeScript>().FadeColored(new Color(1,1,1,0.8f), Color.red, 0.75f);
                //sfxManager.GetComponent<SoundFXManager_C>().FadeVolume(1, 0, 1.5f, true);
                yield return new WaitForSeconds(1f);
                sfxManager.GetComponent<SoundFXManager_C>().FadeVolume(1, 0, 1.5f, false);
                blackSq.GetComponent<FadeScript>().FadeColored(Color.red, Color.black, 0.75f);
                yield return new WaitForSeconds(1.5f);
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

    public void DecisionCutscene(int cutscene, bool goodDecision)
    {
        if (goodDecision)
            decisionNumber = 2;
        else
            decisionNumber = 1;

        switch(cutscene)
        {
            case 15:
                if(goodDecision)
                    StartCoroutine(Cutscene15g(actionCounter));
                else
                    StartCoroutine(Cutscene15e(actionCounter));
                break;
        }
    }
}
