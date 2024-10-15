using UnityEngine;
using UnityEngine.AI;

namespace KrazyKatGames
{
    [CreateAssetMenu(menuName = "A.I/States/Follow Player State")]
    public class FollowPlayerState : AIAnimalState
    {
        public override AIAnimalState Tick(AnimalManager animal)
        {
            if (animal.isPerformingAction)
                return this;

            if (!animal.animalAI.navMeshAgent.enabled)
                animal.animalAI.navMeshAgent.enabled = true;

            if (!animal.animalAI.isFollowingPlayer)
                return SwitchState(animal, animal.animalAI.searchFood);

            NavMeshPath path = new NavMeshPath();

            animal.animalAI.navMeshAgent.CalculatePath(animal.animalAI.playerFollowTarget.transform.position, path);
            animal.animalAI.navMeshAgent.SetPath(path);

            if (animal.animalAI.CurrentTarget != animal.animalAI.playerFollowTarget.transform.position)
                animal.animalAI.CurrentTarget = animal.animalAI.playerFollowTarget.transform.position;

            animal.animalLocomotionManager.PivotTowardsTarget(animal);
            animal.animalLocomotionManager.RotateTowardsAgent(animal);


            animal.animalAI.UpdateAnimalMovementParameters();

            return this;
        }
    }
}