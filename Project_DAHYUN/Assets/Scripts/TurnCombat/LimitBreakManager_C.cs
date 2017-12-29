using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitBreakManager_C : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;

    public GameObject cameraObj;
    public GameObject blackSq;

    private Vector3 playerOrigpos;
    private Vector3 enemyOrigpos;

    // Use this for initialization
    void Start ()
    {
        playerOrigpos = player.transform.position;
        enemyOrigpos = enemy.transform.position;

    }

    public void UseLimitBreak(LimitBreak limitBreak)
    {
        print(limitBreak.name);
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
        cameraObj.GetComponent<CameraController>().LerpCameraSize(175, 120, 0.5f);
        yield return new WaitForSeconds(2);
        player.GetComponent<AnimationController>().PlayAttackAnim();
        yield return new WaitForSeconds(0.75f);
        cameraObj.GetComponent<CameraController>().LerpCameraSize(120, 175, 3.5f);
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
