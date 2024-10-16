using UnityEngine;

namespace KrazyKatGames
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]
    public class HeavyAttackEquippableItemAction : EquippableItemAction
    {
        [Header("Main Hand Heavy Attacks")]
        [SerializeField] private string heavy_Attack_01 = "Main_Heavy_Attack_01"; // Main Hand (Right) Heavy Attack
        [SerializeField] private string heavy_Attack_02 = "Main_Heavy_Attack_02"; // Main Hand (Right) Heavy Attack

        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, EquippableItem equippablePerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, equippablePerformingAction);


            if (playerPerformingAction.playerStatsManager.currentStamina <= 0)
                return;

            if (!playerPerformingAction.playerLocomotionManager.isGrounded)
                return;
            
            playerPerformingAction.isAttacking = true;

            PerformHeavyAttack(playerPerformingAction, equippablePerformingAction);
        }

        private void PerformHeavyAttack(PlayerManager playerPerformingAction, EquippableItem equippablePerformingAction)
        {
        
                PerformMainHandHeavyAttack(playerPerformingAction, equippablePerformingAction);
            
        }
        private void PerformMainHandHeavyAttack(PlayerManager playerPerformingAction, EquippableItem equippablePerformingAction)
        {
            if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;

                if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == heavy_Attack_01)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(equippablePerformingAction, AttackType.HeavyAttack02,
                        heavy_Attack_02, true);
                }
            }
            else if (!playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(equippablePerformingAction, AttackType.HeavyAttack01,
                    heavy_Attack_01, true);
            }
        }
      
    }
}