using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KrazyKatGames
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Off Hand Action")]
    public class OffHandMeleeAction : EquippableItemAction
    {
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, EquippableItem equippablePerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, equippablePerformingAction);
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