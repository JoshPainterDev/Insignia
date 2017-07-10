using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager_C : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void AbilityUsed(Ability ability)
    {
        //Ability abilityUsed = AbilityTools.tools.LookUpAbility(ability.Name);

        switch(ability.Name)
        {
            case "Shadow Strike":
                print("kill em dead wit dat gud shit"); 
                break;
            default:
                break;
        }
    }
}
