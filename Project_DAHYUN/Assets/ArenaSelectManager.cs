using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ArenaSelectManager : MonoBehaviour
{
    public GameObject arenaHandle;
    public GameObject backgroundObj;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject arenaButton1;
    public GameObject arenaButton2;
    public GameObject arenaButton3;
    public GameObject arenaButton4;
    public GameObject arenaButton5;
    public GameObject arenaButton6;
    public GameObject blackSq;

    public Color highlightColor;
    public Color normalColor;

    public Sprite arenaSprite01;
    public Sprite arenaSprite02;
    public Sprite arenaSprite03;
    public Sprite arenaSprite04;
    public Sprite arenaSprite05;
    public Sprite arenaSprite06;

    public GameObject arenaEnemy01;
    public GameObject arenaEnemy02;
    public GameObject arenaEnemy03;

    public GameObject arenaEnemy04;
    public GameObject arenaEnemy05;
    public GameObject arenaEnemy06;

    public GameObject arenaEnemy07;
    public GameObject arenaEnemy08;
    public GameObject arenaEnemy09;

    public GameObject arenaEnemy10;
    public GameObject arenaEnemy11;
    public GameObject arenaEnemy12;

    public GameObject arenaEnemy13;
    public GameObject arenaEnemy14;
    public GameObject arenaEnemy15;

    public GameObject arenaEnemy16;
    public GameObject arenaEnemy17;
    public GameObject arenaEnemy18;
    //public Sprite arenaEnemy03;
    //public Sprite arenaEnemy04;
    //public Sprite arenaEnemy05;
    //public Sprite arenaEnemy06;
    //public Sprite arenaEnemy07;
    //public Sprite arenaEnemy08;
    //public Sprite arenaEnemy09;

    private int selectedArena = 0;
    private bool ready4Input = true;
    private bool useEndlessMode = false;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 6; ++i)
        {
            if (GameController.controller.arenaCompleted[i])
            {
                arenaHandle.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);
            }
        }
    }

    public void ArenaSelected(int arenaNum)
    {
        if(ready4Input)
        {
            ready4Input = false;
            selectedArena = arenaNum;

            ResetColors();
            ChangeArena(arenaNum);

            if (GameController.controller.arenaCompleted[arenaNum - 1])
                useEndlessMode = true;
            else
                useEndlessMode = false;

            switch (arenaNum)
            {
                case 1:
                    arenaButton1.GetComponent<Image>().color = highlightColor;
                    break;
                case 2:
                    arenaButton2.GetComponent<Image>().color = highlightColor;
                    break;
                case 3:
                    arenaButton3.GetComponent<Image>().color = highlightColor;
                    break;
                case 4:
                    arenaButton4.GetComponent<Image>().color = highlightColor;
                    break;
                case 5:
                    arenaButton5.GetComponent<Image>().color = highlightColor;
                    break;
                case 6:
                    arenaButton6.GetComponent<Image>().color = highlightColor;
                    break;
            }

            StartCoroutine(InputDelay());
        }
    }

    public void ChangeArena(int arenaNum)
    {
        GameObject effectClone1;
        GameObject effectClone2;
        GameObject effectClone3;
        Destroy(enemy1.transform.GetChild(0).gameObject);
        Destroy(enemy2.transform.GetChild(0).gameObject);
        Destroy(enemy3.transform.GetChild(0).gameObject);

        switch (arenaNum)
        {
            case 1:
                backgroundObj.GetComponent<SpriteRenderer>().sprite = arenaSprite01;
                effectClone1 = (GameObject)Instantiate(arenaEnemy01, Vector3.zero, transform.rotation);
                effectClone1.transform.SetParent(enemy1.transform, false);
                effectClone1.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1;
                effectClone2 = (GameObject)Instantiate(arenaEnemy02, Vector3.zero, transform.rotation);
                effectClone2.transform.SetParent(enemy2.transform, false);
                effectClone2.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 2;
                effectClone3 = (GameObject)Instantiate(arenaEnemy03, Vector3.zero, transform.rotation);
                effectClone3.transform.SetParent(enemy3.transform, false);
                effectClone3.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 3;
                break;
            case 2:
                backgroundObj.GetComponent<SpriteRenderer>().sprite = arenaSprite02;
                effectClone1 = (GameObject)Instantiate(arenaEnemy04, Vector3.zero, transform.rotation);
                effectClone1.transform.SetParent(enemy1.transform, false);
                effectClone1.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1;
                effectClone2 = (GameObject)Instantiate(arenaEnemy05, Vector3.zero, transform.rotation);
                effectClone2.transform.SetParent(enemy2.transform, false);
                effectClone2.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 2;
                effectClone3 = (GameObject)Instantiate(arenaEnemy06, Vector3.zero, transform.rotation);
                effectClone3.transform.SetParent(enemy3.transform, false);
                effectClone3.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 3;
                break;
            case 3:
                backgroundObj.GetComponent<SpriteRenderer>().sprite = arenaSprite03;
                effectClone1 = (GameObject)Instantiate(arenaEnemy07, Vector3.zero, transform.rotation);
                effectClone1.transform.SetParent(enemy1.transform, false);
                effectClone1.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1;
                effectClone2 = (GameObject)Instantiate(arenaEnemy08, Vector3.zero, transform.rotation);
                effectClone2.transform.SetParent(enemy2.transform, false);
                effectClone2.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 2;
                effectClone3 = (GameObject)Instantiate(arenaEnemy09, Vector3.zero, transform.rotation);
                effectClone3.transform.SetParent(enemy3.transform, false);
                effectClone3.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 3;
                break;
            case 4:
                backgroundObj.GetComponent<SpriteRenderer>().sprite = arenaSprite04;
                effectClone1 = (GameObject)Instantiate(arenaEnemy10, Vector3.zero, transform.rotation);
                effectClone1.transform.SetParent(enemy1.transform, false);
                effectClone1.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1;
                effectClone2 = (GameObject)Instantiate(arenaEnemy11, Vector3.zero, transform.rotation);
                effectClone2.transform.SetParent(enemy2.transform, false);
                effectClone2.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 2;
                effectClone3 = (GameObject)Instantiate(arenaEnemy12, Vector3.zero, transform.rotation);
                effectClone3.transform.SetParent(enemy3.transform, false);
                effectClone3.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 3;
                break;
            case 5:
                backgroundObj.GetComponent<SpriteRenderer>().sprite = arenaSprite05;
                effectClone1 = (GameObject)Instantiate(arenaEnemy13, Vector3.zero, transform.rotation);
                effectClone1.transform.SetParent(enemy1.transform, false);
                effectClone1.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1;
                effectClone2 = (GameObject)Instantiate(arenaEnemy14, Vector3.zero, transform.rotation);
                effectClone2.transform.SetParent(enemy2.transform, false);
                effectClone2.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 2;
                effectClone3 = (GameObject)Instantiate(arenaEnemy15, Vector3.zero, transform.rotation);
                effectClone3.transform.SetParent(enemy3.transform, false);
                effectClone3.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 3;
                break;
            case 6:
                backgroundObj.GetComponent<SpriteRenderer>().sprite = arenaSprite06;
                effectClone1 = (GameObject)Instantiate(arenaEnemy16, Vector3.zero, transform.rotation);
                effectClone1.transform.SetParent(enemy1.transform, false);
                effectClone1.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1;
                effectClone2 = (GameObject)Instantiate(arenaEnemy17, Vector3.zero, transform.rotation);
                effectClone2.transform.SetParent(enemy2.transform, false);
                effectClone2.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 2;
                effectClone3 = (GameObject)Instantiate(arenaEnemy18, Vector3.zero, transform.rotation);
                effectClone3.transform.SetParent(enemy3.transform, false);
                effectClone3.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 3;
                break;
        }
    }

    public void ResetColors()
    {
        arenaButton1.GetComponent<Image>().color = normalColor;
        arenaButton2.GetComponent<Image>().color = normalColor;
        arenaButton3.GetComponent<Image>().color = normalColor;
        arenaButton4.GetComponent<Image>().color = normalColor;
        arenaButton5.GetComponent<Image>().color = normalColor;
        arenaButton6.GetComponent<Image>().color = normalColor;
    }

    IEnumerator LoadArena()
    {
        GameController.controller.curArenaStage = 1;
        blackSq.GetComponent<FadeScript>().FadeIn();

        yield return new WaitForSeconds(2f);

        switch (selectedArena)
        {
            case 1:
                SceneManager.LoadScene("Exposition_Scene01");
                break;
            case 2:
                SceneManager.LoadScene("Exposition_Scene01");
                break;
        }
    }

    public void ConfirmSelection()
    {
        StartCoroutine(LoadArena());
    }

    IEnumerator InputDelay()
    {
        yield return new WaitForSeconds(1f);
        ready4Input = true;
    }
}
