using Living.Enemies.WarriorBoss;
using UnityEngine;

public class WarriorBossHeavyAttackScript : StateMachineBehaviour
{
	private WarriorBoss warriorBoss;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		warriorBoss = animator.GetComponent<WarriorBoss>();
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.ResetTrigger(WarriorBossTrigger.HeavyAttack);
		warriorBoss.isAttacking = false;
	}
}