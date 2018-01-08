using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTPinBounds : MonoBehaviour
{
    public Vector3 boundMax;
    public Vector3 boundMin;
    public float delayMin = 0.5f;
    public float delayMax = 1f;
    public bool active = true;

    private Vector3 origPos;

    // Use this for initialization
    void Start ()
    {
        origPos = this.transform.position;


        if (active)
        {
            StartCoroutine(teleport());
        }
	}

    public void Restart()
    {
        active = true;
        StartCoroutine(teleport());
    }

    public void Stop()
    {
        active = false;
    }

    IEnumerator teleport()
    {
        float randTime = Random.Range(delayMin, delayMax);
        yield return new WaitForSeconds(randTime);
        if(active)
        {
            this.transform.position = new Vector3(origPos.x + Random.Range(boundMin.x, boundMax.x), origPos.y + Random.Range(boundMin.y, boundMax.y), origPos.z + Random.Range(boundMin.z, boundMax.z));
            StartCoroutine(teleport());
        }
    }
}
