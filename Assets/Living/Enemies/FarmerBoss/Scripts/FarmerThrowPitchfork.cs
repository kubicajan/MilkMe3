using UnityEngine;

namespace Living.Enemies.FarmerBoss.Scripts
{
	public class FarmerThrowPitchfork : StateMachineBehaviour
	{
		private FarmerBoss farmerBoss;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			farmerBoss = animator.GetComponent<FarmerBoss>();
			farmerBoss.SetPlayerAsTarget();
			farmerBoss.isAttacking = true;
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			farmerBoss.SetHasPitchfork(false);
			farmerBoss.isAttacking = false;
			animator.ResetTrigger(FarmerBossTrigger.ThrowPitchfork);
		}
	}
}