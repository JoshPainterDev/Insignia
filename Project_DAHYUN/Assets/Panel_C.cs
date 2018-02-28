using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_C : MonoBehaviour
{
    public GameObject LvText;
    public GameObject GoldText;
    public GameObject ClassIcon;

    public Sprite KnightIcon;
    public Sprite CutthroatIcon;
    public Sprite GuardianIcon;
    public Sprite OccultistIcon;

    // Use this for initialization
    void Start ()
    {
        Invoke("InitStuff", 0.1f);
    }

    public void InitStuff()
    {
        LvText.GetComponent<Text>().text = "LV " + GameController.controller.playerLevel;
        GoldText.GetComponent<Text>().text = GameController.controller.playerGoldCredits.ToString("N0");

        switch (GameController.controller.charClasses[GameController.controller.playerNumber])
        {
            case PlayerClass.Knight:
                ClassIcon.GetComponent<Image>().sprite = KnightIcon;
                break;
            case PlayerClass.Cutthroat:
                ClassIcon.GetComponent<Image>().sprite = CutthroatIcon;
                break;
            case PlayerClass.Guardian:
                ClassIcon.GetComponent<Image>().sprite = GuardianIcon;
                break;
            case PlayerClass.Occultist:
                ClassIcon.GetComponent<Image>().sprite = OccultistIcon;
                break;
        }
    }
}
