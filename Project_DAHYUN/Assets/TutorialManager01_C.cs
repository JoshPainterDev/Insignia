using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager01_C : MonoBehaviour
{
    public GameObject panel01;
    public GameObject panel02;

    private CombatManager combatController;
    private int tutorialState = 0;
    private int inputNumber = 0;
    // Use this for initialization
    void Start ()
    {
        combatController = this.GetComponent<CombatManager>();	
	}

    IEnumerator tutorial01()
    {
        yield return new WaitForSeconds(1);
    }

    public void StrikeUsed()
    {
        if (tutorialState != 0)
            return;

        ++tutorialState;

        combatController.DisableMainButtons();
        combatController.HideMainButtons();
        Destroy(panel01);
        StartCoroutine(FakeStrike());
    }

    IEnumerator FakeStrike()
    {
        this.GetComponent<StrikeManager_C>().StrikeUsed("Tutorial Strike", 100);
        yield return new WaitForSeconds(1);
    }
}
