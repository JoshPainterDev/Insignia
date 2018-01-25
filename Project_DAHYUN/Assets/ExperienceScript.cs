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
    public GameObject playerMannequin;

    private Color origColor;

    private float t = 0f;
    private bool lerping = false;
    public float rate = 1f;
    private float init = 0f;
    private float final = 0f;
    private float requiredEXP;
    private bool ding = false;
    private bool newCheck = true;

    // Use this for initialization
    void Start()
    {
        playerMannequin = GameController.controller.playerObject;

        requiredEXP = (GameController.controller.playerLevel * GameController.controller.playerLevel) 
                                     + (GameController.controller.playerLevel * 15);

        origColor = character.GetComponentInChildren<SpriteRenderer>().color;
        playerLevel.GetComponent<Text>().text = "Lv " + GameController.controller.playerLevel;
        StartCoroutine(BarBlinkAnim());
    }

    public bool experienceAnimation(int currentExp, int newExp)
    {
        ding = false;
        newCheck = true;

        if (CheckForDing(currentExp + newExp))
            ding = true;

        float start = currentExp / requiredEXP;
        float end = newExp / requiredEXP;

        if (end > 1)
            end = 1;

        LerpEXP(start, end);

        return ding;
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

                if (ding)
                {
                    //player leveled up
                    ++GameController.controller.playerLevel;
                    StartCoroutine(LevelUpAnim());
                    ding = false;
                    newCheck = false;
                }
                else if(newCheck)
                    rewardManager.GetComponent<RewardManager_C>().checkEquipmentUnlock();
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
        GameController.controller.playerEXP = 0 + (GameController.controller.playerEXP - (int)requiredEXP);

        requiredEXP = (GameController.controller.playerLevel * GameController.controller.playerLevel)
                             + (GameController.controller.playerLevel * 15);

        float newPercent = GameController.controller.playerEXP / requiredEXP;

        GameController.controller.GetComponent<MenuUIAudio>().playLevelUp();

        playerLevel.GetComponent<Text>().text = "Lv " + GameController.controller.playerLevel.ToString();

        expBar.GetComponent<Image>().fillAmount = 0;

        playerMannequin.GetComponent<AnimationController>().PlayAttackAnim();

        yield return new WaitForSeconds(0.25f);

        LerpEXP(0, newPercent);

        yield return new WaitForSeconds(2.5f);

        rewardManager.GetComponent<RewardManager_C>().checkAbilityUnlock();
    }
}
