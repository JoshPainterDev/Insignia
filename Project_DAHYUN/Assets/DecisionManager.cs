using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DecisionManager : MonoBehaviour
{
    public GameObject option1Button;
    public GameObject option2Button;
    public GameObject highlightImage;
    public GameObject areYouSurePanel;
    public GameObject blackSq;
    public GameObject blocker;
    public int decisionNumber = 0;

    private int optionNumber;

	// Use this for initialization
	void Start ()
    {
	}

    public void OptionSelected(int numSelected)
    {
        optionNumber = numSelected;
        blocker.SetActive(true);

        switch (decisionNumber)
        {
            case 0:
                if(optionNumber == 1)
                    StartCoroutine(Decision0(true));
                else
                    StartCoroutine(Decision0(false));
                break;
        }
    }

    public void Cancel()
    {
        blocker.SetActive(false);
        highlightImage.SetActive(false);
        areYouSurePanel.SetActive(false);
    }

    public void ConfirmSelection()
    {
        GameController.controller.playerDecisions[decisionNumber] = optionNumber;
        GameController.controller.Save(GameController.controller.playerName);
        StartCoroutine(ConfirmAnim());
    }

    //////////////////////////////////////////

    IEnumerator Decision0(bool option1Selected)
    {
        GameController.controller.GetComponent<MenuUIAudio>().playButtonClick();

        if(option1Selected)
        {
            highlightImage.transform.localPosition = new Vector3(-147,0,0);
        }
        else
        {
            highlightImage.transform.localPosition = new Vector3(147, 0, 0);
        }

        highlightImage.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        highlightImage.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        highlightImage.SetActive(true);

        areYouSurePanel.SetActive(true);
    }

    IEnumerator ConfirmAnim()
    {
        if (optionNumber == 1)
        {
            Color temp = option2Button.GetComponent<Image>().color;
            option2Button.GetComponent<LerpScript>().LerpToColor(temp, temp - new Color(0, 0, 0, 1), 1.75f);
            temp = option2Button.transform.GetChild(0).GetComponent<Text>().color;
            option2Button.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(temp, temp - new Color(0, 0, 0, 1), 1.75f);
            temp = option2Button.transform.GetChild(1).GetComponent<Text>().color;
            option2Button.transform.GetChild(1).GetComponent<LerpScript>().LerpToColor(temp, temp - new Color(0, 0, 0, 1), 1.75f);
        }
        else
        {
            Color temp = option1Button.GetComponent<Image>().color;
            option1Button.GetComponent<LerpScript>().LerpToColor(temp, temp - new Color(0, 0, 0, 1), 1.75f);
            temp = option1Button.transform.GetChild(0).GetComponent<Text>().color;
            option1Button.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(temp, temp - new Color(0, 0, 0, 1), 1.75f);
            temp = option1Button.transform.GetChild(1).GetComponent<Text>().color;
            option1Button.transform.GetChild(1).GetComponent<LerpScript>().LerpToColor(temp, temp - new Color(0, 0, 0, 1), 1.75f);
        }

        highlightImage.SetActive(false);
        areYouSurePanel.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        if (optionNumber == 2)
        {
            Color temp = option2Button.GetComponent<Image>().color;
            option2Button.GetComponent<LerpScript>().LerpToColor(temp, temp - new Color(0, 0, 0, 1), 1.75f);
            temp = option2Button.transform.GetChild(0).GetComponent<Text>().color;
            option2Button.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(temp, temp - new Color(0, 0, 0, 1), 1.75f);
            temp = option2Button.transform.GetChild(1).GetComponent<Text>().color;
            option2Button.transform.GetChild(1).GetComponent<LerpScript>().LerpToColor(temp, temp - new Color(0, 0, 0, 1), 1.75f);
        }
        else
        {
            Color temp = option1Button.GetComponent<Image>().color;
            option1Button.GetComponent<LerpScript>().LerpToColor(temp, temp - new Color(0, 0, 0, 1), 1.75f);
            temp = option1Button.transform.GetChild(0).GetComponent<Text>().color;
            option1Button.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(temp, temp - new Color(0, 0, 0, 1), 1.75f);
            temp = option1Button.transform.GetChild(1).GetComponent<Text>().color;
            option1Button.transform.GetChild(1).GetComponent<LerpScript>().LerpToColor(temp, temp - new Color(0, 0, 0, 1), 1.75f);
        }

        blackSq.GetComponent<FadeScript>().FadeColored(new Color(0.75f, 0.75f, 0.75f, 0.75f), Color.black, 0.5f);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MainMenu_Tutorial_Scene");
    }
}
