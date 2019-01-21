using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeIdleAnim_C : MonoBehaviour
{
    private float MAX = 0.5f;
    private float MIN = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        if(this.GetComponent<Animator>().isActiveAndEnabled)
        {
            float rand = Random.Range(MIN, MAX);
            StartCoroutine(StartIdle(rand));
        }
    }

    IEnumerator StartIdle(float delay)
    {
        this.GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(delay);
        this.GetComponent<Animator>().enabled = true;
    }
}
