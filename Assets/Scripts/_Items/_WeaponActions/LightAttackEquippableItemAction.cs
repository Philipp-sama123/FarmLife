using UnityEngine;

namespace KrazyKatGames
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
    public class LightAttackEquippableItemAction : EquippableItemAction
    {
        // Main Hand
        [Header("Main Hand Light Attacks")]
        [SerializeField] private string light_Attack_01 = "Main_Light_Attack_01"; // Main Hand (Right) Light Attack
        [SerializeField] private string light_Attack_02 = "Main_Light_Attack_02";
        [SerializeField] private string light_Attack_03 = "Main_Light_Attack_03";
        [SerializeField] private string light_Attack_04 = "Main_Light_Attack_04";

        [Header("Main Hand Running Attacks")]
        [SerializeField] private string run_attack_01 = "Main_Run_Attack_01";

        [Header("Main Hand Rolling Attacks")]
        [SerializeField] private string roll_attack_01 = "Main_Roll_Attack_01";

        [Header("Main Hand Backstep Attacks")]
        [SerializeField] private string backstep_attack_01 = "Main_Backstep_Attack_01";

        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, EquippableItem equippablePerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, equippablePerformingAction);
            Debug.LogWarning("LightAttackEquippableItemAction!!!!");
            
            if (playerPerformingAction.characterStatsManager.currentStamina <= 0)
                return;

            if (!playerPerformingAction.playerLocomotionManager.isGrounded)
                return;

            playerPerformingAction.isAttacking = true;
            
            PerformLightAttack(playerPerformingAction, equippablePerformingAction);
        }
        private void PerformLightAttack(PlayerManager playerPerformingAction, EquippableItem equippablePerformingAction)
        {
            PerformMainHandLightAttack(playerPerformingAction, equippablePerformingAction);
        }
        private void PerformMainHandLightAttack(PlayerManager playerPerformingAction, EquippableItem equippablePerformingAction)
        {
            Debug.LogWarning("!!!!!!-------PerformMainHandLightAttack-------!!!!");

            if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;

                if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_01)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(equippablePerformingAction, AttackType.LightAttack02,
                        light_Attack_02, true);
                }
                else if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_02)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(equippablePerformingAction, AttackType.LightAttack03,
                        light_Attack_03, true);
                }
                else if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_03)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(equippablePerformingAction, AttackType.LightAttack04,
                        light_Attack_04, true);
                }
            }
            else if (!playerPerformingAction.isPerformingAction) // remove this if you want to attack while doing an action
                // also ToDo here: dodge attack, jump attack 
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(equippablePerformingAction, AttackType.LightAttack01,
                    light_Attack_01, true);
            }
        }
    }
}