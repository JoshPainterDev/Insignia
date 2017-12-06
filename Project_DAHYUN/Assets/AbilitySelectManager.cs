using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AbilitySelectManager : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}

    public void BackToMM()
    {
        SceneManager.LoadScene("Character_Scene");
    }
}
