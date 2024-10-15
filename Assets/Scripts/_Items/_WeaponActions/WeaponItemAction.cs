using UnityEngine;

namespace KrazyKatGames
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/ Test Action")]
    public class WeaponItemAction : ScriptableObject
    {
        public int actionID;
        public virtual void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            playerPerformingAction.playerCombatManager.currentWeaponBeingUsed.itemID = weaponPerformingAction.itemID;
            Debug.Log("The Action has fired: " + weaponPerformingAction);
        }
    }
}