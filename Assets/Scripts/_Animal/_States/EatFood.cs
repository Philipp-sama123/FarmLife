using UnityEngine;

namespace KrazyKatGames
{
    [CreateAssetMenu(menuName = "A.I/States/Eat Food")]
    public class EatFoodState : AIAnimalState
    {
        public override AIAnimalState Tick(AnimalManager animal)
        {
            if (animal.isPerformingAction)
                return this;

            if (!animal.animalAI.foodObject)
                return SwitchState(animal, animal.animalAI.searchFood);

            animal.isMoving = false;
            animal.animalAnimatorManager.UpdateAnimatorMovementParameters(0, 0, false);

            animal.animalAnimatorManager.PlayTargetActionAnimation("Eat_Food", true);
            animal.animalStatsManager.ReduceHunger(10);
            animal.animalStatsManager.ResetStaminaHungerRegenerationTimer();

            animal.animalAI.foodObject.gameObject.SetActive(false);
            animal.animalAI.foodObject = null;

            return this;
        }
    }
}