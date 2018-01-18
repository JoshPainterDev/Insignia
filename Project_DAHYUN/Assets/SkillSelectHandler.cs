using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectHandler : MonoBehaviour
{
    public int SkillNumber;
    public Color lockColor = new Color(0.75f, 0.75f, 0.75f, 0.35f);
    public GameObject evilHighlight;
    public GameObject goodHighlight;

    private Color c_EvilDisabled;
    private Color c_GoodDisabled;

    private int SkillSelected = 0;

	// Use this for initialization
	void Start ()
    {
        c_EvilDisabled = evilHighlight.GetComponent<Image>().color;
        c_GoodDisabled = goodHighlight.GetComponent<Image>().color;
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

    public void SelectEvilSkill()
    {
        if(SkillSelected == 0 || SkillSelected == 2)
        {
            print("evil skill");
            SkillSelected = 1;
            Color c = evilHighlight.GetComponent<Image>().color;
            evilHighlight.GetComponent<Image>().color = new Color(c.r, c.g, c.b, 1.0f);
            goodHighlight.GetComponent<Image>().color = c_GoodDisabled;

            GameController.controller.skillTree[SkillNumber - 1] = SkillSelected;
        }
    }

    public void SelectGoodSkill()
    {
        if (SkillSelected == 0 || SkillSelected == 1)
        {
            print("good skill");
            SkillSelected = 2;
            Color c = goodHighlight.GetComponent<Image>().color;
            goodHighlight.GetComponent<Image>().color = new Color(c.r, c.g, c.b, 1.0f);
            evilHighlight.GetComponent<Image>().color = c_EvilDisabled;

            GameController.controller.skillTree[SkillNumber - 1] = SkillSelected;
        }
    }
}
