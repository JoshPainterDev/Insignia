using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatAnimator : MonoBehaviour {

    //public Animator hands, weapon, arms, legs, torso, head, shoes, back;

    const int idle_AS = 0;
    const int hurt_AS = 2;
    const int attack01_AS = 5;

    [HideInInspector]
    private Animator[] childAnimators;
    [HideInInspector]
    public string currentAnimation = "";

    public GameObject playerMannequin;

    private string bodyHeadIdle = "Player_BodyHead_DefaultWhite_Idle";
    private string bodyTorsoIdle = "Player_BodyTorso_DefaultWhite_Idle";
    private string bodyArmsIdle = "Player_BodyArms_DefaultWhite_Idle";
    private string bodyGlovesIdle = "Player_BodyHands_DefaultWhite_Idle";

    // Use this for initialization
    void Start ()
    {
        childAnimators = new Animator[14];
        currentAnimation = "idle";
        int i = 0;
		foreach(Animator child in this.GetComponentsInChildren<Animator>())
        {
            if(i < 7)
                LoadPlayerEquipment(GameController.controller.playerEquippedIDs[i * 2], GameController.controller.playerEquippedIDs[(i * 2 + 1)]);
            else if(i == 12)
                LoadPlayerEquipment(GameController.controller.playerEquippedIDs[14], GameController.controller.playerEquippedIDs[15]);

            child.SetInteger("AnimState", idle_AS);
            childAnimators[i] = child;
            ++i;
        }
	}

    public void ChangeToAnimation(string animName)
    {
        currentAnimation = animName;

        switch (animName)
        {
            case "idle":
                foreach (Animator child in childAnimators)
                {
                    child.SetInteger("AnimState", idle_AS);
                }
                break;
            case "attack01":
                foreach (Animator child in childAnimators)
                {
                    child.SetInteger("AnimState", attack01_AS);
                }
                break;
            default:
                break;
        }
    }

    public void LoadPlayerEquipment(int i, int j)
    {
        EquipmentInfo info = EquipmentInfoManager.equipmentInfoTool.LookUpEquipment(i, j);
        string imageName = info.imgSourceName;

        //head
        if (i < 4)
        {
            playerMannequin.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(imageName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

            if (!info.hideUnderLayer)
            {
                playerMannequin.transform.GetChild(8).GetComponent<SpriteRenderer>().enabled = true;
            }
            else
                playerMannequin.transform.GetChild(8).GetComponent<SpriteRenderer>().enabled = false;
        }
        //torso
        else if (i < 8)
        {
            playerMannequin.transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(imageName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

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

            playerMannequin.transform.GetChild(8).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(newStr, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

            if (!info.hideUnderLayer)
            {
                playerMannequin.transform.GetChild(10).GetComponent<SpriteRenderer>().enabled = true;
            }
            else
                playerMannequin.transform.GetChild(10).GetComponent<SpriteRenderer>().enabled = false;
        }
        //legs
        else if (i < 12)
        {
            playerMannequin.transform.GetChild(2).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(imageName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        }
        //back
        else if (i < 16)
        {
            playerMannequin.transform.GetChild(3).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(imageName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        }
        //gloves
        else if (i < 20)
        {
            playerMannequin.transform.GetChild(4).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(imageName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
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
        else if (i < 24)
        {
            playerMannequin.transform.GetChild(5).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(imageName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        }
        //weapon
        else if (i < 28)
        {
            playerMannequin.transform.GetChild(6).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(imageName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        }
        //aura
        else if (i < 30)
        {
            playerMannequin.transform.GetChild(12).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(imageName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        }

        RefreshAnimations();
    }

    public void RefreshAnimations()
    {
        int counter = 0;

        foreach (Animator child in playerMannequin.GetComponentsInChildren<Animator>())
        {
            child.SetInteger("AnimState", 0);

            ++counter;
        }
    }
}
