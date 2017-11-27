using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Dialogue_Manager_C : MonoBehaviour
{
    public GameObject dialogueManager;
    public GameObject dialogueBox;
    public GameObject playerCopy;
    public GameObject rightSpeaker;
    public GameObject leftSpeaker;
    public GameObject rightImage;
    public GameObject leftImage;
    public GameObject tap2Continue;

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

    //NEW SHIT
    private int dCurrentLine = 0;
    private int dTotalLines = 0;
    private string[] dScript;
    private string[] dSpeaker;
    private bool[] dIsLeftSpeaker;
    [HideInInspector]
    public bool typing = false;
    private IEnumerator typeRoutine;
    private int prevLineNum = -1;
    [HideInInspector]
    public bool dDialogueCompleted = true;

    // Use this for initialization
    void Start()
    {
        expositionManager = this.GetComponent<Exposition_Manager>();
    }

    public void NewDialogue(int totalLines, string[] script, string[] speaker, bool[] isLeftSpeaker, string[] image, bool usesPlayer)
    {
        dDialogueCompleted = false;
        dCurrentLine = 0;
        dTotalLines = totalLines;
        dScript = script;
        dSpeaker = speaker;
        dIsLeftSpeaker = isLeftSpeaker;

        previousSpeaker = "";
        dialogueBox.GetComponent<Text>().text = "";
        rightOrigPos = rightSpeaker.transform.position;
        leftOrigPos = leftSpeaker.transform.position;

        expositionManager.ready4Input = true;
        DialogueHandler();
    }

    public void DialogueHandler()
    {
        string speaker = dSpeaker[dCurrentLine];
        string line = dScript[dCurrentLine];
        bool isLeftSpeaker = dIsLeftSpeaker[dCurrentLine];

        tap2Continue.SetActive(false);

        if (dCurrentLine >= dTotalLines)
        {
            print("current: " + dCurrentLine);
            //expositionManager.NextActon();
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

            yield return new WaitForSeconds(3f);
            tap2Continue.SetActive(true);
            tap2Continue.GetComponent<FadingScript>().Restart();
            tap2Continue.transform.GetChild(0).GetComponent<FadingScript>().Restart();
        }
    }

    public void StopDialogue()
    {
        StartCoroutine(DialogueFinished());
    }

    IEnumerator DialogueFinished()
    {
        expositionManager.NextAction();

        SetLeftVisibile(false, dCurrentLine);
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

        switch(speaker)
        {
            case "Player":
                iconString = "";
                break;
            case "???":
                print("nigguh we made it");
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
            case "Ayo":
                iconString = "";
                break;
            case "Skorge":
                iconString = "";
                break;
            case "Myra":
                iconString = "";
                break;
            case "Tesdin":
                iconString = "";
                break;
            case "Jes":
                iconString = "";
                break;
            case "Stan":
                iconString = "";
                break;
            case "Steve":
                iconString = "";
                break;
            case "Honor Guard":
                iconString = "";
                break;
        }

        return iconString;
    }
}
