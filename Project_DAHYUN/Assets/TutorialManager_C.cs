using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager_C : MonoBehaviour
{
    public int tutorialNumber;
    private CombatManager combatController;

	// Use this for initialization
	void Start ()
    {
        combatController = this.GetComponent<CombatManager>();	
	}

    public void BeginTutorial()
    {
        switch(tutorialNumber)
        {
            case 1:
                StartCoroutine(tutorial01());
                break;
        }
    }

    IEnumerator tutorial01()
    {
        yield return new WaitForSeconds(1);
    }
}
