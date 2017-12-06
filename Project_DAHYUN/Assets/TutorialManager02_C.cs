using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TutorialManager02_C : MonoBehaviour
{
    public GameObject panel01;
    public GameObject panel02;
    public GameObject panel03;

    public GameObject gameCamera;
    public GameObject blackSq;
    public GameObject enemyMannequinn;
    public GameObject Steve;
    public GameObject shroudParticles;
    public GameObject playerHealth;
    public GameObject enemyHealth;
    public GameObject reap_FX;

    private GameObject textBox;
    private CombatManager combatController;
    private int tutorialState = 0;
    private int inputNumber = 0;
    private bool inputEnabled = true;
    // Use this for initialization
    void Start()
    {
        combatController = this.GetComponent<CombatManager>();
    }

    public void StrikeUsed()
    {
        if (!inputEnabled)
            return;

        inputEnabled = false;
        ++tutorialState;

        if(tutorialState < 2)
        {
            combatController.DisableMainButtons();
            combatController.HideMainButtons();
            StartCoroutine(FakeStrike());
        }
        else if (tutorialState == 2)
        {
            combatController.DisableMainButtons();
            combatController.HideMainButtons();
            Destroy(panel01);
            this.GetComponent<StruggleManager_C>().BeginStruggle_Player(false);
        }
    }

    IEnumerator FakeStrike()
    {
        this.GetComponent<StrikeManager_C>().StrikeUsed("Tutorial Strike", 100);
        yield return new WaitForSeconds(1);
        if(tutorialState == 1)
        {
            enemyHealth.GetComponent<HealthScript>().LerpHealth(1, 0.25f);
            yield return new WaitForSeconds(2);
            StartCoroutine(EnemyStrike());
        }
    }

    IEnumerator EnemyStrike()
    {
        this.GetComponent<EnemyCombatScript>().UseFakeStrike();
        yield return new WaitForSeconds(2);
        if (tutorialState == 1)
            playerHealth.GetComponent<HealthScript>().LerpHealth(1, 0.75f);
        else
            playerHealth.GetComponent<HealthScript>().LerpHealth(0.75f, 0.5f);
        yield return new WaitForSeconds(1);
        combatController.ShowMainButtons();
        combatController.EnableMainButtons();
        yield return new WaitForSeconds(0.25f);
        inputEnabled = true;

        if(tutorialState == 1)
        {
            panel01.GetComponent<Image>().enabled = true;
            panel01.transform.GetChild(0).GetComponent<Text>().enabled = true;
        }
    }

    public void StruggleFinished()
    {
        StartCoroutine(LoadExposition());
    }

    IEnumerator LoadExposition()
    {
        GameController.controller.currentEncounter = EncounterToolsScript.tools.SpecifyEncounter(1,0);
        blackSq.GetComponent<FadeScript>().FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("CombatReward_Scene");
    }
}
