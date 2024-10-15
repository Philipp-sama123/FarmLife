using UnityEngine;

namespace KrazyKatGames
{
    public class ResetActionFlag : StateMachineBehaviour
    {
        private CharacterManager character;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (character == null)
            {
                character = animator.GetComponent<CharacterManager>();
            }
            //  THIS IS CALLED WHEN AN ACTION ENDS, AND THE STATE RETURNS TO "EMPTY"
            character.isPerformingAction = false;

            character.characterAnimatorManager.applyRootMotion = false;
            character.characterLocomotionManager.canRotate = true;
            character.characterLocomotionManager.canMove = true;
            character.characterLocomotionManager.isRolling = false;

            // character.characterCombatManager.DisableCanDoCombo();
            // character.characterCombatManager.DisableCanDoBackstepAttack();
            // character.characterCombatManager.DisableCanDoRollingAttack();


            // ToDo: maybe remove here
            character.isJumping = false;
            //     character.isInvulnerable.Value = false;
        }
    }
}