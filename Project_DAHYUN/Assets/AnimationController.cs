using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public bool showAura = false;
    [HideInInspector]
    public bool InCombat = false;
    [HideInInspector]
    public bool facingRight = true;

    private bool[] hiddenContainer = new bool[20];

    public void Awake()
    {
        facingRight = true;
    }

    public void Start()
    {
        if (GameController.controller.playerObject == null)
        {
            DontDestroyOnLoad(gameObject);
            GameController.controller.playerObject = this.gameObject;
            GameController.controller.Load(GameController.controller.charNames[1]);
            //print(GameController.controller.charNames[1]);
            LoadCharacter();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlaySpeed(float newSpeed = 1)
    {
        foreach (Animator child in this.GetComponentsInChildren<Animator>())
        {
            child.speed = newSpeed;
        }
    }

    public void FlipFlop()
    {
        foreach (SpriteRenderer sprite in this.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.flipX = !sprite.flipX;
            facingRight = !facingRight;
        }
    }

    public void HidePlayer()
    {
        int j = 0;

        foreach (SpriteRenderer i in this.GetComponentsInChildren<SpriteRenderer>())
        {
            hiddenContainer[j] = i.enabled;

            i.enabled = false;
            ++j;
        }
    }

    public void ShowPlayer()
    {
        int j = 0;

        foreach (SpriteRenderer i in this.GetComponentsInChildren<SpriteRenderer>())
        {
            i.enabled = hiddenContainer[j];
            ++j;
        }
    }

    public void PlayIdleAnim()
    {
        this.transform.GetChild(6).GetChild(0).GetComponent<SortingGroup>().sortingOrder = -1;
        this.transform.GetChild(6).GetComponent<SpriteRenderer>().sortingOrder = -1;

        foreach (Animator child in this.GetComponentsInChildren<Animator>())
        {
            child.SetInteger("AnimState", 0);
        }
    }

    public void SetCombatState(bool combat)
    {
        InCombat = combat;

        foreach (Animator child in this.GetComponentsInChildren<Animator>())
        {
            child.SetBool("InCombat", combat);
        }
    }

    public void PlayWalkAnim()
    {
        this.transform.GetChild(6).GetChild(0).GetComponent<SortingGroup>().sortingOrder = -5;
        this.transform.GetChild(6).GetComponent<SpriteRenderer>().sortingOrder = -5;

        foreach (Animator child in this.GetComponentsInChildren<Animator>())
        {
            child.SetInteger("AnimState", 1);
        }
    }

    public void PlayCheerAnim()
    {
        this.transform.GetChild(6).GetChild(0).GetComponent<SortingGroup>().sortingOrder = -1;
        this.transform.GetChild(6).GetComponent<SpriteRenderer>().sortingOrder = -1;

        foreach (Animator child in this.GetComponentsInChildren<Animator>())
        {
            child.SetInteger("AnimState", 2);
        }
    }

    public void PlayForcePushAnim()
    {
        this.transform.GetChild(6).GetChild(0).GetComponent<SortingGroup>().sortingOrder = -1;
        this.transform.GetChild(6).GetComponent<SpriteRenderer>().sortingOrder = -1;

        foreach (Animator child in this.GetComponentsInChildren<Animator>())
        {
            child.SetInteger("AnimState", 3);
        }
    }

    public void PlayKnightLBIdleAnim()
    {
        this.transform.GetChild(6).GetChild(0).GetComponent<SortingGroup>().sortingOrder = -5;
        this.transform.GetChild(6).GetComponent<SpriteRenderer>().sortingOrder = -5;

        foreach (Animator child in this.GetComponentsInChildren<Animator>())
        {
            child.SetInteger("AnimState", 4);
        }
    }

    public void PlayAttackAnim()
    {
        this.transform.GetChild(6).GetChild(0).GetComponent<SortingGroup>().sortingOrder = -1;
        this.transform.GetChild(6).GetComponent<SpriteRenderer>().sortingOrder = -1;

        foreach (Animator child in this.GetComponentsInChildren<Animator>())
        {
            child.SetInteger("AnimState", 5);
        }
    }

    public void PlayHoldAttackAnim()
    {
        this.transform.GetChild(6).GetChild(0).GetComponent<SortingGroup>().sortingOrder = -1;
        this.transform.GetChild(6).GetComponent<SpriteRenderer>().sortingOrder = -1;

        foreach (Animator child in this.GetComponentsInChildren<Animator>())
        {
            child.SetInteger("AnimState", 6);
        }
    }

    public void PlayStruggleHoldAnim()
    {
        this.transform.GetChild(6).GetChild(0).GetComponent<SortingGroup>().sortingOrder = -1;
        this.transform.GetChild(6).GetComponent<SpriteRenderer>().sortingOrder = -1;

        foreach (Animator child in this.GetComponentsInChildren<Animator>())
        {
            child.SetInteger("AnimState", 7);
        }
    }

    public void PlayDeathAnim()
    {
        this.transform.GetChild(6).GetChild(0).GetComponent<SortingGroup>().sortingOrder = 5;
        this.transform.GetChild(6).GetComponent<SpriteRenderer>().sortingOrder = 5;

        foreach (Animator child in this.GetComponentsInChildren<Animator>())
        {
            child.SetInteger("AnimState", -1);
        }
    }

    public void LoadCharacter()
    {
        //head
        EquipmentInfo info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[0], GameController.controller.playerEquippedIDs[1]);
        this.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        if (!info.hideUnderLayer)
        {
            this.transform.GetChild(8).GetComponent<SpriteRenderer>().enabled = true;
        }
        else
            this.transform.GetChild(8).GetComponent<SpriteRenderer>().enabled = false;

        //torso
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[2], GameController.controller.playerEquippedIDs[3]);
        this.transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        //print(info.Name);
        if (!info.hideUnderLayer)
        {
            this.transform.GetChild(9).GetComponent<SpriteRenderer>().enabled = true;
            this.transform.GetChild(11).GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            this.transform.GetChild(9).GetComponent<SpriteRenderer>().enabled = false;
            this.transform.GetChild(11).GetComponent<SpriteRenderer>().enabled = false;
        }

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
        this.transform.GetChild(7).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(newStr, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        //legs
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[4], GameController.controller.playerEquippedIDs[5]);
        this.transform.GetChild(2).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        //back
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[6], GameController.controller.playerEquippedIDs[7]);
        this.transform.GetChild(3).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        //gloves
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[8], GameController.controller.playerEquippedIDs[9]);
        this.transform.GetChild(4).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        if (!info.hideUnderLayer)
            this.transform.GetChild(10).GetComponent<SpriteRenderer>().enabled = true;
        else
            this.transform.GetChild(10).GetComponent<SpriteRenderer>().enabled = false;
        //shoes
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[10], GameController.controller.playerEquippedIDs[11]);
        this.transform.GetChild(5).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        //weapon
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[12], GameController.controller.playerEquippedIDs[13]);
        this.transform.GetChild(6).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

        if (info.useMaskTexture)
        {
            Color playerPref = GameController.controller.getPlayerColorPreference();
            GameObject weaponMask = this.transform.GetChild(6).GetChild(0).gameObject;
            weaponMask.GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.maskSpriteName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
            weaponMask.GetComponent<SpriteRenderer>().enabled = true;
            weaponMask.GetComponent<SpriteRenderer>().color = new Color(playerPref.r, playerPref.g, playerPref.b, 0.6f);
            weaponMask.GetComponent<SpriteMask>().enabled = true;
            weaponMask.GetComponent<SpriteMaskAnimator>().setActive(true);
            weaponMask.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = info.maskTexture;
            weaponMask.transform.GetChild(0).GetComponent<SpriteRenderer>().color = info.equipmentColor;
        }
        else
        {
            this.transform.GetChild(6).GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            this.transform.GetChild(6).GetChild(0).GetComponent<SpriteMask>().enabled = false;
        }
            

        //Aura
        info = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(GameController.controller.playerEquippedIDs[14], GameController.controller.playerEquippedIDs[15]);
        this.transform.GetChild(12).GetComponent<Animator>().runtimeAnimatorController = Resources.Load(info.imgSourceName, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

        if(info.useMaskTexture)
        {
            this.transform.GetChild(12).GetComponent<SpriteRenderer>().enabled = false;
            this.transform.GetChild(12).GetComponent<SpriteMask>().enabled = true;
            this.transform.GetChild(12).GetChild(0).GetComponent<SpriteRenderer>().sprite = info.maskTexture;
            this.transform.GetChild(12).GetChild(0).GetComponent<SpriteRenderer>().color = info.equipmentColor;
        }

        if (showAura)
        {
            this.transform.GetChild(12).gameObject.SetActive(true);
        }

        setSkinColor(GameController.controller.getPlayerSkinColor()); // set skin color

        SetCombatState(InCombat);
    }

    public void setSkinColor(Color color)
    {
        this.transform.GetChild(8).GetComponent<SpriteRenderer>().color = color;
        this.transform.GetChild(9).GetComponent<SpriteRenderer>().color = color;
        this.transform.GetChild(10).GetComponent<SpriteRenderer>().color = color;
        this.transform.GetChild(11).GetComponent<SpriteRenderer>().color = color;
    }

    public void seteAuraColor(Color color)
    {
        this.transform.GetChild(12).GetComponent<SpriteRenderer>().color = color;
    }
}
