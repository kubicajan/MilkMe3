using UnityEngine;

namespace Living.Enemies.FarmerBoss.Scripts
{
	public class PickUpPitchforkFarmer : StateMachineBehaviour
	{
		private Code.Living.Enemies.FarmerBoss.FarmerBoss farmerBoss;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			farmerBoss = animator.GetComponent<Code.Living.Enemies.FarmerBoss.FarmerBoss>();
			farmerBoss.isAttacking = true;
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			farmerBoss.isAttacking = false;
			animator.ResetTrigger(FarmerBossTrigger.PickUpPitchfork);
		}
	}
}