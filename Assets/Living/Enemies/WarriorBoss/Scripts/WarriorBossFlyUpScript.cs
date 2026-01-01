using Unity.VisualScripting;
using UnityEngine;

namespace Living.Enemies.WarriorBoss
{
	public class WarriorBossFlyUpScript : StateMachineBehaviour
	{
		private WarriorBoss warriorBoss;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			warriorBoss = animator.GetComponent<WarriorBoss>();
			warriorBoss.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
			warriorBoss.GetComponent<Rigidbody2D>().gravityScale = 0;
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			float flySpeed = 15f;
			warriorBoss.Fly(1, flySpeed);
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
		}
	}
}