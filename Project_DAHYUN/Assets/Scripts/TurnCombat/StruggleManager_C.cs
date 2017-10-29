using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StruggleManager_C : MonoBehaviour {

    private CombatManager combatController;

    public GameObject player;
    public GameObject enemy;
    public GameObject struggleButton_L;
    public GameObject struggleButton_R;
    public GameObject struggle_Counter;
    public GameObject camera;
    public GameObject effectClone;
    public float readySize, pressedSize;
    public Color origColor, pressedColor;
    public float goal = 50f;

    public GameObject hitEffect;
    public GameObject blood01_FX;
    public GameObject blood02_FX;
    public GameObject blood03_FX;
    public GameObject blood04_FX;

    [HideInInspector]
    public int HARD_MODE_FAIL = 1;
    [HideInInspector]
    public int NORMAL_MODE_FAIL = 2;
    [HideInInspector]
    public int EASY_MODE_FAIL = 3;
    private int currentMode;

    //player vars
    private Vector3 playerOrig;
    private Vector3 playerMax = new Vector3(-293, 138, 0);
    private Vector3 playerMin = new Vector3(-765, 138, 0);
    private Vector3 playerStrugglePos = new Vector3(-540, 138, 0);
    //enemy vars
    private Vector3 enemyOrig;
    private Vector3 enemyMax = new Vector3(-468, 160, 0);
    private Vector3 enemyMin = new Vector3(18, 160, 0);
    private Vector3 enemyStrugglePos = new Vector3(-335, 160, 0);

    private Vector3 origCameraPos;
    private Vector3 effectPos;

    private int strugglePressCounter = 0;
    bool struggling_Player = false;
    bool struggling_Enemy = false;
    float percentCompleted = 0;
    float randomVar = 0;
    int frameTracker = 0;
    int timeOutTracker = 0;
    int failCounter = 0;
    int failScale = 30;
    bool playerInitiated = true;
    bool playerCanFail = true;
    int enemyLevel = 1;


    // Use this for initialization
    void Start ()
    {
        combatController = this.GetComponent<CombatManager>();
        playerOrig = player.transform.position;
        enemyOrig = enemy.transform.position;
        origCameraPos = camera.transform.position;
        effectPos = enemy.transform.GetChild(0).transform.GetChild(0).transform.position;
        disableStruggleButtons();
        struggle_Counter.GetComponent<Text>().enabled = false;
        currentMode = EASY_MODE_FAIL;

        if(combatController.enemyInfo != null)
            enemyLevel = combatController.enemyInfo.enemyLevel;

        failScale += GameController.controller.playerProwess;
        goal -= (GameController.controller.playerLevel - enemyLevel) * combatController.playerAttackBoost;
    } 
	
	// Update is called once per frame
	void Update ()
    {
		if(struggling_Player || struggling_Enemy)
        {
            ++frameTracker;
            ++timeOutTracker;

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                leftButtonPressed();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                rightButtonPressed();
            }

            percentCompleted = (float)strugglePressCounter / goal;

            if(percentCompleted >= 0.99f)
            {
                percentCompleted = 0;
                
                if (struggling_Player)
                {
                    struggling_Player = false;
                    struggling_Enemy = false;
                    StartCoroutine(ExecuteEnemy());
                }
                else
                {
                    struggling_Player = false;
                    struggling_Enemy = false;
                    StartCoroutine(StruggleFailed());
                }

            }

            if (frameTracker > 45)
            {
                randomVar = Random.Range(-0.05f, 0.05f);
                frameTracker = 0 + Random.Range(0, 5);
            }
            
            if(struggling_Player)
            {
                player.transform.position = Vector3.Lerp(playerStrugglePos, playerMax, percentCompleted + randomVar);
                enemy.transform.position = Vector3.Lerp(enemyStrugglePos, enemyMin, percentCompleted + randomVar);

                //set the player back for not being fast enough
                if (timeOutTracker > failScale)
                {
                    timeOutTracker = 0;
                    ++failCounter;
                    --strugglePressCounter;
                }

                if (checkFailCounter(currentMode) || (strugglePressCounter < 0))
                {
                    if(playerCanFail)
                    {
                        struggling_Player = false;
                        StartCoroutine(StruggleFailed());
                    }
                }
            }
            else
            {
                player.transform.position = Vector3.Lerp(playerStrugglePos, playerMin, percentCompleted + randomVar);
                enemy.transform.position = Vector3.Lerp(enemyStrugglePos, enemyMax, percentCompleted - randomVar);

                //CHANGE THIS BECAUSE THE ENEMY SHOULD HAVE
                //SEPERATE STRUGGLE CONDITIONS FOR KILLING THE PLAYER!

                //set the player back for not being fast enough
                if (timeOutTracker > failScale)
                {
                    timeOutTracker = 0;
                    ++failCounter;
                    --strugglePressCounter;
                }

                if (checkFailCounter(currentMode) || (strugglePressCounter < 0))
                {
                    struggling_Enemy = false;
                    StartCoroutine(ExecutePlayer());
                }
            }

        }
	}

    public void rightButtonPressed()
    {
        timeOutTracker = 0;

        StartCoroutine(IncrementCounter());

        struggleButton_R.transform.localScale = new Vector3(pressedSize, pressedSize, 1);
        struggleButton_R.GetComponent<Button>().enabled = false;
        struggleButton_R.GetComponent<Image>().color = pressedColor;
        struggleButton_L.transform.localScale = new Vector3(readySize, readySize, 1);
        struggleButton_L.GetComponent<Button>().enabled = true;
        struggleButton_L.GetComponent<Image>().color = origColor;

        this.GetComponent<CombatAudio>().playRandomStrugglePress();
    }

    public void leftButtonPressed()
    {
        timeOutTracker = 0;

        StartCoroutine(IncrementCounter());

        struggleButton_L.transform.localScale = new Vector3(pressedSize, pressedSize, 1);
        struggleButton_L.GetComponent<Button>().enabled = false;
        struggleButton_L.GetComponent<Image>().color = pressedColor;
        struggleButton_R.transform.localScale = new Vector3(readySize, readySize, 1);
        struggleButton_R.GetComponent<Button>().enabled = true;
        struggleButton_R.GetComponent<Image>().color = origColor;

        this.GetComponent<CombatAudio>().playRandomStrugglePress();
    }

    //IGNITE THE SPARK
    public void BeginStruggle_Player(bool canFail = true)
    {
        playerCanFail = canFail;
        StartCoroutine(AlignCharacters(true));
    }

    public void BeginStruggle_Enemy()
    {
        StartCoroutine(AlignCharacters(false));
    }

    //ANIMATE CHARACTERS STRUGGLING
    IEnumerator AlignCharacters(bool playerStarted)
    {
        player.GetComponent<LerpScript>().LerpToPos(playerOrig, playerStrugglePos, 3);
        enemy.GetComponent<LerpScript>().LerpToPos(enemyOrig, enemyStrugglePos, 3);
        camera.GetComponent<CameraController>().LerpCameraSize(150, 100, 2);
        camera.GetComponent<LerpScript>().LerpToPos(origCameraPos, origCameraPos + new Vector3(10,-5,0), 2);
        yield return new WaitForSeconds(0.5f);
        enableStruggleButtons();
        struggle_Counter.GetComponent<Text>().enabled = true;

        if (playerStarted)
        {
            struggling_Player = true;
            playerInitiated = true;
        }
        else
        {
            struggling_Enemy = true;
            playerInitiated = false;
        }
    }

    IEnumerator IncrementCounter()
    {
        ++strugglePressCounter;
        struggle_Counter.GetComponent<LerpScript>().LerpToScale(struggle_Counter.transform.localScale, new Vector3(2,2,1), 4);
        yield return new WaitForSeconds(0.1f);
        struggle_Counter.GetComponent<Text>().text = strugglePressCounter.ToString();
        struggle_Counter.GetComponent<LerpScript>().LerpToScale(struggle_Counter.transform.localScale, new Vector3(1, 1, 1), 4);
    }

    IEnumerator ExecuteEnemy()
    {
        percentCompleted = 0;
        strugglePressCounter = 0;
        disableStruggleButtons();
        struggle_Counter.GetComponent<Text>().enabled = false;
        struggleButton_L.transform.localScale = new Vector3(1, 1, 1);
        struggleButton_L.GetComponent<Image>().color = origColor;
        struggleButton_R.transform.localScale = new Vector3(1, 1, 1);
        struggleButton_R.GetComponent<Image>().color = origColor;
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<AnimationController>().PlayAttackAnim();
        Vector3 spawnPos = new Vector3(player.transform.position.x + 80, player.transform.position.y, 0);
        effectClone = (GameObject)Instantiate(hitEffect, spawnPos, transform.rotation);
        yield return new WaitForSeconds(0.25f);
        //this.GetComponent<EnemyCombatScript>().PlayDeathAnim();
        //play execution anim
        //yield return new WaitForSeconds(0.75f);
        this.GetComponent<CombatManager>().ExecuteEnemy_Strike();
        camera.GetComponent<CameraController>().LerpCameraSize(100, 150, 3);
        yield return new WaitForSeconds(0.75f);
        player.GetComponent<LerpScript>().LerpToPos(player.transform.position, playerOrig, 3);
        enemy.GetComponent<LerpScript>().LerpToPos(enemy.transform.position, enemyOrig, 3);
        //yield return new WaitForSeconds(0.5f);
        enemy.GetComponent<LerpScript>().LerpToColor(origColor, Color.clear, 1.5f);

        if(!playerCanFail)
            this.GetComponent<TutorialManager02_C>().StruggleFinished();
    }

    IEnumerator ExecutePlayer()
    {
        percentCompleted = 0;
        strugglePressCounter = 0;
        disableStruggleButtons();
        struggle_Counter.GetComponent<Text>().enabled = false;
        struggleButton_L.transform.localScale = new Vector3(1, 1, 1);
        struggleButton_L.GetComponent<Image>().color = origColor;
        struggleButton_R.transform.localScale = new Vector3(1, 1, 1);
        struggleButton_R.GetComponent<Image>().color = origColor;

        player.GetComponent<LerpScript>().LerpToPos(player.transform.position, playerOrig, 3);
        enemy.GetComponent<LerpScript>().LerpToPos(enemy.transform.position, enemyOrig, 3);
        camera.GetComponent<CameraController>().LerpCameraSize(100, 150, 3);
        yield return new WaitForSeconds(1f);
    } 

    IEnumerator StruggleFailed()
    {
        percentCompleted = 0;
        strugglePressCounter = 0;
        disableStruggleButtons();
        struggle_Counter.GetComponent<Text>().enabled = false;
        struggleButton_L.transform.localScale = new Vector3(1, 1, 1);
        struggleButton_L.GetComponent<Image>().color = origColor;
        struggleButton_R.transform.localScale = new Vector3(1, 1, 1);
        struggleButton_R.GetComponent<Image>().color = origColor;

        player.GetComponent<LerpScript>().LerpToPos(player.transform.position, playerOrig, 3);
        enemy.GetComponent<LerpScript>().LerpToPos(enemy.transform.position, enemyOrig, 3);
        camera.GetComponent<CameraController>().LerpCameraSize(100, 150, 3);
        yield return new WaitForSeconds(1f);

        if (playerInitiated)
            combatController.StruggleFailed(true);
        else
            combatController.StruggleFailed(false);
    }

    void enableStruggleButtons()
    {
        struggleButton_L.GetComponent<Button>().enabled = true;
        struggleButton_L.GetComponent<Image>().enabled = true;
        struggleButton_L.GetComponentInChildren<Text>().enabled = true;

        struggleButton_R.GetComponent<Button>().enabled = true;
        struggleButton_R.GetComponent<Image>().enabled = true;
        struggleButton_R.GetComponentInChildren<Text>().enabled = true;
    }

    void disableStruggleButtons()
    {
        struggleButton_L.GetComponent<Button>().enabled = false;
        struggleButton_L.GetComponent<Image>().enabled = false;
        struggleButton_L.GetComponentInChildren<Text>().enabled = false;

        struggleButton_R.GetComponent<Button>().enabled = false;
        struggleButton_R.GetComponent<Image>().enabled = false;
        struggleButton_R.GetComponentInChildren<Text>().enabled = false;
    }

    bool checkFailCounter(int mode)
    {
        if(mode == HARD_MODE_FAIL)
        {
            if (failCounter > HARD_MODE_FAIL)
                return true;
        }
        else if (mode == NORMAL_MODE_FAIL)
        {
            if (failCounter > NORMAL_MODE_FAIL)
                return true;
        }
        else if (mode == EASY_MODE_FAIL)
        {
            if (failCounter > EASY_MODE_FAIL)
                return true;
        }

        return false;
    }
}
