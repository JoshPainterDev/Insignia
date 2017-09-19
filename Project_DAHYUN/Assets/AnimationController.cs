using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    // Use this for initialization
    void Start ()
    {
		
	}

    public void PlayIdleAnim()
    {
        this.transform.GetChild(6).GetComponent<SpriteRenderer>().sortingOrder = -1;

        foreach (Animator child in this.GetComponentsInChildren<Animator>())
        {
            child.SetInteger("AnimState", 0);
        }
    }

    public void PlayAttackAnim()
    {
        this.transform.GetChild(6).GetComponent<SpriteRenderer>().sortingOrder = -1;

        foreach (Animator child in this.GetComponentsInChildren<Animator>())
        {
            child.SetInteger("AnimState", 5);
        }
    }

    public void PlayWalkAnim()
    {
        this.transform.GetChild(6).GetComponent<SpriteRenderer>().sortingOrder = -5;

        foreach (Animator child in this.GetComponentsInChildren<Animator>())
        {
            child.SetInteger("AnimState", 1);
        }
    }
}
