using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInfo{
    public enum EquipmentType { Head, Torso, Legs, Back, Gloves, Shoes, Sword, Claws, Bow, Staff, Aura, none};

    public enum EquipmentAnimation{ Sword, Claws, Bow, Staff, none};
    
    public int AttackStat = 0;
    public int DefenseStat = 0;
    public int SpeedStat = 0;
    public int ProwessStat = 0;
    public string Name = "";
    public EquipmentType EquipType;
    public EquipmentAnimation EquipAnimation;
    public string imgSourceName = "";
    public bool hideUnderLayer = true;
    public bool useMaskTexture = false;
    public string maskSpriteName = "";
    public Sprite maskTexture;
    public Color equipmentColor = Color.white;
}
