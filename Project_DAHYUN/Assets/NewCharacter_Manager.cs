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
    public GameObject classPanel;
    public GameObject namePanel;
    public GameObject confirmButton;
    public GameObject snapshotAnchor;

    public int maxNameLength = 16;

    public GameObject textField;
    public GameObject blackSq;

    public GameObject skinPanel;
    public GameObject auraPanel;

    public GameObject skinColor;
    public GameObject auraColor;

    private PlayerClass currentClass = PlayerClass.Knight;
    private bool skinColorActive = false;
    private bool auraColorActive = false;
    private Color startColor;

    // Use this for initialization
    void Start ()
    {
        playerMannequin = GameController.controller.playerObject;

        if (GameController.controller.charClasses.Length == 0)
        {
            GameController.controller.charClasses = new PlayerClass[6];
        }
            

        GameController.controller.setPlayerSkinColor(Color.white);
        GameController.controller.setPlayerColorPreference(Color.white);
        startColor = confirmButton.GetComponent<Image>().color;

        UseDefaultArmor(1);
        Invoke("setKnightFirst", 0.25f);
	}

    void setKnightFirst()
    {
        ChangeClass();
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
        for (int i = 0; i < 8; ++i)
        {
            playerMannequin.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
        }

        switch (classNum)
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

        //THIS IS ONLY FOR NOW: FIX THIS LATER

        if(currentClass != PlayerClass.Knight)
        {
            namePanel.transform.GetChild(2).GetComponent<Text>().text = "Class not available!";
            return;
        }

        if(nameChecksOut(charName))
        {
            namePanel.transform.GetChild(2).GetComponent<Text>().text = "";

            GameController.controller.GetComponent<MenuUIAudio>().playButtonClick();

            string c = charName[0].ToString();
            string temp = c.ToUpper();
            for(int i = 1; i < charName.Length; ++i)
            {
                temp += charName[i].ToString();
            }

            charName = temp;

            GameController.controller.playerName = charName;
            GameController.controller.playerLevel = 1;
            GameController.controller.playerEXP = 0;
            GameController.controller.playerAbility1 = AbilityToolsScript.tools.LookUpAbility("Guard Break");
            GameController.controller.playerAbility2 = AbilityToolsScript.tools.LookUpAbility("none");
            GameController.controller.playerAbility3 = AbilityToolsScript.tools.LookUpAbility("none");
            GameController.controller.playerAbility4 = AbilityToolsScript.tools.LookUpAbility("none");
            GameController.controller.unlockedAbilities[9] = true;
            GameController.controller.totalAbilities = 1;
            GameController.controller.strikeModifier = "none";
            GameController.controller.limitBreakModifier = LimitBreakName.Super_Nova;
            GameController.controller.limitBreakTracker = -1;
            GameController.controller.playerEquippedIDs = new int[16];
            GameController.controller.playerEquipmentList = new bool[30, 4];
            GameController.controller.arenaCompleted = new bool[6];



            for (int i = 0; i < 6; ++i)
                GameController.controller.arenaCompleted[i] = false;

            for (int i = 0; i < 30; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    GameController.controller.playerEquipmentList[i, j] = false; // set every armor to not unlocked
                }

                if((i % 4) == 0) //set the default knight armor to be unlocked
                {
                    GameController.controller.playerEquipmentList[i, 0] = true;
                    //GameController.controller.playerEquipmentList[i, 1] = true;
                    //GameController.controller.playerEquipmentList[i, 2] = true;
                }
            }

            GameController.controller.playerDecisions = new bool[8];

            for (int i = 0; i < 8; ++i)
                GameController.controller.playerDecisions[i] = false;

            GameController.controller.decisionsMade = 0;
            GameController.controller.playerGoodPoints = 0;
            GameController.controller.playerEvilPoints = 0;
            GameController.controller.skillTree = new int[10];

            SetInitialStats(currentClass);

            ++GameController.controller.numChars;
            GameController.controller.playerNumber = GameController.controller.numChars;

            GameController.controller.Save(charName);//DONT FORGET TO SAVE :3

            GameController.controller.charNames[GameController.controller.numChars] = charName;
            GameController.controller.charClasses[GameController.controller.numChars] = currentClass;
            GameController.controller.SaveCharacters();//DONT FORGET TO SAVE :3
            //DONT FORGET TO LOAD EITHER!
            GameController.controller.Load(charName);

            StartCoroutine(LoadNewGame());
        }
        else
        {
            GameController.controller.GetComponent<MenuUIAudio>().playError();
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

                GameController.controller.playerAttack = 5 + 2 + 1 + 1 + 1 + 5 + GameController.controller.playerBaseAtk; // 15atk + 5
                GameController.controller.playerDefense = 3 + 2 + 3 + 2 + 1 + 1 + 1 + GameController.controller.playerBaseDef; // 13def + 3
                GameController.controller.playerProwess = 1 + 1 + 2 + GameController.controller.playerBasePrw; // 4prs + 1
                GameController.controller.playerSpeed = 1 + 1 + 1 + GameController.controller.playerBaseSpd; // 3spd + 1

                GameController.controller.playerEquippedIDs[0] = 0;
                GameController.controller.playerEquippedIDs[1] = 0;
                GameController.controller.playerEquippedIDs[2] = 4;
                GameController.controller.playerEquippedIDs[3] = 0;
                GameController.controller.playerEquippedIDs[4] = 8;
                GameController.controller.playerEquippedIDs[5] = 0;
                GameController.controller.playerEquippedIDs[6] = 12;
                GameController.controller.playerEquippedIDs[7] = 0;
                GameController.controller.playerEquippedIDs[8] = 16;
                GameController.controller.playerEquippedIDs[9] = 0;
                GameController.controller.playerEquippedIDs[10] = 20;
                GameController.controller.playerEquippedIDs[11] = 0;
                GameController.controller.playerEquippedIDs[12] = 24;
                GameController.controller.playerEquippedIDs[13] = 0;
                GameController.controller.playerEquippedIDs[14] = 28;
                GameController.controller.playerEquippedIDs[15] = 0;
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

    IEnumerator LoadNewGame()
    {
        blackSq.GetComponent<FadeScript>().FadeColored(new Color(1, 1, 1, 0), Color.white, 1.0f);
        yield return new WaitForSeconds(1.0f);
        snapshotAnchor.transform.GetChild(2).GetComponent<PlayerCamera_C>().TakeSnapshot();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Exposition_Scene01");
    }

    IEnumerator TakeSnapshot()
    {
        //camera.GetComponent<LerpScript>().LerpToPos(camera.transform.position, mmCameraPos, 1f);
        yield return new WaitForSeconds(0.25f);
        //blackSq.GetComponent<FadeScript>().FadeIn(1.5f);
        //yield return new WaitForSeconds(1.0f);
        //snapshotAnchor.transform.GetChild(2).GetComponent<PlayerCamera_C>().TakeSnapshot();
    }

    private bool nameChecksOut(string charName)
    {
        if (charName == "")
        {
            namePanel.transform.GetChild(2).GetComponent<Text>().text = "You must have a name!";
            return false;
        }
            
        foreach(string character in GameController.controller.charNames)
        {
            if(character == charName)
            {
                namePanel.transform.GetChild(2).GetComponent<Text>().text = "Name already in use!";
                return false;
            }
        }
        if (charName.Length > maxNameLength)
        {
            namePanel.transform.GetChild(2).GetComponent<Text>().text = "Your name must be less than 16 characters!";
            return false;
        }

        return true;
    }

    public void ChangeClass(int classNum = 1)
    {
        switch(classNum)
        {
            case 1:
                currentClass = PlayerClass.Knight;
                classIcon.GetComponent<Image>().sprite = class1.transform.GetChild(0).GetComponent<Image>().sprite;
                class1.GetComponent<ButtonAnimatorScript>().ChangeColor();
                class2.GetComponent<ButtonAnimatorScript>().RevertColor();
                class3.GetComponent<ButtonAnimatorScript>().RevertColor();
                class4.GetComponent<ButtonAnimatorScript>().RevertColor();

                confirmButton.GetComponent<Image>().color = startColor;

                classPanel.transform.GetChild(0).GetComponent<Text>().text = "Class: Knight";
                classPanel.transform.GetChild(1).GetComponent<Text>().text =
                    "Knights are sturdy and lethal warriors of old, " +
                    " upholding their people's virutes by the blade. These honorary warriors focus on Strikes and Physical Abilities.";
                break;
            case 2:
                currentClass = PlayerClass.Guardian;
                classIcon.GetComponent<Image>().sprite = class2.transform.GetChild(0).GetComponent<Image>().sprite;
                class2.GetComponent<ButtonAnimatorScript>().ChangeColor();
                class1.GetComponent<ButtonAnimatorScript>().RevertColor();
                class3.GetComponent<ButtonAnimatorScript>().RevertColor();
                class4.GetComponent<ButtonAnimatorScript>().RevertColor();

                confirmButton.GetComponent<Image>().color = Color.grey;

                classPanel.transform.GetChild(0).GetComponent<Text>().text = "Class: Guardian";
                classPanel.transform.GetChild(1).GetComponent<Text>().text =
                    "Guardians are the righteous bulwark of the kingdom." +
                    " They thrive on the defensive, brandishing a variety of Utility focused Abilities." +
                    " Thought to be divine hands of justice by the common folk.";
                    
                break;
            case 3:
                currentClass = PlayerClass.Occultist;
                classIcon.GetComponent<Image>().sprite = class3.transform.GetChild(0).GetComponent<Image>().sprite;
                class3.GetComponent<ButtonAnimatorScript>().ChangeColor();
                class1.GetComponent<ButtonAnimatorScript>().RevertColor();
                class2.GetComponent<ButtonAnimatorScript>().RevertColor();
                class4.GetComponent<ButtonAnimatorScript>().RevertColor();

                confirmButton.GetComponent<Image>().color = Color.grey;

                classPanel.transform.GetChild(0).GetComponent<Text>().text = "Class: Occultist";
                classPanel.transform.GetChild(1).GetComponent<Text>().text =
                    "The Occultist is defined by attunement and knowledge of all things arcana." +
                    " Occultists weave spells and lethal Strikes together to create a seamless offensive." +
                    " Their prowess with Magical Abilities makes them devastating in combat.";
                    
                break;
            case 4:
                currentClass = PlayerClass.Cutthroat;
                classIcon.GetComponent<Image>().sprite = class4.transform.GetChild(0).GetComponent<Image>().sprite;
                class4.GetComponent<ButtonAnimatorScript>().ChangeColor();
                class1.GetComponent<ButtonAnimatorScript>().RevertColor();
                class2.GetComponent<ButtonAnimatorScript>().RevertColor();
                class3.GetComponent<ButtonAnimatorScript>().RevertColor();

                confirmButton.GetComponent<Image>().color = Color.grey;

                classPanel.transform.GetChild(0).GetComponent<Text>().text = "Class: Cutthroat";
                classPanel.transform.GetChild(1).GetComponent<Text>().text =
                    "Cutthroats are wild mercenaries that live and die by the blade." +
                    " Using a combination of Utility and Physical Abilities, these blade-dancers bolster their Strikes for maximum lethality.";
                break;
        }

        UseDefaultArmor(classNum);
    }
}
