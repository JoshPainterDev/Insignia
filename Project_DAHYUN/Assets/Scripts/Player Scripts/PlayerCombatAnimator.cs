using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatAnimator : MonoBehaviour {

    public Animator hands, weapon, arms, legs, torso, head, shoes, back;

    const int idle_AS = 0;
    const int hurt_AS = 2;

    // Use this for initialization
    void Start ()
    {
		foreach(Animator child in this.GetComponentsInChildren<Animator>())
        {
            child.SetInteger("AnimState", idle_AS);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
