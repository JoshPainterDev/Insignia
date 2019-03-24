using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundFXManager_C : MonoBehaviour
{
    public AudioClip snowCollapse_FX;
    public AudioClip snowSteps_FX;
    public AudioClip skitterScreech_FX;
    public AudioClip exitScene_FX;
    public AudioClip footSteps_FX01;
    public AudioClip laserBombardment_FX;
    public AudioClip laserSwordIgnite_FX;
    public AudioClip laserSwordRetract_FX;
    public AudioClip claymoreIgniteFX, claymoreRetractFX;
    public AudioClip criticalHit_FX;
    public AudioClip bodyCollapse_FX;
    public AudioClip darkRumbling_FX;
    public AudioClip darkRumbling_short_FX;
    public AudioClip wetSquish_FX;
    public AudioClip swordClash_FX;
    public AudioClip ghastlyEntrance_FX;

    AudioSource audioSource;

    public float HIGH_VOLUME = 1.0f;
    public float MEDIUM_VOLUME = 0.7f;
    public float LOW_VOLUME = 0.50f;
    public float BACKGROUND_VOLUME = 0.25f;

    private float t = 0f;
    private bool lerping = false;
    public float rate = 1f;
    private float init = 0f;
    private float final = 0f;
    private bool ambientSound = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (GameController.controller.volumeMuted)
            GameController.controller.volumeScale = 0f;


        HIGH_VOLUME *= GameController.controller.volumeScale;
        MEDIUM_VOLUME *= GameController.controller.volumeScale;
        LOW_VOLUME *= GameController.controller.volumeScale;
        BACKGROUND_VOLUME *= GameController.controller.volumeScale;
    }

    public void playLaserBombardment(bool loop = false)
    {
        if(loop)
        {
            audioSource.loop = true;
            audioSource.volume = MEDIUM_VOLUME * GameController.controller.volumeScale;
            audioSource.clip = laserBombardment_FX;
            audioSource.Play();
        }
        else
        {
            audioSource.PlayOneShot(laserBombardment_FX, HIGH_VOLUME * GameController.controller.volumeScale);
        }

    }

    public void playClaymoreIgnite()
    {
        audioSource.PlayOneShot(claymoreIgniteFX, MEDIUM_VOLUME * GameController.controller.volumeScale);
    }

    public void playClaymoreRetract()
    {
        audioSource.PlayOneShot(claymoreRetractFX, MEDIUM_VOLUME * GameController.controller.volumeScale);
    }

    public void playSwordClash()
    {
        audioSource.PlayOneShot(swordClash_FX, MEDIUM_VOLUME * GameController.controller.volumeScale);
    }

    public void playwetSquish()
    {
        audioSource.PlayOneShot(wetSquish_FX, HIGH_VOLUME * GameController.controller.volumeScale);
    }

    public void playDarkRumbling()
    {
        audioSource.PlayOneShot(darkRumbling_FX, MEDIUM_VOLUME * GameController.controller.volumeScale);
    }

    public void playDarkRumblingShort()
    {
        audioSource.PlayOneShot(darkRumbling_short_FX, MEDIUM_VOLUME * GameController.controller.volumeScale);
    }

    public void playFootSteps()
    {
        audioSource.PlayOneShot(footSteps_FX01, MEDIUM_VOLUME * GameController.controller.volumeScale);
    }

    public void playSwordIgnite()
    {
        audioSource.PlayOneShot(laserSwordIgnite_FX, HIGH_VOLUME * GameController.controller.volumeScale);
    }

    public void playSwordRetract()
    {
        audioSource.PlayOneShot(laserSwordRetract_FX, HIGH_VOLUME * GameController.controller.volumeScale);
    }

    public void playBodyCollapse()
    {
        audioSource.PlayOneShot(bodyCollapse_FX, HIGH_VOLUME * GameController.controller.volumeScale);
    }

    public void playCriticalHit()
    {
        audioSource.PlayOneShot(criticalHit_FX, MEDIUM_VOLUME * GameController.controller.volumeScale);
    }

    public void playSnowCollapse()
    {
        audioSource.PlayOneShot(snowCollapse_FX, HIGH_VOLUME * GameController.controller.volumeScale);
    }

    public void playGhastlyEntrance()
    {
        audioSource.PlayOneShot(ghastlyEntrance_FX, HIGH_VOLUME * GameController.controller.volumeScale);
    }


    public void playSnowSteps()
    {
        audioSource.PlayOneShot(snowSteps_FX, MEDIUM_VOLUME * GameController.controller.volumeScale);
        FadeVolume(0, GameController.controller.volumeScale, 0.15f);
    }

    public void stopAllMusic()
    {
        audioSource.Stop();
    }

    public void playSkitterScreech()
    {
        audioSource.PlayOneShot(skitterScreech_FX, HIGH_VOLUME * GameController.controller.volumeScale);
    }

    public void playExitScene()
    {
        audioSource.PlayOneShot(exitScene_FX, HIGH_VOLUME * GameController.controller.volumeScale);
    }

    public void FadeVolume(float startPercent, float endPercent, float speed = 1f, bool ambient = false)
    {
        init = startPercent * 100f;
        final = endPercent * 100f;
        rate = speed;
        lerping = true;
        ambientSound = ambient;
    }

    void Update()
    {
        if (lerping)
        {
            t += Time.deltaTime * rate;

            float pos = Mathf.Lerp(init, final, t);

            if (ambientSound)
                this.transform.GetChild(0).GetComponent<AudioSource>().volume = pos / 100f;
            else
                audioSource.volume = pos / 100f;

            if (t > 1)
            {
                t = 0;
                lerping = false;
            }
        }
    }
}

