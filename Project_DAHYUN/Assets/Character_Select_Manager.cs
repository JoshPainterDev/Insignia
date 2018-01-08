using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Character_Select_Manager : MonoBehaviour
{
    public GameObject playerMannequin;
    public GameObject checkForDeletePanel;
    public GameObject charSelectPrefab;
    public GameObject gridThing;
    public GameObject blackSq;
    public GameObject deleteButton;
    public GameObject playButton;
    public GameObject nameplate;
    public Color oldButtonColor;
    public Color newButtonColor;

    const int MAX_CHARACTERS = 6;

    private int selectedChar = 0;
    private int charToDelete = 0;
    private bool auraColorActive = false;

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
        HideDeleteCheck();
        RefreshCharacterList();

        if (GameController.controller.numChars != 0)
        {
            selectedChar = 1;
            gridThing.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = newButtonColor;
            LoadCharacterPreview(selectedChar);
        }
        else
        {
            selectedChar = -1;
            LoadDefaultCharacter();
        }
            
    }

    public void Play()
    {
        if(selectedChar == -1)
        {
            CreateCharacter(); 
        }
        else
        {
            string character = GameController.controller.charNames[selectedChar];

            if (GameController.controller.numChars == 0 || character == null || character == "")
            {
                GameController.controller.GetComponent<MenuUIAudio>().playNope();
                return;
            }
            else
            {
                playButton.GetComponent<Button>().enabled = false;
                blackSq.GetComponent<FadeScript>().FadeIn(3.0f);
                Invoke("LoadMainMenu", 2);
            }
        }
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu_Scene");
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
                gridThing.transform.GetChild(i).GetChild(0).GetComponentInChildren<Text>().text = GameController.controller.charNames[i + 1];
            }
        }
    }

    public void CreateCharacter()
    {
        if (GameController.controller.numChars < MAX_CHARACTERS)
        {
            playButton.GetComponent<Button>().enabled = false;
            blackSq.GetComponent<FadeScript>().FadeIn(3.0f);
            Invoke("LoadNewChar", 2);
        }
    }

    private void LoadNewChar()
    {
        SceneManager.LoadScene("NewCharacter_Scene");
    }

    public void CheckForDelete()
    {
        charToDelete = selectedChar;

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
        }

        HideDeleteCheck();
        HideDelete();
        RefreshCharacterList();

        if (GameController.controller.numChars > 0)
        {
            selectedChar = 1;
            LoadCharacterPreview(selectedChar);
        }
        else
            LoadDefaultCharacter();
    }

    public void LoadDefaultCharacter()
    {
        for(int i = 0; i < 8; ++i)
        {
            playerMannequin.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
        }

        playerMannequin.transform.GetChild(12).GetComponent<SpriteRenderer>().enabled = false;
        nameplate.GetComponent<Text>().text = "";
        playButton.transform.GetChild(1).GetComponent<Text>().text = "";

        for (int i = 8; i < 12; ++i)
        {
            playerMannequin.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
            playerMannequin.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void CharacterSelected(int charNum)
    {
        selectedChar = charNum;

        for(int i = 0; i < gridThing.transform.childCount; ++i)
            gridThing.transform.GetChild(i).GetChild(0).GetComponent<Image>().color = oldButtonColor;

        gridThing.transform.GetChild(selectedChar - 1).GetChild(0).GetComponent<Image>().color = newButtonColor;

        if (charNum > GameController.controller.numChars)
        {
            HideDelete();
            selectedChar = -1;
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
        Color skinColor = GameController.controller.getPlayerSkinColor();

        for (int i = 0; i < 8; ++i)
            playerMannequin.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;

        //for (int i = 8; i < 12; ++i)
        //{
        //    playerMannequin.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
        //    playerMannequin.transform.GetChild(i).GetComponent<SpriteRenderer>().color = skinColor;
        //}

        playerMannequin.GetComponent<AnimationController>().LoadCharacter();
        playerMannequin.GetComponent<AnimationController>().PlayCheerAnim();

        int playerLv = GameController.controller.playerLevel;
        PlayerClass playerClass = GameController.controller.charClasses[selectedChar];
        playButton.transform.GetChild(1).GetComponent<Text>().text = "Lv " + playerLv + " " + playerClass;
        nameplate.GetComponent<Text>().text = GameController.controller.charNames[selectedChar];
        nameplate.GetComponent<Text>().color = GameController.controller.getPlayerColorPreference();
    }

    private void ShowPreview()
    {
        for (int i = 0; i < 8; ++i)
            playerMannequin.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
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
