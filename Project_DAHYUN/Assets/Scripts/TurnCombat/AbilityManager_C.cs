using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager_C : MonoBehaviour {

    public GameObject playerMannequin;

    public GameObject illusion_FX;
    public Vector3 playerPos;

    private SpecialCase playerSpecialCase = SpecialCase.None;
    private SpecialCase enemySpecialCase = SpecialCase.None;

    // Use this for initialization
    void Start ()
    {
        playerPos = playerMannequin.transform.position;
    }

    //PLAYER TURN SEQUENCE
    //1. check for special case due to player ability
    //2. check for special case due to enemy ability
    //3. roll accuracy
    //4. buff
    //5. calculate damage
    //6. play animation
    //7. deal damage
    //8. end turn


    public void AbilityUsed(Ability ability)
    {
        playerSpecialCase = ability.specialCase;

        switch(ability.Name)
        {
            case "Shadow Strike":
                //GameObject effectClone = (GameObject)Instantiate(spawnEffect, playerPos, transform.rotation);
                break;
            case "Illusion":
                print("oooo spooopy");
                GameObject effectClone = (GameObject)Instantiate(illusion_FX, playerPos, transform.rotation);
                break;
            default:
                break;
        }
    }
}
