using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LerpScript : MonoBehaviour {

    private bool lerping = false;
    private bool lerpingPosition = false;
    private bool lerpingScale = false;
    private bool lerpingColor = false;
    private float t_P = 0f;
    private float t_S = 0f;
    private float t_C = 0f;
    Vector3 initPos;
    Vector3 finalPos;
    Vector3 initScale;
    Vector3 finalScale;
    Color initColor = Color.white;
    Color finalColor = Color.white;
    private float rate = 1f;
    private bool localPosLerp;

    public void LerpToPos(Vector3 startPos, Vector3 endPos, float speed = 1f, bool local = false)
    {
        t_P = 0f;
        initPos = startPos;
        finalPos = endPos;
        rate = speed;
        lerping = true;
        lerpingPosition = true;
        localPosLerp = local;
    }

    public void LerpToScale(Vector3 startScale, Vector3 endScale, float speed = 1f)
    {
        t_S = 0f;
        initScale = startScale;
        finalScale = endScale;
        rate = speed;
        lerping = true;
        lerpingScale = true;
    }

    public void LerpToColor(Color startColor, Color endColor, float speed = 1f)
    {
        t_C = 0f;
        initColor = startColor;
        finalColor = endColor;
        rate = speed;
        lerping = true;
        lerpingColor = true;
    }

    // Update is called once per frame
    void Update ()
    {
		if(lerping)
        {
            if(lerpingPosition)
            {
                t_P += Time.deltaTime * rate;
                Vector3 pos = Vector3.Lerp(initPos, finalPos, t_P);

                if (localPosLerp)
                    transform.localPosition = pos;
                else
                    transform.position = pos;


                if (t_P > 1)
                {
                    t_P = 0;
                    lerpingPosition = false;
                }
            }

            if (lerpingScale)
            {
                t_S += Time.deltaTime * rate;

                Vector3 scale = Vector3.Lerp(initScale, finalScale, t_S);
                transform.localScale = scale;

                if (t_S > 1)
                {
                    t_S = 0;
                    lerpingScale = false;
                }
            }

            if (lerpingColor)
            {
                t_C += Time.deltaTime * rate;

                Color c = Color.Lerp(initColor, finalColor, t_C);

                if(this.GetComponent<Image>())
                    this.GetComponent<Image>().color = c;
                if(this.GetComponent<SpriteRenderer>())
                    this.GetComponent<SpriteRenderer>().color = c;
                if(this.GetComponent<Text>())
                    this.GetComponent<Text>().color = c;
                    
                if (t_C > 1)
                {
                    t_C = 0;
                    lerpingColor = false;
                }
            }

            if (!lerpingPosition && !lerpingScale && !lerpingColor)
            {
                lerping = false;
            }
        }
	}
}
