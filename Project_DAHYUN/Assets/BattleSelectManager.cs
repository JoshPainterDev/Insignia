using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BattleSelectManager : MonoBehaviour
{
    public GameObject quickButton;
    public GameObject arenaButton;
    public GameObject popUp;

    public GameObject blackSq;

    private Vector3 quickOrigPos;
    private Vector3 arenaOrigPos;
    private bool ready4Input = true;

    public void ClosePopUp()
    {
        popUp.SetActive(false);
    }

    public void QuickBattle()
    {
        if (ready4Input)
        {
            ready4Input = false;
            StartCoroutine(LoadBattle(true));
        }
    }

    public void ArenaBattle()
    {
        if (ready4Input)
        {
            ready4Input = false;
            StartCoroutine(LoadBattle(false));
        }
    }

    IEnumerator LoadBattle(bool quickBattle)
    {
        blackSq.SetActive(true);
        blackSq.GetComponent<FadeScript>().FadeTo(Color.black, 3.0f);

        yield return new WaitForSeconds(0.35f);

        if(quickBattle)
        {
            SceneManager.LoadScene("TurnCombat_Scene");
        }
        else
        {
            SceneManager.LoadScene("ArenaSelect_Scene");
        }
    }
}
