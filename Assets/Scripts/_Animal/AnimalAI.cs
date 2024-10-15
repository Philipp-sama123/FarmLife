using System;
using UnityEngine;
using UnityEngine.AI;

namespace KrazyKatGames
{
    public class AnimalAI : MonoBehaviour
    {
        private AnimalManager animal;
        public float detectionRadius = 15f; // Detection radius for viewing circle
        public float viewAngle = 120f; // Field of view angle for the animal

        [Header("NavMesh Agent")]
        public NavMeshAgent navMeshAgent;

        [Header("Current State")]
        [SerializeField] protected AIAnimalState currentState;


        [Header("States")]
        public IdleState idle;
        public FollowPlayerState followPlayer;
        public SearchFoodState searchFood;
        public EatFoodState eatFood;

        public bool isFollowingPlayer = false;

        // AI (!)
        public PlayerManager playerFollowTarget;

        private Vector3 targetsDirection;
        private float angleToTarget;
        private float distanceFromTarget;

        private Vector3 currentTarget;
        public GameObject foodObject;

        public float AngleToTarget => angleToTarget;
        public float DistanceFromTarget => distanceFromTarget;
        public Vector3 CurrentTarget
        {
            get => currentTarget;
            set => currentTarget = value;
        }
        private void Awake()
        {
            animal = GetComponent<AnimalManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();

            if (navMeshAgent.stoppingDistance <= 0)
                navMeshAgent.stoppingDistance = 1;
        }
        private void Start()
        {
            idle = Instantiate(idle);
            followPlayer = Instantiate(followPlayer);
            searchFood = Instantiate(searchFood);

            currentState = searchFood;
        }


        public void ProcessStateMachine()
        {
            animal.animalStatsManager.RegenerateHunger();


            AIAnimalState nextState = currentState.Tick(animal);

            if (nextState != null)
            {
                currentState = nextState;
            }

            // the position/rotation should be reset after the state machine has processed it
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }
        public float GetAngleOfTarget(Transform characterTransform, Vector3 direction)
        {
            direction.y = 0;
            float angleOfTarget = Vector3.Angle(characterTransform.forward, direction);
            Vector3 cross = Vector3.Cross(characterTransform.forward, direction);

            if (cross.y < 0)
                angleOfTarget = -angleOfTarget;

            return angleOfTarget;
        }

        // Optional: To visualize the viewing circle in the Scene view
        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(animal.animalLocomotionManager.groundedCheck.transform.position, detectionRadius);

            Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward * detectionRadius;
            Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward * detectionRadius;

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        }
        public void UpdateAnimalMovementParameters()
        {
            animal.isMoving = true;
            if (distanceFromTarget > 15)
            {
                animal.animalAnimatorManager.UpdateAnimatorMovementParameters(0, 1f, true);
            }
            else if (distanceFromTarget > 5)
            {
                animal.animalAnimatorManager.UpdateAnimatorMovementParameters(0, 1f, false);
            }
            else if (distanceFromTarget > navMeshAgent.stoppingDistance)
            {
                animal.animalAnimatorManager.UpdateAnimatorMovementParameters(0, .5f, false);
            }
            else
            {
                animal.isMoving = false;
            }
        }
        public void UpdateCurrentTarget()
        {
            targetsDirection = currentTarget - transform.position;
            angleToTarget = GetAngleOfTarget(
                transform,
                targetsDirection
            );
            distanceFromTarget = Vector3.Distance(
                transform.position,
                currentTarget
            );
        }
    }
}