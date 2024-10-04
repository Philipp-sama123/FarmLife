using HoaxGames;
using UnityEngine;

namespace KrazyKatgames
{
    public class AnimalLocomotionManager : MonoBehaviour
    {
        AnimalManager animal;

        [Header("Ground Check & Jumping")]
        public GameObject groundedCheck;
        [SerializeField] protected float gravityForce = -15f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float groundCheckSphereRadius = 1;
        [SerializeField] protected Vector3 yVelocity; // THE FORCE AT WHICH OUR CHARACTER IS PULLED UP OR DOWN (Jumping or Falling)
        [SerializeField] float groundedYVelocity = -20f; // THE FORCE AT WHICH OUR CHARACTER IS STICKING TO THE GROUND WHILST THEY ARE GROUNDED
        [SerializeField] float fallStartYVelocity = -5;
        [SerializeField] float rotationSpeed = 5;

        protected bool fallingVelocityHAsBeenSet = false;
        [SerializeField] protected float inAirTimer = 0;

        [Header("Flags")]
        public bool isGrounded = false;

        protected virtual void Awake()
        {
            animal = GetComponent<AnimalManager>();
        }
        protected virtual void Update()
        {
            HandleGroundCheck();

            if (animal.animalLocomotionManager.isGrounded)
            {
                //  IF WE ARE NOT ATTEMPTING TO JUMP OR MOVE UPWARD
                if (yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocityHAsBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                //  IF WE ARE NOT JUMPING, AND OUR FALLING VELOCITY HAS NOT BEEN SET
                if (!animal.isJumping && !fallingVelocityHAsBeenSet)
                {
                    fallingVelocityHAsBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }

                inAirTimer = inAirTimer + Time.deltaTime;
                animal.animator.SetFloat("InAirTimer", inAirTimer);

                yVelocity.y += gravityForce * Time.deltaTime;
            }

            //  THERE SHOULD ALWAYS BE SOME FORCE APPLIED TO THE Y VELOCITY
            animal.characterController.Move(yVelocity * Time.deltaTime);
        }

        protected void HandleGroundCheck()
        {
            isGrounded = Physics.CheckSphere(groundedCheck.transform.position, groundCheckSphereRadius, groundLayer);
        }
        protected void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(groundedCheck.transform.position, groundCheckSphereRadius);
        }
        public void RotateTowardsAgent(AnimalManager animal)
        {
            if (animal.isMoving)
            {
                animal.transform.rotation = animal.animalAI.navMeshAgent.transform.rotation;
            }
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