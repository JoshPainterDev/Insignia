using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatScript : MonoBehaviour {

    private CombatManager combatManager;
    public GameObject enemyMannequin;
    public GameObject playerMannequin;

    public GameObject seamstress_StrikeFX;

    public int MAX_ATTEMPTS = 5;

    [HideInInspector]
    private Vector3 origPosition;
    private Vector3 playerOrigPos;
    private Vector3 strikePosition;

    [HideInInspector]
    private Difficulty difficulty;

    public EnemyInfo enemyInfo;

    public GameObject standardStrikeHit_FX;
    public GameObject standardStrikeMiss_FX;
    public GameObject blood01_FX;
    public GameObject blood02_FX;
    public GameObject blood03_FX;
    public GameObject blood04_FX;
    private GameObject bloodClone;
    private int abilityAttempts = 0;
    [HideInInspector]
    Ability ability1, ability2, ability3, ability4;
    private int cooldownA1, cooldownA2, cooldownA3, cooldownA4;

    private int originalPlayerHP;

    // Use this for initialization
    void Start ()
    {
        playerMannequin = GameController.controller.playerObject;
        combatManager = this.GetComponent<CombatManager>();
        origPosition = enemyMannequin.transform.position;
        playerOrigPos = playerMannequin.transform.position;
        strikePosition = origPosition - new Vector3(220,0,0);

        difficulty = GameController.controller.difficultyScale;
    }

    public void TickCooldowns()
    {

        if (cooldownA1 > 0)
            --cooldownA1;

        if (cooldownA2 > 0)
            --cooldownA2;

        if (cooldownA3 > 0)
            --cooldownA3;

        if (cooldownA4 > 0)
            --cooldownA4;
    }

    public void ResetEnemy(EnemyInfo info)
    {
        enemyInfo = info;
        ability1 = AbilityToolsScript.tools.LookUpAbility(enemyInfo.ability_1);
        ability2 = AbilityToolsScript.tools.LookUpAbility(enemyInfo.ability_2);
        ability3 = AbilityToolsScript.tools.LookUpAbility(enemyInfo.ability_3);
        ability4 = AbilityToolsScript.tools.LookUpAbility(enemyInfo.ability_4);
    }

    public void BeginEnemyTurn()
    {
        originalPlayerHP = combatManager.getPlayerHealth();

        switch (difficulty)
        {
            case Difficulty.Chill:
                EasyEnemyAI();
                break;
            case Difficulty.Story:
                //ability1 = AbilityToolsScript.tools.LookUpAbility("Hatred");
                //combatManager.HideHealthBars();
                //this.GetComponent<EnemyAbilityManager_C>().AbilityToUse(ability1, combatManager.getPlayerHealth());
                EasyEnemyAI();
                break;
            case Difficulty.Challenge:
                EasyEnemyAI();
                break;
        }
    }

    void EasyEnemyAI()
    {
        //first evaluate random chance to strike
        //regardless of abilities
        int chanceToStrike = Random.Range(0, 1);
        
        int rand = 0;

        if (abilityAttempts >= MAX_ATTEMPTS)
        {
            abilityAttempts = 0;
            chanceToStrike = 100;
        }

        if (chanceToStrike > 29)
        {
            float accuracy = 0;
            rand = Random.Range(0, 100);
            accuracy += 70 + (3 * ((enemyInfo.enemySpeed + combatManager.enemySpeedBoost) 
                - (GameController.controller.playerSpeed + combatManager.playerSpeedBoost)));

            print("Strike selected...");
            abilityAttempts = 0;

            if (combatManager.enemyBlinded)
            {
                combatManager.enemyBlinded = false;
                accuracy -= combatManager.BLINDED_REDUCTION;
                combatManager.currSpecialCase = SpecialCase.None;
            }

            // accuracy check the attack
            if (accuracy > rand)
            {
                print("ENEMY STRIKE INCOMING!");
                StartCoroutine(EnemyStrike());
            }
            else
            {
                print("whoops we missed...");
                EnemyMissStrike();
            }
        }
        else
        {
            int randomAbility = Random.Range(0, 4);

            ++abilityAttempts;
            randomAbility = 3;
            switch (randomAbility)
            {
                case 0:
                    if ((ability1.Name != "-") && (cooldownA1 == 0))
                    {
                        cooldownA1 = ability1.Cooldown + 1;
                        combatManager.HideHealthBars();
                        this.GetComponent<EnemyAbilityManager_C>().AbilityToUse(ability1, combatManager.getPlayerHealth());
                    }
                    else
                        EasyEnemyAI();
                    break;
                case 1:
                    if ((ability2.Name != "-") && (cooldownA2 == 0))
                    {
                        cooldownA2 = ability2.Cooldown + 1;
                        combatManager.HideHealthBars();
                        this.GetComponent<EnemyAbilityManager_C>().AbilityToUse(ability2, combatManager.getPlayerHealth());
                    }
                    else
                        EasyEnemyAI();
                    break;
                case 2:
                    if ((ability3.Name != "-") && (cooldownA3 == 0))
                    {
                        cooldownA3 = ability3.Cooldown + 1;
                        combatManager.HideHealthBars();
                        this.GetComponent<EnemyAbilityManager_C>().AbilityToUse(ability3, combatManager.getPlayerHealth());
                    }
                    else
                        EasyEnemyAI();
                    break;
                case 3:
                    if ((ability4.Name != "-") && (cooldownA4 == 0))
                    {
                        print("wtf");
                        cooldownA4 = ability4.Cooldown + 1;
                        combatManager.HideHealthBars();
                        this.GetComponent<EnemyAbilityManager_C>().AbilityToUse(ability4, combatManager.getPlayerHealth());
                    }
                    else
                        EasyEnemyAI();
                    break;
            }
        }
    }

    void MediumEnemyAI()
    {

    }

    void HardEnemyAI()
    {

    }

    IEnumerator EnemyStrike()
    {
        int damageDealt = 0;
        combatManager.HideHealthBars();

        if(enemyInfo.specialStrikeAnim)
        {
            switch(enemyInfo.enemyName)
            {
                case "The Seamstress":
                    enemyMannequin.GetComponent<LerpScript>().LerpToPos(origPosition, (origPosition - new Vector3(150,0,0)), 3f);
                    yield return new WaitForSeconds(0.5f);
                    enemyMannequin.transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 5);
                    yield return new WaitForSeconds(0.5f);
                    //enemyMannequin.GetComponent<LerpScript>().LerpToPos((origPosition - new Vector3(20, 0, 0)), origPosition - new Vector3(140, 0, 0), 3f);
                    //yield return new WaitForSeconds(0.5f);
                    GameObject effectClone = (GameObject)Instantiate(seamstress_StrikeFX, origPosition, transform.rotation);
                    effectClone.transform.parent = enemyMannequin.transform;
                    effectClone.transform.localPosition = new Vector3(-14, 3, 0);
                    damageDealt = combatManager.DamagePlayer_Strike();
                    this.GetComponent<CombatAudio>().playShadowVanish();
                    yield return new WaitForSeconds(0.25f);
                    enemyMannequin.transform.GetChild(0).GetChild(0).GetComponent<Animator>().speed = 0.75f;
                    GameObject effectClone2 = (GameObject)Instantiate(standardStrikeHit_FX, playerOrigPos, transform.rotation);
                    yield return new WaitForSeconds(0.75f);
                    enemyMannequin.GetComponent<LerpScript>().LerpToPos((origPosition - new Vector3(170, 0, 0)), origPosition, 2f);
                    enemyMannequin.transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 0);
                    enemyMannequin.transform.GetChild(0).GetChild(0).GetComponent<Animator>().speed = 1f;
                    yield return new WaitForSeconds(0.25f);
                    combatManager.EndEnemyTurn(damageDealt, originalPlayerHP);
                    break;
                case "Soul Stealer":
                    enemyMannequin.GetComponent<LerpScript>().LerpToPos(origPosition, strikePosition, 3f);
                    this.GetComponent<CombatAudio>().playRandomSwordMiss();
                    yield return new WaitForSeconds(0.1f);
                    this.GetComponent<CombatAudio>().playRandomSwordHit();
                    enemyMannequin.transform.GetChild(0).GetComponent<EnemyMannequinController>().playAttackAnim();
                    GameObject effectCloneSS = (GameObject)Instantiate(standardStrikeHit_FX, playerOrigPos, transform.rotation);
                    yield return new WaitForSeconds(0.6f);
                    damageDealt = combatManager.DamagePlayer_Strike();
                    enemyMannequin.GetComponent<LerpScript>().LerpToPos(strikePosition, origPosition, 3.5f);
                    yield return new WaitForSeconds(0.25f);
                    combatManager.EndEnemyTurn(damageDealt, originalPlayerHP);
                    break;
            }
        }
        else
        {
            enemyMannequin.GetComponent<LerpScript>().LerpToPos(origPosition, strikePosition, 3f);
            this.GetComponent<CombatAudio>().playRandomSwordMiss();
            yield return new WaitForSeconds(0.1f);
            this.GetComponent<CombatAudio>().playRandomSwordHit();
            enemyMannequin.transform.GetChild(0).GetComponent<EnemyMannequinController>().playAttackAnim();
            GameObject effectClone = (GameObject)Instantiate(standardStrikeHit_FX, playerOrigPos, transform.rotation);
            yield return new WaitForSeconds(0.6f);
            damageDealt = combatManager.DamagePlayer_Strike();
            enemyMannequin.GetComponent<LerpScript>().LerpToPos(strikePosition, origPosition, 3.5f);
            yield return new WaitForSeconds(0.25f);
            combatManager.EndEnemyTurn(damageDealt, originalPlayerHP);
        }
    }

    public void EnemyMissStrike()
    {
        StartCoroutine(AnimateEnemyMissStrike());
    }

    IEnumerator AnimateEnemyMissStrike()
    {
        combatManager.HideHealthBars();
        enemyMannequin.GetComponent<LerpScript>().LerpToPos(origPosition, strikePosition, 3f);
        yield return new WaitForSeconds(0.15f);
        enemyMannequin.transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 5);
        this.GetComponent<CombatAudio>().playRandomSwordMiss();
        yield return new WaitForSeconds(0.25f);
        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerOrigPos, playerOrigPos - new Vector3(70, 0, 0), 5f);
        Vector3 spawnPos = new Vector3(playerOrigPos.x, playerOrigPos.y - 15, 0);
        GameObject effectClone = (GameObject)Instantiate(standardStrikeMiss_FX, spawnPos, transform.rotation);
        yield return new WaitForSeconds(0.75f);
        enemyMannequin.GetComponent<LerpScript>().LerpToPos(strikePosition, origPosition, 3.5f);
        yield return new WaitForSeconds(1f);
        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerOrigPos - new Vector3(50, 0, 0), playerOrigPos, 2f);
        combatManager.ShowHealthBars();
        yield return new WaitForSeconds(0.2f);
        combatManager.EndEnemyTurn(0);
    }

    public void UseFakeStrike()
    {
        StartCoroutine(FakeEnemyStrike());
    }

    IEnumerator FakeEnemyStrike()
    {
        combatManager.HideHealthBars();
        enemyMannequin.GetComponent<LerpScript>().LerpToPos(origPosition, strikePosition, 3f);
        this.GetComponent<CombatAudio>().playRandomSwordMiss();
        yield return new WaitForSeconds(0.1f);
        enemyMannequin.transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 5);
        this.GetComponent<CombatAudio>().playStrikeHit();
        yield return new WaitForSeconds(0.6f);
        generateRandomBlood();
        Vector3 bloodPos = new Vector3(playerOrigPos.x, playerOrigPos.y, 0);
        bloodClone.transform.position = bloodPos;
        yield return new WaitForSeconds(0.15f);
        enemyMannequin.GetComponent<LerpScript>().LerpToPos(strikePosition, origPosition, 3.5f);
        yield return new WaitForSeconds(1f);
        combatManager.ShowHealthBars();
    }

    public void PlayDeathAnim()
    {
        foreach(Animator child in enemyMannequin.GetComponentsInChildren<Animator>())
        {
            child.SetInteger("AnimState", 2);
        }
    }

    private void generateRandomBlood()
    {
        int rand = Random.Range(1, 4);

        switch (rand)
        {
            case 1:
                bloodClone = (GameObject)Instantiate(blood01_FX, Vector3.zero, transform.rotation);
                break;
            case 2:
                bloodClone = (GameObject)Instantiate(blood02_FX, Vector3.zero, transform.rotation);
                break;
            case 3:
                bloodClone = (GameObject)Instantiate(blood03_FX, Vector3.zero, transform.rotation);
                break;
            case 4:
                bloodClone = (GameObject)Instantiate(blood04_FX, Vector3.zero, transform.rotation);
                break;
        }
    }
}
