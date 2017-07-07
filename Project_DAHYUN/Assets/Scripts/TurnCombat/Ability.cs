using UnityEngine;
using System.Collections;


public enum SpecialCase { Illusion, None };
public enum AbilityType { Physical, Magical, Utility, None };

public class Ability{
    public int Accuracy = 100;
    public int AttackBoost = 0;
    public int DefenseBoost = 0;
    public int AttBoostDuration = 3;
    public int DefBoostDuration = 3;
    public int ticks = 1;

    public string Name = "";
    public AbilityType Type = AbilityType.None;
    public int baseDamage = 0;
    public SpecialCase specialCase = SpecialCase.None;
    public bool Buffs = false;
    public string Description = "";
}
