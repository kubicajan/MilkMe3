using System.Collections;
using System.Collections.Generic;
using Living.Enemies.WarriorBoss;
using UnityEngine;

public class FarmerRun : StateMachineBehaviour
{
    private FarmerBoss farmerRun;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        farmerRun = animator.GetComponent<FarmerBoss>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        farmerRun.Move();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
