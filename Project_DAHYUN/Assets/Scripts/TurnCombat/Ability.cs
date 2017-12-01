using UnityEngine;
using System.Collections;


public enum SpecialCase { StunFoe, StunSelf, Blind, VulnerableSelf, Execute, Outrage, Ablaze, Deceive, ShadowClone, None };
[System.Serializable]
public enum AbilityType { Physical, Magical, Utility, None };

[System.Serializable]
public class Ability{
    public int Accuracy = 100;
    public int Cooldown = 3;
    public int AttackBoost = 0;
    public int DefenseBoost = 0;
    public int SpeedBoost = 0;
    public int AttBoostDuration = 3;
    public int DefBoostDuration = 3;
    public int SpdBoostDuration = 3;
    public int Ticks = 1;
    public int HealAmount = 0;
    public int AblazeChance = 0;
    public string Name = "";
    public AbilityType Type = AbilityType.None;
    public int BaseDamage = 0;
    public SpecialCase specialCase = SpecialCase.None;
    public int SpecialValue = 0;
    public bool Buffs = false;
    public bool Heals = false;
    public string Description = "";
    public string Icon = "";
}


