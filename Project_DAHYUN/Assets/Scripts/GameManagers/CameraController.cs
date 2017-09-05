using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private float t_S = 0f;
    private bool lerpingSize = false;
    float initSize;
    float finalSize;
    float rate;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(lerpingSize)
        {
            t_S += Time.deltaTime * rate;

            this.GetComponent<Camera>().orthographicSize = initSize + (t_S * (finalSize - initSize));

            if (t_S > 1)
            {
                t_S = 0;
                lerpingSize = false;
            }
        }
	}

    public void LerpCameraSize(float startSize, float endSize, float speed = 1f)
    {
        t_S = 0f;
        initSize = startSize;
        finalSize = endSize;
        rate = speed;
        lerpingSize = true;
    }
}
