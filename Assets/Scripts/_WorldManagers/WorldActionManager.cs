using System.Linq;
using UnityEngine;

namespace KrazyKatGames
{
    public class WorldActionManager : MonoBehaviour
    {
        public static WorldActionManager instance;

        [Header("Weapon Item Action")]
        public EquippableItemAction[] weaponItemActions;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            for (int i = 0; i < weaponItemActions.Length; i++)
            {
                weaponItemActions[i].actionID = i;
            }
        }

        public EquippableItemAction GetWeaponItemActionByID(int ID)
        {
            return weaponItemActions.FirstOrDefault(action => action.actionID == ID);
        }
    }
}