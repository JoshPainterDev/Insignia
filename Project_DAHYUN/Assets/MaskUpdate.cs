using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskUpdate : StateMachineBehaviour {
    string prevSprite = "";

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Sprite currSprite = animator.gameObject.GetComponent<SpriteRenderer>().sprite;
        //if (prevSprite != currSprite.name)
        //{
        //    prevSprite = currSprite.name;
        //    animator.gameObject.GetComponent<SpriteMask>().sprite = currSprite;
        //}
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Sprite currSprite = animator.gameObject.GetComponent<SpriteRenderer>().sprite;
        //if (prevSprite != currSprite.name)
        //{
        //    prevSprite = currSprite.name;
        //    animator.gameObject.GetComponent<SpriteMask>().sprite = currSprite;
        //}
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //AnimatorClipInfo[] temp = animator.GetCurrentAnimatorClipInfo(0);
        //foreach (AnimatorClipInfo i in temp)
        //{
        //    Debug.Log("Clip Name: " + i.clip.name);
        //    Debug.Log("Clip length: " + i.clip.length);
        //    Debug.Log("Clip frameRate: " + i.clip.frameRate);
        //}
        //Sprite currSprite = animator.gameObject.GetComponent<SpriteRenderer>().sprite;
        //if (prevSprite != currSprite.name)
        //{
        //    prevSprite = currSprite.name;
        //    animator.gameObject.GetComponent<SpriteMask>().sprite = currSprite;
        //}
    }
}
