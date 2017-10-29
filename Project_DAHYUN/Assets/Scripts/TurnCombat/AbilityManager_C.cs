using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager_C : MonoBehaviour {

    private CombatManager combatManager;
    public GameObject playerMannequin;
    public GameObject enemyMannequin;

    public GameObject lightningBlue_FX;
    public GameObject lightningYellow_FX;
    public GameObject lightningStatic_FX;
    public GameObject lightningYellowBurst_FX;
    public GameObject lightningBigBurst_FX;

    public GameObject outrage_FX;
    public GameObject solarFlare_FX;
    public GameObject illusion_FX;
    public GameObject finalCut_FX;

    public GameObject smoke01_FX;
    public GameObject smoke02_FX;
    public GameObject smoke03_FX;

    public GameObject blood01_FX;
    public GameObject blood02_FX;
    public GameObject blood03_FX;
    public GameObject blood04_FX;

    private Vector3 initPlayerPos;
    private Vector3 initEnemyPos;
    private Ability ability;

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
        combatManager.currSpecialCase = SpecialCase.None;

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
            case "Thunder Strike":
                int dieRoll = Random.Range(0, 99);
                float chance = (50f + GameController.controller.playerAttack - combatManager.enemyInfo.enemyDefense);
                chance = Mathf.Clamp(chance, 50f, 100f);
                effectClone = (GameObject)Instantiate(lightningBlue_FX, initPlayerPos + new Vector3(0,10,0), transform.rotation);
                yield return new WaitForSeconds(0.85f);
                playerMannequin.GetComponent<LerpScript>().LerpToPos(initPlayerPos, initPlayerPos + new Vector3(300, 0, 0), 3);
                yield return new WaitForSeconds(0.15f);
                playerMannequin.GetComponent<AnimationController>().PlayAttackAnim();
                yield return new WaitForSeconds(0.5f);
                effectClone = (GameObject)Instantiate(lightningBigBurst_FX, initEnemyPos + new Vector3(0, 20, 0), transform.rotation);
                playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, initPlayerPos, 2);
                yield return new WaitForSeconds(0.75f);
                if (chance > dieRoll)
                    combatManager.currSpecialCase = SpecialCase.StunFoe;
                break;
            case "Guard Break":
                dieRoll = Random.Range(0, 99);
                chance = (75f + GameController.controller.playerProwess - combatManager.enemyInfo.enemyDefense);
                chance = Mathf.Clamp(chance, 75f, 100f);
                combatManager.enemyVulernable = true;
                playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, initPlayerPos + new Vector3(-40, 0, 0), 2);
                yield return new WaitForSeconds(0.2f);
                playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, initPlayerPos + new Vector3(350, 0, 0), 3);
                yield return new WaitForSeconds(0.65f);
                print("chance: " + chance);
                print("roll: " + dieRoll);
                if (chance > dieRoll)
                {
                    combatManager.currSpecialCase = SpecialCase.StunFoe;
                    effectClone = (GameObject)Instantiate(lightningBigBurst_FX, initEnemyPos + new Vector3(0, 20, 0), transform.rotation);
                }
                else
                    combatManager.currSpecialCase = SpecialCase.None;
                playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, initPlayerPos, 3);
                yield return new WaitForSeconds(1);
                break;
            case "Outrage":
                spawnPos = initPlayerPos + new Vector3(0, 80, 0);
                effectClone = (GameObject)Instantiate(outrage_FX, spawnPos, transform.rotation);
                yield return new WaitForSeconds(0.25f);
                combatManager.currSpecialCase = SpecialCase.Outrage;
                combatManager.DamageEnemy_Ability(ability);
                yield return new WaitForSeconds(1);
                break;
            case "Deceive":
                spawnPos = initPlayerPos + new Vector3(0, 60, 0);
                effectClone = (GameObject)Instantiate(illusion_FX, spawnPos, transform.rotation);
                combatManager.currSpecialCase = SpecialCase.Deceive;
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
                combatManager.currSpecialCase = SpecialCase.Execute;
                combatManager.GetComponent<CombatAudio>().playShadowVanish();
                GameObject effectClone2 = (GameObject)Instantiate(smoke02_FX, initPlayerPos, transform.rotation);
                effectClone2.GetComponent<SpriteRenderer>().color = Color.black;
                effectClone2.transform.position -= new Vector3(0, 30, 0);
                GameObject effectClone3 = (GameObject)Instantiate(smoke03_FX, initPlayerPos, transform.rotation);
                effectClone3.GetComponent<SpriteRenderer>().color = Color.black;
                effectClone3.transform.position += new Vector3(0, 30, 0);
                foreach (LerpScript script in playerMannequin.GetComponentsInChildren<LerpScript>())
                    script.LerpToColor(Color.white, Color.clear, 5);
                yield return new WaitForSeconds(0.5f);
                combatManager.GetComponent<CombatAudio>().playFinalCut();
                spawnPos = initPlayerPos + new Vector3(200, 20, 0);
                effectClone = (GameObject)Instantiate(finalCut_FX, spawnPos, transform.rotation);
                effectClone.transform.position = enemyMannequin.transform.position - new Vector3(60,0,0);
                effectClone.GetComponent<SpriteRenderer>().flipX = true;
                yield return new WaitForSeconds(1.25f);
                GameObject effectClone4 = (GameObject)Instantiate(blood04_FX, new Vector3(-320,145,0), transform.rotation);
                yield return new WaitForSeconds(0.5f);
                foreach (LerpScript script in playerMannequin.GetComponentsInChildren<LerpScript>())
                    script.LerpToColor(Color.clear, Color.white, 5);
                yield return new WaitForSeconds(0.25f);
                combatManager.DamageEnemy_Ability(ability);
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

    public void PlayStunAnim(bool playerStunned)
    {
        if (playerStunned)
            StartCoroutine(PlayerStunnedAnim());
        else
            StartCoroutine(EnemyStunnedAnim());
    }

    IEnumerator PlayerStunnedAnim()
    {
        Vector3 eOffset01 = new Vector3(15, 30, 0);
        Vector3 eOffset02 = new Vector3(-20, 0, 0);
        Vector3 eOffset03 = new Vector3(30, -15, 0);
        Vector3 offset = new Vector3(25, 0, 0);
        this.GetComponent<CombatAudio>().playStunnedSFX();
        GameObject electricity = (GameObject)Instantiate(lightningYellowBurst_FX, initPlayerPos - eOffset01, transform.rotation);
        electricity.transform.SetParent(playerMannequin.transform, true);
        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, playerMannequin.transform.position + offset, 1.2f);
        yield return new WaitForSeconds(0.25f);
        GameObject electricity2 = (GameObject)Instantiate(lightningYellowBurst_FX, initPlayerPos - eOffset02, transform.rotation);
        electricity.transform.SetParent(playerMannequin.transform, true);
        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, initPlayerPos - offset, 1.2f);
        yield return new WaitForSeconds(0.25f);
        GameObject electricity3 = (GameObject)Instantiate(lightningYellowBurst_FX, initPlayerPos - eOffset03, transform.rotation);
        electricity.transform.SetParent(playerMannequin.transform, true);
        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, initPlayerPos, 1.2f);
    }

    IEnumerator EnemyStunnedAnim()
    {
        Vector3 eOffset01 = new Vector3(15,40,0);
        Vector3 eOffset02 = new Vector3(-25, 0, 0);
        Vector3 eOffset03 = new Vector3(35, -20, 0);
        Vector3 offset = new Vector3(25, 0, 0);
        this.GetComponent<CombatAudio>().playStunnedSFX();
        GameObject electricity = (GameObject)Instantiate(lightningYellowBurst_FX, initEnemyPos + eOffset01, transform.rotation);
        electricity.transform.SetParent(enemyMannequin.transform, true);
        enemyMannequin.GetComponent<LerpScript>().LerpToPos(enemyMannequin.transform.position, enemyMannequin.transform.position + offset, 1.2f);
        yield return new WaitForSeconds(0.25f);
        GameObject electricity2 = (GameObject)Instantiate(lightningYellowBurst_FX, initEnemyPos + eOffset02, transform.rotation);
        electricity.transform.SetParent(enemyMannequin.transform, true);
        enemyMannequin.GetComponent<LerpScript>().LerpToPos(enemyMannequin.transform.position, initEnemyPos - offset, 1.2f);
        yield return new WaitForSeconds(0.25f);
        GameObject electricity3 = (GameObject)Instantiate(lightningYellowBurst_FX, initEnemyPos + eOffset03, transform.rotation);
        electricity.transform.SetParent(enemyMannequin.transform, true);
        enemyMannequin.GetComponent<LerpScript>().LerpToPos(enemyMannequin.transform.position, initEnemyPos, 1.2f);
    }

    public void UseTutorialAbility()
    {
        StartCoroutine(TutorialAbility());
    }

    IEnumerator TutorialAbility()
    {
        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, playerMannequin.transform.position + new Vector3(-100, 0, 0), 3);
        yield return new WaitForSeconds(0.35f);
        Vector3 spawnPos = initPlayerPos + new Vector3(0, 0, 0);
        GameObject effectClone = (GameObject)Instantiate(solarFlare_FX, spawnPos, transform.rotation);
        effectClone.transform.parent = playerMannequin.transform;
        effectClone.GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(0.75f);
        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, playerMannequin.transform.position + new Vector3(300, 0, 0), 5);
        yield return new WaitForSeconds(1f);
        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, initPlayerPos, 3);
    }
}
