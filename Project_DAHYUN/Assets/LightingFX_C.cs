using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingFX_C : MonoBehaviour
{
    private bool flickering = false;
    private float speed = 1.0f;
    private float lightDuration = 0.15f;
    private LerpScript lerper;
    private Color origC;
    private Color startC;
    private Color endC;
    private Coroutine flickerCR;


    // Start is called before the first frame update
    void Start()
    {
        lerper = this.GetComponent<LerpScript>();
        origC = this.GetComponent<SpriteRenderer>().color;
    }



    public bool getFlickering()
    {
        return flickering;
    }

    public void startFlickering(Color start, Color end, float newSpeed = 1.0f, float lDur = 0.15f)
    {
        flickering = true;
        startC = start;
        endC = end;
        speed = newSpeed;
        lightDuration = lDur;

        flickerCR = StartCoroutine(LerpLighting());
    }

    public void stopFlickering(bool resetColor = false)
    {
        flickering = false;
        StopCoroutine(flickerCR);
        if(resetColor)
            this.GetComponent<SpriteRenderer>().color = origC;
    }

    IEnumerator LerpLighting()
    {
        lerper.LerpToColor(startC, endC, speed);
        yield return new WaitForSeconds(lightDuration + Random.Range(-0.75f * lightDuration, 0.75f * lightDuration));

        if (flickering)
            StartCoroutine(LerpLighting());
    }
}

