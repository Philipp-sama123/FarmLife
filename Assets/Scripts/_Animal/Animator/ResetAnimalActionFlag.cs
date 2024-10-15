using UnityEngine;

namespace KrazyKatGames
{
    public class ResetAnimalActionFlag : StateMachineBehaviour
    {
        private AnimalManager animal;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animal == null)
            {
                animal = animator.GetComponent<AnimalManager>();
            }

            animal.isPerformingAction = false;

            animal.animalAnimatorManager.applyRootMotion = false;

            // ToDo: maybe remove here
            animal.isJumping = false;
        }
    }
}