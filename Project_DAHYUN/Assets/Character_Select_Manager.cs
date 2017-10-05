using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Character_Select_Manager : MonoBehaviour
{
    public GameObject playerMannequin;
    public GameObject checkForDeletePanel;
    public GameObject characterMannequin;
    public GameObject charSelectPrefab;
    public GameObject gridThing;
    public GameObject blackSq;
    public GameObject deleteButton;

    const int MAX_CHARACTERS = 6;

    private int selectedChar = 0;
    private int charToDelete = 0;

    private void Awake()
    {
        //if this is the first character made
        if (GameController.controller.numChars != 0)
        {
            GameController.controller.LoadCharacters();
        }
    }

    // Use this for initialization
    void Start ()
    {
        LoadDefaultCharacter();
        HideDeleteCheck();
        RefreshCharacterList();
    }

    public void RefreshCharacterList()
    {
        for (int i = 0; i < charSelectPrefab.transform.childCount; ++i)
        {
            string comp = GameController.controller.charNames[i + 1];
            if (comp == null || comp == "")
            {
                gridThing.transform.GetChild(i).GetChild(0).GetComponentInChildren<Text>().text = "Empty Slot";
            }
            else
            {
                print(GameController.controller.charNames[i + 1]);
                gridThing.transform.GetChild(i).GetChild(0).GetComponentInChildren<Text>().text = GameController.controller.charNames[i + 1];
            }
        }
    }

    public void CreateCharacter()
    {
        //print("number of existing characters: " + GameController.controller.numChars);

        if (GameController.controller.numChars < MAX_CHARACTERS)
        {
            GameController.controller.CharacterSlot = selectedChar;
            blackSq.GetComponent<FadeScript>().FadeIn(3.0f);
            SceneManager.LoadScene("NewCharacter_Scene");
        }
    }

    public void CheckForDelete()
    {
        charToDelete = selectedChar;

        print(GameController.controller.charNames[selectedChar]);
        if (GameController.controller.charNames[selectedChar] == null)
        {
            GameController.controller.GetComponent<MenuUIAudio>().playNope();
            return;
        }

        checkForDeletePanel.GetComponent<Image>().enabled = true;
        foreach (Image child in checkForDeletePanel.GetComponentsInChildren<Image>())
        {
            child.enabled = true;
        }

        foreach (Text child in checkForDeletePanel.GetComponentsInChildren<Text>())
        {
            child.enabled = true;
        }

        foreach (Text child in checkForDeletePanel.GetComponentsInChildren<Text>())
        {
            child.enabled = true;
        }

        checkForDeletePanel.transform.GetChild(1).GetComponent<Text>().text = "Delete " + GameController.controller.charNames[charToDelete] + "?";
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
        if(charToDelete != 0 && charToDelete <= MAX_CHARACTERS)
        {
            GameController.controller.Delete(GameController.controller.charNames[charToDelete]);
            GameController.controller.charNames[selectedChar] = null;
            GameController.controller.charClasses[selectedChar] = PlayerClass.none;
            --GameController.controller.numChars;
            GameController.controller.SaveCharacters();
            //foreach (string item in GameController.controller.charNames)
            //{
            //    print("character: " + item);
            //}
        }

        HideDeleteCheck();
        HideDelete();
        RefreshCharacterList();
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

        if (charNum > GameController.controller.numChars)
        {
            HideDelete();
            LoadDefaultCharacter();
        }
        else
        {
            ShowDelete();
            LoadCharacterPreview(selectedChar);
        }
    }

    public void LoadCharacterPreview(int charNum)
    {
        GameController.controller.Load(GameController.controller.charNames[charNum]);
        playerMannequin.GetComponent<AnimationController>().LoadCharacter();

        for (int i = 8; i < 12; ++i)
        {
            characterMannequin.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
            characterMannequin.transform.GetChild(i).GetComponent<SpriteRenderer>().color = getSkinColor();
        }
        
        characterMannequin.GetComponent<AnimationController>().LoadCharacter();
    }

    public Color getSkinColor()
    {
        Color player_C = new Color(GameController.controller.playerSkinColor[0],
            GameController.controller.playerSkinColor[1],
            GameController.controller.playerSkinColor[2]);
        return player_C;
    }

    public Color getPlayerColor()
    {
        Color player_C = new Color(GameController.controller.playerColorPreference[0], 
            GameController.controller.playerColorPreference[1], 
            GameController.controller.playerColorPreference[2]);
        return player_C;
    }

    public void ShowDelete()
    {
        GameController.controller.GetComponent<MenuUIAudio>().playButtonClick();
        deleteButton.GetComponent<ButtonAnimatorScript>().ShowButton();
    }

    public void HideDelete()
    {
        deleteButton.GetComponent<ButtonAnimatorScript>().HideButton();
    }
}
