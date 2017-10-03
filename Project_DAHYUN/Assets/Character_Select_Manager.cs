using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Character_Select_Manager : MonoBehaviour
{
    public GameObject checkForDeletePanel;
    public GameObject characterMannequin;
    public GameObject blackSq;

    private int selectedChar = 0;

    private void Awake()
    {
        //if this is the first character made
        if (GameController.controller.numChars != 0)
        {
            GameController.controller.LoadCharacters();
        }
        else
        {
        }
    }

    // Use this for initialization
    void Start ()
    {
        LoadDefaultCharacter();
        HideDeleteCheck();
        //RefreshAccountData();
    }

    public void CreateCharacter()
    {
        print(selectedChar);
        if (checkForExistingChars(selectedChar))
        {
            GameController.controller.CharacterSlot = selectedChar;
            blackSq.GetComponent<FadeScript>().FadeIn(3.0f);
            SceneManager.LoadScene("NewCharacter_Scene");
        }
    }

    public bool checkForExistingChars(int charNum)
    {
        if (charNum == 0)
            return false;

        if(GameController.controller.charNames[charNum] == null)
        {
            print(charNum + " exists");
            return true;
        }
        return false;
    }

    public void CheckForDelete()
    {
        checkForDeletePanel.GetComponent<Image>().enabled = true;
        foreach (Image child in checkForDeletePanel.GetComponentsInChildren<Image>())
        {
            child.enabled = true;
        }

        foreach (Text child in checkForDeletePanel.GetComponentsInChildren<Text>())
        {
            child.GetComponent<Text>().enabled = true;
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
        }

        foreach (Text child in checkForDeletePanel.GetComponentsInChildren<Text>())
        {
            child.GetComponent<Text>().enabled = false;
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

    public void LoadDefaultCharacter()
    {
        for(int i = 0; i < 8; ++i)
        {
            characterMannequin.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
        }

        characterMannequin.transform.GetChild(12).GetComponent<SpriteRenderer>().enabled = false;

        for (int i = 8; i < 12; ++i)
        {
            characterMannequin.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
            characterMannequin.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void CharacterSelected(int charNum)
    {
        selectedChar = charNum;
        print(GameController.controller.numChars);
        if(charNum > GameController.controller.numChars)
            LoadDefaultCharacter();
        else
            LoadCharacterPreview(selectedChar);
    }

    public void LoadCharacterPreview(int charNum)
    {
        GameController.controller.Load(GameController.controller.charNames[charNum]);

        print(GameController.controller.charNames[charNum]);

        for (int i = 8; i < 12; ++i)
        {
            characterMannequin.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
            characterMannequin.transform.GetChild(i).GetComponent<SpriteRenderer>().color = getPlayerColor();
        }
        
        characterMannequin.GetComponent<AnimationController>().LoadCharacter();
    }

    public Color getPlayerColor()
    {
        Color player_C = new Color(GameController.controller.playerColorPreference[0], 
            GameController.controller.playerColorPreference[1], 
            GameController.controller.playerColorPreference[2]);
        return player_C;
    }
}
