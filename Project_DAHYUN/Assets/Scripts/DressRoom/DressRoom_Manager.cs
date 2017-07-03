using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DressRoom_Manager : MonoBehaviour {
    //#DEFINES
    public const int MAX_ARMOR_SETS = 0;
    public const int MAX_WEAPON_SETS = 0;
    //
    public string spriteSheetName_weapon, spriteSheetName_arms, spriteSheetName_legs, spriteSheetName_torso, spriteSheetName_head,
        spriteSheetName_hands, spriteSheetName_shoes, spriteSheetName_back;
    Sprite[][] spriteSheets;
    Sprite[] spriteSheet_weapon, spriteSheet_arms, spriteSheet_legs, spriteSheet_torso, spriteSheet_head, spriteSheet_hands, 
        spriteSheet_shoes, spriteSheet_back;
    int currentHeadID = 0, currentTorsoID = 0, currentArmsID = 0, currentLegsID = 0, currentWeaponID = 0, currentColorID = 0;
    GameObject mannequin;

    // Use this for initialization
    void Start()
    {
        mannequin = GameObject.Find("Player_Mannequin");

        spriteSheet_weapon = Resources.LoadAll<Sprite>("Spritesheet_Weapon_" + spriteSheetName_weapon);
        spriteSheet_arms = Resources.LoadAll<Sprite>("Spritesheet_Arms_" + spriteSheetName_arms);
        spriteSheet_legs = Resources.LoadAll<Sprite>("Spritesheet_Legs_" + spriteSheetName_legs);
        spriteSheet_torso = Resources.LoadAll<Sprite>("Spritesheet_Torso_" + spriteSheetName_torso);
        spriteSheet_head = Resources.LoadAll<Sprite>("Spritesheet_Head_" + spriteSheetName_head);
        spriteSheet_hands = Resources.LoadAll<Sprite>("Spritesheet_Hands_" + spriteSheetName_hands);
        spriteSheet_shoes = Resources.LoadAll<Sprite>("Spritesheet_Shoes_" + spriteSheetName_shoes);
        spriteSheet_back = Resources.LoadAll<Sprite>("Spritesheet_Back_" + spriteSheetName_back);
    }

    public void UpdateAnimationState(int newState, int frameNum)
    {
        ChangeWeapon(newState, frameNum);
    }

    public void ChangeWeapon(int newState, int frameNum = 0)
    {
        //currentWeaponID += dir;

        if (currentWeaponID > MAX_WEAPON_SETS)
            currentWeaponID = 0;

        if (currentWeaponID < 0)
            currentWeaponID = MAX_WEAPON_SETS;

        LoadNewSprite(currentWeaponID, 0);

        foreach (Sprite S in spriteSheet_weapon)
        {
            if (S.name.Equals("Spritesheet_Weapon_" + spriteSheetName_weapon + "_" + currentWeaponID))
            {
                mannequin.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = S;
            }
        }
    }


    public void ChangeArms(int dir)
    {
        currentArmsID += dir;

        if (currentArmsID > MAX_ARMOR_SETS)
            currentArmsID = 0;

        if (currentArmsID < 0)
            currentArmsID = MAX_ARMOR_SETS;

        LoadNewSprite(currentArmsID, 1);

        spriteSheet_weapon = Resources.LoadAll<Sprite>("Spritesheet_Weapon_" + spriteSheetName_weapon);

        foreach (Sprite S in spriteSheet_arms)
        {
            if (S.name.Equals("Spritesheet_Arms_" + spriteSheetName_arms + "_" + currentColorID))
            {
                mannequin.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = S;
                break;
            }
        }
    }

    public void ChangeLegs(int dir)
    {
        currentLegsID += dir;

        if (currentLegsID > MAX_ARMOR_SETS)
            currentLegsID = 0;

        if (currentLegsID < 0)
            currentLegsID = MAX_ARMOR_SETS;

        LoadNewSprite(currentLegsID, 2);

        foreach (Sprite S in spriteSheet_legs)
        {
            if (S.name.Equals("Spritesheet_Legs_" + spriteSheetName_legs + "_" + currentColorID))
            {
                mannequin.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite = S;
            }
        }
    }

    public void ChangeTorso(int dir)
    {
        currentTorsoID += dir;

        if (currentTorsoID > MAX_ARMOR_SETS)
            currentTorsoID = 0;

        if (currentTorsoID < 0)
            currentTorsoID = MAX_ARMOR_SETS;

        LoadNewSprite(currentTorsoID, 3);

        foreach (Sprite S in spriteSheet_torso)
        {
            if (S.name.Equals("Spritesheet_Torso_" + spriteSheetName_torso + "_" + currentColorID))
            {
                mannequin.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sprite = S;
            }
        }
    }

    public void ChangeHead(int dir)
    {
        currentHeadID += dir;

        if (currentHeadID > MAX_ARMOR_SETS)
            currentHeadID = 0; 

        if (currentHeadID < 0)
            currentHeadID = MAX_ARMOR_SETS;

        LoadNewSprite(currentHeadID, 4);

        foreach (Sprite S in spriteSheet_head)
        {
            if (S.name.Equals("Spritesheet_Head_" + spriteSheetName_head + "_" + currentColorID))
            {
                mannequin.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().sprite = S;
            }
        }
    }

    public void ChangeColor(int color)
    {
        foreach (Sprite S in spriteSheet_weapon)
        {
            if (S.name.Equals("Spritesheet_Weapon_" + spriteSheetName_weapon + "_" + color))
            {
                mannequin.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = S;
            }
        }

        foreach (Sprite S in spriteSheet_arms)
        {
            if (S.name.Equals("Spritesheet_Arms_" + spriteSheetName_head + "_" + color))
            {
                mannequin.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = S;
            }
        }

        foreach (Sprite S in spriteSheet_legs)
        {
            if (S.name.Equals("Spritesheet_Legs_" + spriteSheetName_legs + "_" + color))
            {
                mannequin.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite = S;
            }
        }

        spriteSheet_torso = Resources.LoadAll<Sprite>("Spritesheet_Torso_" + spriteSheetName_torso);

        foreach (Sprite S in spriteSheet_torso)
        {
            if (S.name.Equals("Spritesheet_Torso_" + spriteSheetName_torso + "_" + color))
            {
                mannequin.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sprite = S;
            }
        }

        foreach (Sprite S in spriteSheet_head)
        {
            if (S.name.Equals("Spritesheet_Head_" + spriteSheetName_head + "_" + color))
            {
                mannequin.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().sprite = S;
            }
        }
    }

    /////////
    void LoadNewSprite(int setNumber, int bodyPart)
    {
        switch(bodyPart)
        {
            // weapon
            case 0:
                switch (setNumber)
                {
                    case 0:
                        spriteSheetName_weapon = "Static_HonorGuard";
                        spriteSheet_weapon = Resources.LoadAll<Sprite>("Spritesheet_Weapon_" + spriteSheetName_weapon);
                        break;
                    default:
                        spriteSheetName_weapon = "HonorGuard";
                        spriteSheet_weapon = Resources.LoadAll<Sprite>("Spritesheet_Weapon_" + spriteSheetName_weapon);
                        break;
                }
            break;
            // arms
            case 1:
                switch (setNumber)
                {
                    case 0:
                        spriteSheetName_arms = "Static_HonorGuard";
                        spriteSheet_arms = Resources.LoadAll<Sprite>("Spritesheet_Arms_" + spriteSheetName_arms);
                        break;
                    default:
                        spriteSheetName_arms = "Static_HonorGuard";
                        spriteSheet_arms = Resources.LoadAll<Sprite>("Spritesheet_Arms_" + spriteSheetName_arms);
                        break;
                }
                break;
            // legs
            case 2:
                switch (setNumber)
                {
                    case 0:
                        spriteSheetName_legs = "Static_HonorGuard";
                        spriteSheet_legs = Resources.LoadAll<Sprite>("Spritesheet_Legs_" + spriteSheetName_legs);
                        break;
                    default:
                        spriteSheetName_legs = "Static_HonorGuard";
                        spriteSheet_legs = Resources.LoadAll<Sprite>("Spritesheet_Legs_" + spriteSheetName_legs);
                        break;
                }
                break;
            // torso
            case 3:
                switch (setNumber)
                {
                    case 0:
                        spriteSheetName_torso = "Static_HonorGuard";
                        spriteSheet_torso = Resources.LoadAll<Sprite>("Spritesheet_Torso_" + spriteSheetName_torso);
                        break;
                    default:
                        spriteSheetName_torso = "Static_HonorGuard";
                        spriteSheet_torso = Resources.LoadAll<Sprite>("Spritesheet_Torso_" + spriteSheetName_torso);
                        break;
                }
                break;
            // head
            case 4:
                switch (setNumber)
                {
                    case 0:
                        spriteSheetName_head = "Static_HonorGuard";
                        spriteSheet_head = Resources.LoadAll<Sprite>("Spritesheet_Head_" + spriteSheetName_head);
                        break;
                    default:
                        spriteSheetName_head = "Static_HonorGuard";
                        spriteSheet_head = Resources.LoadAll<Sprite>("Spritesheet_Head_" + spriteSheetName_head);
                        break;
                }
                break;
        }
    }
}
