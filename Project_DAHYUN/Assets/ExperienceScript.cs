using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ExperienceScript : MonoBehaviour {
    public GameObject expBar;
    public GameObject character;
    public GameObject playerLevel;
    public Color fadeColor;
    public GameObject rewardManager;

    private Color origColor;

    private float t = 0f;
    private bool lerping = false;
    public float rate = 1f;
    private float init = 0f;
    private float final = 0f;
    private float requiredEXP;
    private bool ding = false;

    // Use this for initialization
    void Start()
    {
        requiredEXP = (GameController.controller.playerLevel * GameController.controller.playerLevel) 
                                     + (GameController.controller.playerLevel * 15);

        origColor = character.GetComponentInChildren<SpriteRenderer>().color;
        StartCoroutine(BarBlinkAnim());
    }

    public void experienceAnimation(int currentExp, int newExp)
    {
        if (CheckForDing(currentExp + newExp))
            ding = true;

        float start = currentExp / requiredEXP;
        float end = newExp / requiredEXP;

        print(start);
        print(end);

        if (end > 1)
            end = 1;

        LerpEXP(start, end);
    }

    public bool CheckForDing(int exp)
    {
        print("required exp: " + requiredEXP);

        if (exp >= requiredEXP)
            return true;

        return false;
    }

    public void LerpEXP(float startPercent, float endPercent, float speed = 1f)
    {
        init = startPercent * 100f;
        final = endPercent * 100f;
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
            expBar.GetComponent<Image>().fillAmount = pos / 100f;

            if (t > 1)
            {
                t = 0;
                lerping = false;

                if(ding)
                {
                    //play a sound effect here
                    StartCoroutine(LevelUpAnim());
                    ding = false;
                }

                rewardManager.GetComponent<RewardManager_C>().ExperienceIsDone();
            }
        }
    }

    IEnumerator CharacterBlinkAnim()
    {
        yield return new WaitForSeconds(1f);

        foreach (SpriteRenderer sprite in character.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.color = fadeColor;
        }

        yield return new WaitForSeconds(0.1f);

        foreach (SpriteRenderer sprite in character.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.color = origColor;
        }

        yield return new WaitForSeconds(0.1f);

        foreach (SpriteRenderer sprite in character.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.color = fadeColor;
        }

        yield return new WaitForSeconds(0.1f);

        foreach (SpriteRenderer sprite in character.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.color = origColor;
        }
    }

    IEnumerator BarBlinkAnim()
    {
        expBar.GetComponent<Image>().color = fadeColor;

        yield return new WaitForSeconds(0.1f);

        expBar.GetComponent<Image>().color = origColor;

        yield return new WaitForSeconds(0.1f);

        expBar.GetComponent<Image>().color = fadeColor;

        yield return new WaitForSeconds(0.1f);

        expBar.GetComponent<Image>().color = origColor;

        yield return new WaitForSeconds(0.1f);

        expBar.GetComponent<Image>().color = fadeColor;

        yield return new WaitForSeconds(0.1f);

        expBar.GetComponent<Image>().color = origColor;
    }

    IEnumerator LevelUpAnim()
    {
        this.GetComponent<LerpScript>().LerpToColor(Color.white, Color.clear, 1);

        foreach (LerpScript lerp in this.GetComponentsInChildren<LerpScript>())
        {
            lerp.LerpToColor(Color.white, Color.clear, 1);
        }

        yield return new WaitForSeconds(3f);

        this.GetComponent<Image>().color = Color.white;
        this.GetComponent<Image>().enabled = false;

        foreach (Image sprite in this.GetComponentsInChildren<Image>())
        {
            sprite.enabled = false;
            sprite.color = Color.white;
        }

        this.transform.GetChild(2).GetComponent<Text>().enabled = false;
    }
}
