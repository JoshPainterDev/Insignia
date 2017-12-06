using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class RewardManager_C : MonoBehaviour
{
    public GameObject experienceHandle;
    public GameObject abilityUnlockHandle;
    public GameObject blackSq;
    public GameObject playerMannequin;
    private Reward reward;
    private Ability unlockAbility;
    private bool levelUp = false;
    private bool WaitForInput = false;

    private bool unlockingEquips = false;

    // Use this for initialization
    void Start()
    {
        //EncounterToolsScript.tools.SpecifyEncounter(1, 0);
        reward = GameController.controller.rewardEarned;
        int currentXP = GameController.controller.playerEXP;
        GameController.controller.playerEXP += reward.experience;

        if (experienceHandle.GetComponent<ExperienceScript>().experienceAnimation(currentXP, reward.experience))
            levelUp = true;
    }

    public void checkAbilityUnlock()
    {
        if (levelUp)
        {
            unlockAbility = RewardToolsScript.tools.CheckForUnlock(GameController.controller.playerLevel);

            if (unlockAbility.Name == "none")
                checkEquipmentUnlock();
            else
                StartCoroutine(UnlockAbility());
        }
        else
            checkEquipmentUnlock();
    }

    public void checkEquipmentUnlock()
    {
        print("get fuqd");

        if(reward.hasEquipment)
        {
            StartCoroutine(UnlockEquipment());
        }
        else
        {
            StartCoroutine(LoadNextScene());
        }
    }

    public void InputDetected()
    {
        if(WaitForInput)
        {
            WaitForInput = false;
            StartCoroutine(HideAbilityHandle());
        }
    }

    IEnumerator UnlockEquipment()
    {
        int equipmentUnlocked = 0;

        yield return new WaitForSeconds(1.5f);
        
        while(equipmentUnlocked < reward.equipment.Length)
        {
            print(reward.equipment[equipmentUnlocked]);
            ++equipmentUnlocked;
        }

        StartCoroutine(LoadNextScene());
    }

    IEnumerator UnlockAbility()
    {
        //set image
        abilityUnlockHandle.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load(unlockAbility.Icon, typeof(Sprite)) as Sprite;
        abilityUnlockHandle.transform.GetChild(3).GetComponent<Text>().text = unlockAbility.Name;
        abilityUnlockHandle.GetComponent<LerpScript>().LerpToScale(new Vector3(0.8f, 0.8f, 0.8f), new Vector3(1,1,1), 2);
        Color temp = abilityUnlockHandle.transform.GetChild(0).GetComponent<Image>().color;
        abilityUnlockHandle.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(temp, temp + new Color(0, 0, 0, 1), 2);
        temp = abilityUnlockHandle.transform.GetChild(1).GetComponent<Image>().color;
        abilityUnlockHandle.transform.GetChild(1).GetComponent<LerpScript>().LerpToColor(temp, temp + new Color(0, 0, 0, 1), 2);
        temp = abilityUnlockHandle.transform.GetChild(2).GetComponent<Text>().color;
        abilityUnlockHandle.transform.GetChild(2).GetComponent<LerpScript>().LerpToColor(temp, temp + new Color(0, 0, 0, 1), 2);
        temp = abilityUnlockHandle.transform.GetChild(3).GetComponent<Text>().color;
        abilityUnlockHandle.transform.GetChild(3).GetComponent<LerpScript>().LerpToColor(temp, temp + new Color(0, 0, 0, 1), 2);
        yield return new WaitForSeconds(0.35f);
        WaitForInput = true;
    }

    IEnumerator HideAbilityHandle()
    {
        abilityUnlockHandle.GetComponent<LerpScript>().LerpToScale(new Vector3(1, 1, 1), new Vector3(0.8f, 0.8f, 0.8f), 2);
        Color temp = abilityUnlockHandle.transform.GetChild(0).GetComponent<Image>().color;
        abilityUnlockHandle.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(temp, temp - new Color(0, 0, 0, 1), 2);
        temp = abilityUnlockHandle.transform.GetChild(1).GetComponent<Image>().color;
        abilityUnlockHandle.transform.GetChild(1).GetComponent<LerpScript>().LerpToColor(temp, temp - new Color(0, 0, 0, 1), 2);
        temp = abilityUnlockHandle.transform.GetChild(2).GetComponent<Text>().color;
        abilityUnlockHandle.transform.GetChild(2).GetComponent<LerpScript>().LerpToColor(temp, temp - new Color(0, 0, 0, 1), 2);
        temp = abilityUnlockHandle.transform.GetChild(3).GetComponent<Text>().color;
        abilityUnlockHandle.transform.GetChild(3).GetComponent<LerpScript>().LerpToColor(temp, temp - new Color(0, 0, 0, 1), 2);

        yield return new WaitForSeconds(1f);
        checkEquipmentUnlock();
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(1.5f);
        RewardToolsScript.tools.ClearReward();
        blackSq.GetComponent<FadeScript>().FadeIn();
        yield return new WaitForSeconds(1.5f);
        print(GameController.controller.currentEncounter.returnOnSuccessScene);
        SceneManager.LoadScene(GameController.controller.currentEncounter.returnOnSuccessScene);
    }
}
