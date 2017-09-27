using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class CraftingManager : MonoBehaviour {

    public GameObject playerMannequin;

    public GameObject camera;
    public GameObject blackSq;

    public GameObject backButton;

    public Vector3 mmCameraPos;

    public void ShowMaterialOptions(int matNum)
    {
        if(matNum == 1)
        {

        }
        else if(matNum == 2)
        {

        }
        else if(matNum == 3)
        {

        }
    }

    public void GoBack()
    {
        StartCoroutine(GoToMainMenu());
    }

    public IEnumerator GoToMainMenu()
    {
        DisableButtons();
        camera.GetComponent<LerpScript>().LerpToPos(camera.transform.position, mmCameraPos, 1f);
        yield return new WaitForSeconds(0.25f);
        blackSq.GetComponent<FadeScript>().FadeIn(1.5f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("MainMenu_Scene");
    }

    public void DisableButtons()
    {
        backButton.GetComponent<Button>().enabled = false;
    }
}
