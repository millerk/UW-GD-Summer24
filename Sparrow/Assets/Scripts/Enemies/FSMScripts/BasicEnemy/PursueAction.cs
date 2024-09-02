using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/PursueAction", fileName = "PursueAction")]
    public class PursueAction : FSMAction
    {
        public float speed = 5f;
        public float maxSpeed = 10f;
        public float avoidanceForceMultiplier = 10f;
        public float raySpacing = 10f; // Distance of rays
        public float minAvoidanceDistance = 2f; // Minimum distance to maintain from obstacles
        public float cornerAvoidanceForceMultiplier = 20f; // Stronger force for corner avoidance
        public LayerMask obstacleLayerMask;
        public float rotationSpeed = 200f; // Speed of rotation towards the player
        public float rotationOffset = 90f; // Offset to correct angle
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
            Vector2 avoidanceForce = CalculateAvoidanceForce(entityTransform);

            ApplyAvoidanceForce(rb, avoidanceForce);
            MoveTowardsPlayer(rb, playerDirection);
            RotateTowardsPlayer(rb, playerDirection);
            HandleStuckScenario(rb);
        }

        private Vector2 CalculateAvoidanceForce(Transform entityTransform)
        {
            Vector2 avoidanceForce = Vector2.zero;
            int numberOfRays = 8;
            float angleStep = 360f / numberOfRays;

            for (int i = 0; i < numberOfRays; i++)
            {
                float rayAngle = i * angleStep;
                Vector2 rayDirection = Quaternion.Euler(0, 0, rayAngle) * Vector2.right;
                RaycastHit2D hit = Physics2D.Raycast(entityTransform.position, rayDirection, raySpacing, obstacleLayerMask);

                if (hit.collider != null)
                {
                    float distanceToObstacle = hit.distance;
                    float distanceToMaintain = minAvoidanceDistance - distanceToObstacle;

                    if (distanceToMaintain > 0)
                    {
                        Vector2 avoidanceVector = GetAvoidanceVector(hit, distanceToMaintain);
                        avoidanceForce += avoidanceVector;

                        // Draw debug rays for visualization
                        Debug.DrawRay(entityTransform.position, rayDirection * hit.distance, Color.red);
                        Debug.DrawRay(hit.point, avoidanceVector, Color.green);
                    }
                }
            }

            return avoidanceForce;
        }

        private Vector2 GetAvoidanceVector(RaycastHit2D hit, float distanceToMaintain)
        {
            Vector2 parallelDirection = Vector2.Perpendicular(hit.normal);
            if (Vector2.Dot(parallelDirection, Vector2.right) < 0)
            {
                parallelDirection = -parallelDirection;
            }

            return parallelDirection * (distanceToMaintain * avoidanceForceMultiplier);
        }

        private void ApplyAvoidanceForce(Rigidbody2D rb, Vector2 avoidanceForce)
        {
            if (avoidanceForce.magnitude > maxSpeed)
            {
                avoidanceForce = avoidanceForce.normalized * maxSpeed;
            }
            rb.AddForce(avoidanceForce);
        }

        private void MoveTowardsPlayer(Rigidbody2D rb, Vector2 playerDirection)
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
            float currentAngle = rb.rotation;
            float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle + rotationOffset);

            rb.rotation = Mathf.MoveTowardsAngle(currentAngle, targetAngle + rotationOffset, rotationSpeed * Time.deltaTime);
            rb.angularVelocity *= (1f - angularDamping * Time.deltaTime);
        }

        private void HandleStuckScenario(Rigidbody2D rb)
        {
            if (rb.velocity.magnitude < 0.1f)
            {
                float randomAngle = Random.Range(0f, 360f);
                rb.velocity = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * speed;
            }
        }
    }
}