using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AbilityButtonScript : MonoBehaviour {
    public GameObject combatManager;

    private void Awake()
    {
        combatManager = GameObject.Find("CombatManager");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClicked(Button button)
    {
        for (int i = 0; i < GameController.controller.playerInventory.Length; ++i)
        {
            //print(i.ToString());
            if(button.name.Contains(i.ToString()))
            {
                print(i);
                combatManager.GetComponent<ItemsManager_C>().ItemUsed(i);
            }
        }
    }
}
