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
    public GameObject reap_FX;
    public GameObject finalCut_FX;

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
        initEnemyPos = enemyMannequinn.transform.position;
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
                effectClone = (GameObject)Instantiate(outrage_FX, initEnemyPos - new Vector3(250,-50,0), transform.rotation);
                effectClone.GetComponent<SpriteRenderer>().flipX = false;
                yield return new WaitForSeconds(0.25f);
                combatManager.currSpecialCase = SpecialCase.Outrage;
                combatManager.DamagePlayer_Ability(ability);
                yield return new WaitForSeconds(1);
                break;
            case "Illusion":
                spawnPos = initEnemyPos - new Vector3(0, 60, 0);
                effectClone = (GameObject)Instantiate(illusion_FX, spawnPos, transform.rotation);
                combatManager.currSpecialCase = SpecialCase.Illusion;
                yield return new WaitForSeconds(0.85f);
                break;
            case "Solar Flare":
                enemyMannequinn.GetComponent<LerpScript>().LerpToPos(enemyMannequinn.transform.position, enemyMannequinn.transform.position - new Vector3(-100, 0, 0), 3);
                yield return new WaitForSeconds(0.35f);
                spawnPos = initPlayerPos + new Vector3(0, 0, 0);
                effectClone = (GameObject)Instantiate(solarFlare_FX, spawnPos, transform.rotation);
                effectClone.transform.parent = enemyMannequinn.transform;
                effectClone.GetComponent<SpriteRenderer>().flipX = true;
                effectClone.transform.localPosition += new Vector3(35,0,0);
                yield return new WaitForSeconds(0.75f);
                enemyMannequinn.GetComponent<LerpScript>().LerpToPos(enemyMannequinn.transform.position, enemyMannequinn.transform.position - new Vector3(300, 0, 0), 5);
                yield return new WaitForSeconds(0.25f);
                combatManager.DamagePlayer_Ability(ability);
                yield return new WaitForSeconds(0.7f);
                enemyMannequinn.GetComponent<LerpScript>().LerpToPos(enemyMannequinn.transform.position, initEnemyPos, 3);
                yield return new WaitForSeconds(0.85f);
                break;
                break;
            case "Reap":
                enemyMannequinn.GetComponent<LerpScript>().LerpToPos(initEnemyPos, enemyMannequinn.transform.position + new Vector3(100, 0, 0), 3);
                yield return new WaitForSeconds(0.25f);
                spawnPos = initEnemyPos + new Vector3(0, 0, 0);
                effectClone = (GameObject)Instantiate(reap_FX, spawnPos, transform.rotation);
                effectClone.transform.parent = enemyMannequinn.transform;
                effectClone.GetComponent<SpriteRenderer>().flipX = false;
                effectClone.transform.position -= new Vector3(80,-30,0);
                enemyMannequinn.GetComponent<LerpScript>().LerpToPos(enemyMannequinn.transform.position, initEnemyPos - new Vector3(300, 0, 0), 5);
                yield return new WaitForSeconds(0.25f);
                combatManager.DamagePlayer_Ability(ability);
                yield return new WaitForSeconds(0.7f);
                enemyMannequinn.GetComponent<LerpScript>().LerpToPos(enemyMannequinn.transform.position, initEnemyPos, 3);
                yield return new WaitForSeconds(0.85f);
                break;
            case "Final Cut": // FIX THIS
                foreach (LerpScript script in enemyMannequinn.GetComponentsInChildren<LerpScript>())
                {
                    script.LerpToColor(Color.white, Color.clear, 5);
                }
                yield return new WaitForSeconds(0.5f);
                spawnPos = initPlayerPos + new Vector3(0, 0, 0);
                effectClone = (GameObject)Instantiate(finalCut_FX, spawnPos, transform.rotation);
                effectClone.transform.position = playerMannequin.transform.position + new Vector3(30, 30, 0);
                effectClone.GetComponent<SpriteRenderer>().flipX = false;
                yield return new WaitForSeconds(0.35f);
                combatManager.DamagePlayer_Ability(ability);
                yield return new WaitForSeconds(1f);
                foreach (LerpScript script in enemyMannequinn.GetComponentsInChildren<LerpScript>())
                {
                    script.LerpToColor(Color.clear, Color.white, 5);
                }
                yield return new WaitForSeconds(0.25f);
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
