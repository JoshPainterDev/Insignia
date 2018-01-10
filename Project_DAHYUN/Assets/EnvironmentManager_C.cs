using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnvironmentManager_C : MonoBehaviour
{
    public GameObject Background;

	// Use this for initialization
	void Start ()
    {
		switch(GameController.controller.currentEncounter.environment)
        {
            case Environment.Castel_Hall:
                Background.GetComponent<SpriteRenderer>().sprite = Resources.Load("\\Environments\\CastleWalls_InnerBridge", typeof(Sprite)) as Sprite;
                Background.transform.position = new Vector3(-463, 183, 0);
                Background.transform.localScale = new Vector3(34, 34, 0);
                break;
            case Environment.none:
                Background.GetComponent<SpriteRenderer>().sprite = Resources.Load("\\Environments\\Horizon_Background", typeof(Sprite)) as Sprite;
                Background.transform.position = new Vector3(-463, 183, 0);
                Background.transform.localScale = new Vector3(34, 34, 0);
                break;
        }
	}
}
