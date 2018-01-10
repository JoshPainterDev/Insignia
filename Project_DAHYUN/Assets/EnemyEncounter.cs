using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Environment {Castel_Hall, none};

public class EnemyEncounter
{
    public int encounterNumber;
    public int totalEnemies = 0;
    public string[] enemyNames;
    public bool[] bossFight;
    public string returnOnSuccessScene;
    public Environment environment;
    public Reward reward;
}
