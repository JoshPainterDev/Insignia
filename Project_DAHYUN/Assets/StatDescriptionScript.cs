using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StatDescriptionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void StatDescription(int stat)
    {
        switch(stat)
        {
            case 0:
                this.GetComponent<Text>().text = "Attack increases Strike and Ability damage.";
                break;
            case 1:
                this.GetComponent<Text>().text = "Defense grants you more health and reduces incoming damage.";
                break;
            case 2:
                this.GetComponent<Text>().text = "Prowess increases your chance of Executing foes when you Strike. It also increases damage on some Abilities.";
                break;
            case 3:
                this.GetComponent<Text>().text = "Speed increases your chance of attacking first and makes you harder to hit. It also increases damage on some Abilities.";
                break;
        }
    }
}
