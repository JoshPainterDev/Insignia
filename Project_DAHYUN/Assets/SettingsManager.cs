using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public GameObject popup;

    public GameObject background;
    public GameObject blackSq;
    public GameObject confirmationPanel;
    public GameObject musicSlider;
    public GameObject difficultySlider;
    public GameObject muteToggle;
    public GameObject menuManager;

    public GameObject difficultyText;
    private AudioSource audioComponent;

    private bool changesMade = false;
    private float musicLevel = 1.0f;

    // Use this for initialization
    void Start ()
    {
        audioComponent = this.GetComponent<AudioSource>();
    }

    public void ResetPosition()
    {
        changesMade = false;
        //background.GetComponent<SpriteRenderer>().color = GameController.controller.getPlayerColorPreference();
        musicSlider.GetComponent<Slider>().value = GameController.controller.volumeScale;
        muteToggle.GetComponent<Toggle>().isOn = GameController.controller.volumeMuted;
        print(GameController.controller.volumeScale);

        switch (GameController.controller.difficultyScale)
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
        StartCoroutine(ResetPosRoutine());
    }

    IEnumerator ResetPosRoutine()
    {
        Vector3 endPos = new Vector3(225, -92, 0);
        Vector3 startPos = endPos + new Vector3(0, 300, 0);
        popup.GetComponent<LerpScript>().LerpToPos(startPos, endPos - new Vector3(0, 20, 0), 3.0f);
        yield return new WaitForSeconds(0.05f);
        popup.GetComponent<Canvas>().enabled = true;
        yield return new WaitForSeconds(0.25f);
        popup.GetComponent<LerpScript>().LerpToPos(endPos - new Vector3(0, 20, 0), endPos, 3.0f);
    }

    public void PingNewVolumeSound()
    {
        audioComponent.volume = GameController.controller.volumeScale;
        audioComponent.Play();
    }

    public void ToggleVolume()
    {
        changesMade = true;
        GameController.controller.volumeMuted = muteToggle.GetComponent<Toggle>().isOn;

        if (GameController.controller.volumeMuted)
            GameController.controller.volumeScale = 0;
    }

    public void ChangeMusicLevel(GameObject slider)
    {
        changesMade = true;

        GameController.controller.volumeScale = slider.GetComponent<Slider>().value;
        AudioListener.volume = slider.GetComponent<Slider>().value;

        if (GameController.controller.volumeMuted)
        {
            GameController.controller.volumeMuted = false;
            muteToggle.GetComponent<Toggle>().isOn = false;
        }
            
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
            StartCoroutine(ExitSettings());
    }

    IEnumerator ExitSettings()
    {
        Vector3 startPos = popup.transform.position;
        yield return new WaitForSeconds(0.1f);
        popup.GetComponent<LerpScript>().LerpToPos(startPos, startPos - new Vector3(0, 20, 0), 2.0f);
        yield return new WaitForSeconds(0.15f);
        popup.GetComponent<LerpScript>().LerpToPos(startPos - new Vector3(0, 20, 0), startPos + new Vector3(0, 500, 0), 2.0f);
        yield return new WaitForSeconds(0.35f);
        popup.GetComponent<Canvas>().enabled = false;
        confirmationPanel.SetActive(false);
        menuManager.GetComponent<MainMenuManager>().EnableButtons();
        popup.SetActive(false);
    }

    public void ApplyAndLeave()
    {
        confirmationPanel.SetActive(false);
        GameController.controller.SaveCharacters();
        GameController.controller.Save(GameController.controller.playerName);
        StartCoroutine(ExitSettings());
    }

    public void DiscardChanges()
    {
        StartCoroutine(ExitSettings());
    }
}
