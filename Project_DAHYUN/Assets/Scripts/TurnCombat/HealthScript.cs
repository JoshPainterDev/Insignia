using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthScript : MonoBehaviour
{

    public GameObject healthBar;

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
        StartCoroutine(StartingAnim());
    }

    public void LerpHealth(float startPercent, float endPercent, float speed = 1f)
    {
        init = startPercent;
        final = endPercent;
        rate = speed;
        lerping = true;
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

    IEnumerator StartingAnim()
    {
        healthBar.GetComponent<Image>().fillAmount = 0;
        yield return new WaitForSeconds(1.5f);
        LerpHealth(0, 100);
    }
}
