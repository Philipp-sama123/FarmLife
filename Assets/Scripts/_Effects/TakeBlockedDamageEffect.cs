using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace KrazyKatGames
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Blocked Damage")]
    public class TakeBlockedDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage;

        [Header("Damage")]
        public float physicalDamage = 0; // break in its subtypes (Slashing, Piercing, Striking) 
        // ToDo: look at Baldur's Gate 3 (!)

        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;

        [Header("Stamina")]
        public float staminaDamage = 0;
        public float finalStaminaDamage = 0;

        [Header("Final Damage")]
        private int finalDamageDealt = 0;

        [Header("Poise")] // Also a future ToDo: 
        public float poiseDamage = 0;
        public bool poiseIsBroken = false;

        // ToDo: Build Ups, like Bleeding, 

        [Header("Animations")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("SoundFx")]
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSoundFX; // used on top of regular SFX if there is elemental Damage present (Magic/Fire/Lightning/Holy) 

        [Header("Damage Direction")]
        public float angleHitFrom; // used to determine what damage animation to play (which direction)
        public Vector3 contactPoint;


        public override void ProcessEffect(CharacterManager character)
        {
            // don't do anything if character is invulnerable
            if (character.isInvulnerable)
                return;
            base.ProcessEffect(character);

            Debug.LogWarning("Hit was BLOCKED by " + character.name + " from: " +
                             (characterCausingDamage != null ? characterCausingDamage.name : null));

            if (character.isDead)
                return;

            CalculateDamage(character);
            CalculateStaminaDamage(character);
            PlayDirectionalBasedBlockingDamageAnimation(character);
            // ToDo: Check for invulnerability
            PlayDamageSFX(character);
            PlayDamageVFX(character);
            
            CheckForGuardBreak(character);
        }
        private void CalculateStaminaDamage(CharacterManager character)
        {
            finalStaminaDamage = staminaDamage;
            float staminaDamageAbsorption = finalStaminaDamage * (character.characterStatsManager.blockingStability / 100);
            float staminaDamageAfterAbsorption = finalStaminaDamage - staminaDamageAbsorption;
            Debug.LogWarning(this.name + "CalculateStaminaDamage- staminaDamage: " + staminaDamageAfterAbsorption);

            character.characterStatsManager.currentStamina -= staminaDamageAfterAbsorption;
        }

        private void CheckForGuardBreak(CharacterManager character)
        {
            // if (character.characterNetworkManager.currentStamina.Value <= 0)
            // Play SFX (!)


            if (character.characterStatsManager.currentStamina <= 0)
            {
                character.characterAnimatorManager.PlayTargetActionAnimation("Guard_Break_01", true);
                character.isBlocking = false;
            }
        }
        private void CalculateDamage(CharacterManager character)
        {

            if (characterCausingDamage != null)
            {
                // ToDo: Check for Damage modifiers and modify base damage
            }
            Debug.LogWarning("CalculateDamage ORIGINAL Physical Damage: " + physicalDamage);

            physicalDamage -= physicalDamage * (1 / character.characterStatsManager.blockingPhysicalAbsorption);

            Debug.LogWarning("CalculateDamage AFTER BLOCKING Physical Damage: " + physicalDamage);

            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }
            Debug.LogWarning("Damage to " + character.name + " finalDamageDealt: " + finalDamageDealt);

            character.characterStatsManager.currentHealth -= finalDamageDealt;
        }

        private void PlayDamageVFX(CharacterManager character)
        {
            character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
        }

        private void PlayDamageSFX(CharacterManager character)
        {
            character.characterSoundFXManager.PlayBlockSoundFX();
        }

        private void PlayDirectionalBasedBlockingDamageAnimation(CharacterManager character)
        {
            if (character.isDead)
                return;

            // 1. Calculate Intensity based on poise damage (!)
            DamageIntensity damageIntensity = WorldUtilityManager.Instance.GetDamageIntensityBasedOnPoiseDamage(poiseDamage);
            switch (damageIntensity)
            {
                case DamageIntensity.Ping:
                    damageAnimation = "Block_Ping_01";
                    break;
                case DamageIntensity.Light:
                    damageAnimation = "Block_Light_01";
                    break;
                case DamageIntensity.Medium:
                    damageAnimation = "Block_Medium_01";
                    break;
                case DamageIntensity.Heavy:
                    damageAnimation = "Block_Heavy_01";
                    break;
                case DamageIntensity.Colossal:
                    damageAnimation = "Block_Colossal_01";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;
            character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
        }
    }
}