using UnityEngine;

namespace Living.Enemies.FarmerBoss.Scripts
{
	public class PickUpPitchforkFarmer : StateMachineBehaviour
	{
		private FarmerBoss farmerBoss;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			farmerBoss = animator.GetComponent<FarmerBoss>();
			farmerBoss.isAttacking = true;
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			farmerBoss.isAttacking = false;
			animator.ResetTrigger(FarmerBossTrigger.PickUpPitchfork);
		}
	}
}