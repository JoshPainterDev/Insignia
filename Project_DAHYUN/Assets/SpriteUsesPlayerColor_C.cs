using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteUsesPlayerColor_C : MonoBehaviour
{
    [Range(0, 1.0f)]
    public float alpha = 1.0f;

    // Use this for initialization
    void Start ()
    {
        Color playerColor = GameController.controller.getPlayerColorPreference();
        playerColor.a = alpha;
        this.GetComponent<SpriteRenderer>().color = playerColor;
    }
}
