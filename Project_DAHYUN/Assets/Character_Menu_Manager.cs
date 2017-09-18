using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Character_Menu_Manager : MonoBehaviour {

    // Holds all of the unlocked equipment information from the players saved file
    [HideInInspector]
    public bool[,] unlockedEquipment = new bool[32, 4];

    public GameObject playerMannequin;
    public GameObject equipmentSelectPopUpPrefab;
    private GameObject equipmentSelectPopUp;

    public GameObject canvas;
    public GameObject camera;
    public GameObject blackSq;
    public Vector3 mmCameraPos;

    //Spritesheets
    private Sprite[] spriteSheet_Head, spriteSheet_Torso, spriteSheet_Legs, spriteSheet_Back, spriteSheet_Gloves, spriteSheet_Shoes, spriteSheet_Weapon, spriteSheet_Aura;
    public Sprite lockedIcon;

    public GameObject[] equipmentOptions;
    public GameObject[] statText;

    public GameObject BackButton;
    public GameObject AbilitiesButton;
    public GameObject PersonaButton;

    private string bodyHeadIdle = "Player_BodyHead_DefaultWhite_Idle";
    private string bodyTorsoIdle = "Player_BodyTorso_DefaultWhite_Idle";
    private string bodyArmsIdle = "Player_BodyArms_DefaultWhite_Idle";
    private string bodyGlovesIdle = "Player_BodyArms_DefaultWhite_Idle";

    private bool refreshing = false;

    EquipmentInfo info;

    // Use this for initialization
    void Start()
    {
        //TEMPORARY SHIT BEFORE I GET SAVING WORKING//
        GameController.controller.playerColorPreference = new float[4];
        GameController.controller.playerColorPreference[0] = 1f;
        GameController.controller.playerColorPreference[1] = 0f;
        GameController.controller.playerColorPreference[2] = 0f;
        GameController.controller.playerColorPreference[3] = 0.75f;
        GameController.controller.playerAttack = 5;
        //******************************************//
        //unlockedEquipment = GameController.controller.playerEquipmentList;
        for (int i = 0; i < 30; ++i)
        {
            for(int j = 0; j < 4; ++j)
            {
                unlockedEquipment[i,j] = false;
            }
        }

        unlockedEquipment[0, 0] = true;
        unlockedEquipment[0, 2] = true;
        unlockedEquipment[4, 0] = true;
        unlockedEquipment[4, 2] = true;
        unlockedEquipment[8, 0] = true;
        unlockedEquipment[8, 2] = true;
        unlockedEquipment[12, 0] = true;
        unlockedEquipment[12, 2] = true;
        unlockedEquipment[16, 0] = true;
        unlockedEquipment[16, 2] = true;
        unlockedEquipment[20, 0] = true;
        unlockedEquipment[20, 2] = true;
        unlockedEquipment[24, 0] = true;
        unlockedEquipment[24, 2] = true;
        unlockedEquipment[24, 3] = true;
        unlockedEquipment[28, 0] = true;
        unlockedEquipment[28, 2] = true;

        spriteSheet_Head = Resources.LoadAll<Sprite>("IconSpritesheets\\Spritesheet_Icons_Head");
        spriteSheet_Torso = Resources.LoadAll<Sprite>("IconSpritesheets\\Spritesheet_Icons_Torso");
        spriteSheet_Legs = Resources.LoadAll<Sprite>("IconSpritesheets\\Spritesheet_Icons_Legs");
        spriteSheet_Back = Resources.LoadAll<Sprite>("IconSpritesheets\\Spritesheet_Icons_Back");
        spriteSheet_Gloves = Resources.LoadAll<Sprite>("IconSpritesheets\\Spritesheet_Icons_Gloves");
        spriteSheet_Shoes = Resources.LoadAll<Sprite>("IconSpritesheets\\Spritesheet_Icons_Shoes");
        spriteSheet_Weapon = Resources.LoadAll<Sprite>("IconSpritesheets\\Spritesheet_Icons_Weapon");
        spriteSheet_Aura = Resources.LoadAll<Sprite>("IconSpritesheets\\Spritesheet_Icons_Aura");

        LoadCharacter();
        UpdateStats();
        LoadPersona();
    }

    public void UpdateStats()
    {
        //int atk = GameController.controller.playerAttack + info.AttackStat;
        //int def = GameController.controller.playerDefense + info.DefenseStat;
        //int prw = GameController.controller.playerProwess + info.ProwessStat;
        //int spd = GameController.controller.playerSpeed + info.SpeedStat;

        //statText[0].GetComponent<Text>().text = atk.ToString();
        //statText[1].GetComponent<Text>().text = def.ToString();
        //statText[2].GetComponent<Text>().text = prw.ToString();
        //statText[3].GetComponent<Text>().text = spd.ToString();
    }

    public void LoadPersona()
    {
        float[] colorPref = GameController.controller.playerColorPreference;
        //PersonaButton.transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(colorPref[0], colorPref[1], colorPref[2], colorPref[3]);
    }

    public void LoadSelectedImage(int i, int j)
    {
        GameObject menuButton;
        int indexI = i;
        int indexJ = j;
        int sheetIndex = (4 * (indexI % 4)) + indexJ;

        info = EquipmentInfoManager.equipmentInfoTool.LookUpEquipment(i, j);
        string imageName = info.imgSourceName;

        //head
        if (indexI < 4)
        {
            menuButton = GameObject.Find("Head_Button");
            menuButton.transform.GetChild(0).GetComponent<Image>().sprite = spriteSheet_Head[(4 * indexI) + indexJ];

            playerMannequin.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(imageName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

            GameController.controller.playerEquippedIDs[0] = i;
            GameController.controller.playerEquippedIDs[1] = j;

            if (!info.hideUnderLayer)
                playerMannequin.transform.GetChild(8).GetComponent<SpriteRenderer>().enabled = true;
            else
                playerMannequin.transform.GetChild(8).GetComponent<SpriteRenderer>().enabled = false;
        }
        //torso
        else if (indexI < 8)
        {
            menuButton = GameObject.Find("Torso_Button");
            menuButton.transform.GetChild(0).GetComponent<Image>().sprite = spriteSheet_Torso[sheetIndex];
            playerMannequin.transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(imageName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

            GameController.controller.playerEquippedIDs[2] = i;
            GameController.controller.playerEquippedIDs[3] = j;

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

            if (!info.hideUnderLayer)
            {
                playerMannequin.transform.GetChild(9).GetComponent<SpriteRenderer>().enabled = true;
            }
            else
                playerMannequin.transform.GetChild(9).GetComponent<SpriteRenderer>().enabled = false;
        }
        //legs
        else if (indexI < 12)
        {
            menuButton = GameObject.Find("Legs_Button");
            menuButton.transform.GetChild(0).GetComponent<Image>().sprite = spriteSheet_Legs[sheetIndex];
            playerMannequin.transform.GetChild(2).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(imageName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

            GameController.controller.playerEquippedIDs[4] = i;
            GameController.controller.playerEquippedIDs[5] = j;
        }
        //back
        else if (indexI < 16)
        {
            menuButton = GameObject.Find("Back_Button");
            menuButton.transform.GetChild(0).GetComponent<Image>().sprite = spriteSheet_Back[sheetIndex];
            playerMannequin.transform.GetChild(3).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(imageName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

            GameController.controller.playerEquippedIDs[6] = i;
            GameController.controller.playerEquippedIDs[7] = j;
        }
        //gloves
        else if (indexI < 20)
        {
            menuButton = GameObject.Find("Gloves_Button");
            menuButton.transform.GetChild(0).GetComponent<Image>().sprite = spriteSheet_Gloves[sheetIndex];
            playerMannequin.transform.GetChild(4).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(imageName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

            GameController.controller.playerEquippedIDs[8] = i;
            GameController.controller.playerEquippedIDs[9] = j;

            if (!info.hideUnderLayer)
            {
                playerMannequin.transform.GetChild(10).GetComponent<SpriteRenderer>().enabled = true;
                playerMannequin.transform.GetChild(11).GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                playerMannequin.transform.GetChild(10).GetComponent<SpriteRenderer>().enabled = false;
                playerMannequin.transform.GetChild(11).GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        //shoes
        else if (indexI < 24)
        {
            menuButton = GameObject.Find("Shoes_Button");
            menuButton.transform.GetChild(0).GetComponent<Image>().sprite = spriteSheet_Shoes[sheetIndex];
            playerMannequin.transform.GetChild(5).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(imageName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

            GameController.controller.playerEquippedIDs[10] = i;
            GameController.controller.playerEquippedIDs[11] = j;
        }
        //weapon
        else if (indexI < 28)
        {
            menuButton = GameObject.Find("Weapon_Button");
            menuButton.transform.GetChild(0).GetComponent<Image>().sprite = spriteSheet_Weapon[sheetIndex];
            playerMannequin.transform.GetChild(6).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(imageName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

            GameController.controller.playerEquippedIDs[12] = i;
            GameController.controller.playerEquippedIDs[13] = j;
        }
        //aura
        else if (indexI < 30)
        {
            //menuButton = GameObject.Find("Aura_Button");
            //menuButton.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(imageName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

            //GameController.controller.playerEquippedIDs[14] = i;
            //GameController.controller.playerEquippedIDs[15] = j;
        }


        RefreshAnimations();
    }

    public void RefreshAnimations()
    {
        int counter = 0;
        bool[] previousState = new bool[16];

        foreach (SpriteRenderer child in playerMannequin.GetComponentsInChildren<SpriteRenderer>())
        {
            previousState[counter] = child.enabled;
            child.enabled = false;
            ++counter;
        }

        counter = 0;

        foreach (Animator child in playerMannequin.GetComponentsInChildren<Animator>())
        {
            child.SetInteger("AnimState", 5);
            ++counter;
        }

        StartCoroutine(Refresh(previousState));
    }

    IEnumerator Refresh(bool[] previousState)
    {
        int counter = 0;

        refreshing = true;

        yield return new WaitForSeconds(0.5f);

        foreach (SpriteRenderer child in playerMannequin.GetComponentsInChildren<SpriteRenderer>())
        {
            child.enabled = previousState[counter];
            ++counter;
        }

        refreshing = false;
    }

    //*4 items per row*

    // HEAD UNLOCKS
    // [0][0]... [0][3] 
    // [1][0]... [1][3]
    // [2][0]... [2][3]
    // [3][0]... [3][3]
    public void ShowHeadEquipment()
    {
        equipmentSelectPopUp = (GameObject)Instantiate(equipmentSelectPopUpPrefab, Vector3.zero, transform.rotation);
        equipmentSelectPopUp.transform.SetParent(canvas.transform);
        equipmentSelectPopUp.transform.localPosition = new Vector3(0, 20, 0);

        //get the iner most grid child to reference later
        GameObject grid = equipmentSelectPopUp.transform.GetChild(1).GetChild(0).gameObject;
        // outer loop should match the key above
        for (int i = 0; i < 4; ++i)
        {
            GameObject rowObject = grid.transform.GetChild(i).gameObject;
            //inner loop is always 4
            for (int j = 0; j < 4; ++j)
            {
                GameObject button = rowObject.transform.GetChild(j).gameObject;
                button.name = "EquipmentSelect_Button_Head" + i + j;
                
                if(unlockedEquipment[i,j])
                {
                    EquipmentInfo info = EquipmentInfoManager.equipmentInfoTool.LookUpEquipment(i,j);
                    button.transform.GetChild(0).GetComponent<Text>().text = info.Name;
                    button.transform.GetChild(1).GetComponent<Image>().sprite = spriteSheet_Head[(4 * i) + j];
                }
                else
                {
                    button.transform.GetChild(0).GetComponent<Text>().text = "???";
                    button.transform.GetChild(1).GetComponent<Image>().sprite = lockedIcon;
                    button.GetComponent<Image>().raycastTarget = false;
                }
            }
        }
    }
    // TORSO UNLOCKS
    // [4][0]... [4][3]
    // [5][0]... [5][3]
    // [6][0]... [6][3]
    // [7][0]... [7][3]
    public void ShowTorsoEquipment()
    {
        equipmentSelectPopUp = (GameObject)Instantiate(equipmentSelectPopUpPrefab, Vector3.zero, transform.rotation);
        equipmentSelectPopUp.transform.SetParent(canvas.transform);
        equipmentSelectPopUp.transform.localPosition = new Vector3(0, 20, 0);

        //get the iner most grid child to reference later
        GameObject grid = equipmentSelectPopUp.transform.GetChild(1).GetChild(0).gameObject;
        // outer loop should match the key above
        for (int i = 0; i < 4; ++i)
        {
            GameObject rowObject = grid.transform.GetChild(i).gameObject;
            //inner loop is always 4
            for (int j = 0; j < 4; ++j)
            {
                GameObject button = rowObject.transform.GetChild(j).gameObject;
                button.name = "EquipmentSelect_Button_Torso" + (4 + i) + j;

                if (unlockedEquipment[i + 4, j])
                {
                    EquipmentInfo info = EquipmentInfoManager.equipmentInfoTool.LookUpEquipment(i + 4, j);
                    button.transform.GetChild(0).GetComponent<Text>().text = info.Name;
                    button.transform.GetChild(1).GetComponent<Image>().sprite = spriteSheet_Torso[(4 * i) + j];
                }
                else
                {
                    button.transform.GetChild(0).GetComponent<Text>().text = "???";
                    button.transform.GetChild(1).GetComponent<Image>().sprite = lockedIcon;
                    button.GetComponent<Image>().raycastTarget = false;
                }
            }
        }
    }
    // LEG UNLOCKS
    // [8][0]... [8][3] 
    // [9][0]... [9][3]
    // [10][0]... [10][3]
    // [11][0]... [11][3]
    public void ShowLegsEquipment()
    {
        equipmentSelectPopUp = (GameObject)Instantiate(equipmentSelectPopUpPrefab, Vector3.zero, transform.rotation);
        equipmentSelectPopUp.transform.SetParent(canvas.transform);
        equipmentSelectPopUp.transform.localPosition = new Vector3(0, 20, 0);

        //get the iner most grid child to reference later
        GameObject grid = equipmentSelectPopUp.transform.GetChild(1).GetChild(0).gameObject;
        // outer loop should match the key above
        for (int i = 0; i < 4; ++i)
        {
            GameObject rowObject = grid.transform.GetChild(i).gameObject;
            //inner loop is always 4
            for (int j = 0; j < 4; ++j)
            {
                GameObject button = rowObject.transform.GetChild(j).gameObject;
                button.name = "EquipmentSelect_Button_Legs" + (8 + i) + j;

                if (unlockedEquipment[i + 8, j])
                {
                    EquipmentInfo info = EquipmentInfoManager.equipmentInfoTool.LookUpEquipment(i + 8, j);
                    button.transform.GetChild(0).GetComponent<Text>().text = info.Name;
                    button.transform.GetChild(1).GetComponent<Image>().sprite = spriteSheet_Legs[(4 * i) + j];
                }
                else
                {
                    button.transform.GetChild(0).GetComponent<Text>().text = "???";
                    button.transform.GetChild(1).GetComponent<Image>().sprite = lockedIcon;
                    button.GetComponent<Image>().raycastTarget = false;
                }
            }
        }
    }
    // BACK UNLOCKS
    // [12][0]... [12][3]
    // [13][0]... [13][3]
    // [14][0]... [14][3]
    // [15][0]... [15][3]
    public void ShowBackEquipment()
    {
        equipmentSelectPopUp = (GameObject)Instantiate(equipmentSelectPopUpPrefab, Vector3.zero, transform.rotation);
        equipmentSelectPopUp.transform.SetParent(canvas.transform);
        equipmentSelectPopUp.transform.localPosition = new Vector3(0, 20, 0);

        //get the iner most grid child to reference later
        GameObject grid = equipmentSelectPopUp.transform.GetChild(1).GetChild(0).gameObject;
        // outer loop should match the key above
        for (int i = 0; i < 4; ++i)
        {
            GameObject rowObject = grid.transform.GetChild(i).gameObject;
            //inner loop is always 4
            for (int j = 0; j < 4; ++j)
            {
                GameObject button = rowObject.transform.GetChild(j).gameObject;
                button.name = "EquipmentSelect_Button_Back" + (12 + i) + j;

                if (unlockedEquipment[i + 12, j])
                {
                    EquipmentInfo info = EquipmentInfoManager.equipmentInfoTool.LookUpEquipment(i + 12, j);
                    button.transform.GetChild(0).GetComponent<Text>().text = info.Name;
                    button.transform.GetChild(1).GetComponent<Image>().sprite = spriteSheet_Back[(4 * i) + j];
                }
                else
                {
                    button.transform.GetChild(0).GetComponent<Text>().text = "???";
                    button.transform.GetChild(1).GetComponent<Image>().sprite = lockedIcon;
                    button.GetComponent<Image>().raycastTarget = false;
                }
            }
        }
    }
    // GLOVE UNLOCKS
    // [16][0]... [16][3] 
    // [17][0]... [17][3]
    // [18][0]... [18][3]
    // [19][0]... [19][3]
    public void ShowGlovesEquipment()
    {
        equipmentSelectPopUp = (GameObject)Instantiate(equipmentSelectPopUpPrefab, Vector3.zero, transform.rotation);
        equipmentSelectPopUp.transform.SetParent(canvas.transform);
        equipmentSelectPopUp.transform.localPosition = new Vector3(0, 20, 0);

        //get the iner most grid child to reference later
        GameObject grid = equipmentSelectPopUp.transform.GetChild(1).GetChild(0).gameObject;
        // outer loop should match the key above
        for (int i = 0; i < 4; ++i)
        {
            GameObject rowObject = grid.transform.GetChild(i).gameObject;
            //inner loop is always 4
            for (int j = 0; j < 4; ++j)
            {
                GameObject button = rowObject.transform.GetChild(j).gameObject;
                button.name = "EquipmentSelect_Button_Torso" + (16 + i) + j;

                if (unlockedEquipment[i + 16, j])
                {
                    EquipmentInfo info = EquipmentInfoManager.equipmentInfoTool.LookUpEquipment(i + 16, j);
                    button.transform.GetChild(0).GetComponent<Text>().text = info.Name;
                    button.transform.GetChild(1).GetComponent<Image>().sprite = spriteSheet_Gloves[(i) + j];
                }
                else
                {
                    button.transform.GetChild(0).GetComponent<Text>().text = "???";
                    button.transform.GetChild(1).GetComponent<Image>().sprite = lockedIcon;
                    button.GetComponent<Image>().raycastTarget = false;
                }
            }
        }
    }
    // SHOES UNLOCKS
    // [20][0]... [20][3]
    // [21][0]... [21][3]
    // [22][0]... [22][3]
    // [23][0]... [23][3]
    public void ShowShoesEquipment()
    {
        equipmentSelectPopUp = (GameObject)Instantiate(equipmentSelectPopUpPrefab, Vector3.zero, transform.rotation);
        equipmentSelectPopUp.transform.SetParent(canvas.transform);
        equipmentSelectPopUp.transform.localPosition = new Vector3(0, 20, 0);

        //get the iner most grid child to reference later
        GameObject grid = equipmentSelectPopUp.transform.GetChild(1).GetChild(0).gameObject;
        // outer loop should match the key above
        for (int i = 0; i < 4; ++i)
        {
            GameObject rowObject = grid.transform.GetChild(i).gameObject;
            //inner loop is always 4
            for (int j = 0; j < 4; ++j)
            {
                GameObject button = rowObject.transform.GetChild(j).gameObject;
                button.name = "EquipmentSelect_Button_Shoes" + (20 + i) + j;

                if (unlockedEquipment[i + 20, j])
                {
                    EquipmentInfo info = EquipmentInfoManager.equipmentInfoTool.LookUpEquipment(i + 20, j);
                    button.transform.GetChild(0).GetComponent<Text>().text = info.Name;
                    button.transform.GetChild(1).GetComponent<Image>().sprite = spriteSheet_Shoes[(4 * i) + j];
                }
                else
                {
                    button.transform.GetChild(0).GetComponent<Text>().text = "???";
                    button.transform.GetChild(1).GetComponent<Image>().sprite = lockedIcon;
                    button.GetComponent<Image>().raycastTarget = false;
                }
            }
        }
    }
    // WEAPON UNLOCKS
    // [24][0]... [24][3] 
    // [25][0]... [25][3]
    // [26][0]... [26][3]
    // [27][0]... [27][3]
    public void ShowWeaponEquipment()
    {
        equipmentSelectPopUp = (GameObject)Instantiate(equipmentSelectPopUpPrefab, Vector3.zero, transform.rotation);
        equipmentSelectPopUp.transform.SetParent(canvas.transform);
        equipmentSelectPopUp.transform.localPosition = new Vector3(0, 20, 0);

        //get the iner most grid child to reference later
        GameObject grid = equipmentSelectPopUp.transform.GetChild(1).GetChild(0).gameObject;

        // outer loop should match the key above
        for (int i = 0; i < 4; ++i)
        {
            GameObject rowObject = grid.transform.GetChild(i).gameObject;
            //inner loop is always 4
            for (int j = 0; j < 4; ++j)
            {
                GameObject button = rowObject.transform.GetChild(j).gameObject;
                button.name = "EquipmentSelect_Button_Weapon" + (24 + i) + j;

                if (unlockedEquipment[i + 24, j])
                {
                    EquipmentInfo info = EquipmentInfoManager.equipmentInfoTool.LookUpEquipment(i + 24, j);
                    button.transform.GetChild(0).GetComponent<Text>().text = info.Name;
                    button.transform.GetChild(1).GetComponent<Image>().sprite = spriteSheet_Weapon[(4 * i) + j];
                }
                else
                {
                    button.transform.GetChild(0).GetComponent<Text>().text = "???";
                    button.transform.GetChild(1).GetComponent<Image>().sprite = lockedIcon;
                    button.GetComponent<Image>().raycastTarget = false;
                }
            }
        }
    }
    // AURA UNLOCKS
    // [28][0]... [28][3]
    // [29][0]... [29][3]
    public void ShowAuraEquipment()
    {
        equipmentSelectPopUp = (GameObject)Instantiate(equipmentSelectPopUpPrefab, Vector3.zero, transform.rotation);
        equipmentSelectPopUp.transform.SetParent(canvas.transform);
        equipmentSelectPopUp.transform.localPosition = new Vector3(0, 20, 0);

        //get the iner most grid child to reference later
        GameObject grid = equipmentSelectPopUp.transform.GetChild(1).GetChild(0).gameObject;
        // outer loop should match the key above
        for (int i = 0; i < 2; ++i)
        {
            GameObject rowObject = grid.transform.GetChild(i).gameObject;
            //inner loop is always 4
            for (int j = 0; j < 4; ++j)
            {
                GameObject button = rowObject.transform.GetChild(j).gameObject;
                button.name = "EquipmentSelect_Button_Aura" + (28 + i) + j;

                if (unlockedEquipment[i + 28, j])
                {
                    EquipmentInfo info = EquipmentInfoManager.equipmentInfoTool.LookUpEquipment(i + 28, j);
                    button.transform.GetChild(0).GetComponent<Text>().text = info.Name;
                    button.transform.GetChild(1).GetComponent<Image>().sprite = spriteSheet_Aura[(4 * i) + j];
                }
                else
                {
                    button.transform.GetChild(0).GetComponent<Text>().text = "???";
                    button.transform.GetChild(1).GetComponent<Image>().sprite = lockedIcon;
                    button.GetComponent<Image>().raycastTarget = false;
                }
            }
        }

        for (int i = 2; i < 4; ++i)
        {
            GameObject rowObject = grid.transform.GetChild(i).gameObject;
            Destroy(rowObject);
        }
    }

    public void LoadCharacter()
    {
        //head
        EquipmentInfo info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[0], GameController.controller.playerEquippedIDs[1]);
        playerMannequin.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        //torso
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[2], GameController.controller.playerEquippedIDs[3]);
        playerMannequin.transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

        string newStr = info.imgSourceName;
        string match = "Torso";
        string replace = "Arms";
        int mSize = 0;
        int tracker = 0;
        //Alters the form of the string to include the Arms animator with the Torso
        foreach (char c in info.imgSourceName)
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

        //sleeve
        playerMannequin.transform.GetChild(7).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(newStr, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        //legs
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[4], GameController.controller.playerEquippedIDs[5]);
        playerMannequin.transform.GetChild(2).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        //back
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[6], GameController.controller.playerEquippedIDs[7]);
        playerMannequin.transform.GetChild(3).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        //gloves
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[8], GameController.controller.playerEquippedIDs[9]);
        playerMannequin.transform.GetChild(4).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        //shoes
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[10], GameController.controller.playerEquippedIDs[11]);
        playerMannequin.transform.GetChild(5).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        //weapon
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[12], GameController.controller.playerEquippedIDs[13]);
        playerMannequin.transform.GetChild(6).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
    }

    public void ShowEquipmentOptions()
    {
        foreach (GameObject button in equipmentOptions)
        {
            button.GetComponent<Image>().enabled = false;
        }
    }

    public void HideEquipmentOptions()
    {
        foreach(GameObject button in equipmentOptions)
        {
            button.GetComponent<Image>().enabled = false;
        }
    }

    public void GoBack()
    {
        StartCoroutine(GoToMainMenu());
    }

    public IEnumerator GoToMainMenu()
    {
        DisableMainButtons();
        DisableEquipmentButtons();
        camera.GetComponent<LerpScript>().LerpToPos(camera.transform.position, mmCameraPos, 1f);
        yield return new WaitForSeconds(0.25f);
        blackSq.GetComponent<FadeScript>().FadeIn(1.5f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("MainMenu_Scene");
    }

    public void HideMainButtons()
    {
        BackButton.GetComponent<Image>().enabled = false;
        BackButton.GetComponentInChildren<Image>().enabled = false;
        AbilitiesButton.GetComponent<Image>().enabled = false;
        AbilitiesButton.GetComponentInChildren<Text>().enabled = false;
        PersonaButton.GetComponent<Image>().enabled = false; ;
        PersonaButton.GetComponentInChildren<Text>().enabled = false;
    }

    public void DisableMainButtons()
    {
        BackButton.GetComponent<Button>().enabled = false;
        AbilitiesButton.GetComponent<Button>().enabled = false;
        PersonaButton.GetComponent<Button>().enabled = false; ;
    }

    public void DisableEquipmentButtons()
    {
        foreach (GameObject buttton in equipmentOptions)
        {
            buttton.GetComponent<Button>().enabled = false;
        }
    }
}
