using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(LerpScript))]
public class FadingScript : MonoBehaviour
{
    public bool startOnAwake = true;
    public float speed = 1.0f;
    private bool active = false;

    public Color otherColor = Color.white;

    private Color origColor;
    // Use this for initialization
    void Start()
    {
        origColor = this.GetComponent<Image>().color;

        if (startOnAwake)
        {
            active = true;
            StartCoroutine(MoveToFirst());
        }
    }

    IEnumerator MoveToFirst()
    {
        if (active)
        {
            this.GetComponent<LerpScript>().LerpToColor(origColor, otherColor, speed);
            yield return new WaitForSeconds(1 / speed);
            StartCoroutine(MoveToSecond());
        }
    }

    IEnumerator MoveToSecond()
    {
        if (active)
        {
            this.GetComponent<LerpScript>().LerpToColor(otherColor, origColor, speed);
            yield return new WaitForSeconds(1 / speed);
            StartCoroutine(MoveToFirst());
        }
    }

    public void StopFading()
    {
        active = false;
    }
}
