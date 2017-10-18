using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LerpScript))]
public class OscillationScript : MonoBehaviour
{
    public bool startOnAwake = true;
    public float speed = 1.0f;
    private bool active = false;

    public Vector3 offset1;
    public Vector3 offset2;

    private Vector3 origPos;
	// Use this for initialization
	void Start ()
    {
        origPos = this.transform.position;
        offset1 += origPos;
        offset2 += origPos;

        if (startOnAwake)
        {
            active = true;
            StartCoroutine(MoveToFirst());
        }
    }

    IEnumerator MoveToFirst()
    {
        if (active)
        {
            this.GetComponent<LerpScript>().LerpToPos(this.transform.position, offset1, speed);
            yield return new WaitForSeconds(1 / speed);
            StartCoroutine(MoveToSecond());
        }
    }

    IEnumerator MoveToSecond()
    {
        if (active)
        {
            this.GetComponent<LerpScript>().LerpToPos(this.transform.position, offset2, speed);
            yield return new WaitForSeconds(1 / speed);
            StartCoroutine(MoveToFirst());
        }
    }

    public void StopMoving()
    {
        active = false;
    }
}
