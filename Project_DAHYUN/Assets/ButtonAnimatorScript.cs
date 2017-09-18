using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ButtonAnimatorScript : MonoBehaviour
{
    MenuUIAudio audioManager;
    public Color newColor = Color.white;

    Vector3 origScale;
    Vector3 origPos;
    Color origColor;
    

    // Use this for initialization
    void Start ()
    {
        audioManager = GameController.controller.GetComponent<MenuUIAudio>();
        origScale = this.transform.localScale;
        origPos = this.transform.position;
        origColor = this.GetComponent<Image>().color;
	}

    public void AnimateButtonClick()
    {

    }
}
