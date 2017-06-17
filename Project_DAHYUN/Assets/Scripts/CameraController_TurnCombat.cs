using UnityEngine;
using System.Collections;

public class CameraController_TurnCombat : MonoBehaviour {
    private float aspectRatio = Screen.width / Screen.height;

	// Use this for initialization
	void Start () {
        Camera.main.projectionMatrix = Matrix4x4.Ortho(0 * aspectRatio, 10f * aspectRatio, 0f, 5.5f, 0.3f, 1000f);
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
