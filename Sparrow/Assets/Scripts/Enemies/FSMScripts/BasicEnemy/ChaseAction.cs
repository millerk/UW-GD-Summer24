using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/ChaseAction", fileName = "ChaseAction")]
    public class ChaseAction : FSMAction
    {
        public float moveSpeed = 5f;  // The speed at which the enemy moves towards the player

        public override void Execute(BaseStateMachine stateMachine)
        {
            // Assuming the enemy has a "Player" GameObject tagged as "Player"
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player == null)
            {
                Debug.LogWarning("Player not found!");
                return;
            }

            // Move towards the player
            stateMachine.transform.position = Vector3.MoveTowards(
                stateMachine.transform.position,
                player.transform.position,
                moveSpeed * Time.deltaTime
            );

            Debug.Log("Chasing player...");
        }
    }
}