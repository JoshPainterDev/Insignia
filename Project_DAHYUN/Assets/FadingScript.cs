using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(LerpScript))]
public class FadingScript : MonoBehaviour
{
    public bool startOnAwake = true;
    public float speed = 1.0f;
    public float delay = 0.0f;
    private bool active = false;

    public Color otherColor = Color.white;

    private Color origColor;

    private void Awake()
    {
        if (this.GetComponent<Image>())
            origColor = this.GetComponent<Image>().color;
        else
            origColor = this.GetComponent<Text>().color - new Color(0,0,0,1);
    }

    // Use this for initialization
    void Start()
    {
        if (startOnAwake)
        {
            active = false;
            StartCoroutine(warmUp());
        }
    }

    IEnumerator MoveToFirst()
    {
        if (active)
        {
            this.GetComponent<LerpScript>().LerpToColor(origColor, otherColor, speed);
            yield return new WaitForSeconds((1 / speed) + delay);
            StartCoroutine(MoveToSecond());
        }
    }

    IEnumerator MoveToSecond()
    {
        if (active)
        {
            this.GetComponent<LerpScript>().LerpToColor(otherColor, origColor, speed);
            yield return new WaitForSeconds((1 / speed));
            StartCoroutine(MoveToFirst());
        }
    }

    public void StopFading()
    {
        active = false;
    }

    public void Restart()
    {
        active = false;
        StartCoroutine(warmUp());
    }

    IEnumerator warmUp()
    {
        
        if (this.GetComponent<Image>())
            this.GetComponent<Image>().color = Color.clear;
        else
            this.GetComponent<Text>().color = Color.clear;

        active = true;
        this.GetComponent<LerpScript>().LerpToColor(new Color(1,1,1,0), otherColor, 1);
        yield return new WaitForSeconds(1f);
        StartCoroutine(MoveToSecond());
    }
}
