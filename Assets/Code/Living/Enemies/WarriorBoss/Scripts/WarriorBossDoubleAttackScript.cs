using Code.Living.Enemies.WarriorBoss;
using Living.Enemies.WarriorBoss;
using UnityEngine;

public class WarriorBossDoubleAttackScript : StateMachineBehaviour
{
	private WarriorBoss warriorBoss;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		warriorBoss = animator.GetComponent<WarriorBoss>();
		warriorBoss.isAttacking = true;
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		warriorBoss.isAttacking = false;
		animator.ResetTrigger(WarriorBossTrigger.DoubleAttack);
	}
}