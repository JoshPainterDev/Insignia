using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Music_Controller : MonoBehaviour
{
    public bool FadeOnStart = false;

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

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        musicLoop = audioSource.clip;

        if (FadeOnStart)
        {
            FadeVolume(0, STARTING_VOLUME, 0.25f);
        }
    }

    public void restartMusic()
    {
        audioSource.PlayOneShot(musicLoop, BACKGROUND_VOLUME);
        audioSource.loop = true;
        FadeVolume(0, BACKGROUND_VOLUME);
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

