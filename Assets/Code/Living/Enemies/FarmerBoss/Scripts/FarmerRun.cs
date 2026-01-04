using UnityEngine;

namespace Living.Enemies.FarmerBoss.Scripts
{
    public class FarmerRun : StateMachineBehaviour
    {
        private FarmerBoss farmerBoss;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            farmerBoss = animator.GetComponent<FarmerBoss>();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            farmerBoss.Move();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }
    }
}
