using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LootManager_C : MonoBehaviour
{
    private GameObject player;
    public GameObject canvasObj;
    public GameObject blackSq;

    public GameObject equipmentUL_Prefab;
    public GameObject goldCredits_Prefab;

    private Sprite[] spriteSheet_Head, spriteSheet_Torso, spriteSheet_Legs, spriteSheet_Back, spriteSheet_Gloves, spriteSheet_Shoes, spriteSheet_Weapon, spriteSheet_Aura;

    private float EQUIPMENT_DROP_RATE = 1.2f;
    private int GOLD_BASE_DROP = 25;

    private int MAX_HEAD_EQUIPMENT = 3;
    private int MAX_TORSO_EQUIPMENT = 3;
    private int MAX_LEGS_EQUIPMENT = 3;
    private int MAX_GLOVES_EQUIPMENT = 3;
    private int MAX_SHOES_EQUIPMENT = 3;
    private int MAX_BACK_EQUIPMENT = 3;

    private int MAX_SWORD_WEAPONS = 3;

    // Use this for initialization
    void Start()
    {
        if (GameController.controller.currentEncounter == null)
        {
            GameController.controller.currentEncounter = new EnemyEncounter();
            GameController.controller.currentEncounter.returnOnSuccessScene = "MainMenu_Scene";
            GameController.controller.currentEncounter.reward = null;
            GameController.controller.currentEncounter.totalEnemies = 3;
        }
        
        player = GameController.controller.playerObject;
    }

    public void GenerateLoot()
    {
        player.GetComponent<AnimationController>().PlayCheerAnim();

        StartCoroutine(LootSequence());
    }

    IEnumerator LootSequence()
    {
        player.GetComponent<AnimationController>().SetCombatState(false);

        if(GameController.controller.currentEncounter.reward != null)
        {
            print("YOU GOT REWARD!");
        }

        //float rand = 1.0f - Random.Range(0.0f, 1.0f);
        float rand = 0.1f;

        //check for equipment drop
        if (rand < EQUIPMENT_DROP_RATE)
        {
            EquipmentInfo equipToUnlock;
            Sprite spriteToUse;
            //float rand2 = Random.Range(0.0f, 1.0f);
            float rand2 = 0.9f;
            int i = 0;
            int j = 0;

            if (rand2 > 0.858f) // helmet drop
            {
                
                spriteSheet_Head = Resources.LoadAll<Sprite>("IconSpritesheets\\Helmet_Spritesheet01");
                i = 0;
                j = Random.Range(0, MAX_HEAD_EQUIPMENT);
                spriteToUse = spriteSheet_Head[j];
            }
            else if(rand2 > 0.715f) // torso drop
            {
                spriteSheet_Torso = Resources.LoadAll<Sprite>("IconSpritesheets\\Torso_Spritesheet01");
                i = 4;
                j = Random.Range(0, MAX_TORSO_EQUIPMENT);
                spriteToUse = spriteSheet_Torso[j];
            }
            else if (rand2 > 0.572f) // leg drop
            {
                spriteSheet_Legs = Resources.LoadAll<Sprite>("IconSpritesheets\\Legs_Spritesheet01");
                i = 8;
                j = Random.Range(0, MAX_LEGS_EQUIPMENT);
                spriteToUse = spriteSheet_Legs[j];
            }
            else if (rand2 > 0.429f) // back drop
            {
                spriteSheet_Back = Resources.LoadAll<Sprite>("IconSpritesheets\\Back_Spritesheet01");
                i = 12;
                j = Random.Range(0, MAX_BACK_EQUIPMENT);
                spriteToUse = spriteSheet_Back[j];
            }
            else if (rand2 > 0.286f) // gloves drop
            {
                spriteSheet_Gloves = Resources.LoadAll<Sprite>("IconSpritesheets\\Gloves_Spritesheet01");
                i = 16;
                j = Random.Range(0, MAX_GLOVES_EQUIPMENT);
                spriteToUse = spriteSheet_Gloves[j];
            }
            else if (rand2 > 0.143f) // shoe drop
            {
                spriteSheet_Shoes = Resources.LoadAll<Sprite>("IconSpritesheets\\Shoes_Spritesheet01");
                i = 20;
                j = Random.Range(0, MAX_SHOES_EQUIPMENT);
                spriteToUse = spriteSheet_Shoes[j];
            }
            else // weapon drop
            {
                spriteSheet_Weapon = Resources.LoadAll<Sprite>("IconSpritesheets\\Sword_Spritesheet01");
                i = 24;
                j = Random.Range(0, MAX_SWORD_WEAPONS);
                spriteToUse = spriteSheet_Weapon[j];
            }

            equipToUnlock = GameController.controller.GetComponent<EquipmentInfoManager>().LookUpEquipment(i, j);

            GameObject equipUL = Instantiate(equipmentUL_Prefab, Vector3.zero, Quaternion.identity) as GameObject;
            equipUL.transform.SetParent(canvasObj.transform);
            equipUL.transform.localPosition = Vector3.zero;

            equipUL.GetComponent<Image>().sprite = spriteToUse;
            equipUL.transform.GetChild(0).GetComponent<Text>().text = equipToUnlock.Name;
        }

        yield return new WaitForSeconds(5.0f);

        GameObject goldCreditsBag = Instantiate(goldCredits_Prefab, Vector3.zero, Quaternion.identity) as GameObject;
        goldCreditsBag.transform.SetParent(canvasObj.transform);
        goldCreditsBag.transform.localPosition = Vector3.zero;
        int randCredits = GameController.controller.playerLevel * (GOLD_BASE_DROP + (GameController.controller.currentEncounter.totalEnemies * Random.Range(15, 25)));
        goldCreditsBag.GetComponent<GoldCredits_C>().totalCoins = randCredits;
        print("Gold Credits Earned: " + randCredits);
        

        //blackSq.GetComponent<FadeScript>().FadeIn();
        yield return new WaitForSeconds(5.5f);
        StartCoroutine(LoadReturnScene());
    }

    IEnumerator LoadReturnScene()
    {
        // this could get complicated depending on where I'm supposed to return to
        // store the return level in the game controller
        blackSq.GetComponent<FadeScript>().FadeIn();
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(GameController.controller.currentEncounter.returnOnSuccessScene);
    }
}
