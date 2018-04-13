using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrangleScript_C : MonoBehaviour
{
    public bool playerUse = true;
    public GameObject Character;
    public GameObject Enemy;

    private Vector3 startPos;
    private Vector3 offset;
    private Vector3 enemyStartPos;
    private bool shaking = false;

    private Coroutine shakeRoutine;

    // Use this for initialization
    void Start()
    {
        //StartAnim(playerUse, Enemy);

    }

    public void StartAnim(bool usedByPlayer, GameObject user, GameObject foe)
    {
        playerUse = usedByPlayer;
        Enemy = foe;
        enemyStartPos = Enemy.transform.position;
        Character = user;

        if (playerUse)
        {
            //this.transform.localPosition = new Vector3(-1, 20.4f, 0);
            offset = new Vector3(0, 30, 0);
        }
        else
        {
            //this.transform.localPosition = new Vector3(2, 18.5f, 0);
            this.GetComponent<SpriteRenderer>().flipX = true;
            offset = new Vector3(0, 30, 0);
        }

        startPos = this.transform.position;
        StartCoroutine(startingAnim());
    }

    IEnumerator startingAnim()
    {
        shaking = true;
        yield return new WaitForSeconds(1.2f);
        
        this.GetComponent<LerpScript>().LerpToPos(startPos, startPos + offset, 5.0f);
        StartCoroutine(shake());
        yield return new WaitForSeconds(0.2f);
        //this.GetComponent<LerpScript>().LerpToPos(startPos + offset, (startPos + offset) + new Vector3(0, 50, 0), 0.60f);
        Enemy.GetComponent<LerpScript>().LerpToPos(enemyStartPos, enemyStartPos + new Vector3(0, 50, 0), 0.50f);
        yield return new WaitForSeconds(1.7f);
        this.GetComponent<Animator>().SetBool("Squish", true);
        this.transform.parent.GetComponent<AudioSource>().Play();
        shaking = false;
        StopCoroutine(shakeRoutine);
        yield return new WaitForSeconds(0.4f);
        Enemy.GetComponent<LerpScript>().LerpToPos(enemyStartPos + new Vector3(0, 50, 0), enemyStartPos + new Vector3(0, 80, 0), 6.0f);
        yield return new WaitForSeconds(0.2f);
        Enemy.GetComponent<LerpScript>().LerpToPos(enemyStartPos + new Vector3(0, 80, 0), enemyStartPos, 7.0f);
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

    IEnumerator shake()
    {
        Vector3 currentPos = this.transform.parent.transform.position;
        this.transform.parent.GetComponent<LerpScript>().LerpToPos(currentPos, currentPos + new Vector3(3, 2, 0), 15.0f);
        yield return new WaitForSeconds(0.05f);
        this.transform.parent.GetComponent<LerpScript>().LerpToPos(currentPos + new Vector3(3, 2, 0), currentPos + new Vector3(0, 4, 0), 15.0f);
        yield return new WaitForSeconds(0.05f);
        if(shaking)
            shakeRoutine = StartCoroutine(shake());
    }
}
