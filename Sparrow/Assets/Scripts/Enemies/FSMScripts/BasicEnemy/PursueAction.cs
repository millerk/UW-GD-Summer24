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
            Vector2 avoidanceForce = Vector2.zero;

            // Use multiple rays for obstacle detection
            int numberOfRays = 8; // Number of rays to cast
            float angleStep = 360f / numberOfRays; // Full circle

            for (int i = 0; i < numberOfRays; i++)
            {
                float rayAngle = i * angleStep;
                Vector2 rayDirection = Quaternion.Euler(0, 0, rayAngle) * playerDirection;
                RaycastHit2D hit = Physics2D.Raycast(entityTransform.position, rayDirection, raySpacing, obstacleLayerMask);

                if (hit.collider != null)
                {
                    float distanceToObstacle = hit.distance;
                    float distanceToMaintain = minAvoidanceDistance - distanceToObstacle;

                    if (distanceToMaintain > 0)
                    {
                        // Avoid corners with stronger force
                        Vector2 parallelDirection = Vector2.Perpendicular(hit.normal);
                        if (Vector2.Dot(parallelDirection, playerDirection) < 0)
                        {
                            parallelDirection = -parallelDirection;
                        }

                        Vector2 avoidanceVector = parallelDirection * (distanceToMaintain * avoidanceForceMultiplier);
                        avoidanceForce += avoidanceVector;

                        // Draw debug rays for visualization
                        Debug.DrawRay(entityTransform.position, rayDirection * hit.distance, Color.red);
                        Debug.DrawRay(hit.point, parallelDirection * 2f, Color.green);
                    }
                }
            }

            // Check if the avoidance force is too strong, which might cause the enemy to get stuck
            if (avoidanceForce.magnitude > maxSpeed)
            {
                avoidanceForce = avoidanceForce.normalized * maxSpeed;
            }

            // Apply avoidance force
            rb.AddForce(avoidanceForce);

            // Calculate desired velocity and ensure it does not exceed maxSpeed
            Vector2 desiredVelocity = playerDirection * speed;
            Vector2 newVelocity = rb.velocity + (desiredVelocity - rb.velocity) * Time.deltaTime;

            if (newVelocity.magnitude > maxSpeed)
            {
                newVelocity = newVelocity.normalized * maxSpeed;
            }

            rb.velocity = newVelocity;

            // Rotate towards player with offset adjustment
            float targetAngle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
            float currentAngle = rb.rotation;
            float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle + rotationOffset);

            // Apply rotation, clamped to prevent excessive spinning
            rb.rotation = Mathf.MoveTowardsAngle(currentAngle, targetAngle + rotationOffset, rotationSpeed * Time.deltaTime);

            // Apply angular damping to prevent uncontrolled spinning
            rb.angularVelocity *= (1f - angularDamping * Time.deltaTime);

            // Backup plan for stuck scenarios
            if (rb.velocity.magnitude < 0.1f) // If the enemy is not moving
            {
                // Rotate randomly to escape the corner
                float randomAngle = Random.Range(0f, 360f);
                rb.velocity = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * speed;
            }
        }
    }
}