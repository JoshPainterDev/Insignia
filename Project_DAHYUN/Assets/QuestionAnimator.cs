using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(LerpScript))]
public class QuestionAnimator : MonoBehaviour
{
    public float Speed = 1;

    private Vector3 origPos, origPos2, origPos3;
    private Vector3 origScale, origScale2, origScale3;
    private Color origColor, origColor2, origColor3;
    private LerpScript lerper, lerper2, lerper3;
    private GameObject q1;
    private GameObject q2;
    private GameObject q3;
    private AudioSource sfxPlayer;
    // Use this for initialization
    void Start ()
    {
        sfxPlayer = this.GetComponent<AudioSource>();

        q1 = this.transform.GetChild(0).gameObject;
        lerper = q1.GetComponent<LerpScript>();
        origPos = q1.transform.position;
        origScale = q1.transform.localScale;
        origColor = q1.GetComponent<SpriteRenderer>().color;
        
        q2 = this.transform.GetChild(1).gameObject;
        lerper2 = q2.GetComponent<LerpScript>();
        origPos2 = q2.transform.position;
        origScale2 = q2.transform.localScale;
        origColor2 = q2.GetComponent<SpriteRenderer>().color;
        q2.GetComponent<SpriteRenderer>().color = Color.clear;

        q3 = this.transform.GetChild(2).gameObject;
        lerper3 = q3.GetComponent<LerpScript>();
        origPos3 = q3.transform.position;
        origScale3 = q3.transform.localScale;
        origColor3 = q3.GetComponent<SpriteRenderer>().color;
        q3.GetComponent<SpriteRenderer>().color = Color.clear;

        StartCoroutine(startingAnimation());
    }

    IEnumerator startingAnimation()
    {
        lerper.LerpToPos(origPos - new Vector3(0, 1, 0), origPos, Speed);
        lerper.LerpToScale(origScale * 0.1f, origScale * 1.5f, Speed);
        lerper.LerpToColor(new Color(origColor.r, origColor.g, origColor.b, 0), origColor, Speed);

        // Q1
        yield return new WaitForSeconds(0.2f);
        lerper.LerpToScale(origScale * 1.5f, origScale, Speed);
        // Q2
        yield return new WaitForSeconds(0.2f);
        lerper2.LerpToColor(new Color(origColor2.r, origColor2.g, origColor2.b, 0), origColor2, Speed);
        lerper2.LerpToScale(origScale2 * 0.1f, origScale2, Speed);
        sfxPlayer.Play();
        // Q3
        yield return new WaitForSeconds(0.2f);
        lerper3.LerpToScale(origScale3 * 0.1f, origScale3, Speed);
        lerper3.LerpToColor(new Color(origColor3.r, origColor3.g, origColor3.b, 0), origColor3, Speed);
        sfxPlayer.Play();

        yield return new WaitForSeconds(0.8f);

        // Q2
        lerper2.LerpToPos(origPos2, origPos2 + new Vector3(-0.15f, -0.1f, 0), Speed * 2);
        lerper2.LerpToScale(origScale2, Vector3.zero, Speed * 2);
        lerper2.LerpToColor(origColor2, new Color(origColor2.r, origColor2.g, origColor2.b, 0), Speed * 2);
        // Q3
        lerper3.LerpToPos(origPos3, origPos3 + new Vector3(0.1f, -0.1f, 0), Speed * 2);
        lerper3.LerpToScale(origScale3, Vector3.zero, Speed * 2);
        lerper3.LerpToColor(origColor3, new Color(origColor3.r, origColor3.g, origColor3.b, 0), Speed * 2);
        yield return new WaitForSeconds(0.1f);
        lerper.LerpToScale(origScale, origScale * 1.5f, Speed * 2);
        yield return new WaitForSeconds(0.1f);
        // Q1
        lerper.LerpToPos(origPos, origPos - new Vector3(0, 25, 0), Speed * 2);
        lerper.LerpToScale(origScale, Vector3.zero, Speed * 2);
        lerper.LerpToColor(origColor, new Color(origColor.r, origColor.g, origColor.b, 0), Speed * 2);
    }
}
