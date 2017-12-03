﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityToolsScript : MonoBehaviour {
    public static AbilityToolsScript tools;

    public int STRONG_DAMAGE = 15;
    public int MEDIUM_DAMAGE = 5;
    public int WEAK_DAMAGE = 2;

    public int STRONG_HEAL = 45;
    public int MEDIUM_HEAL = 25;
    public int WEAK_HEAL = 10;

    public int STRONG_ABLAZE = 75;
    public int MEDIUM_ABLAZE = 50;
    public int WEAK_ABLAZE = 20;

    // Use this for initialization
    void Awake()
    {
        if (tools == null)
        {
            tools = this;
        }
        else if (tools != this)
        {
            Destroy(gameObject);
        }
    }

    public Ability LookUpAbility(string name)
    {
        Ability ability = new Ability();

        switch (name)
        {
            case "none":
                ability.Accuracy = 0;
                ability.BaseDamage = 0;
                ability.Description = "-";
                ability.Cooldown = 0;
                ability.Name = "-";
                ability.specialCase = SpecialCase.None;
                ability.Ticks = 0;
                ability.Type = AbilityType.None;
                break;
            case "Final Cut":
                ability.Accuracy = 85;
                ability.BaseDamage = 2;
                ability.Cooldown = 3;
                ability.Buffs = false;
                ability.Description = "Strike from the shadows to deliver a fatal blow.";
                ability.Name = name;
                ability.specialCase = SpecialCase.Execute;
                ability.SpecialValue = 7;
                ability.Type = AbilityType.Physical;
                break;
            case "Deceive":
                ability.Accuracy = 100;
                ability.BaseDamage = 0;
                ability.Buffs = false;
                ability.Description = "If attacked this turn, dodge the attack and Press your opponent.";
                ability.Name = name;
                ability.specialCase = SpecialCase.Deceive;
                ability.Ticks = 0;
                ability.Type = AbilityType.Utility;
                break;
            //case "Stranglehold":
            //    ability.Accuracy = 100;
            //    ability.BaseDamage = WEAK_DAMAGE;
            //    ability.Buffs = false;
            //    ability.Description = "Reach out and strangle your foe from the shadows. Lingers for 5 turns.";
            //    ability.Name = name;
            //    ability.specialCase = SpecialCase.Linger_L;
            //    ability.Ticks = 1;
            //    ability.Type = AbilityType.Magical;
            //    break;
            case "Solar Flare":
                ability.Accuracy = 75;
                ability.BaseDamage = 4;
                ability.Cooldown = 2;
                ability.Buffs = false;
                ability.Description = "A focused fire blast of star magic.";
                ability.Name = name;
                //ability.specialCase = SpecialCase.Ablaze;
                ability.Ticks = 1;
                ability.AblazeChance = STRONG_ABLAZE;
                ability.Type = AbilityType.Magical;
                break;
            case "Rage":
                ability.Accuracy = 100;
                ability.BaseDamage = 0;
                ability.Buffs = true;
                ability.AttackBoost = 2;
                ability.AttBoostDuration = 3;
                ability.Heals = true;
                ability.HealAmount = WEAK_HEAL;
                ability.Description = "Harness your pain and hatred. Attack boost +2.";
                ability.Name = name;
                ability.specialCase = SpecialCase.None;
                ability.Ticks = 0;
                ability.Type = AbilityType.Utility;
                break;
            case "Outrage":
                ability.Accuracy = 100;
                ability.BaseDamage = 3;
                ability.Cooldown = 2;
                ability.Description = "Explode in a fiery rage. Consumes your attack boost to deal more damage.";
                ability.Name = name;
                ability.specialCase = SpecialCase.Outrage;
                ability.SpecialValue = 10;
                ability.Ticks = 0;
                ability.Type = AbilityType.Magical;
                break;
            case "Shadow Clone":
                ability.Accuracy = 100;
                ability.BaseDamage = 0;
                ability.Description = "Summon 2 servents of shadow to fight by your side.";
                ability.Name = name;
                ability.specialCase = SpecialCase.ShadowClone;
                ability.Ticks = 0;
                ability.Type = AbilityType.Magical;
                break;
            case "Thunder Charge":
                ability.Accuracy = 85;
                ability.BaseDamage = 25;
                ability.Cooldown = 3;
                ability.Buffs = false;
                ability.Description = "A lightning imbued strike. Has a chance to stun your foe.";
                ability.Name = name;
                ability.specialCase = SpecialCase.StunFoe;
                ability.Ticks = 0;
                ability.Type = AbilityType.Magical;
                break;
            case "Guard Break":
                ability.Accuracy = 100;
                ability.BaseDamage = 2;
                ability.Cooldown = 2;
                ability.Buffs = false;
                ability.Description = "Charge your foe and break through their defenses.";
                ability.Name = name;
                ability.specialCase = SpecialCase.StunFoe;
                ability.Ticks = 0;
                ability.Type = AbilityType.Utility;
                break;
            case "Black Rain":
                ability.Accuracy = 100;
                ability.BaseDamage = 30;
                ability.Cooldown = 3;
                ability.Buffs = false;
                ability.Description = "1,000 Needles of Shadow rain down upon your foes.";
                ability.Name = name;
                ability.specialCase = SpecialCase.None;
                ability.Ticks = 0;
                ability.Type = AbilityType.Magical;
                break;
            default:
                ability.Name = "";
                ability.Accuracy = 0;
                ability.Description = "";
                ability.BaseDamage = 0;
                ability.Name = name;
                ability.specialCase = SpecialCase.None;
                ability.Ticks = 0;
                ability.Type = AbilityType.None;
                break;
        }

        return ability;
    }
}
