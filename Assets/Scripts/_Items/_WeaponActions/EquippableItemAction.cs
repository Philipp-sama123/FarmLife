using UnityEngine;

namespace KrazyKatGames
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/ Test Action")]
    public class EquippableItemAction : ScriptableObject
    {
        public int actionID;
        public virtual void AttemptToPerformAction(PlayerManager playerPerformingAction, EquippableItem equippablePerformingAction)
        {
            Debug.LogWarning(
                "playerPerformingAction.playerCombatManager.currentEquippableBeingUsed " + playerPerformingAction.playerCombatManager.
                    currentEquippableBeingUsed);
            playerPerformingAction.playerCombatManager.currentEquippableBeingUsed = equippablePerformingAction;
            Debug.Log("The Action has fired: " + equippablePerformingAction);
        }
    }
}