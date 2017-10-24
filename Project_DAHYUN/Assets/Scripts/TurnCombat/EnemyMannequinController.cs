using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMannequinController : MonoBehaviour {
    const float XOFFSET = 231;

    public GameObject spawnEffect;
    public AudioClip spawnSound;
    private bool visible = true;


    // Use this for initialization
    void Start ()
    {
        StartCoroutine(StartingSpawnSequence());
    }

    IEnumerator StartingSpawnSequence()
    {
        HideEnemy();
        SpawnEffect();
        yield return new WaitForSeconds(1);
        ShowEnemy();
        GameController.controller.GetComponent<MenuUIAudio>().playSoundClip(spawnSound, 0.1f);
    }

    void SpawnEffect()
    {
        Vector3 spawnPos = new Vector3(transform.position.x - XOFFSET, transform.position.y, 0);
        GameObject effectClone = (GameObject)Instantiate(spawnEffect, spawnPos, transform.rotation);
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
        foreach (SpriteRenderer child in this.GetComponentsInChildren<SpriteRenderer>())
        {
            child.enabled = false;
        }
    }

    void ShowEnemy()
    {
        foreach (SpriteRenderer child in this.GetComponentsInChildren<SpriteRenderer>())
        {
            child.enabled = true;
        }
    }
}
