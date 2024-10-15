using UnityEngine;

namespace KrazyKatGames
{
    [CreateAssetMenu(menuName = "A.I/States/Idle")]
    public class IdleState : AIAnimalState
    {
        public override AIAnimalState Tick(AnimalManager animal)
        {
            animal.isMoving = false;
            animal.animalAnimatorManager.UpdateAnimatorMovementParameters(0, 0, false);

            if (animal.animalAI.isFollowingPlayer && animal.animalAI.DistanceFromTarget > 5)
            {
                return SwitchState(animal, animal.animalAI.followPlayer);
            }
            else if (animal.animalStatsManager.CurrentHunger > 25)
            {
                return SwitchState(animal, animal.animalAI.searchFood);
            }
            else
            {
                return this;
            }
        }
    }
}