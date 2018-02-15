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
    private float leftover = 0;
    private Coroutine finalRoutine;

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
        finalRoutine = null;

        if (CheckForDing(currentExp + newExp))
            ding = true;

        float start = (float)currentExp / requiredEXP;
        float end = (float)(currentExp + newExp) / requiredEXP;

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
        print("exp: " + exp);
        print("required exp: " + requiredEXP);

        if (exp >= requiredEXP)
        {
            leftover = exp - requiredEXP;
            return true;
        }

        leftover = 0;

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
                    if(finalRoutine == null)
                        finalRoutine = StartCoroutine(EndAnim());
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
        UpdateStats();

        requiredEXP = (GameController.controller.playerLevel * GameController.controller.playerLevel)
                             + (GameController.controller.playerLevel * 15);

        float newPercent = Mathf.Max(0.0f, leftover / requiredEXP);

        GameController.controller.GetComponent<MenuUIAudio>().playLevelUp();

        playerLevel.GetComponent<Text>().text = "Lv " + GameController.controller.playerLevel.ToString();

        handle.transform.GetChild(2).GetComponent<Text>().text = "Lv " + GameController.controller.playerLevel;
        handle.transform.GetChild(3).GetComponent<Text>().text = "Lv " + (GameController.controller.playerLevel + 1);

        expBar.GetComponent<Image>().fillAmount = 0;
        glitter.transform.localPosition = new Vector3(-185, 78, 0);
        

        yield return new WaitForSeconds(1.25f);

        if (newPercent >= 1.0f)
        {
            
            ding = true;
            leftover -= requiredEXP;
            newPercent = 1.0f;

            LerpEXP(0, newPercent);
        }
        else
        {
            ding = false;
            GameController.controller.playerEXP = (int)leftover;

            print("FINAL PLAYER EXP: " + GameController.controller.playerEXP);

            LerpEXP(0, newPercent);

            if(finalRoutine == null)
            {
                finalRoutine = StartCoroutine(EndAnim());
            }
        }
    }

    IEnumerator EndAnim()
    {
        yield return new WaitForSeconds(1.5f);
        handle.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        combatManager.CheckForMoreEnemies();
    }

    public void UpdateStats()
    {
        GameController contr = GameController.controller;

        switch (GameController.controller.charClasses[GameController.controller.playerNumber])
        {
            case PlayerClass.Knight:
                GameController.controller.playerBaseAtk += 2;
                GameController.controller.playerBaseDef += 2;
                GameController.controller.playerBasePrw += 1;
                GameController.controller.playerBaseSpd += 1;
                break;
            case PlayerClass.Cutthroat:
                GameController.controller.playerBaseAtk += 2;
                GameController.controller.playerBaseDef += 2;
                GameController.controller.playerBasePrw += 1;
                GameController.controller.playerBaseSpd += 1;
                break;
            case PlayerClass.Guardian:
                GameController.controller.playerBaseAtk += 2;
                GameController.controller.playerBaseDef += 2;
                GameController.controller.playerBasePrw += 1;
                GameController.controller.playerBaseSpd += 1;
                break;
            case PlayerClass.Occultist:
                GameController.controller.playerBaseAtk += 2;
                GameController.controller.playerBaseDef += 2;
                GameController.controller.playerBasePrw += 1;
                GameController.controller.playerBaseSpd += 1;
                break;
        }

        int statTotal;
        print("Previous ATT: " + GameController.controller.playerAttack);
        //Attack
        statTotal = contr.playerBaseAtk;
        for (int i = 0; i < 8; ++i)
        {
            EquipmentInfo info = contr.GetComponent<EquipmentInfoManager>().LookUpEquipment(contr.playerEquippedIDs[i * 2], contr.playerEquippedIDs[(i * 2) + 1]);
            statTotal += info.AttackStat;
        }

        GameController.controller.playerAttack = statTotal;
        print("New ATT: " + GameController.controller.playerAttack);

        print("Previous DEF: " + GameController.controller.playerDefense);
        //Defense
        statTotal = GameController.controller.playerBaseDef;
        for (int i = 0; i < 8; ++i)
        {
            EquipmentInfo info = contr.GetComponent<EquipmentInfoManager>().LookUpEquipment(contr.playerEquippedIDs[i * 2], contr.playerEquippedIDs[(i * 2) + 1]);
            statTotal += info.DefenseStat;
        }

        GameController.controller.playerDefense = statTotal;
        print("New DEF: " + GameController.controller.playerDefense);

        print("Previous PRW: " + GameController.controller.playerProwess);
        //Prowess
        statTotal = GameController.controller.playerProwess;
        for (int i = 0; i < 8; ++i)
        {
            EquipmentInfo info = contr.GetComponent<EquipmentInfoManager>().LookUpEquipment(contr.playerEquippedIDs[i * 2], contr.playerEquippedIDs[(i * 2) + 1]);
            statTotal += info.ProwessStat;
        }

        GameController.controller.playerProwess = statTotal;
        print("New PRW: " + GameController.controller.playerDefense);

        print("Previous SPD: " + GameController.controller.playerProwess);
        //Speed
        statTotal = GameController.controller.playerBaseSpd;
        for (int i = 0; i < 8; ++i)
        {
            EquipmentInfo info = contr.GetComponent<EquipmentInfoManager>().LookUpEquipment(contr.playerEquippedIDs[i * 2], contr.playerEquippedIDs[(i * 2) + 1]);
            statTotal += info.SpeedStat;
        }

        GameController.controller.playerSpeed = statTotal;
        print("New SPD: " + GameController.controller.playerProwess);
    }
}
