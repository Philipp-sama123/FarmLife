using UnityEngine;

namespace KrazyKatGames
{
    public class WeaponManager : MonoBehaviour
    {
        public MeleeWeaponDamageCollider meleeDamageCollider;

        private void Awake()
        {
            meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
        }
        public void SetWeaponDamage(CharacterManager characterWieldingWeapon, EquippableItem equippable)
        {
            meleeDamageCollider.characterCausingDamage = characterWieldingWeapon;
            meleeDamageCollider.physicalDamage = equippable.physicalDamage;
            meleeDamageCollider.magicDamage = equippable.magicDamage;
            meleeDamageCollider.fireDamage = equippable.fireDamage;
            meleeDamageCollider.holyDamage = equippable.holyDamage;
            meleeDamageCollider.lightningDamage = equippable.lightningDamage;
            meleeDamageCollider.poiseDamage = equippable.poiseDamage;

            meleeDamageCollider.light_Attack_01_Modifier = equippable.light_attack_01_modifier;
            meleeDamageCollider.light_Attack_02_Modifier = equippable.light_attack_02_modifier;
            meleeDamageCollider.light_Attack_03_Modifier = equippable.light_attack_03_modifier;
            meleeDamageCollider.light_Attack_04_Modifier = equippable.light_attack_04_modifier;

            meleeDamageCollider.heavy_Attack_01_Modifier = equippable.heavy_Attack_01_Modifier;
            meleeDamageCollider.heavy_Attack_02_Modifier = equippable.heavy_Attack_02_Modifier;
            meleeDamageCollider.heavy_Attack_03_Modifier = equippable.heavy_Attack_03_Modifier;
            meleeDamageCollider.heavy_Attack_04_Modifier = equippable.heavy_Attack_04_Modifier;

            meleeDamageCollider.charge_Attack_01_Modifier = equippable.charge_Attack_01_Modifier;
            meleeDamageCollider.charge_Attack_02_Modifier = equippable.charge_Attack_02_Modifier;

            meleeDamageCollider.run_Attack_01_Modifier = equippable.run_Attack_01_Modifier;
            meleeDamageCollider.roll_Attack_01_Modifier = equippable.roll_Attack_01_Modifier;
            meleeDamageCollider.backstep_Attack_01_Modifier = equippable.backstep_Attack_01_Modifier;
        }
    }
}