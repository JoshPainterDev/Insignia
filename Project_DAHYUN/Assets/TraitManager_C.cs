using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitManager_C : MonoBehaviour {

    public GameObject evilStripe, goodStripe;
    int p_EvilPoints, p_GoodPoints;
    float MAX_POINTS = 1500.0f;

    // Use this for initialization
    void Start ()
    {
        GameController.controller.playerEvilPoints = 1500;
        GameController.controller.playerGoodPoints = 700;

        p_EvilPoints = GameController.controller.playerEvilPoints;
        p_GoodPoints = GameController.controller.playerGoodPoints;

        evilStripe.GetComponent<Image>().fillAmount = (float)p_EvilPoints / MAX_POINTS;
        goodStripe.GetComponent<Image>().fillAmount = (float)p_GoodPoints / MAX_POINTS;

    }


}
