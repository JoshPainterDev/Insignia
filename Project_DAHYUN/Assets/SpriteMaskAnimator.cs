using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMaskAnimator : MonoBehaviour
{
    public bool startOnAwake = false;

    private SpriteRenderer spriteRenderer;
    private SpriteMask spriteMask;

    private bool animating = false;
    private string prevSprite;

	// Use this for initialization
	void Start ()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteMask = this.GetComponent<SpriteMask>();

        spriteMask.sprite = spriteRenderer.sprite;

        if (this.GetComponent<SpriteMask>().enabled)
        {
            setActive(true);
        }
    }
	
    public void setActive(bool active)
    {
        animating = active;
    }

    public void UpdateMasks()
    {
        if (animating && (prevSprite != spriteRenderer.sprite.name))
        {
            prevSprite = spriteRenderer.sprite.name;
            spriteMask.sprite = spriteRenderer.sprite; 
        }
    }
}
