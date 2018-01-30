using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LootManager_C : MonoBehaviour
{
    private GameObject player;


    // Use this for initialization
    void Start()
    {

    }

    public void GenerateLoot()
    {
        player.GetComponent<AnimationController>().PlayCheerAnim();

        StartCoroutine(LootSequence());

        StartCoroutine(LoadReturnScene());
    }

    IEnumerator LootSequence()
    {
        // this could get complicated depending on where I'm supposed to return to
        // store the return level in the game controller
        //blackSq.GetComponent<FadeScript>().FadeIn();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(GameController.controller.currentEncounter.returnOnSuccessScene);
    }

    IEnumerator LoadReturnScene()
    {
        // this could get complicated depending on where I'm supposed to return to
        // store the return level in the game controller
        //blackSq.GetComponent<FadeScript>().FadeIn();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(GameController.controller.currentEncounter.returnOnSuccessScene);
    }
}
