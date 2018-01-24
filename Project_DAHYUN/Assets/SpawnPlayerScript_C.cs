using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayerScript_C : MonoBehaviour
{
    public bool faceRight = true;
    private GameObject playerObj;

    // Use this for initialization
    void Start()
    {
        playerObj = GameObject.Find("Player_Mannequin");
        playerObj.transform.position = this.transform.position;
        playerObj.transform.localScale = this.transform.localScale;

        if (!faceRight)
            playerObj.GetComponent<AnimationController>().FlipFlop();
    }
}
