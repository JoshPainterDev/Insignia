using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitBreakManager_C : MonoBehaviour {

    

    // Use this for initialization
    void Start () {
		
	}

    public void UseLimitBreak(LimitBreak limitBreak)
    {

    }

    public LimitBreak LookUpLimitBreak(LimitBreakName lbName)
    {
        LimitBreak limitBreak = new LimitBreak();

        switch(lbName)
        {
            case LimitBreakName.Ascenion:
                limitBreak.attackBoost = 1;
                limitBreak.coolDown = 1;
                limitBreak.defenseBoost = 1;
                limitBreak.prowessBoost = 1;
                limitBreak.speedBoost = 1;
                limitBreak.description = "Take up the holy form of the Ascended One. Encapsulated in the light of the Sun, take up the sword of justice.";
                break;
            case LimitBreakName.Blood_Rage:
                limitBreak.attackBoost = 3;
                limitBreak.coolDown = 2;
                limitBreak.defenseBoost = 1;
                limitBreak.speedBoost = 1;
                limitBreak.description = "Now that you see red, they will see nothing. Harness your anger in a manefestation of onslaught.";
                break;
            case LimitBreakName.Hellion_Form:
                limitBreak.attackBoost = 2;
                limitBreak.coolDown = 1;
                limitBreak.defenseBoost = 0;
                limitBreak.speedBoost = 2;
                limitBreak.description = "'A bat out of hell...' Said to be the physical form of fear, your body takes on an altered demon-like state.";
                break;
            case LimitBreakName.Overdrive:
                limitBreak.attackBoost = 1;
                limitBreak.coolDown = 2;
                limitBreak.defenseBoost = 1;
                limitBreak.speedBoost = 1;
                limitBreak.description = "Overcharging your cybernetic enhancements is not only painful, but very dangerous... for them.";
                break;
            case LimitBreakName.Shadows_Embrace:
                limitBreak.attackBoost = 2;
                limitBreak.coolDown = 2;
                limitBreak.defenseBoost = 0;
                limitBreak.prowessBoost = 2;
                limitBreak.speedBoost = 1;
                limitBreak.description = "Embrace the shadows, and they will perish in darkness.";
                break;
            case LimitBreakName.Solaris_Invictus:
                limitBreak.attackBoost = 3;
                limitBreak.coolDown = 2;
                limitBreak.defenseBoost = 0;
                limitBreak.speedBoost = 1;
                limitBreak.description = "Embody the blaze of the Sun as you take up the torch of Solaris.";
                break;
            case LimitBreakName.none:
                limitBreak.attackBoost = 0;
                limitBreak.coolDown = 0;
                limitBreak.defenseBoost = 0;
                limitBreak.speedBoost = 0;
                limitBreak.prowessBoost = 0;
                limitBreak.description = "";
                break;
            default:
                break;
        }

        return limitBreak;
    }
}
