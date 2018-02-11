using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ExperienceScript : MonoBehaviour {
    public GameObject handle;
    
    public GameObject playerLevel;
    private GameObject expBar;
    private GameObject glitter;

    private CombatManager combatManager;
    private GameObject player;
    private float t = 0f;
    private bool lerping = false;
    public float rate = 1f;
    private float init = 0f;
    private float final = 0f;
    private float requiredEXP;
    private bool ding = false;
    private int EXPtoAdd = 0;

    // Use this for initialization
    void Start()
    {
        Invoke("Initialize", 0.5f);
        //StartCoroutine(BarBlinkAnim());
    }

    private void Initialize()
    {
        combatManager = this.GetComponent<CombatManager>();
        expBar = handle.transform.GetChild(1).gameObject;
        handle.transform.GetChild(2).GetComponent<Text>().text = "Lv " + GameController.controller.playerLevel;
        handle.transform.GetChild(3).GetComponent<Text>().text = "Lv " + (GameController.controller.playerLevel + 1);
        glitter = handle.transform.GetChild(4).gameObject;

        requiredEXP = (GameController.controller.playerLevel * GameController.controller.playerLevel)
                                     + (GameController.controller.playerLevel * 15);

        playerLevel.GetComponent<Text>().text = "Lv " + GameController.controller.playerLevel;

        float percentToLv = (float)GameController.controller.playerEXP / requiredEXP;
        expBar.GetComponent<Image>().fillAmount = percentToLv;

        print("MY EXP BOYZ: " + percentToLv);
    }

    public bool experienceAnimation(int currentExp, int newExp)
    {
        EXPtoAdd = newExp;
        player = GameController.controller.playerObject;
        ding = false;

        if (CheckForDing(currentExp + newExp))
            ding = true;

        float start = (float)currentExp / requiredEXP;
        float end = (float)newExp / requiredEXP;

        if (end > 1)
            end = 1;

        StartCoroutine(StartLerp(start, end));

        return ding;
    }

    IEnumerator StartLerp(float start, float end)
    {
        yield return new WaitForSeconds(2.5f);
        handle.SetActive(true);
        yield return new WaitForSeconds(0.85f);
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
            glitter.transform.localPosition = new Vector3(Mathf.Lerp(-185, 185, pos / 100f), 78, 0);

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
                }
                else
                {
                    StartCoroutine(EndAnim());
                }
            }
        }
    }

    //IEnumerator CharacterBlinkAnim()
    //{
    //    yield return new WaitForSeconds(1f);

    //    foreach (SpriteRenderer sprite in character.GetComponentsInChildren<SpriteRenderer>())
    //    {
    //        sprite.color = fadeColor;
    //    }

    //    yield return new WaitForSeconds(0.1f);

    //    foreach (SpriteRenderer sprite in character.GetComponentsInChildren<SpriteRenderer>())
    //    {
    //        sprite.color = origColor;
    //    }

    //    yield return new WaitForSeconds(0.1f);

    //    foreach (SpriteRenderer sprite in character.GetComponentsInChildren<SpriteRenderer>())
    //    {
    //        sprite.color = fadeColor;
    //    }

    //    yield return new WaitForSeconds(0.1f);

    //    foreach (SpriteRenderer sprite in character.GetComponentsInChildren<SpriteRenderer>())
    //    {
    //        sprite.color = origColor;
    //    }
    //}

    //IEnumerator BarBlinkAnim()
    //{
    //    expBar.GetComponent<Image>().color = fadeColor;

    //    yield return new WaitForSeconds(0.1f);

    //    expBar.GetComponent<Image>().color = origColor;

    //    yield return new WaitForSeconds(0.1f);

    //    expBar.GetComponent<Image>().color = fadeColor;

    //    yield return new WaitForSeconds(0.1f);

    //    expBar.GetComponent<Image>().color = origColor;

    //    yield return new WaitForSeconds(0.1f);

    //    expBar.GetComponent<Image>().color = fadeColor;

    //    yield return new WaitForSeconds(0.1f);

    //    expBar.GetComponent<Image>().color = origColor;
    //}

    IEnumerator LevelUpAnim()
    {
        GameController.controller.playerEXP = 0 + (GameController.controller.playerEXP - (int)requiredEXP);

        requiredEXP = (GameController.controller.playerLevel * GameController.controller.playerLevel)
                             + (GameController.controller.playerLevel * 15);

        float newPercent = (float)GameController.controller.playerEXP / requiredEXP;

        GameController.controller.GetComponent<MenuUIAudio>().playLevelUp();

        playerLevel.GetComponent<Text>().text = "Lv " + GameController.controller.playerLevel.ToString();

        expBar.GetComponent<Image>().fillAmount = 0;

        yield return new WaitForSeconds(0.25f);

        LerpEXP(0, newPercent);

        StartCoroutine(EndAnim());
    }

    IEnumerator EndAnim()
    {
        GameController.controller.playerEXP += EXPtoAdd;

        yield return new WaitForSeconds(2.5f);

        handle.SetActive(false);

        yield return new WaitForSeconds(2.5f);

        combatManager.CheckForMoreEnemies();
    }
}
