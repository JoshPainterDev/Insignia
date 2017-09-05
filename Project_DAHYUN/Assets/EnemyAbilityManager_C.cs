using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbilityManager_C : MonoBehaviour
{
    private CombatManager combatManager;
    public GameObject enemyMannequinn;
    public GameObject playerMannequin;

    public GameObject outrage_FX;
    public GameObject solarFlare_FX;
    public GameObject illusion_FX;

    private Vector3 initEnemyPos;
    private Vector3 initPlayerPos;
    private Ability ability;
    private SpecialCase playerSpecialCase = SpecialCase.None;
    private SpecialCase enemySpecialCase = SpecialCase.None;

    private int origPlayerHP;

    // Use this for initialization
    void Start()
    {
        initPlayerPos = playerMannequin.transform.position;
        initEnemyPos = playerMannequin.transform.position;
        combatManager = this.GetComponent<CombatManager>();
    }

    //ENEMY TURN SEQUENCE
    //1. check for special case due to player ability
    //2. check for special case due to enemy ability
    //3. roll accuracy
    //4. buff
    //5. calculate damage
    //6. play animation
    //7. deal damage
    //8. end turn


    public void AbilityUsed(Ability abilityUsed, int playerHP)
    {
        ability = abilityUsed;
        origPlayerHP = playerHP;
        playerSpecialCase = ability.specialCase;

        StartCoroutine(AnimateAbility(ability.Name));
    }

    IEnumerator AnimateAbility(string abilityName)
    {
        GameObject effectClone;
        Vector3 spawnPos = Vector3.zero;
        switch (abilityName)
        {
            case "Outrage":
                spawnPos = initEnemyPos - new Vector3(0, 80, 0);
                effectClone = (GameObject)Instantiate(outrage_FX, spawnPos, transform.rotation);
                yield return new WaitForSeconds(0.25f);
                combatManager.currSpecialCase = SpecialCase.Outrage;
                combatManager.DamageEnemy_Ability(ability);
                yield return new WaitForSeconds(1);
                break;
            case "Illusion":
                spawnPos = initEnemyPos - new Vector3(0, 60, 0);
                effectClone = (GameObject)Instantiate(illusion_FX, spawnPos, transform.rotation);
                combatManager.currSpecialCase = SpecialCase.Illusion;
                yield return new WaitForSeconds(0.85f);
                break;
            case "Solar Flare":
                spawnPos = initEnemyPos - new Vector3(0, 0, 0);
                effectClone = (GameObject)Instantiate(solarFlare_FX, spawnPos, transform.rotation);
                effectClone.transform.parent = playerMannequin.transform;
                yield return new WaitForSeconds(0.25f);
                combatManager.currSpecialCase = SpecialCase.Outrage;
                combatManager.DamageEnemy_Ability(ability);
                yield return new WaitForSeconds(0.85f);
                break;
            default:
                break;
        }

        if (ability.Type != AbilityType.Utility)
        {
            this.GetComponent<CombatManager>().DamagePlayer_Ability(ability);
        }

        yield return new WaitForSeconds(0.25f);
        EndTurn();
    }

    void EndTurn()
    {


        switch (ability.Name)
        {
            case "Shadow Strike":
                break;
            case "Illusion":

                break;
            default:
                break;
        }

        // if the ability does damage, make sure to animate the damage
        if (ability.BaseDamage != 0)
            this.GetComponent<CombatManager>().EndEnemyTurn(true, origPlayerHP);
        else
            this.GetComponent<CombatManager>().EndEnemyTurn(false);
    }
}
