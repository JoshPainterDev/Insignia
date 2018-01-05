using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitBreakManager_C : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;

    public GameObject cameraObj;
    private Vector3 origCameraPos;
    public GameObject blackSq;

    private Vector3 playerOrigpos;
    private Vector3 enemyOrigpos;

    /// <summary>
    /// Enter Limit Break Effects
    /// </summary>
    public GameObject SuperNova_Start;

    // Use this for initialization
    void Start ()
    {
        playerOrigpos = player.transform.position;
        enemyOrigpos = enemy.transform.position;
        origCameraPos = cameraObj.transform.position;
    }

    public void UseLimitBreak(LimitBreak limitBreak)
    {
        switch (limitBreak.name)
        {
            case LimitBreakName.Ascenion:
                break;
            case LimitBreakName.Blood_Rage:
                break;
            case LimitBreakName.Hellion_Form:
                break;
            case LimitBreakName.Overdrive:
                break;
            case LimitBreakName.Shadows_Embrace:
                break;
            case LimitBreakName.Super_Nova: // default LB
                StartCoroutine(SuperNovaAnim());
                break;
            case LimitBreakName.none:
                break;
        }

        this.GetComponent<CombatManager>().playerLimitBreaking = true;
    }

    IEnumerator SuperNovaAnim()
    {
        cameraObj.GetComponent<CameraController>().LerpCameraSize(175, 120, 1f);
        cameraObj.GetComponent<LerpScript>().LerpToPos(origCameraPos, origCameraPos - new Vector3(80, 20, 0), 1.0f);
        this.GetComponent<CombatAudio>().playLBSuperNovaStart();
        yield return new WaitForSeconds(1f);
        player.GetComponent<AnimationController>().PlayAttackAnim();
        yield return new WaitForSeconds(0.15f);
        player.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        player.transform.GetChild(12).gameObject.SetActive(true);
        player.transform.GetChild(12).GetComponent<SpriteMaskAnimator>().setActive(true);
        Vector3 spawnPos = new Vector3(player.transform.position.x + 20, player.transform.position.y + 65, 0);
        GameObject effectClone = (GameObject)Instantiate(SuperNova_Start, spawnPos, transform.rotation);
        cameraObj.GetComponent<CameraController>().LerpCameraSize(120, 175, 3.5f);
        cameraObj.GetComponent<LerpScript>().LerpToPos(cameraObj.transform.position, origCameraPos, 3.5f);
        yield return new WaitForSeconds(0.75f);
        player.transform.GetChild(12).GetComponent<AudioSource>().enabled = true;
    }

    public LimitBreak LookUpLimitBreak(LimitBreakName lbName)
    {
        LimitBreak limitBreak = new LimitBreak();

        switch(lbName)
        {
            case LimitBreakName.Ascenion:
                limitBreak.attackBoost = 1;
                limitBreak.coolDown = 1;
                limitBreak.defenseBoost = 1;
                limitBreak.prowessBoost = 1;
                limitBreak.speedBoost = 1;
                limitBreak.description = "Take up the holy form of the Ascended One and dawn justice on your foes.";
                break;
            case LimitBreakName.Blood_Rage:
                limitBreak.attackBoost = 3;
                limitBreak.coolDown = 2;
                limitBreak.defenseBoost = 1;
                limitBreak.speedBoost = 1;
                limitBreak.description = "Now that you see red, they will see nothing. Channel your anger into a bloodied onslaught.";
                break;
            case LimitBreakName.Hellion_Form:
                limitBreak.attackBoost = 2;
                limitBreak.coolDown = 1;
                limitBreak.defenseBoost = 0;
                limitBreak.speedBoost = 2;
                limitBreak.description = "Said to be the physical form of fear, take on a demon-like state of the ancient Hellions.";
                break;
            case LimitBreakName.Overdrive:
                limitBreak.attackBoost = 1;
                limitBreak.coolDown = 2;
                limitBreak.defenseBoost = 1;
                limitBreak.speedBoost = 1;
                limitBreak.description = "Overcharging your cybernetic enhancements is not only painful, but very dangerous... for them.";
                break;
            case LimitBreakName.Shadows_Embrace:
                limitBreak.attackBoost = 2;
                limitBreak.coolDown = 2;
                limitBreak.defenseBoost = 0;
                limitBreak.prowessBoost = 2;
                limitBreak.speedBoost = 1;
                limitBreak.description = "There is no turning back. Embrace the shadows. Only silence now.";
                break;
            case LimitBreakName.Super_Nova:
                limitBreak.attackBoost = 3;
                limitBreak.coolDown = 2;
                limitBreak.defenseBoost = 0;
                limitBreak.speedBoost = 1;
                limitBreak.description = "Unleash the might of the Sun as the legendary Nova!";
                break;
            case LimitBreakName.none:
                limitBreak.attackBoost = 0;
                limitBreak.coolDown = 0;
                limitBreak.defenseBoost = 0;
                limitBreak.speedBoost = 0;
                limitBreak.prowessBoost = 0;
                limitBreak.description = "";
                break;
            default:
                break;
        }

        limitBreak.name = lbName;

        return limitBreak;
    }
}
