using UnityEngine;

namespace Living.Enemies.FarmerBoss.Scripts
{
	public class FarmerThrowPitchfork : StateMachineBehaviour
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
			Debug.Log("ResetTrigger called for ThrowPitchfork");
			farmerBoss.isAttacking = false;
			animator.ResetTrigger(FarmerBossTrigger.ThrowPitchfork);
			Debug.Log("ResetTrigger called for ThrowPitchfork");

		}
	}
}
