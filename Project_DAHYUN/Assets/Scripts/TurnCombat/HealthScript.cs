using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public GameObject healthBar;
    public GameObject character;
    public GameObject combatManager;
    public Color fadeColor;
    public bool playerHealth = false;

    private Color[] origColors;

    private float t = 0f;
    private bool lerping = false;
    public float rate = 1f;
    private float init = 0f;
    private float final = 0f;
    private float totalHealth;

    public Sprite greenBar;
    public Sprite yellowBar;
    public Sprite redBar;

    // Use this for initialization
    void Start()
    {
        origColors = new Color[20];
        StartCoroutine(StartingAnim());
        Invoke("getColors", 2);
    }

    public void LerpHealth(float startPercent, float endPercent, float speed = 1f)
    {
        init = startPercent * 100f;
        final = endPercent * 100f;
        rate = speed;
        lerping = true;
    }

    private void getColors()
    {
        int i = 0;
        if (playerHealth)
        {
            foreach (SpriteRenderer sprite in character.GetComponentsInChildren<SpriteRenderer>())
            {
                origColors[i] = sprite.color;
                ++i;
            }
        }
        else
        {
            foreach (SpriteRenderer sprite in character.transform.GetChild(0).gameObject.GetComponentsInChildren<SpriteRenderer>())
            {
                origColors[i] = sprite.color;
                ++i;
            }
        }
    }

    public void setColors(Color color)
    {
        origColors[0] = color;
    }

    public void Hurt()
    {
        StartCoroutine(HurtAnim());
    }

    public void Death()
    {
        StartCoroutine(DeathAnim());
    }

    // Update is called once per frame
    void Update()
    {
        if (lerping)
        {
            t += Time.deltaTime * rate;

            float pos = Mathf.Lerp(init, final, t);
            healthBar.GetComponent<Image>().fillAmount = pos / 100f;

            if (pos < 66)
            {
                if(pos < 26)
                {
                    healthBar.GetComponent<Image>().sprite = redBar;
                }
                else
                    healthBar.GetComponent<Image>().sprite = yellowBar;
            }
            else
                healthBar.GetComponent<Image>().sprite = greenBar;

            if (t > 1)
            {
                t = 0;
                lerping = false;
            }
        }
    }

    public void StartAnim()
    {
        StartCoroutine(StartingAnim());
    }

    IEnumerator StartingAnim()
    {
        healthBar.GetComponent<Image>().fillAmount = 0;
        this.GetComponent<Image>().color = Color.white;
        this.GetComponent<Image>().enabled = true;

        foreach (Image sprite in this.GetComponentsInChildren<Image>())
        {
            if (sprite.gameObject.transform.parent.name != "BoostHandle")
            {
                sprite.enabled = true;
                sprite.color = Color.white;
            }
            else
            {
                sprite.enabled = false;
                sprite.color = Color.clear;
            }
        }

        foreach (Text text in this.GetComponentsInChildren<Text>())
        {
            text.enabled = true;
            //text.color = Color.white;
        }
        yield return new WaitForSeconds(0.25f);
        LerpHealth(0, 1);
    }



    IEnumerator HurtAnim()
    {
        int i = 0;

        if(playerHealth)
        {
            Color skinC = GameController.controller.getPlayerSkinColor();
            foreach (SpriteRenderer sprite in character.GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.color = fadeColor;
            }

            character.GetComponent<AnimationController>().setSkinColor(fadeColor);

            yield return new WaitForSeconds(0.1f);

            foreach (SpriteRenderer sprite in character.GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.color = origColors[i];
                ++i;
            }

            character.GetComponent<AnimationController>().setSkinColor(skinC);

            yield return new WaitForSeconds(0.1f);

            foreach (SpriteRenderer sprite in character.GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.color = fadeColor;
            }

            character.GetComponent<AnimationController>().setSkinColor(fadeColor);

            yield return new WaitForSeconds(0.1f);

            i = 0;
            foreach (SpriteRenderer sprite in character.GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.color = origColors[i];
                ++i;
            }

            character.GetComponent<AnimationController>().setSkinColor(skinC);
        }
        else
        {
            foreach (SpriteRenderer sprite in character.transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.color = fadeColor;
            }

            yield return new WaitForSeconds(0.1f);

            foreach (SpriteRenderer sprite in character.transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.color = origColors[i];
                ++i;
            }

            yield return new WaitForSeconds(0.1f);

            foreach (SpriteRenderer sprite in character.transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.color = fadeColor;
            }

            yield return new WaitForSeconds(0.1f);

            i = 0;
            foreach (SpriteRenderer sprite in character.transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.color = origColors[i];
                ++i;
            }
        }
    }

    IEnumerator DeathAnim()
    {
        this.GetComponent<LerpScript>().LerpToColor(Color.white, Color.clear, 1);

        foreach (LerpScript lerp in this.GetComponentsInChildren<LerpScript>())
        {
            if (lerp.gameObject.transform.parent.name != "BoostHandle")
                lerp.LerpToColor(Color.white, Color.clear, 1);
        }

        yield return new WaitForSeconds(3f);

        this.GetComponent<Image>().color = Color.white;
        this.GetComponent<Image>().enabled = false;

        foreach (Image sprite in this.GetComponentsInChildren<Image>())
        {
            sprite.enabled = false;

            if (sprite.gameObject.transform.parent.name != "BoostHandle")
            {
                sprite.color = Color.white;
            }   
        }

        foreach (Text text in this.GetComponentsInChildren<Text>())
        {
            text.enabled = false;
            //text.color = Color.white;
        }
    }
}
