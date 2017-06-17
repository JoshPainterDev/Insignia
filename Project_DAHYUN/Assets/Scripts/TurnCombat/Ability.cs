using UnityEngine;
using System.Collections;

public class Ability{

    public enum AbilityType {Physical, Magical, Tactical, None};
    public enum BuffTypes {Attack_Up1, Attack_Up2, Attack_Up3, None};

    public static string Name = "";
    public static AbilityType Type = AbilityType.None;
    public static BuffTypes Buff = BuffTypes.None;
    public static int Damage = 0;
    public static bool SpecialCase = false;
    public static bool Buffs = false;
    public static string Description = "";
}
