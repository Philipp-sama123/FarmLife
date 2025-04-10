using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KrazyKatGames
{
    public class WorldUtilityManager : MonoBehaviour
    {
        public static WorldUtilityManager Instance;

        [Header("Layers")]
        [SerializeField] LayerMask characterLayers;
        [SerializeField] LayerMask enviroLayers;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            DontDestroyOnLoad(this);
        }
        public LayerMask GetCharacterLayers()
        {
            return characterLayers;
        }

        public LayerMask GetEnviroLayers()
        {
            return enviroLayers;
        }

        public bool CanIDamageThisTarget(CharacterGroup attackingCharacter, CharacterGroup targetCharacter)
        {
            if (attackingCharacter == CharacterGroup.Team01)
            {
                switch (targetCharacter)
                {
                    case CharacterGroup.Team01: return false;
                    case CharacterGroup.Team02: return true;
                    default:
                        break;
                }
            }
            else if (attackingCharacter == CharacterGroup.Team02)
            {
                switch (targetCharacter)
                {
                    case CharacterGroup.Team01: return true;
                    case CharacterGroup.Team02: return false;
                    default:
                        break;
                }
            }

            return false;
        }
        public float GetAngleOfTarget(Transform characterTransform, Vector3 targetsDirection)
        {
            targetsDirection.y = 0;
            float viewableAngle = Vector3.Angle(characterTransform.forward, targetsDirection);
            Vector3 cross = Vector3.Cross(characterTransform.forward, targetsDirection);

            if (cross.y < 0)
                viewableAngle = -viewableAngle;

            return viewableAngle;
        }

        public DamageIntensity GetDamageIntensityBasedOnPoiseDamage(float poiseDamage)
        {
            // small items
            DamageIntensity damageIntensity = DamageIntensity.Ping;

            // standard Weapon / light attacks
            if (poiseDamage >= 10)
                damageIntensity = DamageIntensity.Light;

            // standard Weapon / medium attacks
            if (poiseDamage >= 30)
                damageIntensity = DamageIntensity.Medium;

            // great Weapon / heavy attacks
            if (poiseDamage >= 70)
                damageIntensity = DamageIntensity.Heavy;

            // ultra Weapon / colossal attacks
            if (poiseDamage >= 120)
                damageIntensity = DamageIntensity.Colossal;

            return damageIntensity;
        }
    }
}