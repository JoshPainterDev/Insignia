using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LimitBreakName { Hellion_Form, Blood_Rage, Shadows_Embrace, Overdrive, Ascenion, Solaris_Invictus, none };

public class LimitBreak
{ 
    public LimitBreakName name = LimitBreakName.none;
    public int coolDown = 0;
    public int attackBoost = 0;
    public int defenseBoost = 0;
    public int prowessBoost = 0;
    public int speedBoost = 0;
    public string description = "";
}
