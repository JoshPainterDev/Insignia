using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInfo{
    public enum EquipmentType { Head, Torso, Legs, Back, Gloves, Shoes, Sword, Claws, Bow, none};

    public enum EquipmentAnimation{ Sword, Claws, Bow, none};
    
    public int AttackStat = 0;
    public int DefenseStat = 0;
    public int SpeedStat = 0;
    public int ProwessStat = 0;
    public string Name = "";
    public EquipmentType EquipType;
    public EquipmentAnimation EquipAnimation;
    public string imgSourceName = "";
    public bool hideUnderLayer = true;
}
