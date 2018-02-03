using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager_C : MonoBehaviour {

    private CombatManager combatManager;
    private GameObject cameraObj;
    public GameObject playerMannequin;
    public GameObject enemyMannequin;
    public GameObject boostHandle;
    public GameObject blackSq;

    public GameObject boostPrefab;

    public GameObject lightningBlue_FX;
    public GameObject lightningYellow_FX;
    public GameObject lightningStatic_FX;
    public GameObject lightningYellowBurst_FX;
    public GameObject lightningBigBurst_FX;

    public GameObject thunderCharge_FX;
    public GameObject outrage_FX;
    public GameObject solarFlare_FX;
    public GameObject illusion_FX;
    public GameObject finalCut_FX;
    public GameObject blackRain_FX;
    public GameObject strangle_FX;

    public GameObject rush_FX;
    public GameObject smoke01_FX;
    public GameObject smoke02_FX;
    public GameObject smoke03_FX;
    public GameObject abilityMiss_FX;

    public GameObject blood01_FX;
    public GameObject blood02_FX;
    public GameObject blood03_FX;
    public GameObject blood04_FX;

    private Vector3 initPlayerPos;
    private Vector3 initEnemyPos;
    private Ability ability;

    private int origEnemyHP, damageReturn;

    // Use this for initialization
    void Start ()
    {
        combatManager = this.GetComponent<CombatManager>();
        playerMannequin = GameController.controller.playerObject;
        initPlayerPos = playerMannequin.transform.position;
        initEnemyPos = enemyMannequin.transform.position;
        cameraObj = combatManager.cameraObj;
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


    public void AbilityToUse(Ability abilityUsed, int enemyHP)
    {
        ability = abilityUsed;
        origEnemyHP = enemyHP;
        combatManager.currSpecialCase = SpecialCase.None;
        damageReturn = 0;

        GameController.controller.playerEvilPoints += ability.EvilPoints;
        GameController.controller.playerGoodPoints += ability.GoodPoints;

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
            case "Hatred":
                spawnPos = initPlayerPos + new Vector3(0, 80, 0);
                effectClone = (GameObject)Instantiate(outrage_FX, spawnPos, transform.rotation);
                this.GetComponent<CombatAudio>().playOutrageSFX();
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(AttackBoostAnim(ability.AttackBoost, ability.AttBoostDuration));
                yield return new WaitForSeconds(2f);
                break;
            case "Thunder Charge":
                int dieRoll = Random.Range(0, 99);
                float chance = (50f + GameController.controller.playerAttack - combatManager.enemyInfo.enemyDefense);
                chance = Mathf.Clamp(chance, 50f, 100f);
                this.GetComponent<CombatAudio>().playThunderChargeFX();
                effectClone = (GameObject)Instantiate(thunderCharge_FX, initPlayerPos + new Vector3(20, 80, 0), transform.rotation);
                //GameObject effectClone02 = (GameObject)Instantiate(lightningYellow_FX, initPlayerPos + new Vector3(10, 40, 0), transform.rotation);
                //GameObject effectClone03 = (GameObject)Instantiate(lightningYellow_FX, initPlayerPos + new Vector3(-10, 40, 0), transform.rotation);
                //effectClone03.GetComponent<SpriteRenderer>().flipX = true;
                yield return new WaitForSeconds(1.5f);
                playerMannequin.GetComponent<AnimationController>().PlayHoldAttackAnim();
                yield return new WaitForSeconds(0.2f);
                GameObject effectClone04 = (GameObject)Instantiate(lightningYellow_FX, initPlayerPos + new Vector3(250, 40, 0), transform.rotation);
                effectClone04.transform.eulerAngles = new Vector3(0,0,90);
                playerMannequin.GetComponent<LerpScript>().LerpToPos(initPlayerPos, initPlayerPos + new Vector3(420, 0, 0), 5);
                blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.yellow, 10.0f);
                yield return new WaitForSeconds(0.2f);
                blackSq.GetComponent<FadeScript>().FadeColored(Color.white, Color.clear, 2.0f);
                yield return new WaitForSeconds(0.5f);
                damageReturn = combatManager.DamageEnemy_Ability(ability);
                yield return new WaitForSeconds(0.5f);
                if (chance > dieRoll)
                {
                    combatManager.currSpecialCase = SpecialCase.StunFoe;
                    effectClone = (GameObject)Instantiate(lightningBigBurst_FX, initEnemyPos + new Vector3(0, 20, 0), transform.rotation);
                }
                yield return new WaitForSeconds(0.75f);
                playerMannequin.GetComponent<AnimationController>().PlayIdleAnim();
                playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, initPlayerPos, 2);
                yield return new WaitForSeconds(0.75f);
                break;
            case "Guard Break":
                dieRoll = Random.Range(0, 99);
                chance = (85f + GameController.controller.playerProwess - combatManager.enemyInfo.enemyDefense);
                chance = Mathf.Clamp(chance, 85f, 100f);
                combatManager.enemyVulnernable = true;
                this.GetComponent<CombatAudio>().playGuardBreakSFX();
                playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, initPlayerPos + new Vector3(-40, 0, 0), 2);
                yield return new WaitForSeconds(0.2f);
                effectClone = (GameObject)Instantiate(rush_FX, playerMannequin.transform.position + new Vector3(15, 5, 0), transform.rotation);
                effectClone.transform.SetParent(playerMannequin.transform, true);
                playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, initPlayerPos + new Vector3(350, 0, 0), 3);
                yield return new WaitForSeconds(0.65f);
                damageReturn = combatManager.DamageEnemy_Ability(ability);
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
            case "Black Rain":
                spawnPos = initPlayerPos + new Vector3(80, 100, 0);
                yield return new WaitForSeconds(0.25f);
                effectClone = (GameObject)Instantiate(blackRain_FX, spawnPos, transform.rotation);
                //this.GetComponent<CombatAudio>().playOutrageSFX();
                yield return new WaitForSeconds(0.35f);
                damageReturn = combatManager.DamageEnemy_Ability(ability);
                effectClone.GetComponent<Animator>().speed = 0.0f;
                yield return new WaitForSeconds(0.65f);
                playerMannequin.GetComponent<AnimationController>().PlayAttackAnim();
                yield return new WaitForSeconds(0.1f);
                effectClone.GetComponent<Animator>().speed = 1.45f;
                yield return new WaitForSeconds(0.5f);
                break;
            case "Outrage":
                spawnPos = initPlayerPos + new Vector3(0, 80, 0);
                effectClone = (GameObject)Instantiate(outrage_FX, spawnPos, transform.rotation);
                this.GetComponent<CombatAudio>().playOutrageSFX();
                yield return new WaitForSeconds(0.25f);
                combatManager.currSpecialCase = ability.specialCase;
                damageReturn = combatManager.DamageEnemy_Ability(ability);
                yield return new WaitForSeconds(1);
                break;
            case "Deceive":
                spawnPos = initPlayerPos + new Vector3(0, 60, 0);
                effectClone = (GameObject)Instantiate(illusion_FX, spawnPos, transform.rotation);
                combatManager.currSpecialCase = ability.specialCase;
                yield return new WaitForSeconds(0.85f);
                break;
            case "Solar Flare":
                playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, playerMannequin.transform.position + new Vector3(-100, 0, 0), 3);
                yield return new WaitForSeconds(0.35f);
                spawnPos = initPlayerPos;
                effectClone = (GameObject)Instantiate(solarFlare_FX, spawnPos, transform.rotation);
                effectClone.transform.parent = playerMannequin.transform;
                effectClone.transform.position += new Vector3(0, 40, 0);
                effectClone.GetComponent<SpriteRenderer>().flipX = true;
                this.GetComponent<CombatAudio>().playOutrageSFX();
                yield return new WaitForSeconds(0.75f);
                playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, playerMannequin.transform.position + new Vector3(300,0,0), 5);
                yield return new WaitForSeconds(0.25f);
                damageReturn = combatManager.DamageEnemy_Ability(ability);
                combatManager.currSpecialCase = ability.specialCase;
                yield return new WaitForSeconds(0.7f);
                playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, initPlayerPos, 3);
                yield return new WaitForSeconds(0.85f);
                break;
            case "Final Cut":
                combatManager.currSpecialCase = SpecialCase.Execute;
                combatManager.GetComponent<CombatAudio>().playSwordRing();
                yield return new WaitForSeconds(0.2f);
                combatManager.GetComponent<CombatAudio>().playShadowVanish();
                GameObject effectClone2 = (GameObject)Instantiate(smoke02_FX, initPlayerPos, transform.rotation);
                effectClone2.GetComponent<SpriteRenderer>().color = Color.black;
                effectClone2.transform.position -= new Vector3(0, 30, 0);
                GameObject effectClone3 = (GameObject)Instantiate(smoke03_FX, initPlayerPos, transform.rotation);
                effectClone3.GetComponent<SpriteRenderer>().color = Color.black;
                effectClone3.transform.position += new Vector3(0, 30, 0);
                int i = 0;
                Color skinColor = GameController.controller.getPlayerSkinColor();
                foreach (LerpScript script in playerMannequin.GetComponentsInChildren<LerpScript>())
                {
                    if(i > 7 && i < 12)
                    {
                        script.LerpToColor(skinColor, Color.clear, 5);
                    }
                    else
                        script.LerpToColor(Color.white, Color.clear, 5);

                    ++i;
                }
                yield return new WaitForSeconds(0.5f);
                combatManager.GetComponent<CombatAudio>().playFinalCut();
                spawnPos = initPlayerPos + new Vector3(200, 20, 0);
                effectClone = (GameObject)Instantiate(finalCut_FX, spawnPos, transform.rotation);
                effectClone.transform.position = enemyMannequin.transform.position - new Vector3(60,0,0);
                effectClone.GetComponent<SpriteRenderer>().flipX = true;
                yield return new WaitForSeconds(1.25f);
                GameObject effectClone4 = (GameObject)Instantiate(blood04_FX, new Vector3(-320,145,0), transform.rotation);
                yield return new WaitForSeconds(0.5f);
                i = 0;
                foreach (LerpScript script in playerMannequin.GetComponentsInChildren<LerpScript>())
                {
                    if (i > 7 && i < 12)
                    {
                        script.LerpToColor(Color.clear, skinColor, 5);
                    }
                    else
                        script.LerpToColor(Color.clear, Color.white, 5);

                    ++i;
                }
                yield return new WaitForSeconds(0.25f);
                damageReturn = combatManager.DamageEnemy_Ability(ability);
                yield return new WaitForSeconds(0.85f);
                break;
            case "Divine Barrier":
                spawnPos = initPlayerPos + new Vector3(0, 80, 0);
                effectClone = (GameObject)Instantiate(outrage_FX, spawnPos, transform.rotation);
                this.GetComponent<CombatAudio>().playOutrageSFX();
                yield return new WaitForSeconds(0.25f);
                StartCoroutine(DefenseBoostAnim(ability.DefenseBoost, ability.DefBoostDuration));
                yield return new WaitForSeconds(2f);
                break;
            case "Strangle":
                spawnPos = new Vector3(0, 0, 0);
                effectClone = (GameObject)Instantiate(strangle_FX, spawnPos, transform.rotation);
                effectClone.transform.SetParent(playerMannequin.transform, false);
                effectClone.transform.GetChild(0).GetComponent<StrangleScript_C>().StartAnim(true, playerMannequin, enemyMannequin);
                playerMannequin.GetComponent<AnimationController>().PlayForcePushAnim();
                yield return new WaitForSeconds(1.5f);
                cameraObj.GetComponent<CameraController>().ShakeCamera(1, true, 2f);
                yield return new WaitForSeconds(2.5f);
                damageReturn = combatManager.DamageEnemy_Ability(ability);
                playerMannequin.GetComponent<AnimationController>().PlayIdleAnim();
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(0.25f);

        // if the ability does damage, make sure to animate the damage
        if(ability.BaseDamage != 0)
            this.GetComponent<CombatManager>().EndPlayerTurn(damageReturn, origEnemyHP);
        else
            this.GetComponent<CombatManager>().EndPlayerTurn(damageReturn);

    }

    public void PlayStunAnim(bool playerStunned)
    {
        if (playerStunned)
            StartCoroutine(PlayerStunnedAnim());
        else
            StartCoroutine(EnemyStunnedAnim());
    }

    //Attack Boost
    IEnumerator AttackBoostAnim(int level, int duration)
    {
        print("ATTACK BOOST SET: " + level);
        combatManager.playerAttackBoost = level;
        combatManager.setBoostDur("Attack", duration, true);

        GameObject effectClone = (GameObject)Instantiate(boostPrefab, initPlayerPos + new Vector3(100, 100, 0), transform.rotation);
        effectClone.GetComponent<StatBoost_C>().boostType = BoostType.Attack;
        effectClone.GetComponent<StatBoost_C>().player = 1;
        yield return new WaitForSeconds(2.25f);
        boostHandle.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.clear, Color.white, 2.5f);
    } 

    //Defense Boost
    IEnumerator DefenseBoostAnim(int level, int duration)
    {
        print("DEFENSE BOOST SET: " + level);
        combatManager.playerDefenseBoost = level;
        combatManager.setBoostDur("Defense", duration, true);

        GameObject effectClone = (GameObject)Instantiate(boostPrefab, initPlayerPos + new Vector3(100, 100, 0), transform.rotation);
        effectClone.GetComponent<StatBoost_C>().boostType = BoostType.Defense;
        effectClone.GetComponent<StatBoost_C>().player = 1;
        yield return new WaitForSeconds(2.25f);
        boostHandle.transform.GetChild(1).GetComponent<LerpScript>().LerpToColor(Color.clear, Color.white, 2.5f);
    }

    //Speed Boost
    IEnumerator SpeedBoostAnim(int level, int duration)
    {
        print("SPEED BOOST SET: " + level);
        combatManager.playerSpeedBoost = level;
        combatManager.setBoostDur("Speed", duration, true);

        GameObject effectClone = (GameObject)Instantiate(boostPrefab, initPlayerPos + new Vector3(100, 100, 0), transform.rotation);
        effectClone.GetComponent<StatBoost_C>().boostType = BoostType.Speed;
        effectClone.GetComponent<StatBoost_C>().player = 1;
        yield return new WaitForSeconds(2.25f);
        boostHandle.transform.GetChild(2).GetComponent<LerpScript>().LerpToColor(Color.clear, Color.white, 2.5f);
    }

    public void PlayerAbilityMiss()
    {
        StartCoroutine(AnimatePlayerAbilityMiss());
    }

    IEnumerator AnimatePlayerAbilityMiss()
    {
        enemyMannequin.GetComponent<LerpScript>().LerpToPos(initEnemyPos, initEnemyPos + new Vector3(70, 0, 0), 5f);
        Vector3 spawnPos = new Vector3(initEnemyPos.x, initEnemyPos.y - 25, 0);
        GameObject effectClone = (GameObject)Instantiate(abilityMiss_FX, spawnPos, transform.rotation);
        this.GetComponent<CombatAudio>().playRandomSwordMiss();
        yield return new WaitForSeconds(0.5f);
        enemyMannequin.GetComponent<LerpScript>().LerpToPos(initEnemyPos + new Vector3(50, 0, 0), initEnemyPos, 2f);
    }

    public void EnemyAbilityMiss()
    {
        StartCoroutine(AnimateEnemyAbilityMiss());
    }

    IEnumerator AnimateEnemyAbilityMiss()
    {
        yield return new WaitForSeconds(0.15f);
        this.GetComponent<CombatAudio>().playRandomSwordMiss();
        yield return new WaitForSeconds(0.25f);
        playerMannequin.GetComponent<LerpScript>().LerpToPos(initPlayerPos, initPlayerPos - new Vector3(70, 0, 0), 5f);
        Vector3 spawnPos = new Vector3(initPlayerPos.x, initPlayerPos.y - 15, 0);
        GameObject effectClone = (GameObject)Instantiate(abilityMiss_FX, spawnPos, transform.rotation);
        yield return new WaitForSeconds(1f);
        playerMannequin.GetComponent<LerpScript>().LerpToPos(initPlayerPos - new Vector3(70, 0, 0), initPlayerPos, 2f);
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
        effectClone.transform.position += new Vector3(0, 40, 0);
        this.GetComponent<CombatAudio>().playOutrageSFX();
        yield return new WaitForSeconds(0.75f);
        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, playerMannequin.transform.position + new Vector3(300, 0, 0), 5);
        yield return new WaitForSeconds(1f);
        playerMannequin.GetComponent<LerpScript>().LerpToPos(playerMannequin.transform.position, initPlayerPos, 3);
    }
}
