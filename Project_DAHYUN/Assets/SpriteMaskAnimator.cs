using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMaskAnimator : MonoBehaviour
{
    public bool startOnAwake = false;

    private SpriteRenderer spriteRenderer;
    private SpriteMask spriteMask;

    private bool animating = false;

	// Use this for initialization
	void Start ()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteMask = this.GetComponent<SpriteMask>();

        spriteMask.sprite = spriteRenderer.sprite;

        if(startOnAwake)
        {
            setActive(true);
        }
    }
	
    public void setActive(bool active)
    {
        animating = active;
    }

	// Update is called once per frame
	void Update ()
    {
		if(animating)
        {
            spriteMask.sprite = spriteRenderer.sprite;
        }
	}
}
