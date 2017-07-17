using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityToolsScript : MonoBehaviour {
    public static AbilityToolsScript tools;

    public int STRONG_DAMAGE = 55;
    public int MEDIUM_DAMAGE = 35;
    public int WEAK_DAMAGE = 15;

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
            case "Shadow Strike":
                ability.Accuracy = 100;
                ability.BaseDamage = MEDIUM_DAMAGE;
                ability.Buffs = false;
                ability.Description = "Strike from the shadows to deliver a fatal blow.";
                ability.Name = name;
                ability.specialCase = SpecialCase.Execute;
                ability.Ticks = 1;
                ability.Type = AbilityType.Physical;
                break;
            case "Illusion":
                ability.Accuracy = 100;
                ability.BaseDamage = 0;
                ability.Buffs = true;
                ability.AttackBoost = 2;
                ability.AttBoostDuration = 3;
                ability.Heals = true;
                ability.HealAmount = WEAK_HEAL;
                ability.Description = "If attacked this turn, dodge the attack and Press your opponent.";
                ability.Name = name;
                ability.specialCase = SpecialCase.Illusion;
                ability.Ticks = 1;
                ability.Type = AbilityType.Magical;
                break;
            case "Stranglehold":
                ability.Accuracy = 100;
                ability.BaseDamage = WEAK_DAMAGE;
                ability.Buffs = false;
                ability.Description = "Reach out and strangle your foe from the shadows. Lingers for 5 turns.";
                ability.Name = name;
                ability.specialCase = SpecialCase.Linger_L;
                ability.Ticks = 1;
                ability.Type = AbilityType.Magical;
                break;
            case "Solar Flare":
                ability.Accuracy = 75;
                ability.BaseDamage = STRONG_DAMAGE;
                ability.Buffs = false;
                ability.Description = "A strong, focused, fire blast. Has a Strong chance of setting the opponent Ablaze.";
                ability.Name = name;
                ability.specialCase = SpecialCase.Ablaze;
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
                ability.Description = "Harness your pain and hatred. Attack boost +2, heal for a small amount.";
                ability.Name = name;
                ability.specialCase = SpecialCase.None;
                ability.Ticks = 0;
                ability.Type = AbilityType.Utility;
                break;
            case "Outrage":
                ability.Accuracy = 100;
                ability.BaseDamage = WEAK_DAMAGE;
                ability.Description = "Explode in a fiery rage. Consumes your attack boost to deal more damage.";
                ability.Name = name;
                ability.specialCase = SpecialCase.Outrage;
                ability.Ticks = 0;
                ability.Type = AbilityType.Magical;
                break;
            default:
                break;
        }

        return ability;
    }
}
