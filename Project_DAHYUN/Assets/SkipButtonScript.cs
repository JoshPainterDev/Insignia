using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SkipButtonScript : MonoBehaviour {

    public GameObject inputDetector;
    private Color origColor;
    private bool noInput = true;
    private float speed = 3f;

	// Use this for initialization
	void Start ()
    {
        origColor = this.GetComponent<Image>().color;
        Hide();
	}

    public void DisableButton()
    {
        this.GetComponent<Button>().enabled = false;
    }

    public void Reveal()
    {
        noInput = false;
        inputDetector.GetComponent<Image>().enabled = false;
        this.GetComponent<LerpScript>().LerpToColor(Color.clear, origColor, speed);
        this.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.clear, Color.white, speed);
        this.GetComponent<Image>().enabled = true;
        this.GetComponentInChildren<Text>().enabled = true;
        Invoke("Hide", 1.5f);
        Invoke("ResetInput", 1.5f);
    }

    public void Hide()
    {
        this.GetComponent<LerpScript>().LerpToColor(origColor, Color.clear, speed);
        this.transform.GetChild(0).GetComponent<LerpScript>().LerpToColor(Color.white, Color.clear, speed);
        this.GetComponent<Image>().enabled = false;
        this.GetComponentInChildren<Text>().enabled = false;
    }

    void ResetInput()
    {
        if(noInput)
        {
            inputDetector.GetComponent<Image>().enabled = true;
        }
        else
        {
            noInput = true;
            Invoke("ResetInput", 1.5f);
        }
    }
}
