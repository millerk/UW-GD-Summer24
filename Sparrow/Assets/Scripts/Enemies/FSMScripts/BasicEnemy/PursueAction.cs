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
                float angle = i * angleStep;
                Vector2 rayDirection = Quaternion.Euler(0, 0, angle) * playerDirection;
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