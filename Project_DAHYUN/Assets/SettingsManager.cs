using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public GameObject background;
    public GameObject blackSq;
    public GameObject confirmationPanel;
    public GameObject musicSlider;
    public GameObject difficultySlider;
    public GameObject muteToggle;

    public GameObject difficultyText;

    private bool changesMade = false;
    private float musicLevel = 1.0f;

    private void Awake()
    {
        GameController.controller.Load(GameController.controller.charNames[1]);
    }

    // Use this for initialization
    void Start ()
    {
        changesMade = false;
        background.GetComponent<SpriteRenderer>().color = GameController.controller.getPlayerColorPreference();
        musicSlider.GetComponent<Slider>().value = GameController.controller.musicScale;
        muteToggle.GetComponent<Toggle>().isOn = GameController.controller.volumeMuted;

        switch(GameController.controller.difficultyScale)
        {
            case Difficulty.Chill:
                difficultySlider.GetComponent<Slider>().value = 0;
                difficultyText.GetComponent<Text>().text = "Chill";
                break;
            case Difficulty.Story:
                difficultySlider.GetComponent<Slider>().value = 1;
                difficultyText.GetComponent<Text>().text = "Story";
                break;
            case Difficulty.Challenge:
                difficultySlider.GetComponent<Slider>().value = 2;
                difficultyText.GetComponent<Text>().text = "Challenge";
                break;
        }
    }

    public void ToggleVolume()
    {
        changesMade = true;
        GameController.controller.volumeMuted = !GameController.controller.volumeMuted;
    }

    public void ChangeMusicLevel(GameObject slider)
    {
        changesMade = true;
        GameController.controller.musicScale = slider.GetComponent<Slider>().value;
        AudioListener.volume = slider.GetComponent<Slider>().value;
    }

    public void ApplyChanges()
    {
        if(changesMade)
        {
            changesMade = false;
            GameController.controller.SaveCharacters();
            GameController.controller.Save(GameController.controller.playerName);
        }
    }

    public void ChangeDifficulty(GameObject scroll)
    {
        int value = (int)scroll.GetComponent<Slider>().value;

        switch(value)
        {
            case 0:
                GameController.controller.difficultyScale = Difficulty.Chill;
                difficultyText.GetComponent<Text>().text = "Chill";
                break;
            case 1:
                GameController.controller.difficultyScale = Difficulty.Story;
                difficultyText.GetComponent<Text>().text = "Story";
                break;
            case 2:
                GameController.controller.difficultyScale = Difficulty.Challenge;
                difficultyText.GetComponent<Text>().text = "Challenge";
                break;
        }

        changesMade = true;
    }

    public void BackToMM()
    {
        if(changesMade)
        {
            confirmationPanel.SetActive(true);
        }
        else
            StartCoroutine(LoadMM());
    }

    IEnumerator LoadMM()
    {
        blackSq.GetComponent<FadeScript>().FadeIn(2);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("MainMenu_Scene");
    }

    public void ApplyAndLeave()
    {
        confirmationPanel.SetActive(false);
        GameController.controller.SaveCharacters();
        GameController.controller.Save(GameController.controller.playerName);
        StartCoroutine(LoadMM());
    }

    public void DiscardChanges()
    {
        StartCoroutine(LoadMM());
    }
}
