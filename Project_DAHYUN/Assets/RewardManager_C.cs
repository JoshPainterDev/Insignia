using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RewardManager_C : MonoBehaviour
{
    public GameObject experienceHandle;
    public GameObject blackSq;
    public GameObject playerMannequin;
    private Reward reward;

    private bool unlockingEquips = false;

    // Use this for initialization
    void Start()
    {
        reward = GameController.controller.rewardEarned;
        experienceHandle.GetComponent<ExperienceScript>().experienceAnimation(GameController.controller.playerEXP, reward.experience);
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
                else
                    StartCoroutine(LoadNextScene());
                break;
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
        yield return new WaitForSeconds(3f);

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
