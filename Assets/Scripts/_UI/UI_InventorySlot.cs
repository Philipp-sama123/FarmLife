using UnityEngine;
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
            Debug.LogWarning("EquipItem" + item.name);
            if (item != null)
            {
                PlayerInputManager.instance.player.playerInventoryManager.weaponsInRightHandSlots[0] = (WeaponItem)item;
                PlayerUIManager.instance.playerUIMainMenuManager.equipmentManager.LoadEquipmentSlots();
            }
        }
    }
}