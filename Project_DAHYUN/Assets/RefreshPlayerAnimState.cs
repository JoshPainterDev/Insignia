using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshPlayerAnimState : StateMachineBehaviour {

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("AnimState", 0);
    }
}
