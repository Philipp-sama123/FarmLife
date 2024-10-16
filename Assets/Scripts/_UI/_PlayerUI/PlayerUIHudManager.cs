using UnityEngine;
using UnityEngine.UI;

namespace KrazyKatGames
{
    public class PlayerUIHudManager : MonoBehaviour
    {
        [Header("Stat Bars")]
        [SerializeField] UI_StatBar staminaBar;
        [SerializeField] UI_StatBar healthBar;

        [Header("Quick Slots")]
        [SerializeField] Image rightWeaponQuickSlotIcon;
        [SerializeField] Image leftWeaponQuickSlotIcon;


        [Header("Boss Health Bar")]
        public Transform bossHealthBarParent;
        public GameObject bossHealthBarObject; 
        
        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            staminaBar.SetStat(Mathf.RoundToInt(newValue));
        }
        public void SetMaxStaminaValue(float maxStamina)
        {
            staminaBar.SetMaxStat(maxStamina);
        }
        public void SetNewHealthValue(int oldValue, int newValue)
        {
            healthBar.SetStat(newValue);
        }
        public void SetMaxHealthValue(float maxStamina)
        {
            healthBar.SetMaxStat(maxStamina);
        }
        public void RefreshHUD()
        {
            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);
            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);
        }
        public void SetRightWeaponQuickSlotIcon(int weaponID)
        {
            EquippableItem equippable = WorldItemDatabase.Instance.GetWeaponByID(weaponID);
            if (equippable == null)
            {
                Debug.Log("Item is null!");
                rightWeaponQuickSlotIcon.enabled = false;
                rightWeaponQuickSlotIcon.sprite = null;
                return;
            }
            if (equippable.itemIcon == null)
            {
                Debug.LogWarning("Item has no Icon!");
                rightWeaponQuickSlotIcon.enabled = false;
                rightWeaponQuickSlotIcon.sprite = null;
                return;
            }
            // Check if you meet item requirements (!)
            rightWeaponQuickSlotIcon.sprite = equippable.itemIcon;
            rightWeaponQuickSlotIcon.enabled = true;
        }
        public void SetLeftWeaponQuickSlotIcon(int weaponID)
        {
            EquippableItem equippable = WorldItemDatabase.Instance.GetWeaponByID(weaponID);
            if (equippable == null)
            {
                Debug.Log("Item is null!");
                leftWeaponQuickSlotIcon.enabled = false;
                leftWeaponQuickSlotIcon.sprite = null;
                return;
            }
            if (equippable.itemIcon == null)
            {
                Debug.LogWarning("Item has no Icon!");
                leftWeaponQuickSlotIcon.enabled = false;
                leftWeaponQuickSlotIcon.sprite = null;
                return;
            }
            // Check if you meet item requirements (!)
            leftWeaponQuickSlotIcon.sprite = equippable.itemIcon;
            leftWeaponQuickSlotIcon.enabled = true;
        }
    }
}