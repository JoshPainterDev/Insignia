using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{

    private float t = 0f;
    private float initScale;
    private float finalScale;
    private float rate = 10f;
    private bool lerpingScale = false;

    public void LerpTimeScale(float startVal, float finalVal, float lerpRate)
    {
        t = 0f;
        initScale = startVal;
        finalScale = finalVal;
        rate = lerpRate;
        lerpingScale = true;
    }

    private void Update()
    {
        if (lerpingScale)
        {
            t += 0.0333f * rate;

            float scale = Mathf.Lerp(initScale, finalScale, t);
            Time.timeScale = scale;

            if (t > 1)
            {
                t = 0;
                lerpingScale = false;
            }
        }
    }
}
