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

    private float t_S = 0f;
    Vector3 initScale;
    Vector3 finalScale;
    public float scaler = 1.2f;
    private float rate = 10f;
    private bool lerpingScale = false;

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
        audioManager.playButtonClick();
        StartCoroutine(AnimateClick());
    }

    public void ShowButton()
    {
        this.GetComponent<Image>().enabled = true;
        foreach(Text child in this.GetComponentsInChildren<Text>())
        {
            child.enabled = true;
        }
    }

    public void HideButton()
    {
        this.GetComponent<Image>().enabled = false;
        foreach (Text child in this.GetComponentsInChildren<Text>())
        {
            child.enabled = false;
        }
    }

    public void ChangeColor()
    {
        this.GetComponent<Image>().color = newColor;
    }

    public void RevertColor()
    {
        this.GetComponent<Image>().color = origColor;
    }

    IEnumerator AnimateClick()
    {
        t_S = 0f;
        initScale = this.transform.localScale;
        finalScale = this.transform.localScale * scaler;
        lerpingScale = true;
        yield return new WaitForSeconds(0.1f);
        t_S = 0f;
        initScale = this.transform.localScale;
        finalScale = origScale;
        lerpingScale = true;
    }

    private void Update()
    {
        if (lerpingScale)
        {
            t_S += Time.deltaTime * rate;

            Vector3 scale = Vector3.Lerp(initScale, finalScale, t_S);
            transform.localScale = scale;

            if (t_S > 1)
            {
                t_S = 0;
                lerpingScale = false;
            }
        }
    }
}
