using UnityEngine;

namespace KrazyKatgames
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]
    public class HeavyAttackWeaponItemAction : WeaponItemAction
    {
        [Header("Main Hand Heavy Attacks")]
        [SerializeField] private string heavy_Attack_01 = "Main_Heavy_Attack_01"; // Main Hand (Right) Heavy Attack
        [SerializeField] private string heavy_Attack_02 = "Main_Heavy_Attack_02"; // Main Hand (Right) Heavy Attack

        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);


            if (playerPerformingAction.playerStatsManager.currentStamina <= 0)
                return;

            if (!playerPerformingAction.playerLocomotionManager.isGrounded)
                return;
            
            playerPerformingAction.isAttacking = true;

            PerformHeavyAttack(playerPerformingAction, weaponPerformingAction);
        }

        private void PerformHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
        
                PerformMainHandHeavyAttack(playerPerformingAction, weaponPerformingAction);
            
        }
        private void PerformMainHandHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;

                if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == heavy_Attack_01)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack02,
                        heavy_Attack_02, true);
                }
            }
            else if (!playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01,
                    heavy_Attack_01, true);
            }
        }
      
    }
}