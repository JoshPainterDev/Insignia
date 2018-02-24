using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TutorialManagerMM_C : MonoBehaviour
{
    public GameObject panel01;
    public GameObject panel02;
    public GameObject panel03;
    public GameObject panel04;
    public GameObject inputPanel;

    public GameObject adventure;
    public GameObject battle;
    public GameObject character;

    public Color selectColor;
    public Color origColor;

    public GameObject gameCamera;
    public GameObject blackSq;

    private GameObject textBox;
    private MainMenuManager menuManager;
    private int tutorialState = 0;
    private int inputNumber = 0;
    private bool inputEnabled = true;
    // Use this for initialization
    void Start()
    {
        menuManager = this.GetComponent<MainMenuManager>();
        GameController.controller.stagesCompleted = 1;
        GameController.controller.Save(GameController.controller.playerName);
    }

    public void InputDetected()
    {
        if (!inputEnabled)
            return;

        ++tutorialState;

        switch(tutorialState)
        {
            case 1:
                FirstInput();
                break;
            case 2:
                SecondInput();
                break;
            case 3:
                ThirdInput();
                break;
            case 4:
                FourthInput();
                break;
        }
    }

    public void FirstInput()
    {
        inputEnabled = false;
        Destroy(panel01);
        LoadPanel02();
    }

    public void SecondInput()
    {
        inputEnabled = false;
        Destroy(panel02);
        LoadPanel03();
    }

    public void ThirdInput()
    {
        inputEnabled = false;
        Destroy(panel03);
        LoadPanel04();
    }

    public void FourthInput()
    {
        inputEnabled = false;
        Destroy(panel04);
        Destroy(inputPanel);
        adventure.GetComponent<Image>().color = origColor;
    }

    public void LoadPanel02()
    {
        panel02.GetComponent<Image>().enabled = true;
        panel02.transform.GetChild(0).GetComponent<Text>().enabled = true;
        panel02.transform.GetChild(1).GetComponent<Text>().enabled = true;
        StartCoroutine(colorFlash(character));
    }

    public void LoadPanel03()
    {
        panel03.GetComponent<Image>().enabled = true;
        panel03.transform.GetChild(0).GetComponent<Text>().enabled = true;
        panel03.transform.GetChild(1).GetComponent<Text>().enabled = true;
        StartCoroutine(colorFlash(battle));
        character.GetComponent<Image>().color = origColor;
    }

    public void LoadPanel04()
    {
        panel04.GetComponent<Image>().enabled = true;
        panel04.transform.GetChild(0).GetComponent<Text>().enabled = true;
        panel04.transform.GetChild(1).GetComponent<Text>().enabled = true;
        StartCoroutine(colorFlash(adventure));
        battle.GetComponent<Image>().color = origColor;
    }

    IEnumerator colorFlash(GameObject target)
    {
        target.GetComponent<Image>().color = selectColor;
        yield return new WaitForSeconds(0.2f);
        target.GetComponent<Image>().color = origColor;
        yield return new WaitForSeconds(0.2f);
        target.GetComponent<Image>().color = selectColor;
        inputEnabled = true;
    }
}
