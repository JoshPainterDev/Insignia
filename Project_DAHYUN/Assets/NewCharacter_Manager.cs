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

    private PlayerClass currentClass = PlayerClass.Knight;

	// Use this for initialization
	void Start ()
    {
        if (GameController.controller.charClasses.Length == 0)
            GameController.controller.charClasses = new PlayerClass[6];

        UseDefaultArmor(0);
	}

    void UseDefaultArmor(int classNum)
    {
        switch(classNum)
        {
            case 0:
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
            GameController.controller.playerAbility1 = null;
            GameController.controller.playerAbility2 = null;
            GameController.controller.playerAbility3 = null;
            GameController.controller.playerAbility4 = null;
            GameController.controller.strikeModifier = "none";
            GameController.controller.limitBreakModifier = LimitBreakName.none;
            GameController.controller.limitBreakTracker = -1;

            SetInitialStats(currentClass);

            GameController.controller.setPlayerColorPreference(Color.yellow);
            GameController.controller.setPlayerSkinColor(Color.white);
            GameController.controller.Save(charName);

            ++GameController.controller.numChars;
            GameController.controller.charNames[GameController.controller.numChars] = charName;
            GameController.controller.charClasses[GameController.controller.numChars] = currentClass;
            GameController.controller.SaveCharacters();//DONT FORGET TO SAVE :3

            blackSq.GetComponent<FadeScript>().FadeIn(3.0f);
            Invoke("LoadCharSelect", 0.5f);
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
                print(GameController.controller.playerSpeed);
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

//    public void LoadMannequin(int classNum)
//    {
//        int it = 0;

//        switch(classNum)
//        {
//            case 1: // Knight Class
//                EquipmentInfo info;

//                foreach(Animator child in playerMannequin.GetComponentsInChildren<Animator>())
//                {
//                    info = EquipmentInfoManager.equipmentInfoTool.LookUpEquipment(it*4, 0);

//                    if(it > 7)
//                    {
//                        break;
//                    }

//                    child.runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
//                }

//                info = EquipmentInfoManager.equipmentInfoTool.LookUpEquipment(4, 0);
//                string imageName = info.imgSourceName;
//                string newStr = imageName;
//                string match = "Torso";
//                string replace = "Arms";
//                int mSize = 0;
//                int tracker = 0;
//                //Alters the form of the string to include the Arms animator with the Torso
//                foreach (char c in imageName)
//                {
//                    if (c == match[mSize])
//                    {
//                        ++mSize;

//                        if (mSize == 5)
//                        {
//                            newStr = newStr.Remove(tracker - 4, mSize);
//                            newStr = newStr.Insert(tracker - 4, replace);
//                            mSize = 0;
//                            --tracker;
//                        }
//                    }
//                    else
//                        mSize = 0;

//                    ++tracker;
//                }

//                playerMannequin.transform.GetChild(7).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(newStr, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
//                playerMannequin.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

//                it = 0;

//                foreach (SpriteRenderer child in playerMannequin.GetComponentsInChildren<SpriteRenderer>())
//                {
//                    if (it > 8 || it == 0)
//                    {
//                        ++it;
//                        continue;
//                    }

//                    child.enabled = true;
//                    ++it;
//                }

//                break;

//            default:
//                foreach (SpriteRenderer child in playerMannequin.GetComponentsInChildren<SpriteRenderer>())
//                {
//                    if (it > 7 || it < 12)
//                    {
//                        ++it;
//                        continue;
//                    }

//                    child.enabled = false;
//                    ++it;
//                }
//                break;
//        }

//        playerMannequin.GetComponent<AnimationController>().PlayAttackAnim();
//    }
}
