using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KrazyKatgames
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Off Hand Action")]
    public class OffHandMeleeAction : WeaponItemAction
    {
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);
            // Check For Power Stance
            // Check for block
            if (!playerPerformingAction.playerCombatManager.canBlock)
                return;

            if (playerPerformingAction.isAttacking)
            {
                playerPerformingAction.isBlocking = false;
                return;
            }

            if (playerPerformingAction.isBlocking)
                return;

            playerPerformingAction.isBlocking = true;
        }
    }
}