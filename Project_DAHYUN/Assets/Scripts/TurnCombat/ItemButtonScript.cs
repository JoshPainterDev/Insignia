using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemButtonScript : MonoBehaviour {
    private GameObject combatManager;

    private void Awake()
    {
        combatManager = GameObject.Find("CombatManager");
    }

    // Use this for initialization
    void Start () {
		
	}

    public void OnClicked(Button button)
    {
        for (int i = 0; i < GameController.controller.playerInventory.Length; ++i)
        {
            if(button.name.Contains(i.ToString()))
            {
                combatManager.GetComponent<ItemsManager_C>().ItemUsed(i);
            }
        }
    }
}
