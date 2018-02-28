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
    private bool ready4Input = true;

    // Use this for initialization
    void Start ()
    {
		
	}

    public void QuickBattle()
    {
        if (ready4Input)
        {
            ready4Input = false;
            StartCoroutine(HideButtons());
        }
    }

    public void ArenaBattle()
    {
        if (ready4Input)
        {
            ready4Input = false;
            StartCoroutine(HideButtons());
        }
    }

    IEnumerator HideButtons()
    {
        quickButton.GetComponent<LerpScript>().LerpToPos(quickOrigPos, quickOrigPos - new Vector3(50,0,0));
        arenaButton.GetComponent<LerpScript>().LerpToPos(arenaOrigPos, arenaOrigPos + new Vector3(50, 0, 0));
        yield return new WaitForSeconds(1f);
    }

    public void GoBack()
    {
        if(ready4Input)
        {
            ready4Input = false;
            StartCoroutine(GoToMainMenu());
        }
    }

    public IEnumerator GoToMainMenu()
    {
        yield return new WaitForSeconds(0.25f);
        blackSq.GetComponent<FadeScript>().FadeIn(1.5f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("MainMenu_Scene");
    }

}
