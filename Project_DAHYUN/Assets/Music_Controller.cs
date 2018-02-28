using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Music_Controller : MonoBehaviour
{
    public bool FadeOnStart = false;

    public AudioClip ShadowTheme;
    private AudioClip musicLoop;

    AudioSource audioSource;

    public float HIGH_VOLUME = 1.0f;
    public float MEDIUM_VOLUME = 0.7f;
    public float LOW_VOLUME = 0.50f;
    public float BACKGROUND_VOLUME = 0.15f;

    public float STARTING_VOLUME = 0.5f;

    private float t = 0f;
    private bool lerping = false;
    public float rate = 1f;
    private float init = 0f;
    private float final = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        musicLoop = audioSource.clip;

        if (GameController.controller.volumeMuted)
            GameController.controller.volumeScale = 0f;

        HIGH_VOLUME *= GameController.controller.volumeScale;
        MEDIUM_VOLUME *= GameController.controller.volumeScale;
        LOW_VOLUME *= GameController.controller.volumeScale;
        BACKGROUND_VOLUME *= GameController.controller.volumeScale;

        if (FadeOnStart)
        {
            FadeVolume(0, STARTING_VOLUME * GameController.controller.volumeScale, 0.25f);
        }
    }

    public void PlayShadowTheme()
    {
        audioSource.PlayOneShot(ShadowTheme, BACKGROUND_VOLUME);
        audioSource.loop = true;
        FadeVolume(0, BACKGROUND_VOLUME * GameController.controller.volumeScale);
    }

    public void restartMusic()
    {
        audioSource.PlayOneShot(musicLoop, BACKGROUND_VOLUME * GameController.controller.volumeScale);
        audioSource.loop = true;
        FadeVolume(0, BACKGROUND_VOLUME * GameController.controller.volumeScale);
    }

    public void stopAllMusic()
    {
        audioSource.Stop();
    }

    public void FadeVolume(float startPercent, float endPercent, float speed = 1f)
    {
        init = startPercent * 100f;
        final = endPercent * 100f;
        rate = speed;
        lerping = true;
    }

    public void FadeOutVolume(float speed = 1.0f)
    {

        init = audioSource.volume * 100f;
        final = 0;
        rate = speed;
        lerping = true;
    }

    void Update()
    {
        if (lerping)
        {
            t += Time.deltaTime * rate;

            float pos = Mathf.Lerp(init, final, t);
            audioSource.volume = pos / 100f;

            if (t > 1)
            {
                t = 0;
                lerping = false;
            }
        }
    }
}

