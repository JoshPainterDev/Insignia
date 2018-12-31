using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MenuUIAudio : MonoBehaviour
{
    public AudioClip buttonSelect01;
    public AudioClip nope;
    public AudioClip back;
    public AudioClip typingEffect;
    public AudioClip levelUp;
    public AudioClip buttonClick;
    public AudioClip error;
    public AudioClip heartbeat;
    AudioSource audioSource;

    public float HIGH_VOLUME = 1.0f;
    public float MEDIUM_VOLUME = 0.7f;
    public float LOW_VOLUME = 0.50f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (GameController.controller.volumeMuted)
            GameController.controller.volumeScale = 0f;

        HIGH_VOLUME *= GameController.controller.volumeScale;
        MEDIUM_VOLUME *= GameController.controller.volumeScale;
        LOW_VOLUME *= GameController.controller.volumeScale;
    }

    public void playSoundClip(AudioClip sound, float volume = 0.5f)
    {
        audioSource.PlayOneShot(sound, volume * GameController.controller.volumeScale);
    }

    public void playTypingEffect()
    {
        audioSource.PlayOneShot(typingEffect, MEDIUM_VOLUME * GameController.controller.volumeScale);
    }

    public void playError()
    {
        audioSource.PlayOneShot(error, MEDIUM_VOLUME * GameController.controller.volumeScale);
    }

    public void playButtonClick()
    {
        audioSource.PlayOneShot(buttonClick, MEDIUM_VOLUME * GameController.controller.volumeScale);
    }

    public void playLevelUp()
    {
        audioSource.PlayOneShot(levelUp, MEDIUM_VOLUME * GameController.controller.volumeScale);
    }

    public void playNope()
    {
        audioSource.PlayOneShot(nope, MEDIUM_VOLUME * GameController.controller.volumeScale);
    }

    public void playBack()
    {
        audioSource.PlayOneShot(back, MEDIUM_VOLUME * GameController.controller.volumeScale);
    }

    public void playHeartbeat()
    {
        audioSource.PlayOneShot(heartbeat, MEDIUM_VOLUME * GameController.controller.volumeScale);
    }

    public void stopAllAudio()
    {
        audioSource.Stop();
    }
}
