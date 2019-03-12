using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(LerpScript))]
public class StanceBoost_C : MonoBehaviour
{
    public BoostType boostType;
    public int player = 1;
    public float Speed;

    public Sprite aSprite;
    public Sprite dSprite;
    public Sprite fSprite;

    public AudioClip aSFX;
    public AudioClip dSFX;
    public AudioClip fSFX;

    private Vector3 origPos;
    private Vector3 origScale;
    private Color origColor;
    private LerpScript lerper;

    // Use this for initialization
    void Start()
    {
        lerper = this.GetComponent<LerpScript>();
        origPos = this.transform.position + new Vector3(10, 100, 0);
        origScale = this.transform.localScale;
        origColor = this.GetComponent<SpriteRenderer>().color;

        this.transform.localScale = Vector3.zero;

        switch(boostType)
        {
            case BoostType.Agressive:
                this.GetComponent<SpriteRenderer>().sprite = aSprite;
                this.GetComponent<AudioSource>().clip = aSFX;
                break;
            case BoostType.Defensive:
                this.GetComponent<SpriteRenderer>().sprite = dSprite;
                this.GetComponent<AudioSource>().clip = dSFX;
                break;
            case BoostType.Focused:
                this.GetComponent<SpriteRenderer>().sprite = fSprite;
                this.GetComponent<AudioSource>().clip = fSFX;
                break;
        }

        StartCoroutine(startingAnimation());
    }

    IEnumerator startingAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        lerper.LerpToPos(origPos - new Vector3(0, 30, 0), origPos, Speed);
        lerper.LerpToScale(origScale * 0.1f, origScale, Speed * 2.0f);
        yield return new WaitForSeconds(1.0f / Speed);
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(2.75f / Speed);
        lerper.LerpToScale(origScale, Vector3.zero, Speed * 2.75f);
        lerper.LerpToColor(origColor, new Color(origColor.r, origColor.g, origColor.b, 0), Speed);
        bool playerStance = player == 1 ? true : false;
        FindObjectOfType<CombatManager>().ChangeStanceIcon(this.GetComponent<SpriteRenderer>().sprite, playerStance);
    }
}
