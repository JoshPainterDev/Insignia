using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCounterScript : MonoBehaviour {

    private int totalEnemies;
    private int remainingEnemies;
    private GameObject original;
    private GameObject[] tallies;
    // Use this for initialization
    void Start ()
    {
        //totalEnemies = GameController.controller.currentEncounter.totalEnemies;
        totalEnemies = 7;
        remainingEnemies = totalEnemies;
        tallies = new GameObject[totalEnemies];

        original = this.transform.GetChild(0).transform.gameObject;
        original.transform.position -= new Vector3(totalEnemies * 15,0,0);

        for (int i = 0; i < totalEnemies; ++i)
        {
            Vector2 newPos = original.transform.position + new Vector3(i * 15,0,0);
            tallies[i] = Instantiate(original, newPos, Quaternion.identity);
            tallies[i].transform.parent = this.transform;
            tallies[i].transform.position = newPos;
            tallies[i].transform.localScale = Vector3.one;
        }
    }

    public void EnemyDied()
    {
        //maybe do some ui animation here?
        Destroy(tallies[remainingEnemies - 1]);
        --remainingEnemies;
    }
}
