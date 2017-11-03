using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeManager_C : MonoBehaviour {

    public GameObject playerMannequin;
    public GameObject enemyMannequin;
    public CombatManager combatManager;
    public float strikeAnimDuration;
    private Vector3 initPlayerPos;
    private Vector3 initEnemyPos;
    private Vector3 bloodPos;
    private Vector2 strikeOffset = new Vector2(0f , 0f);
    private string strikeModifier;
    private GameObject effectClone;
    private GameObject bloodClone;
    private int origEnemyHealth;
    private float percentageMod;

    //ASSETS
    public GameObject standardStrikeHit_FX;
    public GameObject standardStrikeMiss_FX;
    public GameObject shadowStrike_FX;
    public GameObject seratedStrike_FX;
    public GameObject blood01_FX;
    public GameObject blood02_FX;
    public GameObject blood03_FX;
    public GameObject blood04_FX;

    // Use this for initialization
    void Start ()
    {
        initPlayerPos = playerMannequin.transform.position;
        initEnemyPos = enemyMannequin.transform.GetChild(0).transform.GetChild(0).transform.position;
        combatManager = this.GetComponent<CombatManager>();
    }
	
    public void StrikeUsed(string strikeMod, int originalEnemyHP, float percentStrikeDamage = 1.0f)
    {
        strikeModifier = strikeMod;
        origEnemyHealth = originalEnemyHP;
        percentageMod = percentStrikeDamage;

        if (strikeModifier == "")
        {
            //animate player mannequin
            StartCoroutine(AnimatePlayerStrike());
        }
        else if (strikeModifier == "Shadow Strike")
        {
            Vector2 FXoffset = new Vector2(0, 20);
            Vector3 spawnPos = new Vector3(initPlayerPos.x + FXoffset.x, initPlayerPos.y + FXoffset.y, 0);
            effectClone = (GameObject)Instantiate(shadowStrike_FX, spawnPos, transform.rotation);
            effectClone.transform.parent = playerMannequin.transform;
            effectClone.GetComponent<Animator>().enabled = false;
            effectClone.GetComponent<SpriteRenderer>().enabled = false;
            //animate player mannequin
            StartCoroutine(AnimatePlayerStrike());
        }
        else if (strikeModifier == "Serrated Strike")
        {
            Vector2 FXoffset = new Vector2(150, 0);
            Vector3 spawnPos = new Vector3(initPlayerPos.x + FXoffset.x, initPlayerPos.y + FXoffset.y, 0);
            effectClone = (GameObject)Instantiate(seratedStrike_FX, spawnPos, transform.rotation);
            effectClone.transform.parent = playerMannequin.transform;
            //animate player mannequin
            StartCoroutine(AnimatePlayerStrike());
        }
        else if(strikeModifier == "Tutorial Strike")
        {   
            StartCoroutine(TutorialStrike());
        }
        else
        {
            //animate player mannequin
            StartCoroutine(AnimatePlayerStrike());
        }

    }

    IEnumerator AnimatePlayerStrike()
    {
        Vector3 pos1;
        Vector3 pos2;
        combatManager.HideHealthBars();

        print("Strike Mod: " + strikeModifier);

        switch (strikeModifier)
        {
            case "Shadow Strike":
                pos1 = new Vector3(initPlayerPos.x + 250, initPlayerPos.y, 0);
                playerMannequin.GetComponent<LerpScript>().LerpToPos(initPlayerPos, pos1, strikeAnimDuration / .05f);
                yield return new WaitForSeconds(0.1f);
                effectClone.GetComponent<Animator>().enabled = true;
                effectClone.GetComponent<SpriteRenderer>().enabled = true;
                this.GetComponent<CombatManager>().DamageEnemy_Strike(percentageMod);
                playerMannequin.GetComponent<AnimationController>().PlayAttackAnim();
                yield return new WaitForSeconds(0.2f);
                generateRandomBlood();
                bloodPos = new Vector3(initEnemyPos.x, initEnemyPos.y, 0);
                bloodClone.transform.position = bloodPos;
                yield return new WaitForSeconds(0.5f);
                playerMannequin.GetComponent<LerpScript>().LerpToPos(pos1, initPlayerPos, strikeAnimDuration / .25f);
                break;
            case "Serrated Strike":
                pos1 = new Vector3(initPlayerPos.x - 150, initPlayerPos.y, 0);
                pos2 = new Vector3(pos1.x + 300, initPlayerPos.y, 0);
                playerMannequin.GetComponent<LerpScript>().LerpToPos(initPlayerPos, pos1, strikeAnimDuration / .1f);
                yield return new WaitForSeconds(0.25f);
                playerMannequin.GetComponent<AnimationController>().PlayAttackAnim();
                playerMannequin.GetComponent<LerpScript>().LerpToPos(pos1, pos2, strikeAnimDuration / .25f);
                yield return new WaitForSeconds(0.25f);
                generateRandomBlood();
                bloodPos = new Vector3(initEnemyPos.x, initEnemyPos.y, 0);
                bloodClone.transform.position = bloodPos;
                this.GetComponent<CombatManager>().DamageEnemy_Strike(percentageMod);
                yield return new WaitForSeconds(0.25f);
                playerMannequin.GetComponent<LerpScript>().LerpToPos(pos2, initPlayerPos, strikeAnimDuration / .25f);
                break;
            default:
                pos1 = new Vector3(initPlayerPos.x + 250, initPlayerPos.y, 0);
                playerMannequin.GetComponent<LerpScript>().LerpToPos(initPlayerPos, pos1, strikeAnimDuration / .1f);
                yield return new WaitForSeconds(0.25f);
                playerMannequin.GetComponent<AnimationController>().PlayAttackAnim();
                Vector3 spawnPos = new Vector3(initEnemyPos.x, initEnemyPos.y, 0);
                effectClone = (GameObject)Instantiate(standardStrikeHit_FX, spawnPos, transform.rotation);
                yield return new WaitForSeconds(0.5f);
                generateRandomBlood();
                bloodPos = new Vector3(initEnemyPos.x, initEnemyPos.y, 0);
                bloodClone.transform.position = bloodPos;
                this.GetComponent<CombatManager>().DamageEnemy_Strike(percentageMod);
                yield return new WaitForSeconds(0.25f);
                playerMannequin.GetComponent<LerpScript>().LerpToPos(pos1, initPlayerPos, strikeAnimDuration / .25f);
                break;
        }

        yield return new WaitForSeconds(0.5f);
        combatManager.ShowHealthBars();
        this.GetComponent<CombatManager>().EndPlayerTurn(true, origEnemyHealth);
    }

    public void PlayerStrikeMiss()
    {
        Destroy(effectClone);
        StartCoroutine(AnimatePlayerStrikeMiss());
    }

    IEnumerator AnimatePlayerStrikeMiss()
    {
        combatManager.HideHealthBars();
        Vector3 pos1 = new Vector3(initPlayerPos.x + 250, initPlayerPos.y, 0);
        playerMannequin.GetComponent<LerpScript>().LerpToPos(initPlayerPos, pos1, strikeAnimDuration / .1f);
        yield return new WaitForSeconds(0.15f);
        enemyMannequin.GetComponent<LerpScript>().LerpToPos(initEnemyPos, initEnemyPos + new Vector3(50, 0, 0), 4.5f);
        Vector3 spawnPos = new Vector3(initEnemyPos.x, initEnemyPos.y - 25, 0);
        effectClone = (GameObject)Instantiate(standardStrikeMiss_FX, spawnPos, transform.rotation);
        playerMannequin.GetComponent<AnimationController>().PlayAttackAnim();
        yield return new WaitForSeconds(0.5f);
        playerMannequin.GetComponent<LerpScript>().LerpToPos(pos1, initPlayerPos, strikeAnimDuration / .25f);
        yield return new WaitForSeconds(0.2f);
        enemyMannequin.GetComponent<LerpScript>().LerpToPos(initEnemyPos + new Vector3(50, 0, 0), initEnemyPos, 2f);
        combatManager.ShowHealthBars();
        yield return new WaitForSeconds(0.2f);
        combatManager.EndPlayerTurn(false);
    }

    IEnumerator TutorialStrike()
    {
        combatManager.HideHealthBars();
        Vector3 pos1 = new Vector3(initPlayerPos.x + 250, initPlayerPos.y, 0);
        playerMannequin.GetComponent<LerpScript>().LerpToPos(initPlayerPos, pos1, strikeAnimDuration / .1f);
        yield return new WaitForSeconds(0.25f);
        playerMannequin.GetComponent<AnimationController>().PlayAttackAnim();
        Vector3 spawnPos = new Vector3(initEnemyPos.x, initEnemyPos.y, 0);
        effectClone = (GameObject)Instantiate(standardStrikeHit_FX, spawnPos, transform.rotation);
        yield return new WaitForSeconds(0.5f);
        generateRandomBlood();
        bloodPos = new Vector3(initEnemyPos.x + 15, initEnemyPos.y + 5, 0);
        bloodClone.transform.position = bloodPos;
        yield return new WaitForSeconds(0.25f);
        playerMannequin.GetComponent<LerpScript>().LerpToPos(pos1, initPlayerPos, strikeAnimDuration / .25f);
        combatManager.ShowHealthBars();
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
