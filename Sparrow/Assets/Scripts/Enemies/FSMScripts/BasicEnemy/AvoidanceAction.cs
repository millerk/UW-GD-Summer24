using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/AvoidanceAction", fileName = "AvoidanceAction")]
    public class AvoidanceAction : FSMAction
    {
        public float avoidanceForceMultiplier = 10f;
        public float raySpacing = 10f; // Distance of rays
        public float minAvoidanceDistance = 2f; // Minimum distance to maintain from obstacles
        public float cornerAvoidanceForceMultiplier = 20f; // Stronger force for corner avoidance
        public LayerMask obstacleLayerMask;
        public float maxSpeed = 10f;

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
            Vector2 avoidanceForce = CalculateAvoidanceForce(entityTransform.position, playerDirection);

            // Check if the avoidance force is too strong, which might cause the entity to get stuck
            if (avoidanceForce.magnitude > maxSpeed)
            {
                avoidanceForce = avoidanceForce.normalized * maxSpeed;
            }

            // Apply avoidance force
            rb.AddForce(avoidanceForce);
        }

        private Vector2 CalculateAvoidanceForce(Vector2 entityPosition, Vector2 playerDirection)
        {
            Vector2 avoidanceForce = Vector2.zero;
            int numberOfRays = 8; // Number of rays to cast
            float angleStep = 360f / numberOfRays; // Full circle

            for (int i = 0; i < numberOfRays; i++)
            {
                float rayAngle = i * angleStep;
                Vector2 rayDirection = Quaternion.Euler(0, 0, rayAngle) * playerDirection;
                RaycastHit2D hit = Physics2D.Raycast(entityPosition, rayDirection, raySpacing, obstacleLayerMask);

                if (hit.collider != null)
                {
                    float distanceToObstacle = hit.distance;
                    float distanceToMaintain = minAvoidanceDistance - distanceToObstacle;

                    if (distanceToMaintain > 0)
                    {
                        Vector2 parallelDirection = Vector2.Perpendicular(hit.normal);
                        if (Vector2.Dot(parallelDirection, playerDirection) < 0)
                        {
                            parallelDirection = -parallelDirection;
                        }

                        Vector2 avoidanceVector = parallelDirection * (distanceToMaintain * avoidanceForceMultiplier);
                        avoidanceForce += avoidanceVector;

                        // Draw debug rays for visualization
                        Debug.DrawRay(entityPosition, rayDirection * hit.distance, Color.red);
                        Debug.DrawRay(hit.point, parallelDirection * 2f, Color.green);
                    }
                }
            }

            return avoidanceForce;
        }
    }
}