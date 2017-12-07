using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OverlayScript : MonoBehaviour
{
    public GameObject cameraObj;

    float startAlpha = 0;
    float endAlpha = 0;
    float pulseSpeed = 0;

    // Use this for initialization
    void Start () {
		
	}

    public void Pulse(float startA, float endA, float speed = 1.0f)
    {
        startAlpha = startA;
        endAlpha = endA;
        pulseSpeed = speed;
        StartCoroutine(PulseRoutine());
    }

    IEnumerator PulseRoutine()
    {
        Color startC = new Color(this.GetComponent<Image>().color.r, this.GetComponent<Image>().color.g, this.GetComponent<Image>().color.b, startAlpha);
        Color endC = new Color(this.GetComponent<Image>().color.r, this.GetComponent<Image>().color.g, this.GetComponent<Image>().color.b, endAlpha);
        this.GetComponent<LerpScript>().LerpToColor(startC, endC, pulseSpeed);
        cameraObj.GetComponent<CameraController>().LerpCameraSize(175, 150, 3);
        GameController.controller.GetComponent<MenuUIAudio>().playHeartbeat();
        yield return new WaitForSeconds(1.5f / pulseSpeed);
        this.GetComponent<LerpScript>().LerpToColor(endC, startC, pulseSpeed);
        cameraObj.GetComponent<CameraController>().LerpCameraSize(150, 175, 3);
    }
}
