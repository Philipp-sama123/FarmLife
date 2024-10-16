using System;
using UnityEngine;

namespace KrazyKatGames
{
    public class EquippableModelInstantiationSlot : MonoBehaviour
    {
        public EquippableModelSlot equippableSlot;
        public GameObject currentWeaponModel;

        public void UnloadWeapon()
        {
            if (currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }

        public void LoadWeapon(GameObject weaponModel)
        {
            currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;

            weaponModel.transform.localPosition = Vector3.zero;
            weaponModel.transform.localRotation = Quaternion.identity;
            weaponModel.transform.localScale = Vector3.one;
        }
        public void PlaceWeaponModelInUnequippedSlot(GameObject weaponModel, EquipmentClass equipmentClass, PlayerManager player)
        {
            // TODO: Move Weapon on Back closer or move outward depending on chest equipment
            Debug.LogWarning(
                "WEAPON MODEL INSTANTIATION SLOT: PlaceWeaponModelInUnequippedSlot "
                + weaponModel.name
                + " Weapon Class "
                + equipmentClass);

            currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;

            switch (equipmentClass)
            {
                case EquipmentClass.MeleeWeapon:
                    weaponModel.transform.localPosition = new Vector3();
                    weaponModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    break;
                case EquipmentClass.Shield:
                    weaponModel.transform.localPosition = new Vector3(0.1f,0.25f,0f);
                    weaponModel.transform.localRotation = Quaternion.Euler(320, 15, 40);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(equipmentClass), equipmentClass, null);
            }
        }

        public void PlaceWeaponModelIntoSlot(GameObject weaponModel)
        {
            currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;
            Debug.LogWarning("WEAPON MODEL INSTANTIATION SLOT: PlaceWeaponModelIntoSlot " + gameObject.name);
            weaponModel.transform.localPosition = Vector3.zero;
            weaponModel.transform.localRotation = Quaternion.identity;
            weaponModel.transform.localScale = Vector3.one;
        }
    }
}