using UnityEngine;
using System.Collections;


public enum SpecialCase { Illusion, Linger_S, Linger_L, Blind, Execute, Outrage, Ablaze, ShadowClone, None };
public enum AbilityType { Physical, Magical, Utility, None };

public class Ability{
    public int Accuracy = 100;
    public int Cooldown = 3;
    public int AttackBoost = 0;
    public int DefenseBoost = 0;
    public int AttBoostDuration = 3;
    public int DefBoostDuration = 3;
    public int Ticks = 1;
    public int HealAmount = 0;
    public int AblazeChance = 0;
    public string Name = "";
    public AbilityType Type = AbilityType.None;
    public int BaseDamage = 0;
    public SpecialCase specialCase = SpecialCase.None;
    public bool Buffs = false;
    public bool Heals = false;
    public string Description = "";
}


