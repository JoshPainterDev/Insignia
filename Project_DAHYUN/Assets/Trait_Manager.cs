using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Trait_Manager : MonoBehaviour
{
    public Color c_Good;
    public Color c_Evil;
    public Color c_EvilActive;
    public Color c_EvilDisabled;
    public Color c_GoodActive;
    public Color c_GoodDisabled;
    public GameObject background;
    public GameObject traitTab;

    public int[] unlockRequirements;

    public GameObject content;

    private int p_EvilPoints;
    private int p_GoodPoints;
    private int MAX_UNLOCKS;
    private int[] skills;
    

    // Use this for initialization
    void Start ()
    {
        MAX_UNLOCKS = unlockRequirements.Length;
        p_EvilPoints = GameController.controller.playerEvilPoints;
        p_GoodPoints = GameController.controller.playerGoodPoints;
        skills = GameController.controller.skillTree;

        setBackground();
        Invoke("lockSkills", 0.1f);
	}

    public void lockSkills()
    {
        //this loop goes through in reverse order!
        for(int i = 0; i < MAX_UNLOCKS; ++i)
        {

            if (skills[MAX_UNLOCKS - i - 1] == 2)
            {
                content.transform.GetChild(i).GetComponent<SkillSelectHandler>().SelectGoodSkill();
                continue;
            }

            if (skills[MAX_UNLOCKS - i - 1] == 1)
            {
                content.transform.GetChild(i).GetComponent<SkillSelectHandler>().SelectEvilSkill();
                continue;
            }

            if (p_EvilPoints < unlockRequirements[MAX_UNLOCKS - i - 1])
            {
                content.transform.GetChild(i).GetComponent<SkillSelectHandler>().LockEvilSkill();
            }

            if (p_GoodPoints < unlockRequirements[MAX_UNLOCKS - i - 1])
            {
                content.transform.GetChild(i).GetComponent<SkillSelectHandler>().LockGoodSkill();
            }
        }
    }

    void setBackground()
    {
        float c = Mathf.Abs(p_EvilPoints - p_GoodPoints);

        if(c < 150)
        {
            background.GetComponent<Image>().color = new Color(1,1,1,1);
        }
        else if (p_EvilPoints > p_GoodPoints)
        {
            Color tempC = c_Evil;
            tempC.a = (float)(p_EvilPoints - p_GoodPoints) / 1500.0f;
            background.GetComponent<Image>().color = tempC;
        }
        else
        {
            Color tempC = c_Good;
            tempC.a = (float)(p_GoodPoints - p_EvilPoints) / 1000.0f;
            background.GetComponent<Image>().color = tempC;
        }
    }

    public void CloseTab()
    {
        traitTab.SetActive(false);
    }
}
