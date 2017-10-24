using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CombatAudio : MonoBehaviour {

    public AudioClip combatLoop;
    public AudioClip select_UI;
    public AudioClip abilitySelect_UI;
    public AudioClip back_UI;
    public AudioClip nope_UI;
    public AudioClip strugglePress01;
    public AudioClip strugglePress02;
    public AudioClip strugglePress03;
    public AudioClip strugglePress04;
    public AudioClip strugglePress05;
    public AudioClip strugglePress06;
    public AudioClip shadowVanish;
    public AudioClip finalCut;
    AudioSource audioSource;

    public float HIGH_VOLUME = 1.0f;
    public float MEDIUM_VOLUME = 0.7f;
    public float LOW_VOLUME = 0.50f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playShadowVanish()
    {
        audioSource.PlayOneShot(shadowVanish, LOW_VOLUME);
    }

    public void playFinalCut()
    {
        audioSource.PlayOneShot(finalCut, MEDIUM_VOLUME);
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

    public void playUINope()
    {
        audioSource.PlayOneShot(nope_UI, MEDIUM_VOLUME);
    }

    public void playRandomStrugglePress()
    {
        int rand = Random.Range(1, 6);

        switch(rand)
        {
            case 1:
                audioSource.PlayOneShot(strugglePress01, LOW_VOLUME);
                break;
            case 2:
                audioSource.PlayOneShot(strugglePress02, LOW_VOLUME);
                break;
            case 3:
                audioSource.PlayOneShot(strugglePress03, LOW_VOLUME);
                break;
            case 4:
                audioSource.PlayOneShot(strugglePress04, LOW_VOLUME);
                break;
            case 5:
                audioSource.PlayOneShot(strugglePress05, LOW_VOLUME);
                break;
            case 6:
                audioSource.PlayOneShot(strugglePress06, LOW_VOLUME);
                break;
        }
    }

    public void stopAllAudio()
    {
        audioSource.Stop();
    }
}
