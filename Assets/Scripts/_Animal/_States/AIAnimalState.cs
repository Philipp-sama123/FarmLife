using UnityEngine;

namespace KrazyKatgames
{
    public class AIAnimalState : ScriptableObject
    {
        public virtual AIAnimalState Tick(AnimalManager animal)
        {
            return this;
        }
        protected virtual AIAnimalState SwitchState(AnimalManager animal, AIAnimalState newState)
        {
            Debug.LogWarning(animal.name + " SwitchState to " + newState.name);
            
            ResetStateFlags(animal);
            return newState;
        }
        protected virtual void ResetStateFlags(AnimalManager animal)
        {
        }
    }
}