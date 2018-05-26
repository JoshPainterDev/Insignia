using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePopUp : MonoBehaviour {

    GameObject parent;
    public string externalCallObj;

    private void Awake()
    {
        parent = transform.parent.gameObject;
    }

    public void CloseWindow()
    {
        if(externalCallObj != "")
        {
            print(externalCallObj);
            CloseEquipmentPopup();
        }

        Destroy(parent);
    }

    public void CloseEquipmentPopup()
    {
        GameObject externalObj = GameObject.Find(externalCallObj);
        externalObj.GetComponent<Character_Menu_Manager>().UnhighlightEquipment();
    }
}
