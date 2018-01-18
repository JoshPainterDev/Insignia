using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectHandler : MonoBehaviour
{
    public Color lockColor = new Color(0.75f, 0.75f, 0.75f, 0.35f);

	// Use this for initialization
	void Start () {
		
	}

    public void LockEvilSkill()
    {
        this.transform.GetChild(1).GetComponent<Button>().enabled = false;
        this.transform.GetChild(1).GetComponent<Image>().color = lockColor;
    }

    public void LockGoodSkill()
    {
        this.transform.GetChild(2).GetComponent<Button>().enabled = false;
        this.transform.GetChild(2).GetComponent<Image>().color = lockColor;
    }
}
