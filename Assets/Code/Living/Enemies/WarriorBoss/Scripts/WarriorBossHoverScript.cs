using UnityEngine;

namespace Code.Living.Enemies.WarriorBoss.Scripts
{
	public class WarriorBossHoverScript : StateMachineBehaviour
	{
		private WarriorBoss warriorBoss;
		private int direction = 1;
		private int lastLoop = 0;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			warriorBoss = animator.GetComponent<WarriorBoss>();
			direction = 1;
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			int currentLoop = Mathf.FloorToInt(stateInfo.normalizedTime);
			if (currentLoop != lastLoop)
			{
				direction *= -1;
				lastLoop = currentLoop;
			}

			float flySpeed = 0.5f;
			warriorBoss.StartFly(direction, flySpeed);
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
		}
	}
}