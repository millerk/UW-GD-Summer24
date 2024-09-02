using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/PursueAction2", fileName = "PursueAction2")]
    public class PursueAction2 : FSMAction
    {
        public float speed = 5f;
        public float maxSpeed = 10f;
        public float rotationSpeed = 200f; // Speed of rotation towards the player
        public float rotationOffset = 90f; // Offset to correct angle (adjust if needed)
        public float angularDamping = 1f; // Angular damping to reduce spinning

        public override void Execute(BaseStateMachine stateMachine)
        {
            MonoBehaviour monoBehaviour = (MonoBehaviour)stateMachine;
            Transform entityTransform = monoBehaviour.transform;
            Rigidbody2D rb = monoBehaviour.GetComponent<Rigidbody2D>();

            if (rb == null)
            {
                Debug.LogWarning("Rigidbody2D component not found on stateMachine");
                return;
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogWarning("Player not found");
                return;
            }

            Vector2 playerDirection = (player.transform.position - entityTransform.position).normalized;

            // Pursue player
            PursuePlayer(rb, playerDirection);

            // Rotate towards player
            RotateTowardsPlayer(rb, playerDirection);

            // Handle stuck scenarios
            HandleStuck(rb);
        }

        private void PursuePlayer(Rigidbody2D rb, Vector2 playerDirection)
        {
            Vector2 desiredVelocity = playerDirection * speed;
            Vector2 newVelocity = rb.velocity + (desiredVelocity - rb.velocity) * Time.deltaTime;

            if (newVelocity.magnitude > maxSpeed)
            {
                newVelocity = newVelocity.normalized * maxSpeed;
            }

            rb.velocity = newVelocity;
        }

        private void RotateTowardsPlayer(Rigidbody2D rb, Vector2 playerDirection)
        {
            float targetAngle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
            targetAngle += 180f; // Match the offset used in CannonLockedOnState

            float currentAngle = rb.rotation;

            rb.rotation = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
            rb.angularVelocity *= (1f - angularDamping * Time.deltaTime);
        }

        private void HandleStuck(Rigidbody2D rb)
        {
            if (rb.velocity.magnitude < 0.1f) // If the entity is not moving
            {
                float randomAngle = Random.Range(0f, 360f);
                rb.velocity = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * speed;
            }

            rb.rotation = (Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg) - 90f;
        }
    }
}