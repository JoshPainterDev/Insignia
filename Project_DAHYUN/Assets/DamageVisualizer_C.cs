using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DamageVisualizer_C : MonoBehaviour
{
    public GameObject damagePrefab;
    public GameObject criticalPrefab;
    public GameObject canvas;
    public bool playerHealth = true;
    public Color enemyColor;

    private float floatSpeed = 3.0f;
    private float fadeSpeed = 1.5f;
    private Vector3 damageScale;

    private Vector3 playerPos = new Vector3(-210, 0, 0);
    private Vector3 enemyPos = new Vector3(210, 0, 0);
    private Vector3 driftOffset = new Vector3(0, 200, 0);
    private GameObject effectClone;

	// Use this for initialization
	void Start ()
    {
    }

    public void SpawnDamage(int damage, bool criticalHit = false)
    {
        Vector3 spawnPos;
        Color textColor = Color.white;

        damageScale = new Vector3(1, 1, 1);

        if (playerHealth)
        {
            spawnPos = playerPos;
            
            if (criticalHit)
            {
                effectClone = (GameObject)Instantiate(criticalPrefab, Vector3.zero, transform.rotation);
                textColor = enemyColor;
                damageScale = new Vector3(1.35f, 1.35f, 1);
            }
            else
                effectClone = (GameObject)Instantiate(damagePrefab, Vector3.zero, transform.rotation);
        }
        else
        {
            spawnPos = enemyPos;

            if (criticalHit)
            {
                effectClone = (GameObject)Instantiate(criticalPrefab, Vector3.zero, transform.rotation);
                textColor = GameController.controller.getPlayerColorPreference();
                damageScale = new Vector3(1.35f, 1.35f, 1);
            }
            else
                effectClone = (GameObject)Instantiate(damagePrefab, Vector3.zero, transform.rotation);

        } 

        effectClone.transform.SetParent(canvas.transform);
        effectClone.transform.GetChild(1).GetComponent<Text>().color = textColor;
        effectClone.transform.localPosition = spawnPos;
        effectClone.transform.localScale = damageScale;  
        StartCoroutine(animateText(damage));
    }

    IEnumerator animateText(int damage)
    {
        if (playerHealth)
            driftOffset = effectClone.transform.position + new Vector3(50, 75, 0);
        else
            driftOffset = effectClone.transform.position + new Vector3(-50, 75, 0);
        effectClone.GetComponent<LerpScript>().LerpToPos(effectClone.transform.position, driftOffset, floatSpeed);

        for (int i = 0; i < 2; ++i)
        {
            GameObject current = effectClone.transform.GetChild(i).gameObject;
            current.GetComponent<Text>().text = damage.ToString();
            Color curColor = current.GetComponent<Text>().color;
            current.GetComponent<LerpScript>().LerpToColor(Color.clear, curColor, 2.0f);
        }

        yield return new WaitForSeconds(1.15f);

        effectClone.GetComponent<LerpScript>().LerpToPos(driftOffset, driftOffset - new Vector3(0, 35, 0), floatSpeed);
        yield return new WaitForSeconds(0.15f);
        effectClone.GetComponent<LerpScript>().LerpToPos(effectClone.transform.position, driftOffset + new Vector3(0,200,0), floatSpeed);

        for (int i = 0; i < 2; ++i)
        {
            GameObject current = effectClone.transform.GetChild(i).gameObject;
            Color curColor = current.GetComponent<Text>().color;
            current.GetComponent<LerpScript>().LerpToColor(curColor, Color.clear, fadeSpeed);
        }
    }
}
