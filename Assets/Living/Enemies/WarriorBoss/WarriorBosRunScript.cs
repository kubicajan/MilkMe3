using Unity.VisualScripting;
using UnityEngine;

namespace Living.Enemies.WarriorBoss
{
	public class WarriorBosRunScript : StateMachineBehaviour
	{
		private WarriorBoss warriorBoss;
		private bool isAttackSelected = false;
		private string selectedAttack = null;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			warriorBoss = animator.GetComponent<WarriorBoss>();
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			warriorBoss.Move();
			if (!isAttackSelected)
			{
				selectedAttack = warriorBoss.SelectAttack();
				Debug.Log(selectedAttack);

				if (selectedAttack != null)
				{
					isAttackSelected = true;
					animator.SetTrigger(selectedAttack);
				}
			}
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (selectedAttack != null)
			{
				animator.ResetTrigger(selectedAttack);
				isAttackSelected = false;
			}
		}
	}
}