using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Dialogue_Manager_C : MonoBehaviour
{
    public GameObject dialogueBox;
    public GameObject playerCopy;

    private Exposition_Manager expositionManager;
    private int total = 0;
    private int counter;
    private GameObject playerImage;

    // Use this for initialization
    void Start ()
    {
        expositionManager = this.GetComponent<Exposition_Manager>();
	}

    public void NewDialogue(int totalLines, string[] script, string[] speaker, bool[] leftSpeaker, string[] image, bool usesPlayer)
    {
        float waitTime = 0;
        total = totalLines;

        if(usesPlayer)
        {
            playerImage = Instantiate(playerCopy, Vector3.zero, Quaternion.identity) as GameObject;
            playerImage.transform.localScale = new Vector3(1,1,1);
        }

        for (int i = 0; i < totalLines; ++i)
        {
            if (i > 0)
                waitTime += (script[i - 1].Length * 0.09f) + 1f;
            else
                waitTime = 1;
            // the previous waittime + length of typing + a reading buffer
            StartCoroutine(LoadNextLine(waitTime, script[i], speaker[i], leftSpeaker[i], image[i]));
        }
    }

    public IEnumerator LoadNextLine(float waitTime, string line, string speaker, bool leftSpeaker, string image)
    {
        //wait a long time to start the next dialogue
        yield return new WaitForSeconds(waitTime);

        dialogueBox.GetComponent<Text>().text = "";
        for (int i = 0; i < line.Length; ++i)
        {
            yield return new WaitForSeconds(0.02f);
            //typewriter effect
            dialogueBox.GetComponent<Text>().text = dialogueBox.GetComponent<Text>().text + line[i].ToString();
        }

        ++counter;

        if (counter >= total)
        {
            yield return new WaitForSeconds(1.75f);
            dialogueBox.GetComponent<Text>().text = "";
            expositionManager.EndDialogue();
        }   
    }
}
