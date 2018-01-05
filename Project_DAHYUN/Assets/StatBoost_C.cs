using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(LerpScript))]
public class StatBoost_C : MonoBehaviour
{
    public int player = 1;
    public float Speed;

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

        StartCoroutine(startingAnimation());
    }

    IEnumerator startingAnimation()
    {
        lerper.LerpToPos(origPos - new Vector3(30 * player, 30 * player, 0), origPos, Speed);
        lerper.LerpToScale(origScale * 0.1f, origScale, Speed);
        lerper.LerpToColor(new Color(origColor.r, origColor.g, origColor.b, 0), origColor, Speed);
        yield return new WaitForSeconds(1.5f);
        lerper.LerpToScale(origScale, origScale * 1.5f, Speed * 2);
        yield return new WaitForSeconds(0.1f);
        lerper.LerpToPos(origPos, origPos + new Vector3(20 * player, 100, 0), Speed * 2);
        lerper.LerpToScale(origScale * 1.5f, Vector3.zero, Speed * 2);
        lerper.LerpToColor(origColor, new Color(origColor.r, origColor.g, origColor.b, 0), Speed * 2);
    }
}
