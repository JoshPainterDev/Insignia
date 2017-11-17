using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions; // needed for Regex
using UnityEngine.SceneManagement;
using UnityEngine;

public class NewCharacter_Manager : MonoBehaviour {

    public GameObject playerMannequin;
    public GameObject class1;
    public GameObject class2;
    public GameObject class3;
    public GameObject class4;
    public GameObject classIcon;

    public GameObject textField;
    public GameObject blackSq;

    public GameObject skinPanel;
    public GameObject auraPanel;

    public GameObject skinColor;
    public GameObject auraColor;

    private PlayerClass currentClass = PlayerClass.Knight;
    private bool skinColorActive = false;
    private bool auraColorActive = false;

    // Use this for initialization
    void Start ()
    {
        if (GameController.controller.charClasses.Length == 0)
            GameController.controller.charClasses = new PlayerClass[6];

        UseDefaultArmor(1);
	}

    public void toggleSkinColor()
    {
        if (skinColorActive)
        {
            skinColorActive = false;
            skinPanel.gameObject.SetActive(false);
        }
        else
        {
            skinColorActive = true;
            skinPanel.gameObject.SetActive(true);
            auraColorActive = false;
            auraPanel.gameObject.SetActive(false);
        }
    }

    public void toggleAuraColor()
    {
        if (auraColorActive)
        {
            auraColorActive = false;
            auraPanel.gameObject.SetActive(false);
        }
        else
        {
            auraColorActive = true;
            auraPanel.gameObject.SetActive(true);
            skinColorActive = false;
            skinPanel.gameObject.SetActive(false);
        }
    }

    public void changeSkinColor(GameObject image)
    {
        Color color = image.GetComponent<Image>().color;
        GameController.controller.setPlayerSkinColor(color);
        skinColor.GetComponent<Image>().color = color;
        playerMannequin.GetComponent<AnimationController>().setSkinColor(color);
    }

    public void changeAuraColor(GameObject image)
    {
        Color color = image.GetComponent<Image>().color;
        GameController.controller.setPlayerColorPreference(color);
        auraColor.GetComponent<Image>().color = color;
        playerMannequin.GetComponent<AnimationController>().seteAuraColor(color);
    }

    void UseDefaultArmor(int classNum)
    {
        switch(classNum)
        {
            case 1:
                for (int i = 0; i < 16; ++i)
                {
                    if (i % 2 == 0)
                    {
                        GameController.controller.playerEquippedIDs[i] = i * 2;
                    }
                    else
                        GameController.controller.playerEquippedIDs[i] = 0;
                }
                break;
        }

        GameController.controller.playerEquippedIDs[14] = 28;
        GameController.controller.playerEquippedIDs[15] = 2;
        playerMannequin.GetComponent<AnimationController>().LoadCharacter();
    }

    public void FinalizeCharacter()
    {
        string charName = textField.GetComponent<InputField>().text;
        charName = Regex.Replace(charName, @"[^a-zA-Z0-9 ]", "");

        if(nameChecksOut(charName))
        {
            GameController.controller.GetComponent<MenuUIAudio>().playButtonClick();

            //GameController.controller.playerEquippedIDs[12] = 24;
            //GameController.controller.playerEquippedIDs[13] = 0;

            GameController.controller.playerName = charName;
            GameController.controller.playerLevel = 1;
            GameController.controller.playerAbility1 = AbilityToolsScript.tools.LookUpAbility("none");
            GameController.controller.playerAbility2 = new Ability();
            GameController.controller.playerAbility3 = new Ability();
            GameController.controller.playerAbility4 = new Ability();
            GameController.controller.strikeModifier = "none";
            GameController.controller.limitBreakModifier = LimitBreakName.none;
            GameController.controller.limitBreakTracker = -1;

            SetInitialStats(currentClass);

            GameController.controller.Save(charName);

            ++GameController.controller.numChars;
            GameController.controller.charNames[GameController.controller.numChars] = charName;
            GameController.controller.charClasses[GameController.controller.numChars] = currentClass;
            GameController.controller.SaveCharacters();//DONT FORGET TO SAVE :3

            blackSq.GetComponent<FadeScript>().FadeIn(3.0f);
            Invoke("LoadTutorial", 0.5f);
        }
        else
        {
            GameController.controller.GetComponent<MenuUIAudio>().playNope();
            print("Bad Name! only chars and numbers!");
        }
    }

    private void SetInitialStats(PlayerClass classToUse)
    {
        switch(classToUse)
        {
            case PlayerClass.Knight:
                GameController.controller.playerBaseAtk = 5;
                GameController.controller.playerBaseDef = 3;
                GameController.controller.playerBasePrw = 1;
                GameController.controller.playerBaseSpd = 1;

                GameController.controller.playerAttack = 5 + 5 + 1;
                GameController.controller.playerDefense = 3 + 6;
                GameController.controller.playerProwess = 1 + 2;
                GameController.controller.playerSpeed = 1;
                break;
            default:
                break;
        }
    }

    public void CancelCharacter()
    {
        GameController.controller.GetComponent<MenuUIAudio>().playBack();
        blackSq.GetComponent<FadeScript>().FadeIn(3.0f);
        Invoke("LoadCharSelect", 0.5f);
    }

    private void LoadCharSelect()
    {
        SceneManager.LoadScene("CharacterSelect_Scene");
    }

    private void LoadTutorial()
    {
        SceneManager.LoadScene("Exposition_Scene05");
    }

    private bool nameChecksOut(string charName)
    {
        if (charName == "")
            return false;
        foreach(string character in GameController.controller.charNames)
        {
            if(character == charName)
                return false;
        }

        return true;
    }

    public void ChangeClass(int classNum)
    {
        switch(classNum)
        {
            case 1:
                currentClass = PlayerClass.Knight;
                classIcon.GetComponent<Image>().sprite = class1.transform.GetChild(0).GetComponent<Image>().sprite;
                class2.GetComponent<ButtonAnimatorScript>().RevertColor();
                class3.GetComponent<ButtonAnimatorScript>().RevertColor();
                class4.GetComponent<ButtonAnimatorScript>().RevertColor();
                break;
            case 2:
                currentClass = PlayerClass.Knight;
                classIcon.GetComponent<Image>().sprite = class2.transform.GetChild(0).GetComponent<Image>().sprite;
                class1.GetComponent<ButtonAnimatorScript>().RevertColor();
                class3.GetComponent<ButtonAnimatorScript>().RevertColor();
                class4.GetComponent<ButtonAnimatorScript>().RevertColor();
                break;
            case 3:
                currentClass = PlayerClass.Knight;
                classIcon.GetComponent<Image>().sprite = class3.transform.GetChild(0).GetComponent<Image>().sprite;
                class1.GetComponent<ButtonAnimatorScript>().RevertColor();
                class2.GetComponent<ButtonAnimatorScript>().RevertColor();
                class4.GetComponent<ButtonAnimatorScript>().RevertColor();
                break;
            case 4:
                currentClass = PlayerClass.Knight;
                classIcon.GetComponent<Image>().sprite = class4.transform.GetChild(0).GetComponent<Image>().sprite;
                class1.GetComponent<ButtonAnimatorScript>().RevertColor();
                class2.GetComponent<ButtonAnimatorScript>().RevertColor();
                class3.GetComponent<ButtonAnimatorScript>().RevertColor();
                break;
        }

        UseDefaultArmor(classNum);
    }
}
