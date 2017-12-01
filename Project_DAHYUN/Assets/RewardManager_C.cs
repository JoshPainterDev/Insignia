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
    private bool levelUp = false;
    private bool WaitForInput = false;

    private bool unlockingEquips = false;

    // Use this for initialization
    void Start()
    {
        reward = GameController.controller.rewardEarned;
        if (experienceHandle.GetComponent<ExperienceScript>().experienceAnimation(GameController.controller.playerEXP, reward.experience))
            levelUp = true;
        playerMannequin.GetComponent<AnimationController>().PlayAttackAnim();
    }

    public void UnlockSequenceFinished(int index)
    {
        switch(index)
        {
            case 0:
                if (reward.hasEquipment)
                {
                    unlockingEquips = true;
                    StartCoroutine(UnlockEquipment(0));
                }
                else if (reward.hasAbility)
                {
                    StartCoroutine(UnlockAbility(0));
                }
                else
                    StartCoroutine(LoadNextScene());
                break;
            case 1:
                if (reward.hasAbility)
                {
                    StartCoroutine(UnlockAbility(0));
                }
                else if(levelUp)
                {
                    print("Player Level: " + GameController.controller.playerLevel);
                    reward.ability = RewardToolsScript.tools.CheckForUnlock(GameController.controller.playerLevel);

                    if(reward.ability.Name != "none")
                        StartCoroutine(UnlockAbility(1));
                }
                else
                    StartCoroutine(LoadNextScene());
                break;
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

    IEnumerator UnlockEquipment(int index)
    {
        int myIndex = index;


        yield return new WaitForSeconds(1.5f);
        ++myIndex;
        if (myIndex < reward.equipment.Length)
            StartCoroutine(UnlockEquipment(myIndex));
        else
            UnlockSequenceFinished(1);
    }

    IEnumerator UnlockAbility(int index)
    {
        //set image
        abilityUnlockHandle.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load(reward.ability.Icon, typeof(Sprite)) as Sprite;
        abilityUnlockHandle.transform.GetChild(3).GetComponent<Text>().text = reward.ability.Name;
        abilityUnlockHandle.GetComponent<LerpScript>().LerpToScale(new Vector3(0.8f, 0.8f, 0.8f), new Vector3(1,1,1), 1);
        Color temp = abilityUnlockHandle.transform.GetChild(0).GetComponent<Image>().color;
        abilityUnlockHandle.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(temp, temp + new Color(0,0,0,1), 1);
        temp = abilityUnlockHandle.transform.GetChild(1).GetComponent<Image>().color;
        abilityUnlockHandle.transform.GetChild(1).GetComponent<LerpScript>().LerpToColor(temp, temp + new Color(0, 0, 0, 1), 1);
        temp = abilityUnlockHandle.transform.GetChild(2).GetComponent<Text>().color;
        abilityUnlockHandle.transform.GetChild(2).GetComponent<LerpScript>().LerpToColor(temp, temp + new Color(0, 0, 0, 1), 1);
        temp = abilityUnlockHandle.transform.GetChild(3).GetComponent<Text>().color;
        abilityUnlockHandle.transform.GetChild(3).GetComponent<LerpScript>().LerpToColor(temp, temp + new Color(0, 0, 0, 1), 1);
        yield return new WaitForSeconds(2f);
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
        blackSq.GetComponent<FadeScript>().FadeIn(0.75f);
        yield return new WaitForSeconds(2f);
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3f);
        RewardToolsScript.tools.ClearReward();
        blackSq.GetComponent<FadeScript>().FadeIn();
        yield return new WaitForSeconds(1.5f);
        print(GameController.controller.currentEncounter.returnOnSuccessScene);
        SceneManager.LoadScene(GameController.controller.currentEncounter.returnOnSuccessScene);
    }
}
