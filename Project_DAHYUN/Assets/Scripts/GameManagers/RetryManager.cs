using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RetryManager : MonoBehaviour
{
    public GameObject playerMannequin;
    public GameObject retryButton;
    public GameObject mainmenuButton;
    public GameObject blackSq;
    public Color selectColor;

    // Use this for initialization
    void Start()
    {
        
        StartCoroutine(StartingSequence());
    }

    IEnumerator StartingSequence()
    {
        yield return new WaitForSeconds(0.15f);
        playerMannequin.GetComponent<AnimationController>().PlayDeathAnim();
        playerMannequin.GetComponent<AnimationController>().SetPlaySpeed(0.5f);
        yield return new WaitForSeconds(1.25f);
        blackSq.GetComponent<FadeScript>().FadeColored(Color.black, Color.clear, 0.75f);
        yield return new WaitForSeconds(2f);
        //blackSq.GetComponent<FadeScript>().FadeIn(0.5f);
        ShowButtons();
        Color temp = retryButton.GetComponent<Image>().color;
        retryButton.GetComponent<LerpScript>().LerpToColor(temp, temp + new Color(0,0,0,1), 1f);
        Color temp2 = retryButton.GetComponentInChildren<Text>().color;
        retryButton.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(temp2, temp2 + new Color(0, 0, 0, 1), 1f);

        Color temp3 = mainmenuButton.GetComponent<Image>().color;
        mainmenuButton.GetComponent<LerpScript>().LerpToColor(temp3, temp3 + new Color(0, 0, 0, 1), 1f);
        Color temp4 = mainmenuButton.GetComponentInChildren<Text>().color;
        mainmenuButton.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(temp4, temp4 + new Color(0, 0, 0, 1), 1f);

    }

    public void ButtonPressed(int buttonNum)
    {
        DisableButtons();

        if (buttonNum == 1)
        {
            StartCoroutine(RetryLastFight());
        }
        else
        {
            StartCoroutine(ReturnToMM());
        }
    }

    IEnumerator RetryLastFight()
    {
        Color origColor = retryButton.GetComponent<Image>().color;
        retryButton.GetComponent<Image>().color = selectColor;
        mainmenuButton.SetActive(false);
        retryButton.GetComponent<Image>().color = selectColor;
        yield return new WaitForSeconds(0.1f);
        retryButton.GetComponent<Image>().color = origColor;
        yield return new WaitForSeconds(0.1f);
        retryButton.GetComponent<Image>().color = selectColor;
        yield return new WaitForSeconds(0.1f);
        retryButton.GetComponent<Image>().color = origColor;
        blackSq.GetComponent<FadeScript>().FadeColored(Color.clear, Color.white, 1.0f);
        yield return new WaitForSeconds(0.1f);
        Color temp = retryButton.GetComponent<Image>().color;
        retryButton.GetComponent<LerpScript>().LerpToColor(temp, temp - new Color(0, 0, 0, 1), 0.75f);
        Color temp2 = retryButton.GetComponentInChildren<Text>().color;
        retryButton.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(temp2, temp2 - new Color(0, 0, 0, 1), 0.75f);
        yield return new WaitForSeconds(2.15f);
        SceneManager.LoadScene("TurnCombat_Scene");
    }

    IEnumerator ReturnToMM()
    {
        Color origColor = mainmenuButton.GetComponent<Image>().color;
        retryButton.SetActive(false);
        mainmenuButton.GetComponent<Image>().color = selectColor;
        yield return new WaitForSeconds(0.1f);
        mainmenuButton.GetComponent<Image>().color = origColor;
        yield return new WaitForSeconds(0.1f);
        mainmenuButton.GetComponent<Image>().color = selectColor;
        yield return new WaitForSeconds(0.1f);
        mainmenuButton.GetComponent<Image>().color = origColor;
        blackSq.GetComponent<FadeScript>().FadeIn();
        Color temp = retryButton.GetComponent<Image>().color;
        retryButton.GetComponent<LerpScript>().LerpToColor(temp, temp - new Color(0, 0, 0, 1), 0.75f);
        Color temp2 = retryButton.GetComponentInChildren<Text>().color;
        retryButton.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(temp2, temp2 - new Color(0, 0, 0, 1), 0.75f);

        Color temp3 = mainmenuButton.GetComponent<Image>().color;
        mainmenuButton.GetComponent<LerpScript>().LerpToColor(temp3, temp3 - new Color(0, 0, 0, 1), 0.75f);
        Color temp4 = mainmenuButton.GetComponentInChildren<Text>().color;
        mainmenuButton.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(temp4, temp4 - new Color(0, 0, 0, 1), 0.75f);

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu_Scene");
    }

    public void HideButtons()
    {
        retryButton.SetActive(false);
        mainmenuButton.SetActive(false);
    }

    public void ShowButtons()
    {
        retryButton.SetActive(true);
        mainmenuButton.SetActive(true);
    }

    public void EnableButtons()
    {
        retryButton.GetComponent<Button>().enabled = true;
        mainmenuButton.GetComponent<Button>().enabled = true;
    }

    public void DisableButtons()
    {
        retryButton.GetComponent<Button>().enabled = false;
        mainmenuButton.GetComponent<Button>().enabled = false;
    }
}
