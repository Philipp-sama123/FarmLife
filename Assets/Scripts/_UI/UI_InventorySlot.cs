using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace KrazyKatGames
{
    public class UI_InventorySlot : MonoBehaviour
    {
        [SerializeField] public Image image;
        public Item item;
        public void LoadItem(Item itemToLoad)
        {
            if (itemToLoad != null)
                image.sprite = itemToLoad.itemIcon;
            else
                Debug.LogWarning("No itemIcon (!)");
            item = itemToLoad;
        }
        public void EquipItem()
        {
            if (item != null)
            {
                // Get the player's inventory manager
                PlayerInventoryManager playerInventoryManager = PlayerInputManager.instance.player.playerInventoryManager;

                // Cast the selected item to a WeaponItem
                Item newWeapon = item;
                // Remove from the inventory
                playerInventoryManager.itemsInInventory.Remove(item);

                Debug.LogWarning("EquipItem: " + newWeapon.itemName);

                // Store the weapon that gets removed (if any)
                Item removedWeapon = playerInventoryManager.equipmentsInRightHandSlots[playerInventoryManager.equipmentsInRightHandSlots.Length - 1];

                // Shift all other weapons in the array one position to the right
                for (int i = playerInventoryManager.equipmentsInRightHandSlots.Length - 1; i > 0; i--)
                {
                    playerInventoryManager.equipmentsInRightHandSlots[i] = playerInventoryManager.equipmentsInRightHandSlots[i - 1];
                }

                // Insert the new weapon at the first position (index 0)
                playerInventoryManager.equipmentsInRightHandSlots[0] = (EquippableItem)newWeapon;

                // Log the removed weapon if the last slot was not null
                if (removedWeapon != null)
                {
                    Debug.Log("Removed weapon from last slot: " + removedWeapon.itemName);
                    
                    if (removedWeapon != WorldItemDatabase.Instance.unarmedEquippable)
                        playerInventoryManager.AddItemToInventory(removedWeapon);
                    else
                    {
                        Debug.Log("Unarmed Weapon: " + removedWeapon.name);
                    }
                }

                // Update the UI and equipment models
                PlayerUIManager.instance.playerUIMainMenuManager.equipmentManager.LoadEquipmentSlots();
                PlayerUIManager.instance.playerUIMainMenuManager.inventoryManager.LoadInventorySlots();
            }
        }
    }
}