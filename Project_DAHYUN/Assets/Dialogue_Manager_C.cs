using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class Dialogue_Manager_C : MonoBehaviour
{
    [HideInInspector]
    public float MEDIUM_VOLUME = 0.7f;

    public GameObject dialogueBox;
    public GameObject playerCopy;
    public GameObject rightSpeaker;
    public GameObject leftSpeaker;
    public GameObject rightImage;
    public GameObject leftImage;
    public GameObject tap2Continue;
    public AudioClip typing_SFX;

    private float fadeSpeed = 4.0f;
    private bool isLeftVisibile = false;
    private bool isRightVisibile = false;
    private Vector3 rightOrigPos;
    private Vector3 leftOrigPos;
    private string previousSpeaker;
    private Exposition_Manager expositionManager;
    private int total = 0;
    private int counter;
    private GameObject playerImage;

    private int dCurrentLine = 0;
    private int dTotalLines = 0;
    private string[] dScript;
    private string[] dSpeaker;
    private bool[] dIsLeftSpeaker;
    private Coroutine showTTC;
    private bool TTCisRunning = false;
    [HideInInspector]
    public bool typing = false;
    private IEnumerator typeRoutine;
    private int prevLineNum = -1;
    [HideInInspector]
    public bool dDialogueCompleted = true;
    private string playername;

    Sprite playerSprite;

    // Use this for initialization
    void Start()
    {
        playername = GameController.controller.playerName;
        expositionManager = this.GetComponent<Exposition_Manager>();
        string FilePath = Application.dataPath + "/Resources/CloseUps/Character_CloseUp_Player_" + GameController.controller.playerName + ".png";

        if (File.Exists(FilePath))
        {
            byte[] fileData = File.ReadAllBytes(FilePath);
            Texture2D tex = new Texture2D(2, 2);
            tex.filterMode = FilterMode.Point;
            tex.Apply();
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
            tex.filterMode = FilterMode.Point;
            tex.Apply();
            playerSprite = Sprite.Create(tex, new Rect(0.0F, 0.0F, tex.width, tex.height), new Vector2(0.5F, 0.5F), 52);
        }
        
    }

    public void NewDialogue(int totalLines, string[] script, string[] speaker, bool[] isLeftSpeaker, string[] image, bool usesPlayer)
    {
        dDialogueCompleted = false;
        dCurrentLine = 0;
        dTotalLines = totalLines;
        dScript = script;
        dSpeaker = speaker;
        dIsLeftSpeaker = isLeftSpeaker;
        prevLineNum = -1;

        previousSpeaker = "";
        dialogueBox.GetComponent<Text>().text = "";
        rightOrigPos = rightSpeaker.transform.position;
        leftOrigPos = leftSpeaker.transform.position;

        expositionManager.ready4Input = true;

        if (typeRoutine != null)
            StopCoroutine(typeRoutine);

        DialogueHandler();
    }

    public void DialogueHandler()
    {
        string speaker = dSpeaker[dCurrentLine];
        string line = dScript[dCurrentLine];
        bool isLeftSpeaker = dIsLeftSpeaker[dCurrentLine];

        tap2Continue.SetActive(false);

        //check if typing effect is active
        if (TTCisRunning)
        {
            StopCoroutine(showTTC);
            TTCisRunning = false;
        }

        //check for end of dialogue
        if (dCurrentLine >= dTotalLines)
        {
            dDialogueCompleted = true;
            tap2Continue.SetActive(false);
            dialogueBox.GetComponent<Text>().text = "";
            StopDialogue();
            return;
        }

        //check for new speaker
        if (previousSpeaker != speaker)
        {
            previousSpeaker = speaker;

            if (isLeftSpeaker)
            {
                SetLeftVisibile(true, dCurrentLine);

                if (isRightVisibile)
                    SetRightVisibile(false, dCurrentLine);
            }
            else
            {
                SetRightVisibile(true, dCurrentLine);

                if (isLeftVisibile)
                    SetLeftVisibile(false, dCurrentLine);
            }
        }

        dialogueBox.GetComponent<Text>().text = "";

        if (!typing)
        {
            typing = true;
            playTypingEffect();
            typeRoutine = TypeLine(line);
            StartCoroutine(typeRoutine);
        }
        else
            StartCoroutine(FastForward(line));
    }

    IEnumerator FastForward(string line)
    {
        StopCoroutine(typeRoutine);
        tap2Continue.SetActive(false);
        yield return new WaitForSeconds(0.01f);
        dialogueBox.GetComponent<Text>().text = line;
        StartCoroutine(DoneTyping(dCurrentLine));
    }

    IEnumerator TypeLine(string line)
    {
        for (int i = 0; i < line.Length; ++i)
        {
            yield return new WaitForSeconds(0.01f);
            //typewriter effect
            dialogueBox.GetComponent<Text>().text = dialogueBox.GetComponent<Text>().text + line[i].ToString();
        }

        StartCoroutine(DoneTyping(dCurrentLine));
    }

    IEnumerator DoneTyping(int lineNum)
    {
        if (lineNum > prevLineNum)
        {
            prevLineNum = lineNum;

            dCurrentLine = lineNum + 1; // finished a line in the script
            
            typing = false;
            expositionManager.NextAction();
            stopTypingEffect();

            yield return new WaitForSeconds(0.1f);

            if (!TTCisRunning)
            {
                TTCisRunning = true;

                showTTC =  StartCoroutine(showTouch2Continue());
            }
            else
            {
                StopCoroutine(showTTC);
                showTTC = StartCoroutine(showTouch2Continue());
            }
        }
        else
        {
            print("wtf is going on");
        }
    }

    IEnumerator showTouch2Continue()
    {
        Color origColor = tap2Continue.GetComponent<Text>().color;
        Color clearColor = new Color(origColor.r, origColor.g, origColor.b, 0);
        tap2Continue.GetComponent<Text>().color = clearColor;
        yield return new WaitForSeconds(3f);
        tap2Continue.SetActive(true);
        tap2Continue.GetComponent<LerpScript>().LerpToColor(clearColor, origColor, 4.5f);
        TTCisRunning = false;
    }


    public void StopDialogue()
    {
        StartCoroutine(DialogueFinished());
    }

    IEnumerator DialogueFinished()
    {
        expositionManager.NextAction();

        if(isLeftVisibile)
            SetLeftVisibile(false, dCurrentLine);
        else
            SetRightVisibile(false, dCurrentLine);

        yield return new WaitForSeconds(1f);
        expositionManager.EndDialogue();
    }

    public void SetRightVisibile(bool visible, int lineNum)
    {
        string speaker = dSpeaker[lineNum];
        string imgSource = LookUpSpeakerIcon(speaker);

        //show the right speaker
        if (visible)
        {
            isRightVisibile = true;
            rightImage.GetComponent<Image>().enabled = true;

            if (speaker == playername)
            {
                rightImage.GetComponent<Image>().sprite = playerSprite;
            }
            else
                rightImage.GetComponent<Image>().sprite = Resources.Load(imgSource, typeof(Sprite)) as Sprite;

            rightImage.GetComponent<LerpScript>().LerpToColor(new Color(1, 1, 1, 0), Color.white, fadeSpeed);
            rightSpeaker.GetComponent<Text>().text = speaker;
            rightSpeaker.GetComponent<LerpScript>().LerpToColor(new Color(1, 1, 1, 0), Color.white, fadeSpeed);
            rightSpeaker.GetComponent<LerpScript>().LerpToPos(rightOrigPos, rightOrigPos - new Vector3(25, 0, 0), fadeSpeed);
        }
        else // hide right speaker
        {
            isRightVisibile = false;
            rightImage.GetComponent<LerpScript>().LerpToColor(Color.white, new Color(1, 1, 1, 0), fadeSpeed);
            rightSpeaker.GetComponent<LerpScript>().LerpToColor(Color.white, new Color(1, 1, 1, 0), fadeSpeed);
            rightSpeaker.GetComponent<LerpScript>().LerpToPos(rightOrigPos - new Vector3(25, 0, 0), rightOrigPos, fadeSpeed);
        }
    }

    public void SetLeftVisibile(bool visibile, int lineNum)
    {
        string speaker = dSpeaker[lineNum];
        string imgSource = LookUpSpeakerIcon(speaker);

        if (visibile)
        {
            isLeftVisibile = true;
            leftImage.GetComponent<Image>().enabled = true;

            if (speaker == playername)
            {
                leftImage.GetComponent<Image>().sprite = playerSprite;
            }
            else
                leftImage.GetComponent<Image>().sprite = Resources.Load(imgSource, typeof(Sprite)) as Sprite;

            leftImage.transform.localScale = new Vector3(-1, 1, 0);
            leftImage.GetComponent<LerpScript>().LerpToColor(new Color(1, 1, 1, 0), Color.white, fadeSpeed);
            leftSpeaker.GetComponent<Text>().text = speaker;
            leftSpeaker.GetComponent<LerpScript>().LerpToColor(new Color(1, 1, 1, 0), Color.white, fadeSpeed);
            leftSpeaker.GetComponent<LerpScript>().LerpToPos(leftOrigPos, leftOrigPos + new Vector3(25, 0, 0), fadeSpeed);
        }
        else
        {
            isLeftVisibile = false;
            leftImage.GetComponent<LerpScript>().LerpToColor(Color.white, new Color(1, 1, 1, 0), fadeSpeed);
            leftSpeaker.GetComponent<LerpScript>().LerpToColor(Color.white, new Color(1, 1, 1, 0), fadeSpeed);
            leftSpeaker.GetComponent<LerpScript>().LerpToPos(leftOrigPos + new Vector3(25, 0, 0), leftOrigPos, fadeSpeed);
        }
    }

    public string LookUpSpeakerIcon(string speaker)
    {
        string iconString = "";

        if (speaker == playername)
            speaker = "Player";

        switch(speaker)
        {
            case "Player":
                iconString = "CloseUps\\Character_CloseUp_Player_" + playername;
                break;
            case "???":
                iconString = "CloseUps\\Character_CloseUp_Unknown";
                break;
            case "Theron":
                iconString = "CloseUps\\Character_CloseUp_Theron";
                break;
            case "Slade":
                iconString = "CloseUps\\Character_CloseUp_Slade";
                break;
            case "Not Steve":
                iconString = "CloseUps\\Character_CloseUp_Slade";
                break;
            case "Hyun":
                iconString = "CloseUps\\Character_CloseUp_Seamstress";
                break;
            case "Ayo":
                iconString = "CloseUps\\Character_CloseUp_Ayo";
                break;
            case "Skorje":
                iconString = "";
                break;
            case "Cmd. Agyrii":
                iconString = "CloseUps\\Character_CloseUp_Agyrii";
                break;
            case "Ikilik":
                iconString = "CloseUps\\Character_CloseUp_Ikilik";
                break;
            case "Shino-Bot":
                iconString = "CloseUps\\Character_CloseUp_ShinoBot";
                break;
            case "Tesdin":
                iconString = "CloseUps\\Character_CloseUp_Tesdin";
                break;
            case "Jes":
                iconString = "";
                break;
            case "Stan":
                iconString = "";
                break;
            case "Steve":
                iconString = "CloseUps\\Character_CloseUp_Steve";
                break;
            case "Cmd. Vixon":
                iconString = "CloseUps\\Character_CloseUp_GeneralVixon";
                break;
            case "H. Officer":
                iconString = "CloseUps\\Character_CloseUp_HammerfellOfficer";
                break;
            case "Honor Guard":
                iconString = "CloseUps\\Character_CloseUp_HammerfellKnight";
                break;
            case "S. Officer":
                iconString = "CloseUps\\Character_CloseUp_SolarisOfficer";
                break;
            case "S. Knight":
                iconString = "CloseUps\\Character_CloseUp_SolarisKnight";
                break;
            case "Skitter":
                iconString = "CloseUps\\Character_CloseUp_Skitter";
                break;
            case "King Gerard":
                iconString = "CloseUps\\Character_CloseUp_KingGerard";
                break;
            default:
                iconString = "CloseUps\\Character_CloseUp_Unknown";
                break;
        }

        return iconString;
    }

    public void playTypingEffect()
    {
        this.GetComponent<AudioSource>().PlayOneShot(typing_SFX, MEDIUM_VOLUME);
    }

    private void stopTypingEffect()
    {
        this.GetComponent<AudioSource>().Stop();
    }
}
