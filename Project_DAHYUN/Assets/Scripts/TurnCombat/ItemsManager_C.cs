using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager_C : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void ItemUsed(int itemUsed)
    {
        string itemName = GameController.controller.playerInventory[itemUsed];

        print(GameController.controller.playerInventoryQuantity[itemUsed]);

        switch(itemName)
        {
            case "Void Balls":

                break;
            default:
                break;
        }

        // decrement item that was used
        GameController.controller.RemoveItemFromInventory(itemName);
    }
}
