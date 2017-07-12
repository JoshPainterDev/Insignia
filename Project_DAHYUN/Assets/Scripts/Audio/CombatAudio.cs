using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CombatAudio : MonoBehaviour {

    public AudioClip select_UI;
    public AudioClip abilitySelect_UI;
    public AudioClip back_UI;
    AudioSource audioSource;

    public float LOUD_VOLUME = 1.0f;
    public float MEDIUM_VOLUME = 0.7f;
    public float QUIET_VOLUME = 0.50f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playUISelect()
    {
        audioSource.PlayOneShot(select_UI, MEDIUM_VOLUME);
    }

    public void playUIAbilitySelect()
    {
        audioSource.PlayOneShot(abilitySelect_UI, MEDIUM_VOLUME);
    }

    public void playUIBack()
    {
        audioSource.PlayOneShot(back_UI, MEDIUM_VOLUME);
    }

    public void stopAllAudio()
    {

    }
}
