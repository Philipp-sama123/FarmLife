using UnityEngine;

namespace KrazyKatGames
{
    public class PlayerUIEquipmentManager : MonoBehaviour
    {
        [SerializeField] UI_EquipmentSlot equipmentSlot_01;
        [SerializeField] UI_EquipmentSlot equipmentSlot_02;
        [SerializeField] UI_EquipmentSlot equipmentSlot_03;
        [SerializeField] UI_EquipmentSlot equipmentSlot_04;

        private void OnEnable()
        {
            LoadEquipmentSlots();
        }
        public void LoadEquipmentSlots()
        {
            PlayerManager player = PlayerInputManager.instance.player;
            
            equipmentSlot_01.LoadItem(player.playerInventoryManager.equipmentsInRightHandSlots[0]);
            equipmentSlot_02.LoadItem(player.playerInventoryManager.equipmentsInRightHandSlots[1]);
            equipmentSlot_03.LoadItem(player.playerInventoryManager.equipmentsInRightHandSlots[2]);
            equipmentSlot_04.LoadItem(player.playerInventoryManager.equipmentsInRightHandSlots[3]);
        }
    }
}