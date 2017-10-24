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

    private bool isLeftVisibile = false;
    private bool isRightVisibile = false;
    private Vector3 rightOrigPos;
    private Vector3 leftOrigPos;
    private string previousSpeaker;
    private Exposition_Manager expositionManager;
    private int total = 0;
    private int counter;
    private GameObject playerImage;

    // Use this for initialization
    void Start ()
    {
        expositionManager = this.GetComponent<Exposition_Manager>();
    }

    public void NewDialogue(int totalLines, string[] script, string[] speaker, bool[] isLeftSpeaker, string[] image, bool usesPlayer)
    {
        float waitTime = 0;
        float readTime = 1f;
        total = totalLines;
        previousSpeaker = "";
        dialogueBox.GetComponent<Text>().text = "";


        rightOrigPos = rightSpeaker.transform.position;
        leftOrigPos = leftSpeaker.transform.position;

        if (usesPlayer)
        {
            playerImage = Instantiate(playerCopy, Vector3.zero, Quaternion.identity) as GameObject;
            playerImage.transform.localScale = new Vector3(1,1,1);
        }

        for (int i = 0; i < totalLines; ++i)
        {
            if (i > 0)
                waitTime += (script[i - 1].Length * 0.095f) + (i * readTime);
            else
                waitTime = 1;
            // the previous waittime + length of typing + a reading buffer
            StartCoroutine(LoadNextLine(waitTime, script[i], speaker[i], isLeftSpeaker[i], image[i]));
        }
    }

    public IEnumerator LoadNextLine(float waitTime, string line, string speaker, bool isLeftSpeaker, string image)
    {
        //wait a long time to start the next dialogue
        yield return new WaitForSeconds(waitTime);

        if (previousSpeaker != speaker)
        {
            previousSpeaker = speaker;
            string imgSource = LookUpSpeakerIcon(speaker);

            if (isLeftSpeaker)
            {
                isLeftVisibile = true;
                leftImage.GetComponent<Image>().enabled = true;
                leftImage.GetComponent<Image>().sprite = Resources.Load(imgSource, typeof(Sprite)) as Sprite;
                leftImage.GetComponent<LerpScript>().LerpToColor(new Color(1, 1, 1, 0), Color.white, 2f);
                leftSpeaker.GetComponent<Text>().text = speaker;
                leftSpeaker.GetComponent<LerpScript>().LerpToColor(new Color(1, 1, 1, 0), Color.white, 2f);
                leftSpeaker.GetComponent<LerpScript>().LerpToPos(leftOrigPos, leftOrigPos + new Vector3(25, 0, 0), 1f);

                if (isRightVisibile)
                {
                    isRightVisibile = false;
                    rightImage.GetComponent<LerpScript>().LerpToColor(Color.white, new Color(1,1,1,0), 2f);
                    rightSpeaker.GetComponent<LerpScript>().LerpToColor(Color.white, new Color(1, 1, 1, 0), 2f);
                    rightSpeaker.GetComponent<LerpScript>().LerpToPos(rightOrigPos - new Vector3(25, 0, 0), rightOrigPos, 2f);
                }
            }
            else
            {
                isRightVisibile = true;
                rightImage.GetComponent<Image>().enabled = true;
                rightImage.GetComponent<Image>().sprite = Resources.Load(imgSource, typeof(Sprite)) as Sprite;
                rightImage.GetComponent<LerpScript>().LerpToColor(new Color(1, 1, 1, 0), Color.white, 2f);
                rightSpeaker.GetComponent<Text>().text = speaker;
                rightSpeaker.GetComponent<LerpScript>().LerpToColor(new Color(1, 1, 1, 0), Color.white, 2f);
                rightSpeaker.GetComponent<LerpScript>().LerpToPos(rightOrigPos, rightOrigPos - new Vector3(25, 0, 0), 1f);

                if (isLeftVisibile)
                {
                    isLeftVisibile = false;
                    leftImage.GetComponent<LerpScript>().LerpToColor(Color.white, new Color(1, 1, 1, 0), 5f);
                    leftSpeaker.GetComponent<LerpScript>().LerpToColor(Color.white, new Color(1, 1, 1, 0), 2f);
                    leftSpeaker.GetComponent<LerpScript>().LerpToPos(leftOrigPos + new Vector3(25, 0, 0), leftOrigPos, 2f);
                }
            }
        }

        dialogueBox.GetComponent<Text>().text = "";

        for (int i = 0; i < line.Length; ++i)
        {
            yield return new WaitForSeconds(0.01f);
            //typewriter effect
            dialogueBox.GetComponent<Text>().text = dialogueBox.GetComponent<Text>().text + line[i].ToString();
        }

        ++counter;

        if (counter >= total)
        {
            counter = 0;
            yield return new WaitForSeconds(1.5f);

            if(isLeftSpeaker)
            {
                leftSpeaker.GetComponent<LerpScript>().LerpToColor(Color.white, new Color(1, 1, 1, 0), 2f);
                leftImage.GetComponent<LerpScript>().LerpToColor(Color.white, new Color(1, 1, 1, 0), 2f);
                leftSpeaker.GetComponent<LerpScript>().LerpToPos(leftImage.transform.position, leftOrigPos, 2f);
            }
            else
            {
                rightSpeaker.GetComponent<LerpScript>().LerpToColor(Color.white, new Color(1, 1, 1, 0), 2f);
                rightImage.GetComponent<LerpScript>().LerpToColor(Color.white, new Color(1, 1, 1, 0), 2f);
                rightSpeaker.GetComponent<LerpScript>().LerpToPos(rightSpeaker.transform.position, rightOrigPos, 2f);
            }

            yield return new WaitForSeconds(1.5f);
            expositionManager.EndDialogue();
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
                iconString = "";
                break;
            case "Theron":
                iconString = "";
                break;
            case "Slade":
                iconString = "";
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
