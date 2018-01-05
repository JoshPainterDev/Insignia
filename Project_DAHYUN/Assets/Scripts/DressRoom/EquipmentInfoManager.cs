using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInfoManager : MonoBehaviour{

    public static EquipmentInfoManager equipmentInfoTool;

    public Sprite perlinTexture;
    public Sprite whiteSqTexture;
    public Sprite redBlackSpaceTexture;
    public Sprite monoSpaceTexture;

    private const string prefixHead = "Animations\\Equipment\\Head";
    private const string prefixTorso = "Animations\\Equipment\\Torso";
    private const string prefixLegs = "Animations\\Equipment\\Legs";
    private const string prefixBack = "Animations\\Equipment\\Back";
    private const string prefixGloves = "Animations\\Equipment\\Gloves";
    private const string prefixShoes = "Animations\\Equipment\\Shoes";
    private const string prefixWeapon = "Animations\\Equipment\\Weapons";
    private const string prefixAura = "Animations\\Equipment\\Aura";

    /// <Equipment List>
    /// 1. Novice Armor
    /// 2. Proto-Set
    /// 3. Slayer Armor
    /// 4.
    /// <>

    void Awake()
    {
        if (equipmentInfoTool == null)
        {
            DontDestroyOnLoad(gameObject);
            equipmentInfoTool = this;
        }
        else if (equipmentInfoTool != this)
        {
            Destroy(gameObject);
        }
    }

    public EquipmentInfo LookUpEquipment(int i, int j)
    {
        EquipmentInfo equipInfo = new EquipmentInfo();

        // HELMETS //
        if (i < 4)
        {
            switch (i)
            {
                case 0:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Novice Helmet";
                            equipInfo.AttackStat = 0;
                            equipInfo.DefenseStat = 1;
                            equipInfo.ProwessStat = 0;
                            equipInfo.SpeedStat = 0;
                            equipInfo.hideUnderLayer = false;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            equipInfo.imgSourceName = prefixHead + "\\Novice_Armor\\Player_Head_NoviceHelm_AnimController";
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            equipInfo.imgSourceName = prefixHead + "\\Test_Suit\\Player_Head_TestHelm_AnimController";
                            break;
                        case 2:
                            equipInfo.Name = "Slayer Mask";
                            equipInfo.AttackStat = 5;
                            equipInfo.DefenseStat = 3;
                            equipInfo.ProwessStat = 3;
                            equipInfo.SpeedStat = 1;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            equipInfo.imgSourceName = prefixHead + "\\Slayer_Outfit\\Player_Head_SlayerHead_AnimController";
                            equipInfo.hideUnderLayer = false;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
                case 1:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 2:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
                case 2:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 2:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
                case 3:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 2:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
            }
        }
        // TORSOS //
        else if (i < 8)
        {
            switch (i)
            {
                case 4:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Novice Chestplate";
                            equipInfo.AttackStat = 0;
                            equipInfo.DefenseStat = 1;
                            equipInfo.ProwessStat = 0;
                            equipInfo.SpeedStat = 0;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Torso;
                            equipInfo.imgSourceName = prefixTorso + "\\Novice_Armor\\Player_Torso_NoviceTorso_AnimController";
                            equipInfo.hideUnderLayer = true;
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Torso;
                            equipInfo.imgSourceName = prefixTorso + "\\Test_Suit\\Player_Torso_TestTorso_AnimController";
                            break;
                        case 2:
                            equipInfo.Name = "Slayer Chest";
                            equipInfo.AttackStat = 3;
                            equipInfo.DefenseStat = 5;
                            equipInfo.ProwessStat = 1;
                            equipInfo.SpeedStat = 0;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Torso;
                            equipInfo.imgSourceName = prefixTorso + "\\Slayer_Outfit\\Player_Torso_SlayerTorso_AnimController";
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
                case 5:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 2:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
                case 6:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 2:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
                case 7:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 2:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
            }
        }
        // LEGS //
        else if (i < 12)
        {
            switch (i)
            {
                case 8:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Novice Leg Guards";
                            equipInfo.AttackStat = 0;
                            equipInfo.DefenseStat = 1;
                            equipInfo.ProwessStat = 0;
                            equipInfo.SpeedStat = 0;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Legs;
                            equipInfo.imgSourceName = prefixLegs + "\\Novice_Armor\\Player_Legs_NoviceLegs_AnimController";
                            break;
                        case 1:
                            equipInfo.Name = "Test Legs";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Legs;
                            equipInfo.imgSourceName = prefixLegs + "\\Test_Suit\\Player_Legs_TestLegs_AnimController";
                            break;
                        case 2:
                            equipInfo.Name = "Slayer Legs";
                            equipInfo.AttackStat = 2;
                            equipInfo.DefenseStat = 3;
                            equipInfo.ProwessStat = 1;
                            equipInfo.SpeedStat = 0;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Legs;
                            equipInfo.imgSourceName = prefixLegs + "\\Slayer_Outfit\\Player_Legs_SlayerLegs_AnimController";
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
                case 9:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 2:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
                case 10:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 2:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
                case 11:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 2:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
            }
        }
        // Back //
        else if (i < 16)
        {
            switch (i)
            {
                case 12:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Squire's Shield";
                            equipInfo.AttackStat = 0;
                            equipInfo.DefenseStat = 1;
                            equipInfo.ProwessStat = 0;
                            equipInfo.SpeedStat = 0;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Back;
                            equipInfo.imgSourceName = prefixBack + "\\Novice_Armor\\Player_Back_BackShield_AnimController";
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Back;
                            equipInfo.imgSourceName = prefixBack + "\\Test_Suit\\Player_Back_TestCape_AnimController";
                            break;
                        case 2:
                            equipInfo.Name = "Slayer Cape";
                            equipInfo.AttackStat = 1;
                            equipInfo.DefenseStat = 0;
                            equipInfo.ProwessStat = 2;
                            equipInfo.SpeedStat = 1;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Back;
                            equipInfo.imgSourceName = prefixBack + "\\Slayer_Outfit\\Player_Back_SlayerCape_AnimController";
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
                case 13:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 2:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
                case 14:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 2:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
                case 15:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 2:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
            }
        }
        // Gloves //
        else if (i < 20)
        {
            switch (i)
            {
                case 16:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Novice Gloves";
                            equipInfo.AttackStat = 1;
                            equipInfo.DefenseStat = 1;
                            equipInfo.ProwessStat = 0;
                            equipInfo.SpeedStat = 0;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Gloves;
                            equipInfo.imgSourceName = prefixGloves + "\\Novice_Armor\\Player_Gloves_NoviceGloves_AnimController";
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Gloves;
                            equipInfo.imgSourceName = prefixGloves + "\\Test_Suit\\Player_Gloves_TestGloves_AnimController";
                            break;
                        case 2:
                            equipInfo.Name = "Slayer Gloves";
                            equipInfo.AttackStat = 2;
                            equipInfo.DefenseStat = 2;
                            equipInfo.ProwessStat = 1;
                            equipInfo.SpeedStat = 0;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Gloves;
                            equipInfo.imgSourceName = prefixGloves + "\\Slayer_Outfit\\Player_Gloves_SlayerGloves_AnimController";
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
                case 17:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 2:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
                case 18:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 2:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
                case 19:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 2:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
            }
        }
        // SHOES //
        else if (i < 24)
        {
            switch (i)
            {
                case 20:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Novice Boots";
                            equipInfo.AttackStat = 0;
                            equipInfo.DefenseStat = 1;
                            equipInfo.ProwessStat = 0;
                            equipInfo.SpeedStat = 0;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Shoes;
                            equipInfo.imgSourceName = prefixShoes + "\\Novice_Armor\\Player_Shoes_NoviceBoots_AnimController";
                            break;
                        case 1:
                            equipInfo.Name = "Test Shoes";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Shoes;
                            equipInfo.imgSourceName = prefixShoes + "\\Test_Suit\\Player_Shoes_TestShoes_AnimController";
                            break;
                        case 2:
                            equipInfo.Name = "Slayer Greaves";
                            equipInfo.AttackStat = 1;
                            equipInfo.DefenseStat = 2;
                            equipInfo.ProwessStat = 2;
                            equipInfo.SpeedStat = 1;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Shoes;
                            equipInfo.imgSourceName = prefixShoes + "\\Slayer_Outfit\\Player_Shoes_SlayerShoes_AnimController";
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
                case 21:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 2:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
                case 22:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 2:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
                case 23:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 2:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
            }
        }
        // WEAPON //
        else if (i < 28)
        {
            Color weaponColor = Color.white;

            switch (i)
            {
                case 24:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Novice Sword";
                            equipInfo.AttackStat = 5;
                            equipInfo.DefenseStat = 0;
                            equipInfo.ProwessStat = 2;
                            equipInfo.SpeedStat = 0;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Sword;
                            equipInfo.imgSourceName = prefixWeapon + "\\Novice_Sword\\Player_Weapon_NoviceSword_AnimController";
                            break;
                        case 1:
                            equipInfo.Name = "Dark Mors";
                            equipInfo.AttackStat = 9;
                            equipInfo.DefenseStat = 7;
                            equipInfo.ProwessStat = 7;
                            equipInfo.SpeedStat = 3;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Sword;
                            equipInfo.imgSourceName = prefixWeapon + "\\DarkMors_Sword\\Player_Weapon_DarkMors_AnimController";
                            break;
                        case 2:
                            equipInfo.Name = "Slayer Sword";
                            equipInfo.AttackStat = 7;
                            equipInfo.DefenseStat = 1;
                            equipInfo.ProwessStat = 3;
                            equipInfo.SpeedStat = 3;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Sword;
                            equipInfo.imgSourceName = prefixWeapon + "\\Slayer_Sword\\Player_Weapon_SlayerSword_AnimController";
                            break;
                        case 3:
                            equipInfo.Name = "Doom Blade";
                            equipInfo.AttackStat = 6;
                            equipInfo.DefenseStat = 3;
                            equipInfo.ProwessStat = 2;
                            equipInfo.SpeedStat = 1;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Sword;
                            equipInfo.imgSourceName = prefixWeapon + "\\Doom_Sword\\Player_Weapon_DoomBlade_AnimController";
                            break;
                    }
                    break;
                case 25:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Test Sword";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Sword;
                            equipInfo.imgSourceName = prefixWeapon + "\\Test_Sword\\Player_Weapon_TestSword_AnimController";
                            break;
                        case 1:
                            equipInfo.Name = "Galaxy's Edge";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Sword;
                            equipInfo.imgSourceName = prefixWeapon + "\\Test_Sword\\Player_Weapon_SlayerSword_AnimController";
                            equipInfo.useMaskTexture = true;
                            equipInfo.maskTexture = monoSpaceTexture;
                            weaponColor = GameController.controller.getPlayerColorPreference();
                            equipInfo.equipmentColor = weaponColor;
                            break;
                        case 2:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
                case 26:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 2:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
                case 27:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 2:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
            }
        }
        // AURA //
        else if (i > 27)
        {
            Color auraColor = Color.white;

            switch (i)
            {
                case 28:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "SuperNova Aura";
                            equipInfo.AttackStat = 3;
                            equipInfo.DefenseStat = 0;
                            equipInfo.ProwessStat = 0;
                            equipInfo.SpeedStat = 3;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Aura;
                            equipInfo.imgSourceName = prefixAura + "\\SuperNova_Aura\\Player_Aura_SuperNova_AnimController";
                            equipInfo.useMaskTexture = true;
                            equipInfo.maskTexture = whiteSqTexture;
                            auraColor = GameController.controller.getPlayerColorPreference();
                            auraColor.a = 255;
                            equipInfo.equipmentColor = auraColor;
                            break;
                        case 1:
                            equipInfo.Name = "Storm Aura";
                            equipInfo.AttackStat = 8;
                            equipInfo.DefenseStat = 2;
                            equipInfo.ProwessStat = 2;
                            equipInfo.SpeedStat = 10;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Aura;
                            equipInfo.imgSourceName = prefixAura + "\\Storm_Aura\\Player_Aura_Storm_AnimController";
                            equipInfo.useMaskTexture = true;
                            equipInfo.maskTexture = perlinTexture;
                            auraColor = GameController.controller.getPlayerColorPreference();
                            auraColor.a = 0.9f;
                            equipInfo.equipmentColor = auraColor;
                            break;
                        case 2:
                            equipInfo.Name = "Blaze Aura";
                            equipInfo.AttackStat = 3;
                            equipInfo.DefenseStat = 0;
                            equipInfo.ProwessStat = 0;
                            equipInfo.SpeedStat = 3;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Aura;
                            equipInfo.imgSourceName = prefixAura + "\\SuperNova_Aura\\Player_Aura_SuperNova_AnimController";
                            equipInfo.equipmentColor = Color.white;
                            break;
                        case 3:
                            equipInfo.Name = "idklol aura";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Aura;
                            equipInfo.imgSourceName = prefixAura + "\\Test_Aura\\Player_Aura_Blaze_AnimController";
                            break;
                    }
                    break;
                case 29:
                    switch (j)
                    {
                        case 0:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 1:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 2:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                        case 3:
                            equipInfo.Name = "Test Helmet";
                            equipInfo.AttackStat = 69;
                            equipInfo.DefenseStat = 69;
                            equipInfo.ProwessStat = 69;
                            equipInfo.SpeedStat = 69;
                            equipInfo.EquipType = EquipmentInfo.EquipmentType.Head;
                            break;
                    }
                    break;
            }

        }
        return equipInfo;
    }
}
