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
    public AudioClip stunned_SFX;
    public AudioClip thunderCharge_SFX;
    public AudioClip shoulder_Check_SFX;
    public AudioClip outrage_SFX;
    public AudioClip shadowVanish;
    public AudioClip swordRing;
    public AudioClip swordDrop;
    public AudioClip finalCut;
    public AudioClip swordHit01;
    public AudioClip swordHit02;
    public AudioClip swordHit03;
    public AudioClip swordHit04;
    public AudioClip swordHit05;
    public AudioClip swordHit06;
    public AudioClip swordHit07;
    public AudioClip swordHit08;
    public AudioClip swordImpactArmor01;
    public AudioClip swordSwoosh01;
    public AudioClip swordDraw01;
    public AudioClip swordMiss01;
    public AudioClip swordMiss02;
    public AudioClip swordMiss03;
    public AudioClip swordMiss04;
    public AudioClip swordBlood01;
    public AudioClip swordBlood02;
    public AudioClip bloodSplatter01;
    public AudioClip plasmaIgnite;
    AudioSource audioSource;

    public float HIGH_VOLUME = 1.0f;
    public float MEDIUM_VOLUME = 0.7f;
    public float LOW_VOLUME = 0.50f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playSoundEffect(AudioClip soundClip, float volume = 0.5f)
    {
        audioSource.PlayOneShot(soundClip, volume);
    }

    public void playRandomSwordHit()
    {
        int rand = Random.Range(0, 7);

        switch(rand)
        {
            case 0:
                audioSource.PlayOneShot(swordHit01, LOW_VOLUME);
                break;
            case 1:
                audioSource.PlayOneShot(swordHit02, LOW_VOLUME);
                break;
            case 2:
                audioSource.PlayOneShot(swordHit03, LOW_VOLUME);
                break;
            case 3:
                audioSource.PlayOneShot(swordHit04, LOW_VOLUME);
                break;
            case 4:
                audioSource.PlayOneShot(swordHit05, LOW_VOLUME);
                break;
            case 5:
                audioSource.PlayOneShot(swordHit06, LOW_VOLUME);
                break;
            case 6:
                audioSource.PlayOneShot(swordHit07, LOW_VOLUME);
                break;
            case 7:
                audioSource.PlayOneShot(swordHit08, LOW_VOLUME);
                break;
        }
    }

    public void playSwordHit()
    {
        audioSource.PlayOneShot(swordHit01, MEDIUM_VOLUME);
    }

    public void playStrikeHit()
    {
        StartCoroutine(StrikeSound());   
    }

    IEnumerator StrikeSound()
    {
        audioSource.PlayOneShot(swordBlood02, LOW_VOLUME);
        yield return new WaitForSeconds(0.2f);
        audioSource.PlayOneShot(swordMiss04, MEDIUM_VOLUME);
        //audioSource.PlayOneShot(swordSwoosh01, MEDIUM_VOLUME);
        yield return new WaitForSeconds(0.1f);
        audioSource.PlayOneShot(swordRing, MEDIUM_VOLUME);
        //yield return new WaitForSeconds(0.1f);
        
    }

    public void playRandomSwordMiss()
    {
        int rand = Random.Range(0, 3);

        switch (rand)
        {
            case 0:
                audioSource.PlayOneShot(swordMiss01, LOW_VOLUME);
                break;
            case 1:
                audioSource.PlayOneShot(swordMiss02, LOW_VOLUME);
                break;
            case 2:
                audioSource.PlayOneShot(swordMiss03, LOW_VOLUME);
                break;
            case 3:
                audioSource.PlayOneShot(swordMiss04, LOW_VOLUME);
                break;
        }
    }

    public void playPlasmaIgnite()
    {
        audioSource.PlayOneShot(plasmaIgnite, MEDIUM_VOLUME);
    }

    public void playStunnedSFX()
    {
        audioSource.PlayOneShot(stunned_SFX, LOW_VOLUME);
    }

    public void playBloodSplatter()
    {
        audioSource.PlayOneShot(bloodSplatter01, MEDIUM_VOLUME);
    }

    public void playOutrageSFX()
    {
        audioSource.PlayOneShot(outrage_SFX, MEDIUM_VOLUME);
    }

    public void playThunderChargeFX()
    {
        audioSource.PlayOneShot(thunderCharge_SFX, MEDIUM_VOLUME);
    }

    public void playGuardBreakSFX()
    {
        audioSource.PlayOneShot(shoulder_Check_SFX, MEDIUM_VOLUME);
    }

    public void playSwordDrop()
    {
        audioSource.PlayOneShot(swordDrop, LOW_VOLUME);
    }

    public void playSwordRing()
    {
        audioSource.PlayOneShot(swordRing, LOW_VOLUME);
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
