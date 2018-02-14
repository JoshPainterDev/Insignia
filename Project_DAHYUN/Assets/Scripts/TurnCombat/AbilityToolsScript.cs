using System.Collections;
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

    public Ability IndexToAbilityLookUp(int index)
    {
        string AbilityName = "";

        switch(index)
        {
            case 0:
                AbilityName = "none";
                break;
            case 1:
                AbilityName = "Final Cut";
                break;
            case 2:
                AbilityName = "Deceive";
                break;
            case 3:
                AbilityName = "Stranglehold";
                break;
            case 4:
                AbilityName = "Solar Flare";
                break;
            case 5:
                AbilityName = "Hatred";
                break;
            case 6:
                AbilityName = "Outrage";
                break;
            case 7:
                AbilityName = "Shadow Clone";
                break;
            case 8:
                AbilityName = "Thunder Charge";
                break;
            case 9:
                AbilityName = "Guard Break";
                break;
            case 10:
                AbilityName = "Black Rain";
                break;
            case 11:
                AbilityName = "Divine Shield";
                break;
            case 12:
                AbilityName = "Stangle";
                break;
            case 13:
                AbilityName = "Murder-Stroke";
                break;
        }

        return LookUpAbility(AbilityName);
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
                ability.Icon = "AbilityIcons\\none_AbilityIcon";
                ability.AbilityIndex = 0;
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
                ability.AbilityIndex = 1;
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
                ability.AbilityIndex = 2;
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
            //    ability.AbilityIndex = 3;
            //    break;
            case "Solar Flare":
                ability.Accuracy = 85;
                ability.BaseDamage = 5;
                ability.Cooldown = 2;
                ability.Buffs = false;
                ability.Description = "A focused fire blast of star magic that blinds foes.";
                ability.Name = name;
                ability.specialCase = SpecialCase.Blind;
                ability.Ticks = 1;
                ability.Type = AbilityType.Magical;
                ability.Icon = "AbilityIcons\\SolarFlare_AbilityIcon";
                ability.AbilityIndex = 4;
                break;
            case "Hatred":
                ability.Accuracy = 100;
                ability.BaseDamage = 0;
                ability.Buffs = true;
                ability.AttackBoost = 2;
                ability.AttBoostDuration = 2;
                ability.Heals = true;
                ability.HealAmount = WEAK_HEAL;
                ability.Description = "Seeth in bloodlust and anger. Attack boost +2 for 2 turns.";
                ability.Name = name;
                ability.specialCase = SpecialCase.None;
                ability.Ticks = 0;
                ability.Type = AbilityType.Utility;
                ability.Icon = "AbilityIcons\\Hatred_AbilityIcon";
                ability.AbilityIndex = 5;
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
                ability.AbilityIndex = 6;
                break;
            case "Shadow Clone":
                ability.Accuracy = 100;
                ability.BaseDamage = 0;
                ability.Description = "Summon 2 servents of shadow to fight by your side.";
                ability.Name = name;
                ability.specialCase = SpecialCase.ShadowClone;
                ability.Ticks = 0;
                ability.Type = AbilityType.Magical;
                ability.AbilityIndex = 7;
                break;
            case "Thunder Charge":
                ability.Accuracy = 85;
                ability.BaseDamage = 10;
                ability.Cooldown = 3;
                ability.Buffs = false;
                ability.Description = "A lightning imbued blade charge. Has a chance to stun your foe.";
                ability.Name = name;
                ability.specialCase = SpecialCase.StunFoe;
                ability.Ticks = 0;
                ability.Type = AbilityType.Physical;
                ability.Icon = "AbilityIcons\\ThunderCharge_AbilityIcon";
                ability.AbilityIndex = 8;
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
                ability.Icon = "AbilityIcons\\GuardBreak_AbilityIcon";
                ability.AbilityIndex = 9;
                break;
            case "Black Rain":
                ability.Accuracy = 90;
                ability.BaseDamage = 15;
                ability.Cooldown = 3;
                ability.Buffs = false;
                ability.Description = "1,000 Needles of Dark Magic rain down on your foes.";
                ability.Name = name;
                ability.specialCase = SpecialCase.None;
                ability.Ticks = 0;
                ability.Type = AbilityType.Magical;
                ability.Icon = "AbilityIcons\\BlackRain_AbilityIcon";
                ability.AbilityIndex = 10;
                break;
            case "Divine Barrier":
                ability.Accuracy = 100;
                ability.BaseDamage = 0;
                ability.DefenseBoost = 2;
                ability.DefBoostDuration = 2;
                ability.Cooldown = 3;
                ability.Buffs = true;
                ability.Description = "Channel holy light as a protective shielding around you. Boost Defense by 2";
                ability.Name = name;
                ability.specialCase = SpecialCase.None;
                ability.Ticks = 0;
                ability.Type = AbilityType.Utility;
                ability.Icon = "AbilityIcons\\DivineShield_AbilityIcon";
                ability.AbilityIndex = 10;
                break;
            case "Strangle":
                ability.Accuracy = 100;
                ability.BaseDamage = 15;
                ability.Cooldown = 3;
                ability.Buffs = false;
                ability.Description = "Channeling dark energy, you reach out and crush your opponent in your grasp.";
                ability.Name = name;
                ability.specialCase = SpecialCase.None;
                ability.Ticks = 0;
                ability.Type = AbilityType.Magical;
                ability.Icon = "AbilityIcons\\Strangle_AbilityIcon";
                ability.AbilityIndex = 10;
                break;
            case "Murder-Stroke":
                ability.Accuracy = 85;
                ability.BaseDamage = 10;
                ability.Cooldown = 3;
                ability.Buffs = false;
                ability.Description = "A puncturing strike that causes your foe to become vulnerable.";
                ability.Name = name;
                ability.specialCase = SpecialCase.None;
                ability.Ticks = 0;
                ability.Type = AbilityType.Utility;
                ability.Icon = "AbilityIcons\\Murder-Stroke_AbilityIcon";
                ability.AbilityIndex = 11;
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
                ability.AbilityIndex = 0;
                break;
        }

        return ability;
    }
}
