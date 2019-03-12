using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum BoostType {Attack, Defense, Speed, Agressive, Defensive, Focused, none};



[RequireComponent(typeof(LerpScript))]
public class StatBoost_C : MonoBehaviour
{
    public BoostType boostType;
    public int player = 1;
    public float Speed;

    public Sprite attSprite;
    public Sprite defSprite;
    public Sprite spdSprite;

    public AudioClip attSFX;
    public AudioClip defSFX;
    public AudioClip spdSFX;

    private Vector3 origPos;
    private Vector3 origScale;
    private Color origColor;
    private LerpScript lerper;

    // Use this for initialization
    void Start()
    {
        lerper = this.GetComponent<LerpScript>();
        origPos = this.transform.position;
        origScale = this.transform.localScale;
        origColor = this.GetComponent<SpriteRenderer>().color;

        this.transform.localScale = Vector3.zero;

        switch(boostType)
        {
            case BoostType.Attack:
                this.GetComponent<SpriteRenderer>().sprite = attSprite;
                this.GetComponent<AudioSource>().clip = attSFX;
                break;
            case BoostType.Defense:
                this.GetComponent<SpriteRenderer>().sprite = defSprite;
                this.GetComponent<AudioSource>().clip = defSFX;
                break;
            case BoostType.Speed:
                this.GetComponent<SpriteRenderer>().sprite = spdSprite;
                this.GetComponent<AudioSource>().clip = spdSFX;
                break;
        }

        StartCoroutine(startingAnimation());
    }

    IEnumerator startingAnimation()
    {
        this.GetComponent<AudioSource>().Play();
        lerper.LerpToPos(origPos - new Vector3(30 * player, 30, 0), origPos, Speed);
        lerper.LerpToScale(origScale * 0.1f, origScale, Speed);
        lerper.LerpToColor(new Color(origColor.r, origColor.g, origColor.b, 0), origColor, Speed);
        yield return new WaitForSeconds(1.5f);
        lerper.LerpToScale(origScale, origScale * 1.5f, Speed * 2);
        yield return new WaitForSeconds(0.1f);
        lerper.LerpToPos(origPos, origPos + new Vector3(7 * player, 100, 0), Speed);
        lerper.LerpToScale(origScale * 1.5f, Vector3.zero, Speed * 2);
        lerper.LerpToColor(origColor, new Color(origColor.r, origColor.g, origColor.b, 0), Speed * 2);
    }
}
