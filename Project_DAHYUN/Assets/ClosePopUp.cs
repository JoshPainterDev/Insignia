using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePopUp : MonoBehaviour {

    GameObject parent;

    private void Awake()
    {
        parent = transform.parent.gameObject;
    }

    public void CloseWindow()
    {
        Destroy(parent);
    }
}
