using UnityEngine;

namespace Living.Enemies.FarmerBoss.Scripts
{
	public class AttackJumpFarmer : StateMachineBehaviour
	{
		private FarmerBoss farmerBoss;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			farmerBoss = animator.GetComponent<FarmerBoss>();
			farmerBoss.isAttacking = true;
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			farmerBoss.isAttacking = false;
		}
	}
}