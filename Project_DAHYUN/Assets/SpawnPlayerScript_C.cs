using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayerScript_C : MonoBehaviour
{
    public bool faceRight = true;
    public bool visibleOnStart = true;
    private GameObject playerObj;

    private void Awake()
    {
        playerObj = GameObject.Find("Player_Mannequin");
        playerObj.SetActive(visibleOnStart);
        playerObj.transform.position = this.transform.position;
        playerObj.transform.localScale = this.transform.localScale;
        //print("start Pos: " + playerObj.transform.position);
    }

    // Use this for initialization
    void Start()
    {
        
        playerObj.GetComponent<AnimationController>().PlayIdleAnim();

        if (!faceRight)
            playerObj.GetComponent<AnimationController>().FlipFlop();
    }
}
