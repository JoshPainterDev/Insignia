using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FadeScript : MonoBehaviour {

    private Color initColor;
    private Color finalColor;
    private bool lerpingColor = false;
    private float t_C = 0f;
    private float rate = 1f;

    // Use this for initialization
    void Start ()
    {
        this.GetComponent<Image>().enabled = true;
        FadeOut();
	}
	
    public void FadeTo(Color endColor, float speed = 1.0f)
    {
        initColor = this.GetComponent<Image>().color;
        finalColor = endColor;
        rate = speed;
        lerpingColor = true;
        t_C = 0;
    }

    public void FadeIn(float speed = 1.0f)
    {
        initColor = this.GetComponent<Image>().color;
        finalColor = new Color(initColor.r, initColor.g, initColor.b, 1);
        lerpingColor = true;
        rate = speed;
        t_C = 0;
    }

    public void FadeOut(float speed = 1.0f)
    {
        initColor = this.GetComponent<Image>().color;
        finalColor = new Color(initColor.r, initColor.g, initColor.b, 0);
        lerpingColor = true;
        rate = speed;
        t_C = 0;
    }

	// Update is called once per frame
	void Update ()
    {
        if (lerpingColor)
        {
            t_C += Time.deltaTime * rate;

            Color c = Color.Lerp(initColor, finalColor, t_C);

            if (this.GetComponent<Image>())
                this.GetComponent<Image>().color = c;
            if (this.GetComponent<SpriteRenderer>())
                this.GetComponent<SpriteRenderer>().color = c;
            if (this.GetComponent<Text>())
                this.GetComponent<Text>().color = c;

            if (t_C > 1)
            {
                t_C = 0;
                lerpingColor = false;
            }
        }
    }


}
