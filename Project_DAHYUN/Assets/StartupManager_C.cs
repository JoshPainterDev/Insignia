using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartupManager_C : MonoBehaviour
{
    public GameObject cameraObj;
    public GameObject tap2Start;
    public GameObject inputDetector;
    public GameObject title;
    public GameObject blackSq;
	// Use this for initialization
	void Start ()
    {
        title.GetComponent<SpriteRenderer>().color = Color.clear;
        tap2Start.SetActive(false);
        blackSq.GetComponent<Image>().enabled = true;
        inputDetector.SetActive(false);
        blackSq.GetComponent<FadeScript>().FadeColored(Color.white, new Color(1, 1, 1, 0), 0.25f);
        StartCoroutine(StartupSequence());
    }

    IEnumerator StartupSequence()
    {
        yield return new WaitForSeconds(0.25f);
        Vector3 newCamPos = cameraObj.transform.position + new Vector3(0, 135, 0);
        cameraObj.GetComponent<LerpScript>().LerpToPos(cameraObj.transform.position, newCamPos, 0.25f);
        yield return new WaitForSeconds(5f);
        title.GetComponent<LerpScript>().LerpToColor(new Color(1,1,1,0), Color.white, 0.5f);
        yield return new WaitForSeconds(0.5f);
        inputDetector.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        tap2Start.SetActive(true);
        tap2Start.GetComponent<FadingScript>().Restart();
    }

    public void startGame()
    {
        StartCoroutine(LoadCharSelect());
    }

    IEnumerator LoadCharSelect()
    {
        blackSq.GetComponent<FadeScript>().FadeColored(new Color(1,1,1,0), Color.white, 0.75f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("CharacterSelect_Scene");
    }
}
