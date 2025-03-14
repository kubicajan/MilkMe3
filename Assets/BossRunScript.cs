using System.Collections;
using System.Collections.Generic;
using Living.Enemies;
using UnityEngine;

public class BossRunScript : StateMachineBehaviour
{
	private WarriorBoss warriorBoss;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		warriorBoss = animator.GetComponent<WarriorBoss>();
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		warriorBoss.Move();
		string attack = warriorBoss.ShouldAttack();

		if (attack != null)
		{
			animator.SetTrigger(attack);
		}
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.ResetTrigger("Heavy_attack");
		animator.ResetTrigger("Double_attack");
	}
}