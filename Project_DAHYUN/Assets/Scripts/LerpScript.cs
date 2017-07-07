using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpScript : MonoBehaviour {

    private bool lerping = false;
    private float t = 0f;
    Vector3 initPos;
    Vector3 finalPos;
    private float rate = 1f;

    public void LerpToPos(Vector3 startPos, Vector3 endPos, float speed = 1f)
    {
        initPos = startPos;
        finalPos = endPos;
        rate = speed;
        lerping = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(lerping)
        {
            t += Time.deltaTime * rate;

            if(t > 1)
            {
                t = 1;
                lerping = false;
            }

            Vector3 pos = Vector3.Lerp(initPos, finalPos, t);
            transform.position = pos;
        }
	}
}
