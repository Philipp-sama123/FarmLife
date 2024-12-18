using UnityEngine;

namespace KrazyKatGames
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffect
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

            if (character.isDead)
                return;

            CalculateDamage(character);
            PlayDirectionalBasedDamageAnimation(character);
            // ToDo: Check for invulnerability

            // Calculate Damage
            // Check Damage Direction
            // Check for Buildups (poison,bleed, ....) 
            PlayDamageSFX(character);
            PlayDamageVFX(character);
            // If Character is A.I. check for new target if character causing damage is present 
        }

        private void CalculateDamage(CharacterManager character)
        {
            if (characterCausingDamage != null)
            {
                // ToDo: Check for Damage modifiers and modify base damage (Physical or Elemental Damage Buff)
            }

            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);
            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }

            character.characterStatsManager.currentHealth -= finalDamageDealt;
            // subtract poise damage from the character total 
            character.characterStatsManager.totalPoiseDamage -= poiseDamage;

            float remainingPoise = character.characterStatsManager.basePoiseDefense
                                   + character.characterStatsManager.offensivePoiseBonus
                                   + character.characterStatsManager.totalPoiseDamage;

            if (remainingPoise <= 0)
                poiseIsBroken = true;

            // since Character is hit --> Reset Poise Timer (!)
            character.characterStatsManager.poiseResetTimer = character.characterStatsManager.defaultPoiseResetTime;
        }

        private void PlayDamageVFX(CharacterManager character)
        {
            character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
        }

        private void PlayDamageSFX(CharacterManager character)
        {
            AudioClip physicalDamageSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.physicalDamageSFX);
            character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX);
            character.characterSoundFXManager.PlayDamageGruntSoundFX();
        }

        private void PlayDirectionalBasedDamageAnimation(CharacterManager character)
        {
            if (character.isDead)
                return;

            if (poiseIsBroken)
            {
                if (angleHitFrom >= 145 && angleHitFrom <= 180)
                {
                    damageAnimation =
                        character.characterAnimatorManager.forward_Medium_Damage;
                }
                else if (angleHitFrom <= -145 && angleHitFrom >= -180)
                {
                    damageAnimation =
                        character.characterAnimatorManager.forward_Medium_Damage;
                }
                else if (angleHitFrom >= -45 && angleHitFrom <= 45)
                {
                    damageAnimation =
                        character.characterAnimatorManager.backward_Medium_Damage;
                }
                else if (angleHitFrom >= -144 && angleHitFrom <= -45)
                {
                    damageAnimation =
                        character.characterAnimatorManager.left_Medium_Damage;
                }
                else if (angleHitFrom >= 45 && angleHitFrom <= 144)
                {
                    damageAnimation =
                        character.characterAnimatorManager.right_Medium_Damage;
                }
            }
            else
            {
                if (angleHitFrom >= 145 && angleHitFrom <= 180)
                {
                    damageAnimation =
                        character.characterAnimatorManager.forward_Ping_Damage;
                }
                else if (angleHitFrom <= -145 && angleHitFrom >= -180)
                {
                    damageAnimation =
                        character.characterAnimatorManager.forward_Ping_Damage;
                }
                else if (angleHitFrom >= -45 && angleHitFrom <= 45)
                {
                    damageAnimation =
                        character.characterAnimatorManager.backward_Ping_Damage;
                }
                else if (angleHitFrom >= -144 && angleHitFrom <= -45)
                {
                    damageAnimation =
                        character.characterAnimatorManager.left_Ping_Damage;
                }
                else if (angleHitFrom >= 45 && angleHitFrom <= 144)
                {
                    damageAnimation =
                        character.characterAnimatorManager.right_Ping_Damage;
                }
            }
            character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;

            if (poiseIsBroken)
            {
                //restrict Movement and Actions (--> isStunned)
                character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
            }
            else
            {
                //if not poise broken just play an upperbody animation without restricting movement
                character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, false, false, true, true);
            }
        }
    }
}