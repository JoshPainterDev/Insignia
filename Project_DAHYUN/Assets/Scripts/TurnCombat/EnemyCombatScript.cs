using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatScript : MonoBehaviour {

    private CombatManager combatManager;
    public GameObject enemyMannequin;
    public GameObject playerMannequin;

    [HideInInspector]
    private Vector3 origPosition;
    private Vector3 playerOrigPos;
    private Vector3 strikePosition;

    [HideInInspector]
    private int difficulty;

    public EnemyInfo enemyInfo;

    public GameObject blood01_FX;
    public GameObject blood02_FX;
    public GameObject blood03_FX;
    public GameObject blood04_FX;
    private GameObject bloodClone;
    [HideInInspector]
    Ability ability1, ability2, ability3, ability4;
    private int cooldownA1, cooldownA2, cooldownA3, cooldownA4;

    private int originalPlayerHP;

    // Use this for initialization
    void Start ()
    {
        combatManager = this.GetComponent<CombatManager>();
        origPosition = enemyMannequin.transform.position;
        playerOrigPos = playerMannequin.transform.position;
        strikePosition = origPosition - new Vector3(250,0,0);

        GameController.controller.difficultyScale = 1;
        difficulty = GameController.controller.difficultyScale;
    }

    public void BeginEnemyTurn()
    {
        enemyInfo = combatManager.enemyInfo;
        ability1 = AbilityToolsScript.tools.LookUpAbility(enemyInfo.ability_1);
        ability2 = AbilityToolsScript.tools.LookUpAbility(enemyInfo.ability_2);
        ability3 = AbilityToolsScript.tools.LookUpAbility(enemyInfo.ability_3);
        ability4 = AbilityToolsScript.tools.LookUpAbility(enemyInfo.ability_4);

        originalPlayerHP = combatManager.getPlayerHealth();

        switch (difficulty)
        {
            case 1:
                EasyEnemyAI();
                break;
            case 2:
                MediumEnemyAI();
                break;
            case 3:
                HardEnemyAI();
                break;
        }
    }

    void EasyEnemyAI()
    {
        print("Easy AI:");
        //first evaluate random chance to strike
        //regardless of abilities
        int chanceToStrike = Random.Range(100, 100);

        if(chanceToStrike > 35)
        {
            print("Strike selected...");
            StartCoroutine(EnemyStrike());
        }
        else
        {
            print("selecting ability...");
            int randomAbility = Random.Range(0, 3);

            switch (randomAbility)
            {
                case 0:
                    if((ability1.Name != "") && (cooldownA1 == 0))
                    {
                        combatManager.HideHealthBars();
                        this.GetComponent<EnemyAbilityManager_C>().AbilityUsed(ability1, combatManager.getPlayerHealth());
                    }
                    else
                        StartCoroutine(EnemyStrike());
                    break;
                case 1:
                    if ((ability2.Name != "") && (cooldownA2 == 0))
                    {
                        combatManager.HideHealthBars();
                        this.GetComponent<EnemyAbilityManager_C>().AbilityUsed(ability2, combatManager.getPlayerHealth());
                    }
                    else
                        StartCoroutine(EnemyStrike());
                    break;
                case 2:
                    if ((ability3.Name != "") && (cooldownA3 == 0))
                    {
                        combatManager.HideHealthBars();
                        this.GetComponent<EnemyAbilityManager_C>().AbilityUsed(ability3, combatManager.getPlayerHealth());
                    }
                    else
                        StartCoroutine(EnemyStrike());
                    break;
                case 3:
                    if ((ability4.Name != "") && (cooldownA4 == 0))
                    {
                        combatManager.HideHealthBars();
                        this.GetComponent<EnemyAbilityManager_C>().AbilityUsed(ability4, combatManager.getPlayerHealth());
                    }
                    else
                        StartCoroutine(EnemyStrike());
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
        combatManager.HideHealthBars();
        enemyMannequin.GetComponent<LerpScript>().LerpToPos(origPosition, strikePosition, 3f);
        yield return new WaitForSeconds(0.7f);
        combatManager.DamagePlayer_Strike();
        enemyMannequin.GetComponent<LerpScript>().LerpToPos(strikePosition, origPosition, 3.5f);
        yield return new WaitForSeconds(0.25f);
        combatManager.EndEnemyTurn(true, originalPlayerHP);
    }

    public void UseFakeStrike()
    {
        StartCoroutine(FakeEnemyStrike());
    }

    IEnumerator FakeEnemyStrike()
    {
        combatManager.HideHealthBars();
        enemyMannequin.GetComponent<LerpScript>().LerpToPos(origPosition, strikePosition, 3f);
        yield return new WaitForSeconds(0.7f);
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
