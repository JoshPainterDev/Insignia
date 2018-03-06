using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DecisionManager : MonoBehaviour
{
    public GameObject option1Button;
    public GameObject option2Button;
    public GameObject option1Tint;
    public GameObject option2Tint;
    public GameObject cameraObj;
    private Color option1Color;
    private Color option2Color;
    public GameObject areYouSurePanel;
    public GameObject blackSq;
    public int decisionNumber = 0;
    public int decisionPoints = 100;
    private GameObject player;

    private int optionNumber;

	// Use this for initialization
	void Start ()
    {
        player = GameController.controller.playerObject;
        option1Color = option1Button.GetComponent<Outline>().effectColor;
        option2Color = option2Button.GetComponent<Outline>().effectColor;
    }

    public void BeginDecision()
    {
        cameraObj.GetComponent<LerpScript>().LerpToPos(cameraObj.transform.position,
                        new Vector3(player.transform.position.x, cameraObj.transform.position.y, cameraObj.transform.position.z), 0.2f);

        Invoke("showDecisions", 1.0f);
    }

    void showDecisions()
    {
        for(int i = 0; i < this.transform.childCount; ++i )
        {
            this.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void OptionSelected(int numSelected)
    {
        optionNumber = numSelected;

        option1Button.GetComponent<Button>().enabled = false;
        option2Button.GetComponent<Button>().enabled = false;       

        switch (decisionNumber)
        {
            case 0:
                if(optionNumber == 1)
                {
                    StartCoroutine(Decision0(true));
                }
                else
                {
                    StartCoroutine(Decision0(false));
                }
                break;
        }
    }

    public void ConfirmSelection()
    {
        bool choice = false;

        if (optionNumber == 1)
            choice = true;

        GameController.controller.playerDecisions[decisionNumber] = choice;
        GameController.controller.Save(GameController.controller.playerName);
        StartCoroutine(ConfirmAnim());
    }

    //////////////////////////////////////////

    IEnumerator Decision0(bool option1Selected)
    {
        GameController.controller.GetComponent<MenuUIAudio>().playButtonClick();

        if (option1Selected)
        {
            yield return new WaitForSeconds(0.1f);

            areYouSurePanel.GetComponent<Outline>().effectColor = option1Color;

            option2Button.GetComponent<Button>().enabled = true;

            option1Tint.SetActive(false);
            option2Tint.SetActive(true);

            areYouSurePanel.transform.position = option1Button.transform.position - new Vector3(0, 100, 0);

            areYouSurePanel.SetActive(true);
        }
        else
        {
            yield return new WaitForSeconds(0.1f);

            areYouSurePanel.GetComponent<Outline>().effectColor = option2Color;

            option1Button.GetComponent<Button>().enabled = true;

            option2Tint.SetActive(false);
            option1Tint.SetActive(true);

            areYouSurePanel.transform.position = option2Button.transform.position - new Vector3(0, 100, 0);

            areYouSurePanel.SetActive(true);
        }
    }

    IEnumerator ConfirmAnim()
    {
        if (optionNumber == 1)
        {
            option2Tint.SetActive(false);

            option1Button.SetActive(false);

            yield return new WaitForSeconds(0.1f);

            option1Button.SetActive(true);

            yield return new WaitForSeconds(0.1f);

            option1Button.SetActive(false);

            yield return new WaitForSeconds(0.1f);

            option1Button.SetActive(true);

            yield return new WaitForSeconds(0.8f);

            Color temp = option2Button.GetComponent<Image>().color;
            option2Button.GetComponent<LerpScript>().LerpToColor(temp, temp - new Color(0, 0, 0, 1), 1.75f);
            temp = option2Button.transform.GetChild(0).GetComponent<Text>().color;
        }
        else
        {
            option2Button.SetActive(false);

            yield return new WaitForSeconds(0.1f);

            option2Button.SetActive(true);

            yield return new WaitForSeconds(0.1f);

            option2Button.SetActive(false);

            yield return new WaitForSeconds(0.1f);

            option2Button.SetActive(true);

            yield return new WaitForSeconds(0.8f);

            option1Tint.SetActive(false);
            Color temp = option1Button.GetComponent<Image>().color;
            option1Button.GetComponent<LerpScript>().LerpToColor(temp, temp - new Color(0, 0, 0, 1), 1.75f);
        }

        areYouSurePanel.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        if (optionNumber == 2)
        {
            Color temp = option2Button.GetComponent<Image>().color;
            option2Button.GetComponent<LerpScript>().LerpToColor(temp, temp - new Color(0, 0, 0, 1), 1.75f);
            //ADD POINTS FOR MAKING A DECISION
            GameController.controller.playerGoodPoints += decisionPoints;
        }
        else
        {
            Color temp = option1Button.GetComponent<Image>().color;
            option1Button.GetComponent<LerpScript>().LerpToColor(temp, temp - new Color(0, 0, 0, 1), 1.75f);
            //ADD POINTS FOR MAKING A DECISION
            GameController.controller.playerEvilPoints += decisionPoints;
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < this.transform.childCount; ++i)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
