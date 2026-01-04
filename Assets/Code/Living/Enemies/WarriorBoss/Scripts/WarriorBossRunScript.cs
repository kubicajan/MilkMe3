using Unity.VisualScripting;
using UnityEngine;

namespace Living.Enemies.WarriorBoss
{
	public class WarriorBossRunScript : StateMachineBehaviour
	{
		private WarriorBoss warriorBoss;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			warriorBoss = animator.GetComponent<WarriorBoss>();
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			warriorBoss.Move();
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
		}
	}
}