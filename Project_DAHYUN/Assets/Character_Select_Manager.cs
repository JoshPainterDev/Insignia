using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Character_Select_Manager : MonoBehaviour
{
    public GameObject checkForDeletePanel;

    private int selectedChar = 0;

    // Use this for initialization
    void Start ()
    {
        HideDeleteCheck();
	}

    public void CharacterSelected(int charNum)
    {
        selectedChar = charNum;
    }

    public void CheckForDelete()
    {
        checkForDeletePanel.GetComponent<Image>().enabled = true;
        foreach (Image child in checkForDeletePanel.GetComponentsInChildren<Image>())
        {
            child.enabled = true;
            child.gameObject.GetComponent<Text>().enabled = true;
        }

        foreach (Text child in checkForDeletePanel.GetComponentsInChildren<Text>())
        {
            child.enabled = true;
        }
    }

    public void HideDeleteCheck()
    {
        checkForDeletePanel.GetComponent<Image>().enabled = false;
        foreach (Image child in checkForDeletePanel.GetComponentsInChildren<Image>())
        {
            child.enabled = false;
            child.gameObject.GetComponent<Text>().enabled = false;
        }

        foreach (Text child in checkForDeletePanel.GetComponentsInChildren<Text>())
        {
            child.enabled = false;
        }
    }

    public void DeleteCharacter()
    {
        //selectedChar
        switch(selectedChar)
        {
            case 1:
                print("poof, hes gone");
                break;
            case 2:
                print("poof, hes gone");
                break;
            case 3:
                print("poof, hes gone");
                break;
        }
    }
}
