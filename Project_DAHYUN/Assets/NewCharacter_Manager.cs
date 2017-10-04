using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions; // needed for Regex
using UnityEngine.SceneManagement;
using UnityEngine;

public class NewCharacter_Manager : MonoBehaviour {

    public GameObject playerMannequin;
    public GameObject textField;
<<<<<<< Updated upstream
    public GameObject blackSq;
=======
    public GameObject class1;
    public GameObject class2;
    public GameObject class3;
    public GameObject class4;
    public GameObject classIcon;
    public GameObject blackSq;
    public PlayerClass currentClass = PlayerClass.Knight;
>>>>>>> Stashed changes

	// Use this for initialization
	void Start ()
    {
        if (GameController.controller.charClasses.Length == 0)
            GameController.controller.charClasses = new PlayerClass[6];
    }

    public void CancelCharacter()
    {
        blackSq.GetComponent<FadeScript>().FadeIn(3.0f);
        Invoke("LoadCharSelect", 0.5f);
    }

    private void LoadCharSelect()
    {
        SceneManager.LoadScene("CharacterSelect_Scene");
    }

    public void FinalizeCharacter()
    {
        string charName = textField.GetComponent<InputField>().text;
        charName = Regex.Replace(charName, @"[^a-zA-Z0-9 ]", "");

        if(charName != "")
        {
            for(int i = 0; i < 15; ++i)
            {
                if(i%2 == 0)
                {
                    print(i%2);
                    GameController.controller.playerEquippedIDs[i] = i;
                }
                else
                {
                    GameController.controller.playerEquippedIDs[0] = 0;
                }
            }

            GameController.controller.characterName = charName;
            GameController.controller.playerLevel = 1;
            GameController.controller.playerLevel = 1;
            GameController.controller.playerAbility1 = null;
            GameController.controller.playerAbility2 = null;
            GameController.controller.playerAbility3 = null;
            GameController.controller.playerAbility4 = null;
            GameController.controller.strikeModifier = "none";

            GameController.controller.playerSkinColor[0] = 1;
            GameController.controller.playerSkinColor[1] = 0;
            GameController.controller.playerSkinColor[2] = 0;
            print("Saving: " + charName);
            GameController.controller.Save(charName);

            ++GameController.controller.numChars; //increase number of characters

            GameController.controller.charNames[GameController.controller.numChars] = charName;
            GameController.controller.charClasses[GameController.controller.numChars] = currentClass;
            GameController.controller.SaveCharacters();


            blackSq.GetComponent<FadeScript>().FadeIn(3);
            Invoke("LoadCharSelect", 0.5f);
        }
        else
        {
            print("Bade Name! only chars and numbers!");
        }
    }

    public void CancelCharacter()
    {
        blackSq.GetComponent<FadeScript>().FadeIn(3);
        Invoke("LoadCharSelect", 0.5f);
    }

    public void LoadCharSelect()
    {
        SceneManager.LoadScene("CharacterSelect_Scene");
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
                currentClass = PlayerClass.none;
                classIcon.GetComponent<Image>().sprite = class2.transform.GetChild(0).GetComponent<Image>().sprite;
                class1.GetComponent<ButtonAnimatorScript>().RevertColor();
                class3.GetComponent<ButtonAnimatorScript>().RevertColor();
                class4.GetComponent<ButtonAnimatorScript>().RevertColor();
                break;
            case 3:
                currentClass = PlayerClass.none;
                classIcon.GetComponent<Image>().sprite = class3.transform.GetChild(0).GetComponent<Image>().sprite;
                class2.GetComponent<ButtonAnimatorScript>().RevertColor();
                class1.GetComponent<ButtonAnimatorScript>().RevertColor();
                class4.GetComponent<ButtonAnimatorScript>().RevertColor();
                break;
            case 4:
                currentClass = PlayerClass.none;
                classIcon.GetComponent<Image>().sprite = class4.transform.GetChild(0).GetComponent<Image>().sprite;
                class2.GetComponent<ButtonAnimatorScript>().RevertColor();
                class3.GetComponent<ButtonAnimatorScript>().RevertColor();
                class1.GetComponent<ButtonAnimatorScript>().RevertColor();
                break;
            default:
                currentClass = PlayerClass.none;
                break;
        }

        LoadMannequin(classNum);
    }

    public void LoadMannequin(int classNum)
    {
        int it = 0;

        switch(classNum)
        {
            case 1: //Knight class
                EquipmentInfo info;

                foreach (Animator child in playerMannequin.GetComponentsInChildren<Animator>())
                {
                    info = EquipmentInfoManager.equipmentInfoTool.LookUpEquipment(it * 4, 0);

                    if ((it > 7)) //&& (it < 12)
                    {
                        break;
                    }

                    child.runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
                    child.enabled = true;

                    ++it;
                }

                info = EquipmentInfoManager.equipmentInfoTool.LookUpEquipment(4, 0);
                string imageName = info.imgSourceName;
                string newStr = imageName;
                string match = "Torso";
                string replace = "Arms";
                int mSize = 0;
                int tracker = 0;
                //Alters the form of the string to include the Arms animator with the Torso
                foreach (char c in imageName)
                {
                    if (c == match[mSize])
                    {
                        ++mSize;

                        if (mSize == 5)
                        {
                            newStr = newStr.Remove(tracker - 4, mSize);
                            newStr = newStr.Insert(tracker - 4, replace);
                            mSize = 0;
                            --tracker;
                        }
                    }
                    else
                        mSize = 0;

                    ++tracker;
                }

                playerMannequin.transform.GetChild(7).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(newStr, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
                playerMannequin.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

                it = 0;
                foreach (SpriteRenderer child in playerMannequin.GetComponentsInChildren<SpriteRenderer>())
                {
                    if (it > 8 || it == 0)
                    {
                        ++it;
                        continue;
                    }

                    child.enabled = true;

                    ++it;
                }

                
                break;
            default: //none
                foreach (SpriteRenderer child in playerMannequin.GetComponentsInChildren<SpriteRenderer>())
                {
                    if ((it > 7) && (it < 12))
                    {
                        ++it;
                        continue;
                    }

                    child.enabled = false;

                    ++it;
                }
                break;
        }

        playerMannequin.GetComponent<AnimationController>().PlayAttackAnim();
    }
}
