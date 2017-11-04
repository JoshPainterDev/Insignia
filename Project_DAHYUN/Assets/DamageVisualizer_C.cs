using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DamageVisualizer_C : MonoBehaviour
{
    public GameObject damagePrefab;
    public GameObject canvas;
    public bool playerHealth = true;

    private float floatSpeed = 3.0f;
    private float fadeSpeed = 1.5f;

    private Vector3 playerPos = new Vector3(-250, 0, 0);
    private Vector3 enemyPos = new Vector3(250, 0, 0);
    private Vector3 driftOffset = new Vector3(0, 200, 0);
    private GameObject effectClone;

	// Use this for initialization
	void Start ()
    {
        SpawnDamage();
    }

    public void SpawnDamage()
    {
        Vector3 spawnPos;

        if(playerHealth)
            spawnPos = playerPos;
        else
            spawnPos = enemyPos;

        effectClone = (GameObject)Instantiate(damagePrefab, spawnPos, transform.rotation);
        effectClone.transform.SetParent(canvas.transform);
        effectClone.transform.GetChild(1).GetComponent<Text>().color = GameController.controller.getPlayerColorPreference();
        effectClone.transform.localPosition = spawnPos;
        StartCoroutine(animateText());
    }

    IEnumerator animateText()
    {
        driftOffset = effectClone.transform.position + new Vector3(0,75,0);
        effectClone.GetComponent<LerpScript>().LerpToPos(effectClone.transform.position, driftOffset, floatSpeed);

        for (int i = 0; i < 3; ++i)
        {
            GameObject current = effectClone.transform.GetChild(i).gameObject;
            Color curColor = current.GetComponent<Text>().color;
            current.GetComponent<LerpScript>().LerpToColor(Color.clear, curColor, 2.0f);
        }

        yield return new WaitForSeconds(1f);

        effectClone.GetComponent<LerpScript>().LerpToPos(driftOffset, driftOffset - new Vector3(0, 35, 0), floatSpeed);
        yield return new WaitForSeconds(0.15f);
        effectClone.GetComponent<LerpScript>().LerpToPos(effectClone.transform.position, driftOffset + new Vector3(0,150,0), floatSpeed);

        for (int i = 0; i < 3; ++i)
        {
            GameObject current = effectClone.transform.GetChild(i).gameObject;
            Color curColor = current.GetComponent<Text>().color;
            current.GetComponent<LerpScript>().LerpToColor(curColor, Color.clear, fadeSpeed);
        }
    }
}
