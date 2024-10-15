using UnityEngine;
using UnityEngine.AI;

namespace KrazyKatGames
{
    [CreateAssetMenu(menuName = "A.I/States/Search Food")]
    public class SearchFoodState : AIAnimalState
    {
        public float baseWanderRadius = 25f; // Base wander radius
        public float wanderTimer = 10f; // Time between each new random wander point
        public float stuckCheckTimer = 10f; // Time to check if the animal is stuck
        public float minWaitTime = 2.5f; // Minimum wait time after wandering
        public float maxWaitTime = 15f; // Maximum wait time after wandering
        private float currentWanderTimer = 0f;
        private float stuckTimer = 0f;
        private float waitTimer = 0f;
        private bool isWaiting = false;

        private Vector3 lastPosition; // To detect if the animal is stuck

        public override AIAnimalState Tick(AnimalManager animal)
        {
            if (animal.isPerformingAction)
                return this;

            if (!animal.animalAI.navMeshAgent.enabled)
                animal.animalAI.navMeshAgent.enabled = true;

            if (animal.animalAI.isFollowingPlayer)
                return SwitchState(animal, animal.animalAI.followPlayer);

            // Dynamic hunger check for switching to idle state
            if (animal.animalStatsManager.CurrentHunger < 25)
                return SwitchState(animal, animal.animalAI.idle);

            if (animal.animalAI.foodObject == null)
                animal.animalAI.foodObject = SearchForFoodInView(animal);

            // If food is found, move towards it
            if (animal.animalAI.foodObject != null)
            {
                return HandleFoodFound(animal);
            }
            else if (!isWaiting)
            {
                WanderAround(animal); // Wandering logic
            }
            else
            {
                HandleWait(animal); // Handle waiting after wandering
            }

            return this;
        }

        // Handling food found logic
        private AIAnimalState HandleFoodFound(AnimalManager animal)
        {
            NavMeshPath path = new NavMeshPath();
            animal.animalAI.navMeshAgent.CalculatePath(animal.animalAI.foodObject.transform.position, path);
            animal.animalAI.navMeshAgent.SetPath(path);

            if (animal.animalAI.CurrentTarget != animal.animalAI.foodObject.transform.position)
                animal.animalAI.CurrentTarget = animal.animalAI.foodObject.transform.position;

            animal.animalAI.UpdateCurrentTarget();

            if (animal.animalAI.DistanceFromTarget < animal.animalAI.navMeshAgent.stoppingDistance)
            {
                return SwitchState(animal, animal.animalAI.eatFood);
            }
            else
            {
                MoveTowardsTarget(animal);
            }

            return this;
        }

        // Wandering around logic
        private void WanderAround(AnimalManager animal)
        {
            currentWanderTimer += Time.deltaTime;
            stuckTimer += Time.deltaTime;

            // If wander timer is not up, continue moving towards the target
            if (currentWanderTimer <= wanderTimer)
            {
                CheckIfStuck(animal); // Check if stuck while wandering
                MoveTowardsTarget(animal);
            }
            else
            {
                // Reset the wander timer and choose a new random target
                currentWanderTimer = 0f;
                stuckTimer = 0f;
                lastPosition = animal.transform.position;

                animal.animalAI.CurrentTarget = GetRandomWanderPosition(animal.transform.position, GetAdaptiveWanderRadius(animal));
                NavMeshPath path = new NavMeshPath();
                animal.animalAI.navMeshAgent.CalculatePath(animal.animalAI.CurrentTarget, path);
                animal.animalAI.navMeshAgent.SetPath(path);

                // Begin waiting after the wander
                isWaiting = true;
                waitTimer = Random.Range(minWaitTime, maxWaitTime);  // Random wait time before next movement
            }
        }

        // Handle waiting logic after wandering
        private void HandleWait(AnimalManager animal)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                isWaiting = false; // Stop waiting and continue wandering
            }
        }

        // Adaptive wander radius based on animal's hunger and stamina
        private float GetAdaptiveWanderRadius(AnimalManager animal)
        {
            float hungerFactor = Mathf.Clamp01(animal.animalStatsManager.CurrentHunger / 100f);
            float staminaFactor = Mathf.Clamp01(animal.animalStatsManager.CurrentStamina / 100f);

            // If the animal is hungry, it might roam further in search of food
            return baseWanderRadius * (1.2f - hungerFactor) * staminaFactor;
        }

        // Move towards target
        private void MoveTowardsTarget(AnimalManager animal)
        {
            animal.animalAI.UpdateCurrentTarget();
            animal.animalLocomotionManager.RotateTowardsTarget(animal.animalAI.CurrentTarget);

            animal.animalAI.UpdateAnimalMovementParameters();
        }

        // Get random wander position within a radius
        private Vector3 GetRandomWanderPosition(Vector3 origin, float radius)
        {
            Vector3 randomDirection;
            NavMeshHit navHit;
            do
            {
                randomDirection = Random.insideUnitSphere * radius;
                randomDirection += origin;
                NavMesh.SamplePosition(randomDirection, out navHit, radius, -1);
            } while (Mathf.Abs(navHit.position.x - origin.x) < 3); // Ensure some distance from origin

            return navHit.position;
        }

        // Check if the animal is stuck (no movement for a certain duration)
        private void CheckIfStuck(AnimalManager animal)
        {
            if (stuckTimer >= stuckCheckTimer)
            {
                float distanceMoved = Vector3.Distance(animal.transform.position, lastPosition);

                if (distanceMoved < 0.5f) // If barely moved, consider stuck
                {
                    Debug.LogWarning(animal.name + " appears stuck, recalculating path...");
                    animal.animalAI.CurrentTarget = GetRandomWanderPosition(animal.transform.position, baseWanderRadius);
                    animal.animalAI.navMeshAgent.ResetPath(); // Clear any blocked paths
                    stuckTimer = 0f; // Reset the stuck timer
                }

                lastPosition = animal.transform.position; // Update the last known position
                stuckTimer = 0f; // Reset the stuck timer
            }
        }

        // Method to search for food within the detection radius
        private GameObject SearchForFoodInView(AnimalManager animal)
        {
            Collider[] objectsInRadius = Physics.OverlapSphere(animal.transform.position, animal.animalAI.detectionRadius);

            foreach (Collider col in objectsInRadius)
            {
                if (col.CompareTag("Food"))
                {
                    Vector3 directionToTarget = (col.transform.position - animal.transform.position).normalized;
                    float angleToTarget = Vector3.Angle(animal.transform.forward, directionToTarget);

                    if (angleToTarget < animal.animalAI.viewAngle / 2)
                    {
                        return col.gameObject; // Food found within view angle and radius
                    }
                }
            }

            return null; // No food found
        }
    }
}
