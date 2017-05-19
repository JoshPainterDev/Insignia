using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DressRoom_Manager : MonoBehaviour {
    //#DEFINES
    public const int MAX_OPTIONS = 3;
    //
    Sprite[] spriteSheet;
    int currentHeadID = 0, currentTorsoID = 0, currentArmsID = 0, currentLegsID = 0, currentWeaponID = 0;
    Image mannequin;
    // Use this for initialization
    void Start()
    {
        mannequin = GameObject.Find("Mannequin").GetComponent<Image>();
        spriteSheet = Resources.LoadAll<Sprite>("insertimagenamehere");
    }

    public void ChangeHead(int dir)
    {
        if (currentHeadID < MAX_OPTIONS)
            currentHeadID += dir;
        else if (currentHeadID < 0)
            currentHeadID = MAX_OPTIONS;
        else
            currentHeadID = 0;

        foreach (Sprite S in spriteSheet)
        {
            if (S.name.Equals("myimagenamehere" + currentHeadID + currentTorsoID + currentArmsID + currentLegsID + currentWeaponID))
            {
                mannequin.sprite = S;
            }
        }
    }

    public void ChangeTorso(int dir)
    {
        if (currentTorsoID < MAX_OPTIONS)
            currentTorsoID += dir;
        else if (currentTorsoID < 0)
            currentTorsoID = MAX_OPTIONS;
        else
            currentTorsoID = 0;

        foreach (Sprite S in spriteSheet)
        {
            if (S.name.Equals("myimagenamehere" + currentHeadID + currentTorsoID + currentArmsID + currentLegsID + currentWeaponID))
            {
                mannequin.sprite = S;
            }
        }
    }

    public void ChangeArms(int dir)
    {
        if (currentArmsID < MAX_OPTIONS)
            currentArmsID += dir;
        else if (currentArmsID < 0)
            currentArmsID = MAX_OPTIONS;
        else
            currentArmsID = 0;


        foreach (Sprite S in spriteSheet)
        {
            if (S.name.Equals("myimagenamehere" + currentHeadID + currentTorsoID + currentArmsID + currentLegsID + currentWeaponID))
            {
                mannequin.sprite = S;
            }
        }
    }

    public void ChangeLegs(int dir)
    {
        if (currentLegsID < MAX_OPTIONS)
            currentLegsID += dir;
        else if (currentLegsID < 0)
            currentLegsID = MAX_OPTIONS;
        else
            currentLegsID = 0;

        foreach (Sprite S in spriteSheet)
        {
            if (S.name.Equals("myimagenamehere" + currentHeadID + currentTorsoID + currentArmsID + currentLegsID + currentWeaponID))
            {
                mannequin.sprite = S;
            }
        }
    }

    public void ChangeWeapon(int dir)
    {
        if (currentWeaponID < MAX_OPTIONS)
            currentWeaponID += dir;
        else if (currentWeaponID < 0)
            currentWeaponID = MAX_OPTIONS;
        else
            currentWeaponID = 0;

        foreach (Sprite S in spriteSheet)
        {
            if (S.name.Equals("myimagenamehere" + currentHeadID + currentTorsoID + currentArmsID + currentLegsID + currentWeaponID))
            {
                mannequin.sprite = S;
            }
        }
    }
}
