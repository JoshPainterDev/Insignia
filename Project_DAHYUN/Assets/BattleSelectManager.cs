using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BattleSelectManager : MonoBehaviour
{
    public GameObject quickButton;
    public GameObject arenaButton;

    public GameObject blackSq;

    private Vector3 quickOrigPos;
    private Vector3 arenaOrigPos;

    // Use this for initialization
    void Start ()
    {
		
	}

    public void QuickBattle()
    {
        StartCoroutine(HideButtons());
    }

    public void ArenaBattle()
    {
        StartCoroutine(HideButtons());

    }

    IEnumerator HideButtons()
    {
        quickButton.GetComponent<LerpScript>().LerpToPos(quickOrigPos, quickOrigPos - new Vector3(50,0,0));
        arenaButton.GetComponent<LerpScript>().LerpToPos(arenaOrigPos, arenaOrigPos + new Vector3(50, 0, 0));
        yield return new WaitForSeconds(1f);
    }
}
