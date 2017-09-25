using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RewardManager_C : MonoBehaviour
{
    public GameObject experienceHandle;
    public GameObject blackSq;
    private Reward reward;

	// Use this for initialization
	void Start ()
    {
        reward = GameController.controller.rewardEarned;

        experienceHandle.GetComponent<ExperienceScript>().experienceAnimation(GameController.controller.playerEXP, reward.experience);
    }

    public void ExperienceIsDone()
    {
        StartCoroutine(GetTheGoods());
    }

    IEnumerator GetTheGoods()
    {
        yield return new WaitForSeconds(3f);
        RewardToolsScript.tools.ClearReward();
        blackSq.GetComponent<FadeScript>().FadeIn();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(GameController.controller.currentEncounter.returnOnSuccessScene);
    }
}
