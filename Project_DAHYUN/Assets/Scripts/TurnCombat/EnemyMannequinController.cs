using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMannequinController : MonoBehaviour {
    public GameObject spawnEffect;
    public AudioClip spawnSound;
    public float spawnSoundVolume = 0.5f;
    public AudioClip deathSound;
    public float deathSoundVolume = 0.5f;
    private bool visible = true;
    [HideInInspector]
    public Vector3 enemySpawnPos;
    private GameObject body;


    // Use this for initialization
    void Start ()
    {
        enemySpawnPos = this.transform.position;
        StartCoroutine(StartingSpawnSequence());
    }

    public void playAttackAnim()
    {
        this.transform.GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 5);
    }

    public void playFlinchAnim()
    {
        this.transform.GetChild(0).GetComponent<Animator>().SetInteger("AnimState", -2);
    }

    public void RefreshMannequinn()
    {
        body = this.transform.GetChild(0).gameObject;
    }

    IEnumerator StartingSpawnSequence()
    {
        RefreshMannequinn();
        HideEnemy();
        SpawnEffect();
        yield return new WaitForSeconds(1.5f);
        ShowEnemy();
        yield return new WaitForSeconds(0.3f);
        GameController.controller.GetComponent<MenuUIAudio>().playSoundClip(spawnSound, spawnSoundVolume);
    }

    void SpawnEffect()
    {
        Vector3 spawnPos = enemySpawnPos + new Vector3(0, 15, 0);
        GameObject effectClone = (GameObject)Instantiate(spawnEffect, spawnPos, transform.rotation);
    }

    public void PlayDeathSound()
    {
        GameController.controller.GetComponent<MenuUIAudio>().playSoundClip(deathSound, deathSoundVolume);
    }

    void ToggleVisible()
    {
        if (visible)
            HideEnemy();
        else
            ShowEnemy();
    }

    void HideEnemy()
    {
        body.GetComponent<SpriteRenderer>().enabled = false;
    }

    void ShowEnemy()
    {
        body.GetComponent<SpriteRenderer>().enabled = true;
    }
}
