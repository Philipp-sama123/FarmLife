using UnityEngine;
using UnityEngine.Serialization;

namespace KrazyKatGames
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player;

        public EquippableItem currentEquippableBeingUsed;

        [Header("Flags")]
        public bool canComboWithMainHandWeapon = false;
        // public bool canComboWithOffHandWeapon = false; 
        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
            lockOnTransform = GetComponentInChildren<LockOnTransform>().transform;
        }

        public void PerformEquipmentBasedAction(EquippableItemAction equippableAction, EquippableItem equippablePerformingAction)
        {
            //  PERFORM THE ACTION
            equippableAction.AttemptToPerformAction(player, equippablePerformingAction);
        }


        #region Animation Events
        public override void EnableCanDoCombo()
        {
            base.EnableCanDoCombo();
            player.playerCombatManager.canComboWithMainHandWeapon = true;
        }
        public override void DisableCanDoCombo()
        {
            player.playerCombatManager.canComboWithMainHandWeapon = false;
        }

        public void DrainStaminaBasedOnAttack()
        {
            if (currentEquippableBeingUsed == null)
                return;

            float staminaDeducted = 0f;
            switch (currentAttackType)
            {
                case AttackType.LightAttack01:
                    staminaDeducted = currentEquippableBeingUsed.baseStaminaCost * currentEquippableBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                case AttackType.LightAttack02:
                    staminaDeducted = currentEquippableBeingUsed.baseStaminaCost * currentEquippableBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                case AttackType.LightAttack03:
                    staminaDeducted = currentEquippableBeingUsed.baseStaminaCost * currentEquippableBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                case AttackType.LightAttack04:
                    staminaDeducted = currentEquippableBeingUsed.baseStaminaCost * currentEquippableBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack01:
                    staminaDeducted = currentEquippableBeingUsed.baseStaminaCost * currentEquippableBeingUsed.heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack02:
                    staminaDeducted = currentEquippableBeingUsed.baseStaminaCost * currentEquippableBeingUsed.heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack03:
                    staminaDeducted = currentEquippableBeingUsed.baseStaminaCost * currentEquippableBeingUsed.heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack04:
                    staminaDeducted = currentEquippableBeingUsed.baseStaminaCost * currentEquippableBeingUsed.heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.ChargedAttack01:
                    staminaDeducted = currentEquippableBeingUsed.baseStaminaCost * currentEquippableBeingUsed.chargedAttackStaminaCostMultiplier;
                    break;
                case AttackType.ChargedAttack02:
                    staminaDeducted = currentEquippableBeingUsed.baseStaminaCost * currentEquippableBeingUsed.chargedAttackStaminaCostMultiplier;
                    break;
                case AttackType.RollingAttack_01:
                    staminaDeducted = currentEquippableBeingUsed.baseStaminaCost * currentEquippableBeingUsed.rollingAttackStaminaCostMultiplier;
                    break;
                case AttackType.RunningAttack01:
                    staminaDeducted = currentEquippableBeingUsed.baseStaminaCost * currentEquippableBeingUsed.runningAttackStaminaCostMultiplier;
                    break;
                case AttackType.BackstepAttack_01:
                    staminaDeducted = currentEquippableBeingUsed.baseStaminaCost * currentEquippableBeingUsed.backstepAttackStaminaCostMultiplier;
                    break;
                default:
                    break;
            }
            Debug.LogWarning("For " + player.name + " staminaDeducted -" + staminaDeducted);
            player.playerStatsManager.currentStamina -= Mathf.RoundToInt(staminaDeducted);
        }
        #endregion
    }
}