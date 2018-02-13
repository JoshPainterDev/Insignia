using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class enemyCounterScript : MonoBehaviour {

    private int totalEnemies;
    private int remainingEnemies;
    private GameObject original;
    public GameObject combatManager;
    private GameObject[] tallies;
    // Use this for initialization
    void Start ()
    {
        Invoke("Initialize", 0.15f);
    }

    void Initialize()
    {
        totalEnemies = GameController.controller.currentEncounter.totalEnemies;
        remainingEnemies = totalEnemies;
        tallies = new GameObject[totalEnemies];
        tallies[0] = this.gameObject;

        original = this.transform.GetChild(0).transform.gameObject;
        Vector3 origin = original.transform.position;
        tallies[0] = original;

        for (int i = 1; i < totalEnemies; ++i)
        {
            Vector2 newPos = Vector3.zero;
            tallies[i] = Instantiate(original, newPos, Quaternion.identity);
            newPos = origin - new Vector3(i * 12, 0, 0);
            tallies[i].transform.SetParent(this.transform);
            tallies[i].transform.position = newPos;
            tallies[i].transform.localScale = original.transform.localScale;
        }
    }

    public void EnemyDied()
    {
        //maybe do some ui animation here?
        StartCoroutine(RemoveTally());
    }

    IEnumerator RemoveTally()
    {
        int removalNum = remainingEnemies - 1;
        yield return new WaitForSeconds(3f);
        tallies[removalNum].GetComponent<LerpScript>().LerpToColor(Color.white, Color.clear, 3);
        tallies[removalNum].GetComponent<LerpScript>().LerpToPos(tallies[removalNum].transform.position, tallies[removalNum].transform.position + new Vector3(0,20,0), 3);
        combatManager.GetComponent<CombatAudio>().playSwordHit();
        yield return new WaitForSeconds(1f);
        Destroy(tallies[remainingEnemies - 1]);
        --remainingEnemies;
    }
}
