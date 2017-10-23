using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TutorialManager02_C : MonoBehaviour
{
    public GameObject panel01;
    public GameObject panel02;
    public GameObject panel03;

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
        }
    }

    public void FirstInput()
    {
        Destroy(panel01);
        LoadPanel02();
    }

    public void SecondInput()
    {
        Destroy(panel02);
        LoadPanel03();
    }

    public void ThirdInput()
    {
        Destroy(panel03);
    }

    public void LoadPanel02()
    {
        panel02.GetComponent<Image>().enabled = true;
        panel02.transform.GetChild(0).GetComponent<Text>().enabled = true;
        panel02.transform.GetChild(1).GetComponent<Text>().enabled = true;

    }

    public void LoadPanel03()
    {
        panel03.GetComponent<Image>().enabled = true;
        panel03.transform.GetChild(0).GetComponent<Text>().enabled = true;
        panel03.transform.GetChild(1).GetComponent<Text>().enabled = true;
    }
}
