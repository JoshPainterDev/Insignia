using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager_C : MonoBehaviour {

    private CombatManager combatManager;
    public GameObject playerMannequin;
    public GameObject enemyMannequin;

    public GameObject outrage_FX;
    public GameObject solarFlare_FX;
    public GameObject illusion_FX;
    public GameObject finalCut_FX;

    private Vector3 initPlayerPos;
    private Vector3 initEnemyPos;
    private Ability ability;
    private SpecialCase playerSpecialCase = SpecialCase.None;
    private SpecialCase enemySpecialCase = SpecialCase.None;

    private int origEnemyHP;

    // Use this for initialization
    void Start ()
    {
        initPlayerPos = playerMannequin.transform.position;
        initEnemyPos = enemyMannequin.transform.position;
        combatManager = this.GetComponent<CombatManager>();
    }

    //PLAYER TURN SEQUENCE
    //1. check for special case due to player ability
    //2. check for special case due to enemy ability
    //3. roll accuracy
    //4. buff
    //5. calculate damage
    //6. play animation
    //7. deal damage
    //8. end turn


    public void AbilityUsed(Ability abilityUsed, int enemyHP)
    {
        ability = abilityUsed;
        origEnemyHP = enemyHP;
        playerSpecialCase = ability.specialCase;

        StartCoroutine(AnimateAbility(ability.Name));

        switch (ability.Name)
        {
            case "Shadow Strike":
                break;
            case "Illusion":
                break;
            default:
                break;
        }
    }

    IEnumerator AnimateAbility(string abilityName)
    {
        GameObject effectClone;
        Vector3 spawnPos = Vector3.zero;
        switch (abilityName)
        {
            case "Outrage":
                spawnPos = initPlayerPos + new Vector3(0, 80, 0);
                effectClone = (GameObject)Instantiate(outrage_FX, spawnPos, transform.rotation);
                yield return new WaitForSeconds(0.25f);
                combatManager.currSpecialCase = SpecialCase.Outrage;
                combatManager.DamageEnemy_Ability(ability);
                yield return new WaitForSeconds(1);
                break;
            case "Illusion":
                spawnPos = initPlayerPos + new Vector3(0, 60, 0);
                effectClone = (GameObject)Instantiate(illusion_FX, spawnPos, transform.rotation);
                combatManager.currSpecialCase = SpecialCase.Illusion;
                yield return new WaitForSeconds(0.85f);
                break;
            case "Solar Flare":
                playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, playerMannequin.transform.position + new Vector3(-100, 0, 0), 3);
                yield return new WaitForSeconds(0.35f);
                spawnPos = initPlayerPos + new Vector3(0, 0, 0);
                effectClone = (GameObject)Instantiate(solarFlare_FX, spawnPos, transform.rotation);
                effectClone.transform.parent = playerMannequin.transform;
                effectClone.GetComponent<SpriteRenderer>().flipX = true;
                yield return new WaitForSeconds(0.75f);
                playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, playerMannequin.transform.position + new Vector3(300,0,0), 5);
                yield return new WaitForSeconds(0.25f);
                combatManager.DamageEnemy_Ability(ability);
                yield return new WaitForSeconds(0.7f);
                playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, initPlayerPos, 3);
                yield return new WaitForSeconds(0.85f);
                break;
            case "Final Cut":
                foreach (LerpScript script in playerMannequin.GetComponentsInChildren<LerpScript>())
                {
                    script.LerpToColor(Color.white, Color.clear, 5);
                }
                yield return new WaitForSeconds(0.5f);
                spawnPos = initPlayerPos + new Vector3(0, 0, 0);
                effectClone = (GameObject)Instantiate(finalCut_FX, spawnPos, transform.rotation);
                effectClone.transform.position = enemyMannequin.transform.position - new Vector3(300,0,0);
                effectClone.GetComponent<SpriteRenderer>().flipX = true;
                yield return new WaitForSeconds(0.35f);
                combatManager.DamageEnemy_Ability(ability);
                yield return new WaitForSeconds(1f);
                foreach(LerpScript script in playerMannequin.GetComponentsInChildren<LerpScript>())
                {
                    script.LerpToColor(Color.clear, Color.white, 5);
                }
                yield return new WaitForSeconds(0.25f);
                yield return new WaitForSeconds(0.85f);
                break;
            default:
                break;
        }

        if(ability.Type != AbilityType.Utility)
        {
            this.GetComponent<CombatManager>().DamageEnemy_Ability(ability);
        }

        yield return new WaitForSeconds(0.25f);

        // if the ability does damage, make sure to animate the damage
        if(ability.BaseDamage != 0)
            this.GetComponent<CombatManager>().EndPlayerTurn(true, origEnemyHP);
        else
            this.GetComponent<CombatManager>().EndPlayerTurn(false);

    }
}
