using UnityEngine;

namespace Code.Living.Enemies.WarriorBoss.Scripts
{
	public class WarriorBossFlyUpScript : StateMachineBehaviour
	{
		private WarriorBoss warriorBoss;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			warriorBoss = animator.GetComponent<WarriorBoss>();
			warriorBoss.GetRigidBody().bodyType = RigidbodyType2D.Kinematic;
			warriorBoss.GetRigidBody().gravityScale = 0;
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
		}
	}
}