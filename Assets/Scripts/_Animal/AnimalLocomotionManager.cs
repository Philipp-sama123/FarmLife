using HoaxGames;
using UnityEngine;

namespace KrazyKatGames
{
    public class AnimalLocomotionManager : CharacterLocomotionManager
    {
        AnimalManager animal;

        [Header("Ground Check & Jumping")]
        public GameObject groundedCheck;
        [SerializeField] float rotationSpeed = 5;


        protected override void Awake()
        {
            base.Awake();

            animal = GetComponent<AnimalManager>();
        }
        protected override void Update()
        {
            base.Update();
        }


        public void RotateTowardsAgent(AnimalManager animal)
        {
            if (animal.isMoving)
            {
                animal.transform.rotation = animal.animalAI.navMeshAgent.transform.rotation;
            }
        }
        protected override void  HandleGroundCheck()
        {
            isGrounded = Physics.CheckSphere(groundedCheck.transform.position, groundCheckSphereRadius, groundLayer);
        }
        public virtual void PivotTowardsTarget(AnimalManager animal)
        {
            if (animal.isPerformingAction)
                return;

            if (animal.animalAI.AngleToTarget >= 146 && animal.animalAI.AngleToTarget <= 180)
            {
                animal.animalAnimatorManager.PlayTargetActionAnimation("Turn_180_R", true);
            }
            else if (animal.animalAI.AngleToTarget <= -146 && animal.animalAI.AngleToTarget >= -180)
            {
                animal.animalAnimatorManager.PlayTargetActionAnimation("Turn_180_L", true);
            }
        }
        public void RotateTowardsTarget(Vector3 targetPosition)
        {
            Vector3 targetDirection = targetPosition - animal.transform.position;
            targetDirection.y = 0;
            targetDirection.Normalize();

            if (targetDirection == Vector3.zero)
                targetDirection = animal.transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            animal.transform.rotation = Quaternion.Slerp(animal.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}