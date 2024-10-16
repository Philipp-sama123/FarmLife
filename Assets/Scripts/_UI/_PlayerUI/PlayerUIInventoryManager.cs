using System;
using System.Collections;
using System.Collections.Generic;
using KrazyKatGames;
using UnityEngine;

namespace KrazyKatGames
{
    public class PlayerUIInventoryManager : MonoBehaviour
    {
        [SerializeField] GameObject inventorySlot;
        [SerializeField] GameObject inventoryParent;
        private List<UI_InventorySlot> slots = new List<UI_InventorySlot>();
        private void OnEnable()
        {
            InitializeInventorySlots();
        }
        public void InitializeInventorySlots()
        {
            ResetSlots();

            if (PlayerInputManager.instance.player.playerInventoryManager.itemsInInventory.Count > 0)
            {
                foreach (var item in PlayerInputManager.instance.player.playerInventoryManager.itemsInInventory)
                {
                    var newInventorySlot = Instantiate(inventorySlot, inventoryParent.transform);
                    var ui_inventorySlot = newInventorySlot.GetComponent<UI_InventorySlot>();
                    ui_inventorySlot.LoadItem(item);
                    Debug.LogWarning("inventoryItem " + ui_inventorySlot);
                    slots.Add(ui_inventorySlot);
                    //  inventoryItem.SetIcon(Im)
                }
            }
        }
        public void ClickInventorySlot()
        {
            
        }
        private void ResetSlots()
        {
            foreach (var slot in slots)
            {
                Destroy(slot.gameObject);
            }
            slots.Clear();
        }
    }
}