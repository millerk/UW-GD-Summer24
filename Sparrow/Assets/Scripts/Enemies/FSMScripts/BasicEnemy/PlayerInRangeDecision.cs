using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Decisions/PlayerInRangeDecision")]
    public class PlayerInRangeDecision : Decision
    {
        public float detectionRange = 10f; // Set this in the Inspector or via script
        public LayerMask detectionLayerMask; // Layer mask for player and walls

        public override bool Decide(BaseStateMachine stateMachine)
        {
            // Find the player by tag
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            // Check if the player exists
            if (player == null)
            {
                Debug.LogWarning("Player not found!");
                return false;
            }

            // Calculate the distance between the player and the state machine
            float distanceToPlayer = Vector3.Distance(player.transform.position, stateMachine.transform.position);

            // Check if the player is within the detection range
            if (distanceToPlayer > detectionRange)
            {
                return false;
            }

            // Perform a raycast to check for line of sight, considering walls and player layers
            RaycastHit2D ray = Physics2D.Raycast(stateMachine.transform.position, player.transform.position - stateMachine.transform.position, detectionRange, detectionLayerMask);

            if (ray.collider != null)
            {
                // Check if the object hit by the raycast is the player
                bool hasLineOfSight = ray.collider.CompareTag("Player");

                // Debugging: Draw the ray in the Scene view
                Debug.DrawRay(stateMachine.transform.position, (player.transform.position - stateMachine.transform.position).normalized * detectionRange, hasLineOfSight ? Color.green : Color.red);

                // Return true if the player is within range and visible
                return hasLineOfSight;
            }

            // If the raycast did not hit anything, return false
            return false;
        }
    }
}