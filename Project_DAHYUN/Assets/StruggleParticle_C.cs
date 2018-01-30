using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StruggleParticle_C : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var mainMod = this.transform.GetChild(0).GetComponent<ParticleSystem>().main;

        mainMod.startColor = GameController.controller.getPlayerColorPreference();
    }
}
