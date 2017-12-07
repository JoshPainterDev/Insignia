using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    public string BossName = "";

	// Use this for initialization
	void Start ()
    {
		switch(BossName)
        {
            case "Seamstress":
                StartCoroutine(Seamstress());
                break;
        }
	}

    IEnumerator Seamstress()
    {
        yield return new WaitForSeconds(1.85f);
        this.GetComponent<Animator>().SetInteger("AnimState", -1);
        yield return new WaitForSeconds(0.75f);
        this.GetComponent<Animator>().SetInteger("AnimState", 0);
    }
}
